/*
 *  Part of PPather
 *  Copyright Pontus Borg 2008
 * 
 */

using System;
using System.Collections.Generic;
using PatherPath.Graph;
using Wmo;

namespace WowTriangles
{
    // Fully automatic triangle loader for a MPQ/WoW world


    public abstract class TriangleSupplier
    {
        public abstract void GetTriangles(TriangleCollection to, float min_x, float min_y, float max_x, float max_y);

        public virtual void Close()
        {
        }
    }


    /// <summary>
    ///     A chunked collection of triangles
    /// </summary>
    public class ChunkedTriangleCollection
    {
        public static int TriangleFlagDeepWater = 1;
        public static int TriangleFlagObject = 2;
        public static int TriangleFlagModel = 4;
        private int NOW;
        private bool UseMatrix = true;
        private bool UseOctree = false;


        private SparseMatrix2D<TriangleCollection> chunks;

        private List<TriangleCollection> loadedChunks = new List<TriangleCollection>();
        private int maxCached = 1000;
        private List<TriangleSupplier> suppliers = new List<TriangleSupplier>();

        public ChunkedTriangleCollection(float chunkSize)
        {
            // this.chunkSize = chunkSize;
            chunks = new SparseMatrix2D<TriangleCollection>(8);
        }


        public void Close()
        {
            foreach (TriangleSupplier s in suppliers)
                s.Close();
            suppliers = null;
            loadedChunks = null;
            chunks = null;
        }

        public void SetMaxCached(int maxCached)
        {
            this.maxCached = maxCached;
        }

        private void EvictIfNeeded()
        {
            if (loadedChunks.Count >= maxCached)
            {
                TriangleCollection toEvict = null;
                foreach (TriangleCollection tc in loadedChunks)
                {
                    int LRU = tc.LRU;
                    if (toEvict == null || LRU < toEvict.LRU)
                    {
                        toEvict = tc;
                    }
                }
                loadedChunks.Remove(toEvict);
                chunks.Clear(toEvict.grid_x, toEvict.grid_y);
                PathGraph.Log("Evict chunk at " + toEvict.base_x + " " + toEvict.base_y);
            }
        }

        public void AddSupplier(TriangleSupplier supplier)
        {
            suppliers.Add(supplier);
        }

        public void GetGridStartAt(float x, float y, out int grid_x, out int grid_y)
        {
            x = ChunkReader.ZEROPOINT - x;
            grid_x = (int) (x/ChunkReader.TILESIZE);
            y = ChunkReader.ZEROPOINT - y;
            grid_y = (int) (y/ChunkReader.TILESIZE);
        }

        private void GetGridLimits(int grid_x, int grid_y,
                                   out float min_x, out float min_y,
                                   out float max_x, out float max_y)
        {
            max_x = ChunkReader.ZEROPOINT - (grid_x)*ChunkReader.TILESIZE;
            min_x = max_x - ChunkReader.TILESIZE;
            max_y = ChunkReader.ZEROPOINT - (grid_y)*ChunkReader.TILESIZE;
            min_y = max_y - ChunkReader.TILESIZE;
        }

        private void LoadChunkAt(float x, float y)
        {
            int grid_x, grid_y;
            GetGridStartAt(x, y, out grid_x, out grid_y);

            if (chunks.IsSet(grid_x, grid_y))
                return;
            EvictIfNeeded();
            var tc = new TriangleCollection();


            float min_x, max_x, min_y, max_y;
            GetGridLimits(grid_x, grid_y, out min_x, out min_y, out max_x, out max_y);

            PathGraph.Log("Got asked for triangles at " + x + ", " + y);
            PathGraph.Log("Need triangles grid (" + min_x + " , " + min_y + ") - (" + max_x + ", " + max_y);

            tc.SetLimits(min_x - 1, min_y - 1, -1E30f, max_x + 1, max_y + 1, 1E30f);
            foreach (TriangleSupplier s in suppliers)
            {
                s.GetTriangles(tc, min_x, min_y, max_x, max_y);
            }
            tc.CompactVertices();
            tc.ClearVertexMatrix(); // not needed anymore
            tc.base_x = grid_x;
            tc.base_y = grid_y;
            PathGraph.Log("  it got " + tc.GetNumberOfTriangles() + " triangles and " + tc.GetNumberOfVertices() +
                          " vertices");


            loadedChunks.Add(tc);
            chunks.Set(grid_x, grid_y, tc);


            PathGraph.Log("Got triangles grid (" + tc.min_x + " , " + tc.min_y + ") - (" + tc.max_x + ", " + tc.max_y);
        }


        public TriangleCollection GetChunkAt(float x, float y)
        {
            LoadChunkAt(x, y);
            int grid_x, grid_y;
            GetGridStartAt(x, y, out grid_x, out grid_y);
            TriangleCollection tc = chunks.Get(grid_x, grid_y);
            tc.LRU = NOW++;
            return tc;
        }


        public bool IsSpotBlocked(float x, float y, float z,
                                  float toonHeight, float toonSize)
        {
            TriangleCollection tc = GetChunkAt(x, y);

            ICollection<int> ts, tst, tsm;
            ts = null;
            if (UseOctree)
            {
                TriangleOctree ot = tc.GetOctree();
                tst = ts = ot.FindTrianglesInBox(x - toonSize, y - toonSize, z + toonHeight - toonSize*2,
                                                 x + toonSize, y + toonSize, z + toonHeight);
            }

            if (UseMatrix)
            {
                TriangleMatrix tm = tc.GetTriangleMatrix();
                tsm = ts = tm.GetAllCloseTo(x, y, toonSize);
            }

            Vector toon;
            toon.x = x;
            toon.y = y;
            toon.z = z + toonHeight - toonSize;

            //            for(int t = 0 ; t<tc.GetNumberOfTriangles(); t++)
            //            {

            foreach (int t in ts)
            {
                Vector vertex0;
                Vector vertex1;
                Vector vertex2;
                int flags;
                tc.GetTriangleVertices(t,
                                       out vertex0.x, out vertex0.y, out vertex0.z,
                                       out vertex1.x, out vertex1.y, out vertex1.z,
                                       out vertex2.x, out vertex2.y, out vertex2.z, out flags);
                float d = Utils.PointDistanceToTriangle(toon, vertex0, vertex1, vertex2);
                if (d < toonSize)
                    return true;
            }

            return false;
        }


        public void CheckAllCollides(float x, float y, float z, TriangleCollection paintI)
        {
            TriangleCollection tc = GetChunkAt(x, y);
            TriangleMatrix tm = tc.GetTriangleMatrix();
            ICollection<int> ts = tm.GetAllCloseTo(x, y, 15.0f);

            Vector s0;
            Vector s1;
            s0.x = x;
            s0.y = y;
            s0.z = z;

            foreach (int t in ts)
                //for(int t = 0 ; t<tc.GetNumberOfTriangles(); t++)
            {
                Vector vertex0;
                Vector vertex1;
                Vector vertex2;
                Vector intersect;

                tc.GetTriangleVertices(t,
                                       out vertex0.x, out vertex0.y, out vertex0.z,
                                       out vertex1.x, out vertex1.y, out vertex1.z,
                                       out vertex2.x, out vertex2.y, out vertex2.z);


                s1.x = (vertex0.x + vertex1.x + vertex2.x)/3;
                s1.y = (vertex0.y + vertex1.y + vertex2.y)/3;
                s1.z = (vertex0.z + vertex1.z + vertex2.z)/3 - 0.1f;
                //paintI.AddMarker(s1.x, s1.y, s1.z + 0.1f); 

                if (Utils.SegmentTriangleIntersect(s0, s1, vertex0, vertex1, vertex2, out intersect))
                {
                    if (paintI != null)
                    {
                        //    AddVisible(paintI, intersect.x, intersect.y, intersect.z); 
                    }
                    // blocked!
                }
            }
        }


        public bool IsStepBlocked(float x0, float y0, float z0,
                                  float x1, float y1, float z1,
                                  float toonHeight, float toonSize, TriangleCollection paintI)
        {
            TriangleCollection tc = GetChunkAt(x0, y0);

            float dx = x0 - x1;
            float dy = y0 - y1;
            float dz = z0 - z1;
            var stepLength = (float) Math.Sqrt(dx*dx + dy*dy + dz + dz);
            // 1: check steepness

            // TODO

            // 2: check is there is a big step 

            float mid_x = (x0 + x1)/2.0f;
            float mid_y = (y0 + y1)/2.0f;
            float mid_z = (z0 + z1)/2.0f;
            float mid_z_hit = 0;
            float mid_dz = Math.Abs(stepLength);
            //if (mid_dz < 1.0f) mid_dz = 1.0f;
            int mid_flags = 0;
            if (FindStandableAt(mid_x, mid_y, mid_z - mid_dz, mid_z + mid_dz, out mid_z_hit, out mid_flags, toonHeight,
                                toonSize))
            {
                float dz0 = Math.Abs(z0 - mid_z_hit);
                float dz1 = Math.Abs(z1 - mid_z_hit);

                // PathGraph.Log("z0 " + z0 + " z1 " + z1 + " dz0 " + dz0+ " dz1 " + dz1 );
                if (dz0 > stepLength/2.0 && dz0 > 1.0)
                    return true; // too steep

                if (dz1 > stepLength/2.0 && dz1 > 1.0)
                    return true; // too steep
            }
            else
            {
                // bad!
                return true;
            }

            ICollection<int> ts, tsm, tst;
            ts = null;
            if (UseOctree)
            {
                TriangleOctree ot = tc.GetOctree();
                tst = ts = ot.FindTrianglesInBox(Utils.min(x0, x1), Utils.min(y0, y1), Utils.min(z0, z1),
                                                 Utils.max(x0, x1), Utils.max(y0, y1), Utils.max(z0, z1));
            }
            if (UseMatrix)
            {
                TriangleMatrix tm = tc.GetTriangleMatrix();
                tsm = ts = tm.GetAllInSquare(Utils.min(x0, x1), Utils.min(y0, y1), Utils.max(x0, x1), Utils.max(y0, y1));
            }


            // 3: check collision with objects

            bool coll = false;
            Vector from, from_up;
            Vector to, to_up;
            from.x = x0;
            from.y = y0;
            from.z = z0 + toonSize;
            from_up.x = x0;
            from_up.y = y0;
            from_up.z = z0 + toonHeight - toonSize;

            to.x = x1;
            to.y = y1;
            to.z = z1 + toonSize;
            to_up.x = x1;
            to_up.y = y1;
            to_up.z = z1 + toonHeight - toonSize;


            foreach (int t in ts)
                //for(int t = 0 ; t<tc.GetNumberOfTriangles(); t++)
            {
                Vector vertex0;
                Vector vertex1;
                Vector vertex2;
                Vector intersect;

                tc.GetTriangleVertices(t,
                                       out vertex0.x, out vertex0.y, out vertex0.z,
                                       out vertex1.x, out vertex1.y, out vertex1.z,
                                       out vertex2.x, out vertex2.y, out vertex2.z);

                if (Utils.SegmentTriangleIntersect(from, to_up, vertex0, vertex1, vertex2, out intersect) ||
                    Utils.SegmentTriangleIntersect(from_up, to, vertex0, vertex1, vertex2, out intersect))
                {
                    if (paintI != null)
                    {
                        //paintI.AddMarker(intersect.x, intersect.y, intersect.z); 
                    }
                    //PathGraph.Log("Collided at " + intersect);
                    coll = true;
                    return true;
                    // blocked!
                }
            }


            return coll;
        }

        public bool FindStandableAt(float x, float y, float min_z, float max_z,
                                    out float z0, out int flags, float toonHeight, float toonSize)
        {
            TriangleCollection tc = GetChunkAt(x, y);
            ICollection<int> ts, tsm, tst;
            float minCliffD = 0.5f;

            ts = null;
            if (UseOctree)
            {
                TriangleOctree ot = tc.GetOctree();
                tst =
                    ts = ot.FindTrianglesInBox(x - minCliffD, y - minCliffD, min_z, x + minCliffD, y + minCliffD, max_z);
            }

            if (UseMatrix)
            {
                TriangleMatrix tm = tc.GetTriangleMatrix();
                tsm = ts = tm.GetAllCloseTo(x, y, 1.0f);
            }

            Vector s0, s1;
            s0.x = x;
            s0.y = y;
            s0.z = min_z;
            s1.x = x;
            s1.y = y;
            s1.z = max_z;

            float best_z = -1E30f;
            int best_flags = 0;
            bool found = false;


            foreach (int t in ts)
            {
                Vector vertex0;
                Vector vertex1;
                Vector vertex2;
                Vector intersect;
                int t_flags;

                tc.GetTriangleVertices(t,
                                       out vertex0.x, out vertex0.y, out vertex0.z,
                                       out vertex1.x, out vertex1.y, out vertex1.z,
                                       out vertex2.x, out vertex2.y, out vertex2.z, out t_flags);

                Vector normal;
                Utils.GetTriangleNormal(vertex0, vertex1, vertex2, out normal);
                var angle_z = (float) Math.Sin(45.0/360.0*Math.PI*2); //
                if (Utils.abs(normal.z) > angle_z)
                {
                    if (Utils.SegmentTriangleIntersect(s0, s1, vertex0, vertex1, vertex2, out intersect))
                    {
                        if (intersect.z > best_z)
                        {
                            if (!IsSpotBlocked(intersect.x, intersect.y, intersect.z, toonHeight, toonSize))
                            {
                                best_z = intersect.z;
                                best_flags = t_flags;
                                found = true;
                            }
                        }
                    }
                }
            }
            if (found)
            {
                Vector up, dn, tmp;
                up.z = best_z + 2;
                dn.z = best_z - 5;
                bool[] nearCliff = {true, true, true, true};

                bool allGood = true;
                foreach (int t in ts)
                {
                    Vector vertex0;
                    Vector vertex1;
                    Vector vertex2;

                    tc.GetTriangleVertices(t,
                                           out vertex0.x, out vertex0.y, out vertex0.z,
                                           out vertex1.x, out vertex1.y, out vertex1.z,
                                           out vertex2.x, out vertex2.y, out vertex2.z);

                    float[] dx = {minCliffD, -minCliffD, 0, 0};
                    float[] dy = {0, 0, minCliffD, -minCliffD};
                    // Check if it is close to a "cliff"

                    allGood = true;
                    for (int i = 0; i < 4; i++)
                    {
                        if (nearCliff[i])
                        {
                            up.x = dn.x = x + dx[i];
                            up.y = dn.y = y + dy[i];
                            if (Utils.SegmentTriangleIntersect(up, dn, vertex0, vertex1, vertex2, out tmp))
                                nearCliff[i] = false;
                        }
                        allGood &= !nearCliff[i];
                    }
                    if (allGood)
                        break;
                }

                allGood = true;
                for (int i = 0; i < 4; i++)
                    allGood &= !nearCliff[i];
                if (!allGood)
                {
                    z0 = best_z;
                    flags = best_flags;
                    return false; // too close to cliff
                }
            }
            z0 = best_z;
            flags = best_flags;
            return found;
        }
    }


    /// <summary>
    /// </summary>
    public class TriangleCollection
    {
        private readonly IndexArray triangles = new IndexArray();
        private readonly VertexArray vertices = new VertexArray();
        public int LRU;
        public float base_x, base_y;
        public bool changed = true;
        private TriangleMatrix collisionMatrix;
        public float[] color;
        public bool fill = false;
        public int grid_x, grid_y;

        private float limit_max_x = 1E30f;
        private float limit_max_y = 1E30f;
        private float limit_max_z = 1E30f;

        private float limit_min_x = -1E30f;
        private float limit_min_y = -1E30f;
        private float limit_min_z = -1E30f;
        public float max_x = -1E30f;
        public float max_y = -1E30f;
        public float max_z = -1E30f;

        public float min_x = 1E30f;
        public float min_y = 1E30f;
        public float min_z = 1E30f;
        private int no_triangles;
        private int no_vertices;

        private TriangleOctree oct;
        private TriangleQuadtree quad;
        private SparseFloatMatrix3D<int> vertexMatrix = new SparseFloatMatrix3D<int>(0.1f);

        public void Clear()
        {
            no_triangles = 0;
            no_vertices = 0;
            vertexMatrix = new SparseFloatMatrix3D<int>(0.1f);
            changed = true;
        }

        public TriangleOctree GetOctree()
        {
            if (oct == null)
                oct = new TriangleOctree(this);
            return oct;
        }

        // remove unused vertices
        public void CompactVertices()
        {
            var used_indices = new bool[GetNumberOfVertices()];
            var old_to_new = new int[GetNumberOfVertices()];

            // check what vertives are used
            for (int i = 0; i < GetNumberOfTriangles(); i++)
            {
                int v0, v1, v2;
                GetTriangle(i, out v0, out v1, out v2);
                used_indices[v0] = true;
                used_indices[v1] = true;
                used_indices[v2] = true;
            }

            // figure out new indices and move
            int sum = 0;
            for (int i = 0; i < used_indices.Length; i++)
            {
                if (used_indices[i])
                {
                    old_to_new[i] = sum;
                    float x, y, z;
                    vertices.Get(i, out x, out y, out z);
                    vertices.Set(sum, x, y, z);
                    sum++;
                }
                else
                    old_to_new[i] = -1;
            }

            vertices.SetSize(sum);

            // Change all triangles
            for (int i = 0; i < GetNumberOfTriangles(); i++)
            {
                int v0, v1, v2, flags;
                GetTriangle(i, out v0, out v1, out v2, out flags);
                triangles.Set(i, old_to_new[v0], old_to_new[v1], old_to_new[v2], flags);
            }
            no_vertices = sum;
        }


        public TriangleQuadtree GetQuadtree()
        {
            if (quad == null)
                quad = new TriangleQuadtree(this);
            return quad;
        }

        public TriangleMatrix GetTriangleMatrix()
        {
            if (collisionMatrix == null)
                collisionMatrix = new TriangleMatrix(this);
            return collisionMatrix;
        }

        public void SetLimits(float min_x, float min_y, float min_z,
                              float max_x, float max_y, float max_z)
        {
            limit_max_x = max_x;
            limit_max_y = max_y;
            limit_max_z = max_z;

            limit_min_x = min_x;
            limit_min_y = min_y;
            limit_min_z = min_z;
        }

        public void GetLimits(out float min_x, out float min_y, out float min_z,
                              out float max_x, out float max_y, out float max_z)
        {
            max_x = limit_max_x;
            max_y = limit_max_y;
            max_z = limit_max_z;

            min_x = limit_min_x;
            min_y = limit_min_y;
            min_z = limit_min_z;
        }


        public void PaintPath(float x, float y, float z, float x2, float y2, float z2)
        {
            int v0 = AddVertex(x, y, z + 0.1f);
            int v1 = AddVertex(x, y, z + 0.5f);
            int v2 = AddVertex(x2, y2, z2 + 0.1f);

            //int v0 = AddVertex(x, y, z + 2.0f - 0.5f);
            //int v1 = AddVertex(x, y, z + 2.0f);
            //int v2 = AddVertex(x2, y2, z2+2.0f-0.5f);

            AddTriangle(v0, v1, v2);
            AddTriangle(v2, v1, v0);
        }

        public void AddMarker(float x, float y, float z)
        {
            int v0 = AddVertex(x, y, z);
            int v1 = AddVertex(x + 0.3f, y, z + 1.0f);
            int v2 = AddVertex(x - 0.3f, y, z + 1.0f);
            int v3 = AddVertex(x, y + 0.3f, z + 1.0f);
            int v4 = AddVertex(x, y - 0.3f, z + 1.0f);
            AddTriangle(v0, v1, v2);
            AddTriangle(v2, v1, v0);
            AddTriangle(v0, v3, v4);
            AddTriangle(v4, v3, v0);
        }

        public void AddBigMarker(float x, float y, float z)
        {
            int v0 = AddVertex(x, y, z);
            int v1 = AddVertex(x + 1.3f, y, z + 4);
            int v2 = AddVertex(x - 1.3f, y, z + 4);
            int v3 = AddVertex(x, y + 1.3f, z + 4);
            int v4 = AddVertex(x, y - 1.3f, z + 4);
            AddTriangle(v0, v1, v2);
            AddTriangle(v2, v1, v0);
            AddTriangle(v0, v3, v4);
            AddTriangle(v4, v3, v0);
        }


        public void GetBBox(out float min_x, out float min_y, out float min_z,
                            out float max_x, out float max_y, out float max_z)
        {
            max_x = this.max_x;
            max_y = this.max_y;
            max_z = this.max_z;

            min_x = this.min_x;
            min_y = this.min_y;
            min_z = this.min_z;
        }

        public int AddVertex(float x, float y, float z)
        {
            // Create new if needed or return old one

            if (vertexMatrix.IsSet(x, y, z))
                return vertexMatrix.Get(x, y, z);


            vertices.Set(no_vertices, x, y, z);
            vertexMatrix.Set(x, y, z, no_vertices);
            return no_vertices++;
        }


        // big list if triangles (3 vertice IDs per triangle)
        public int AddTriangle(int v0, int v1, int v2, int flags)
        {
            // check limits
            if (!CheckVertexLimits(v0) &&
                !CheckVertexLimits(v1) &&
                !CheckVertexLimits(v2))
                return -1;
            // Create new
            SetMinMax(v0);
            SetMinMax(v1);
            SetMinMax(v2);

            triangles.Set(no_triangles, v0, v1, v2, flags);
            changed = true;
            return no_triangles++;
        }

        // big list if triangles (3 vertice IDs per triangle)
        public int AddTriangle(int v0, int v1, int v2)
        {
            return AddTriangle(v0, v1, v2, 0);
        }

        private void SetMinMax(int v)
        {
            float x, y, z;
            GetVertex(v, out x, out y, out z);
            if (x < min_x)
                min_x = x;
            if (y < min_y)
                min_y = y;
            if (z < min_z)
                min_z = z;

            if (x > max_x)
                max_x = x;
            if (y > max_y)
                max_y = y;
            if (z > max_z)
                max_z = z;
        }

        private bool CheckVertexLimits(int v)
        {
            float x, y, z;
            GetVertex(v, out x, out y, out z);
            if (x < limit_min_x || x > limit_max_x)
                return false;
            if (y < limit_min_y || y > limit_max_y)
                return false;
            if (z < limit_min_z || z > limit_max_z)
                return false;

            return true;
        }

        public void GetBoundMax(out float x, out float y, out float z)
        {
            x = max_x;
            y = max_y;
            z = max_z;
        }

        public void GetBoundMin(out float x, out float y, out float z)
        {
            x = min_x;
            y = min_y;
            z = min_z;
        }

        public int GetNumberOfTriangles()
        {
            return no_triangles;
        }

        public int GetNumberOfVertices()
        {
            return no_vertices;
        }

        public void GetVertex(int i, out float x, out float y, out float z)
        {
            vertices.Get(i, out x, out y, out z);
        }

        public void GetTriangle(int i, out int v0, out int v1, out int v2)
        {
            int w;
            triangles.Get(i, out v0, out v1, out v2, out w);
        }

        public void GetTriangle(int i, out int v0, out int v1, out int v2, out int flags)
        {
            triangles.Get(i, out v0, out v1, out v2, out flags);
        }

        public void GetTriangleVertices(int i,
                                        out float x0, out float y0, out float z0,
                                        out float x1, out float y1, out float z1,
                                        out float x2, out float y2, out float z2, out int flags)
        {
            int v0, v1, v2;

            triangles.Get(i, out v0, out v1, out v2, out flags);
            vertices.Get(v0, out x0, out y0, out z0);
            vertices.Get(v1, out x1, out y1, out z1);
            vertices.Get(v2, out x2, out y2, out z2);
        }

        public void GetTriangleVertices(int i,
                                        out float x0, out float y0, out float z0,
                                        out float x1, out float y1, out float z1,
                                        out float x2, out float y2, out float z2)
        {
            int v0, v1, v2, flags;

            triangles.Get(i, out v0, out v1, out v2, out flags);
            vertices.Get(v0, out x0, out y0, out z0);
            vertices.Get(v1, out x1, out y1, out z1);
            vertices.Get(v2, out x2, out y2, out z2);
        }

        public float[] GetFlatVertices()
        {
            var flat = new float[no_vertices*3];
            for (int i = 0; i < no_vertices; i++)
            {
                int off = i*3;
                vertices.Get(i, out flat[off], out flat[off + 1], out flat[off + 2]);
            }
            return flat;
        }

        public void ClearVertexMatrix()
        {
            vertexMatrix = new SparseFloatMatrix3D<int>(0.1f);
        }

        public void ReportSize(string pre)
        {
            PathGraph.Log(pre + "no_vertices: " + no_vertices);
            PathGraph.Log(pre + "no_triangles: " + no_triangles);
        }


        public void AddAllTrianglesFrom(TriangleCollection set)
        {
            for (int i = 0; i < set.GetNumberOfTriangles(); i++)
            {
                float v0x, v0y, v0z;
                float v1x, v1y, v1z;
                float v2x, v2y, v2z;
                set.GetTriangleVertices(i,
                                        out v0x, out v0y, out v0z,
                                        out v1x, out v1y, out v1z,
                                        out v2x, out v2y, out v2z);
                int v0 = AddVertex(v0x, v0y, v0z);
                int v1 = AddVertex(v1x, v1y, v1z);
                int v2 = AddVertex(v2x, v2y, v2z);
                AddTriangle(v0, v1, v2);
            }
        }

        private class IndexArray : QuadArray<int>
        {
        }

        private class VertexArray : TrioArray<float>
        {
        }
    }


    public class TriangleMatrix
    {
        private readonly SparseFloatMatrix2D<List<int>> matrix;
        private int maxAtOne;
        private float resolution = 2.0f;

        public TriangleMatrix(TriangleCollection tc)
        {
            {
                DateTime pre = DateTime.Now;
                PathGraph.Log("Build hash  " + tc.GetNumberOfTriangles());
                matrix = new SparseFloatMatrix2D<List<int>>(resolution, tc.GetNumberOfTriangles());

                Vector vertex0;
                Vector vertex1;
                Vector vertex2;

                for (int i = 0; i < tc.GetNumberOfTriangles(); i++)
                {
                    tc.GetTriangleVertices(i,
                                           out vertex0.x, out vertex0.y, out vertex0.z,
                                           out vertex1.x, out vertex1.y, out vertex1.z,
                                           out vertex2.x, out vertex2.y, out vertex2.z);

                    float minx = Utils.min(vertex0.x, vertex1.x, vertex2.x);
                    float maxx = Utils.max(vertex0.x, vertex1.x, vertex2.x);
                    float miny = Utils.min(vertex0.y, vertex1.y, vertex2.y);
                    float maxy = Utils.max(vertex0.y, vertex1.y, vertex2.y);

                    Vector box_center;
                    Vector box_halfsize;
                    box_halfsize.x = resolution/2;
                    box_halfsize.y = resolution/2;
                    box_halfsize.z = 1E6f;

                    int startx = matrix.LocalToGrid(minx);
                    int endx = matrix.LocalToGrid(maxx);
                    int starty = matrix.LocalToGrid(miny);
                    int endy = matrix.LocalToGrid(maxy);

                    for (int x = startx; x <= endx; x++)
                        for (int y = starty; y <= endy; y++)
                        {
                            float grid_x = matrix.GridToLocal(x);
                            float grid_y = matrix.GridToLocal(y);
                            box_center.x = grid_x + resolution/2;
                            box_center.y = grid_y + resolution/2;
                            box_center.z = 0;
                            if (Utils.TestTriangleBoxIntersect(vertex0, vertex1, vertex2, box_center, box_halfsize))
                                AddTriangleAt(grid_x, grid_y, i);
                        }
                }
                DateTime post = DateTime.Now;
                TimeSpan ts = post.Subtract(pre);
                PathGraph.Log("done " + maxAtOne + " time " + ts);
            }
        }

        private void AddTriangleAt(float x, float y, int triangle)
        {
            List<int> l = matrix.Get(x, y);
            if (l == null)
            {
                l = new List<int>(8); // hmm
                l.Add(triangle);

                matrix.Set(x, y, l);
            }
            else
            {
                l.Add(triangle);
            }

            if (l.Count > maxAtOne)
                maxAtOne = l.Count;
        }

        public Set<int> GetAllCloseTo(float x, float y, float distance)
        {
            List<List<int>> close = matrix.GetAllInSquare(x - distance, y - distance, x + distance, y + distance);
            var all = new Set<int>();

            foreach (var l in close)
            {
                all.AddRange(l);
            }
            return all;
        }

        public ICollection<int> GetAllInSquare(float x0, float y0, float x1, float y1)
        {
            var all = new Set<int>();
            List<List<int>> close = matrix.GetAllInSquare(x0, y0, x1, y1);


            foreach (var l in close)
            {
                all.AddRange(l);
            }
            return all;
        }
    }


    /////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////


    public class TriangleOctree
    {
        private const int SplitSize = 64;

        private readonly Vector max;
        private readonly Vector min;
        private readonly TriangleCollection tc;
        public Node rootNode;


        public TriangleOctree(TriangleCollection tc)
        {
            PathGraph.Log("Build oct " + tc.GetNumberOfTriangles());
            this.tc = tc;
            tc.GetBBox(out min.x, out min.y, out min.z,
                       out max.x, out max.y, out max.z);
            rootNode = new Node(this, min, max);

            var tlist = new SimpleLinkedList();
            for (int i = 0; i < tc.GetNumberOfTriangles(); i++)
            {
                tlist.AddNew(i);
            }
            rootNode.Build(tlist, 0);
            PathGraph.Log("done");
        }

        public Set<int> FindTrianglesInBox(float min_x, float min_y, float min_z,
                                           float max_x, float max_y, float max_z)
        {
            var min = new Vector(min_x, min_y, min_z);
            var max = new Vector(max_x, max_y, max_z);
            var found = new Set<int>();
            rootNode.FindTrianglesInBox(min, max, found);
            return found;
        }

        public class Node
        {
            private readonly TriangleOctree tree;

            public Node[,,] children; // [2,2,2]
            public Vector max;
            public Vector mid;
            public Vector min;
            private Node parent;

            public int[] triangles;

            public Node(TriangleOctree tree,
                        Vector min,
                        Vector max)
            {
                this.tree = tree;
                this.min = min;
                this.max = max;
                mid.x = (min.x + max.x)/2;
                mid.y = (min.y + max.y)/2;
                mid.z = (min.z + max.z)/2;

                //triangles = new SimpleLinkedList();  // assume being a leaf node
            }

            public void Build(SimpleLinkedList triangles, int depth)
            {
                if (triangles.Count < SplitSize || depth >= 8)
                {
                    this.triangles = new int[triangles.Count];
                    SimpleLinkedList.Node rover = triangles.first;
                    int i = 0;
                    while (rover != null)
                    {
                        this.triangles[i] = rover.val;
                        rover = rover.next;
                        i++;
                    }
                    if (triangles.Count >= SplitSize)
                    {
                        //Vector size;
                        //Utils.sub(out size, max, min);
                        //PathGraph.Log("New leaf " + depth + " size: " + triangles.Count + " " + size);
                    }
                }
                else
                {
                    this.triangles = null;

                    var xl = new float[3] {min.x, mid.x, max.x};
                    var yl = new float[3] {min.y, mid.y, max.y};
                    var zl = new float[3] {min.z, mid.z, max.z};

                    var boxhalfsize = new Vector(
                        mid.x - min.x,
                        mid.y - min.y,
                        mid.z - min.z);


                    // allocate children
                    //SimpleLinkedList[, ,] childTris = new SimpleLinkedList[2, 2, 2];
                    children = new Node[2,2,2];

                    Vector vertex0;
                    Vector vertex1;
                    Vector vertex2;

                    //foreach (int triangle in triangles)
                    for (int x = 0; x < 2; x++)
                    {
                        for (int y = 0; y < 2; y++)
                        {
                            for (int z = 0; z < 2; z++)
                            {
                                SimpleLinkedList.Node rover = triangles.GetFirst();
                                var childTris = new SimpleLinkedList();

                                children[x, y, z] = new Node(tree,
                                                             new Vector(xl[x], yl[y], zl[z]),
                                                             new Vector(xl[x + 1], yl[y + 1], zl[z + 1]));
                                children[x, y, z].parent = this;
                                int c = 0;
                                while (rover != null)
                                {
                                    c++;
                                    SimpleLinkedList.Node next = rover.next;
                                    int triangle = rover.val;
                                    tree.tc.GetTriangleVertices(triangle,
                                                                out vertex0.x, out vertex0.y, out vertex0.z,
                                                                out vertex1.x, out vertex1.y, out vertex1.z,
                                                                out vertex2.x, out vertex2.y, out vertex2.z);

                                    if (Utils.TestTriangleBoxIntersect(vertex0, vertex1, vertex2,
                                                                       children[x, y, z].mid, boxhalfsize))
                                    {
                                        childTris.Steal(rover, triangles);
                                    }
                                    rover = next;
                                }
                                if (c == 0)
                                {
                                    children[x, y, z] = null; // drop that
                                }
                                else
                                {
                                    //PathGraph.Log(depth + " of " + c + " stole " + childTris.RealCount + "(" + childTris.Count + ")" + " left is " + triangles.RealCount + "(" + triangles.Count + ")"); 
                                    children[x, y, z].Build(childTris, depth + 1);
                                    triangles.StealAll(childTris);
                                }
                                /*if (depth == 0)
                                {
                                    PathGraph.Log("Post tris: " + triangles.Count);
                                    PathGraph.Log("count: " + c); 
                                }*/
                            }
                        }
                    }
                }
            }

            public void FindTrianglesInBox(Vector box_min, Vector box_max, Set<int> found)
            {
                if (triangles != null)
                {
                    found.AddRange(triangles);
                }
                else
                {
                    for (int x = 0; x < 2; x++)
                    {
                        for (int y = 0; y < 2; y++)
                        {
                            for (int z = 0; z < 2; z++)
                            {
                                Node child = children[x, y, z];
                                if (child != null)
                                {
                                    if (Utils.TestBoxBoxIntersect(box_min, box_max, child.min, child.max))
                                        child.FindTrianglesInBox(box_min, box_max, found);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////


    // Very primitive Doule linked list of integers
    // very fast to move nodes from one list to another
    public class SimpleLinkedList
    {
        public Node first;
        public Node last;
        private int nodes;

        public int Count
        {
            get { return nodes; }
        }

        public int RealCount
        {
            get
            {
                int i = 0;
                Node rover = first;
                while (rover != null)
                {
                    i++;
                    rover = rover.next;
                }
                return i;
            }
        }

        public Node GetFirst()
        {
            return first;
        }

        public void AddNew(int i)
        {
            var n = new Node(i);
            n.next = first;
            n.prev = null;
            if (first != null)
                first.prev = n;
            first = n;
            if (last == null)
                last = n;


            nodes++;
        }


        private void Error(string error)
        {
            PathGraph.Log(error);
        }

        public void Check()
        {
            if (first != null && first.prev != null)
                Error("First element must have prev == null");
            if (last != null && last.next != null)
                Error("Last element must have next == null");
            if (Count != RealCount)
                Error("Count != RealCount");
        }

        public void Steal(Node n, SimpleLinkedList from)
        {
            // unlink n from other list

            if (n == from.first)
            {
                // n was first
                from.first = n.next;
                if (from.first != null)
                    from.first.prev = null;
            }
            else
            {
                n.prev.next = n.next;
            }

            if (n == from.last)
            {
                // n was last
                from.last = n.prev;
                if (from.last != null)
                    from.last.next = null;
            }
            else
            {
                n.next.prev = n.prev;
            }

            from.nodes--;

            if (first != null)
                first.prev = n;
            n.next = first;
            n.prev = null;
            first = n;
            if (last == null)
                last = n;

            nodes++;
        }

        public void StealAll(SimpleLinkedList other)
        {
            // put them first
            //Check();
            // other.Check(); 
            if (other.first == null)
                return; // other empty
            if (first == null) // me empty
            {
                first = other.first;
                last = other.last;
            }
            else
            {
                other.last.next = first;
                first.prev = other.last;
                first = other.first;

                other.last = null;
                other.first = null;
            }

            nodes += other.nodes;
            other.nodes = 0;
            //Check();
            //other.Check();
        }

        public class Node
        {
            public Node next;
            public Node prev;
            public int val;

            public Node(int val)
            {
                this.val = val;
            }

            public bool IsLast()
            {
                return next == null;
            }
        }
    }


    /////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////

    /// <summary>
    ///     Quadtree (splits on x and y)
    /// </summary>
    public class TriangleQuadtree
    {
        private const int SplitSize = 64;

        private readonly Vector max;
        private readonly Vector min;
        private readonly TriangleCollection tc;
        public Node rootNode;


        public TriangleQuadtree(TriangleCollection tc)
        {
            PathGraph.Log("Build oct " + tc.GetNumberOfTriangles());
            this.tc = tc;
            tc.GetBBox(out min.x, out min.y, out min.z,
                       out max.x, out max.y, out max.z);
            rootNode = new Node(this, min, max);

            var tlist = new SimpleLinkedList();
            for (int i = 0; i < tc.GetNumberOfTriangles(); i++)
            {
                tlist.AddNew(i);
            }
            rootNode.Build(tlist, 0);
            PathGraph.Log("done");
        }

        public class Node
        {
            private readonly TriangleQuadtree tree;

            public Node[,] children; // [2,2]
            public Vector max;
            public Vector mid;
            public Vector min;
            private Node parent;

            public int[] triangles;

            public Node(TriangleQuadtree tree,
                        Vector min,
                        Vector max)
            {
                this.tree = tree;
                this.min = min;
                this.max = max;
                mid.x = (min.x + max.x)/2;
                mid.y = (min.y + max.y)/2;
                mid.z = 0;
            }

            public void Build(SimpleLinkedList triangles, int depth)
            {
                if (triangles.Count < SplitSize || depth >= 10)
                {
                    this.triangles = new int[triangles.Count];
                    SimpleLinkedList.Node rover = triangles.first;
                    int i = 0;
                    while (rover != null)
                    {
                        this.triangles[i] = rover.val;
                        rover = rover.next;
                        i++;
                    }
                    if (triangles.Count >= SplitSize)
                    {
                        Vector size;
                        Utils.sub(out size, ref max, ref min);
                        PathGraph.Log("New leaf " + depth + " size: " + triangles.Count + " " + size);
                    }
                }
                else
                {
                    this.triangles = null;

                    var xl = new float[3] {min.x, mid.x, max.x};
                    var yl = new float[3] {min.y, mid.y, max.y};

                    var boxhalfsize = new Vector(
                        mid.x - min.x,
                        mid.y - min.y,
                        1E10f);


                    children = new Node[2,2];

                    Vector vertex0;
                    Vector vertex1;
                    Vector vertex2;

                    // if (depth <= 3)
                    //     PathGraph.Log(depth + " Pre tris: " + triangles.Count);

                    int ugh = 0;
                    //foreach (int triangle in triangles)
                    for (int x = 0; x < 2; x++)
                    {
                        for (int y = 0; y < 2; y++)
                        {
                            SimpleLinkedList.Node rover = triangles.GetFirst();
                            var childTris = new SimpleLinkedList();

                            children[x, y] = new Node(tree,
                                                      new Vector(xl[x], yl[y], 0),
                                                      new Vector(xl[x + 1], yl[y + 1], 0));
                            children[x, y].parent = this;
                            int c = 0;
                            while (rover != null)
                            {
                                c++;
                                SimpleLinkedList.Node next = rover.next;
                                int triangle = rover.val;
                                tree.tc.GetTriangleVertices(triangle,
                                                            out vertex0.x, out vertex0.y, out vertex0.z,
                                                            out vertex1.x, out vertex1.y, out vertex1.z,
                                                            out vertex2.x, out vertex2.y, out vertex2.z);

                                if (Utils.TestTriangleBoxIntersect(vertex0, vertex1, vertex2,
                                                                   children[x, y].mid, boxhalfsize))
                                {
                                    childTris.Steal(rover, triangles);

                                    ugh++;
                                }
                                rover = next;
                            }
                            if (c == 0)
                            {
                                children[x, y] = null; // drop that
                            }
                            else
                            {
                                //PathGraph.Log(depth + " of " + c + " stole " + childTris.RealCount + "(" + childTris.Count + ")" + " left is " + triangles.RealCount + "(" + triangles.Count + ")"); 
                                children[x, y].Build(childTris, depth + 1);
                                triangles.StealAll(childTris);
                            }
                            /*if (depth == 0)
                            {
                                PathGraph.Log("Post tris: " + triangles.Count);
                                PathGraph.Log("count: " + c); 
                            }*/
                        }
                    }
                }
            }
        }
    }
}