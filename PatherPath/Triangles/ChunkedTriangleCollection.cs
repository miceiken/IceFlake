/*
 *  Part of PPather
 *  Copyright Pontus Borg 2008
 * 
 */

using System;
using System.Collections.Generic;
using Wmo;
namespace WowTriangles
{
    /// <summary>
    /// A chunked collection of triangles
    /// </summary>
    public class ChunkedTriangleCollection : IDisposable
    {
        private bool UseOctree = false;
        private bool UseMatrix = true;

        public const int TriangleFlagDeepWater = 1;
        public const int TriangleFlagObject = 2;
        public const int TriangleFlagModel = 4;

        List<TriangleSupplier> suppliers = new List<TriangleSupplier>();
        SparseMatrix2D<TriangleCollection> chunks;

        public List<TriangleCollection> loadedChunks = new List<TriangleCollection>();
        int NOW = 0;
        int maxCached = 1000;

        private bool m_Updated = false;
        public bool Updated
        {
            get
            {
                return m_Updated;
            }
            set
            {
                m_Updated = false;
            }
        }

        public ChunkedTriangleCollection(float chunkSize)
        {
            // this.chunkSize = chunkSize;
            chunks = new SparseMatrix2D<TriangleCollection>(8);
        }

        public void Dispose()
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
                Console.WriteLine("Evict chunk at " + toEvict.base_x + " " + toEvict.base_y);
                m_Updated = true;
            }

        }

        public void AddSupplier(TriangleSupplier supplier)
        {
            suppliers.Add(supplier);
        }

        public void GetGridStartAt(float x, float y, out int grid_x, out int grid_y)
        {
            x = Wmo.ChunkReader.ZEROPOINT - x;
            grid_x = (int)(x / ChunkReader.TILESIZE);
            y = Wmo.ChunkReader.ZEROPOINT - y;
            grid_y = (int)(y / ChunkReader.TILESIZE);

        }
        private void GetGridLimits(int grid_x, int grid_y,
                                    out float min_x, out float min_y,
                                    out float max_x, out float max_y)
        {
            max_x = ChunkReader.ZEROPOINT - (float)(grid_x) * ChunkReader.TILESIZE;
            min_x = max_x - ChunkReader.TILESIZE;
            max_y = ChunkReader.ZEROPOINT - (float)(grid_y) * ChunkReader.TILESIZE;
            min_y = max_y - ChunkReader.TILESIZE;
        }

        public float GetMaxZ(float x, float y)
        {
            TriangleCollection tc = GetChunkAt(x, y);
            ICollection<int> ts;

            ts = null;
            if (UseMatrix)
            {
                TriangleMatrix tm = tc.GetTriangleMatrix();
                ts = tm.GetAllCloseTo(x, y, 0);
            }

            float best_z = float.MinValue;

            foreach (int t in ts)
            {
                Vector vertex0;
                Vector vertex1;
                Vector vertex2;
                int t_flags;

                tc.GetTriangleVertices(t,
                        out vertex0.x, out vertex0.y, out vertex0.z,
                        out vertex1.x, out vertex1.y, out vertex1.z,
                        out vertex2.x, out vertex2.y, out vertex2.z, out t_flags);

                if (vertex0.z > best_z) { best_z = vertex0.z; }
                if (vertex1.z > best_z) { best_z = vertex1.z; }
                if (vertex2.z > best_z) { best_z = vertex2.z; }

            }
            return best_z;
        }

        private void LoadChunkAt(float x, float y)
        {
            int grid_x, grid_y;
            GetGridStartAt(x, y, out grid_x, out grid_y);

            if (chunks.IsSet(grid_x, grid_y))
                return;
            EvictIfNeeded();
            TriangleCollection tc = new TriangleCollection();

            float min_x, max_x, min_y, max_y;
            GetGridLimits(grid_x, grid_y, out  min_x, out  min_y, out  max_x, out  max_y);

            Console.WriteLine("Got asked for triangles at " + x + ", " + y);
            Console.WriteLine("Need triangles grid (" + min_x + " , " + min_y + ") - (" + max_x + ", " + max_y);

            tc.SetLimits(min_x - 1, min_y - 1, -1E30f, max_x + 1, max_y + 1, 1E30f);
            foreach (TriangleSupplier s in suppliers)
            {
                s.GetTriangles(tc, min_x, min_y, max_x, max_y);
            }
            tc.CompactVertices();
            tc.ClearVertexMatrix(); // not needed anymore
            tc.base_x = grid_x;
            tc.base_y = grid_y;
            Console.WriteLine("  it got " + tc.GetNumberOfTriangles() + " triangles and " + tc.GetNumberOfVertices() + " vertices");


            loadedChunks.Add(tc);
            chunks.Set(grid_x, grid_y, tc);


            Console.WriteLine("Got triangles grid (" + tc.min_x + " , " + tc.min_y + ") - (" + tc.max_x + ", " + tc.max_y);
            m_Updated = true;
        }

        public TriangleCollection LastTriangleCollection = null;

        public TriangleCollection GetChunkAt(float x, float y)
        {
            LoadChunkAt(x, y);
            int grid_x, grid_y;
            GetGridStartAt(x, y, out grid_x, out grid_y);
            TriangleCollection tc = chunks.Get(grid_x, grid_y);
            tc.LRU = NOW++;
            LastTriangleCollection = tc;
            return tc;
        }


        public bool IsSpotBlocked(float x, float y, float z, float toonHeight, float toonSize)
        {
            TriangleCollection tc = GetChunkAt(x, y);

            List<int> ts = null, tst, tsm;
            if (UseOctree)
            {
                TriangleOctree ot = tc.GetOctree();
                tst = ts = ot.FindTrianglesInBox(
                    x - toonSize,
                    y - toonSize,
                    z + toonHeight - toonSize * 2,
                    x + toonSize,
                    y + toonSize,
                    z + toonHeight);
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



                s1.x = (vertex0.x + vertex1.x + vertex2.x) / 3;
                s1.y = (vertex0.y + vertex1.y + vertex2.y) / 3;
                s1.z = (vertex0.z + vertex1.z + vertex2.z) / 3 - 0.1f;
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
                                  float toonHeight, float toonSize)
        {
            TriangleCollection tc = GetChunkAt(x0, y0);

            float dx = x0 - x1;
            float dy = y0 - y1;
            float dz = z0 - z1;
            float stepLength = (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
            // 1: check steepness

            // TODO

            // 2: check is there is a big step 

            float mid_x = (x0 + x1) / 2.0f;
            float mid_y = (y0 + y1) / 2.0f;
            float mid_z = (z0 + z1) / 2.0f;
            float mid_z_hit = 0;
            float mid_dz = Math.Abs(stepLength);
            //if (mid_dz < 1.0f) mid_dz = 1.0f;
            int mid_flags = 0;
            if (FindStandableAt(mid_x, mid_y, mid_z - mid_dz, mid_z + mid_dz, out mid_z_hit, out mid_flags, toonHeight, toonSize))
            {
                float dz0 = Math.Abs(z0 - mid_z_hit);
                float dz1 = Math.Abs(z1 - mid_z_hit);

                // Console.WriteLine("z0 " + z0 + " z1 " + z1 + " dz0 " + dz0+ " dz1 " + dz1 );
                if (dz0 > stepLength / 2.0 && dz0 > 1.0)
                    return true; // too steep

                if (dz1 > stepLength / 2.0 && dz1 > 1.0)
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
            {
                //for(int t = 0 ; t<tc.GetNumberOfTriangles(); t++)
                Vector vertex0;
                Vector vertex1;
                Vector vertex2;
                Vector intersect;
                int t_flags;

                tc.GetTriangleVertices(t,
                        out vertex0.x, out vertex0.y, out vertex0.z,
                        out vertex1.x, out vertex1.y, out vertex1.z,
                        out vertex2.x, out vertex2.y, out vertex2.z, out t_flags);

                if ((Utils.SegmentTriangleIntersect(from, to_up, vertex0, vertex1, vertex2, out intersect) || Utils.SegmentTriangleIntersect(from_up, to, vertex0, vertex1, vertex2, out intersect)) && (t_flags & TriangleFlagDeepWater) == 0)
                {
                    //Console.WriteLine("Collided at " + intersect);
                    return true;
                    // blocked!
                }
            }
            return false;
        }

        public bool FindStandableAt2(float x, float y, float min_z, float max_z, out float z0, out int flags, float toonHeight, float toonSize, bool IgnoreGradient)
        {
            TriangleCollection tc = GetChunkAt(x, y);
            ICollection<int> ts, tsm, tst;
            float minCliffD = 0.5f, gap = 0.2F;

            ts = null;
            if (UseOctree)
            {
                TriangleOctree ot = tc.GetOctree();
                tst = ts = ot.FindTrianglesInBox(x - minCliffD, y - minCliffD, min_z, x + minCliffD, y + minCliffD, max_z);
            }
            if (UseMatrix)
            {
                TriangleMatrix tm = tc.GetTriangleMatrix();
                tsm = ts = tm.GetAllCloseTo(x, y, 2.0f);
            }

            Vector s0, s1, s2, s3, s4, s5, s6, s7, s8, s9;
            s0.x = x; s0.y = y; s0.z = min_z;
            s1.x = x; s1.y = y; s1.z = max_z;

            s2.x = x + gap; s2.y = y; s2.z = min_z;
            s3.x = x + gap; s3.y = y; s3.z = max_z;

            s4.x = x - gap; s4.y = y; s4.z = min_z;
            s5.x = x - gap; s5.y = y; s5.z = max_z;

            s6.x = x; s6.y = y + gap; s6.z = min_z;
            s7.x = x; s7.y = y + gap; s7.z = max_z;

            s8.x = x; s8.y = y - gap; s8.z = min_z;
            s9.x = x; s9.y = y - gap; s9.z = max_z;

            float best_z = float.MaxValue;
            int best_flags = 0;
            bool found = false;

            //System.Diagnostics.Debug.WriteLine(string.Format("x: {0} y: {1} min_z: {2} max_z: {3}", x, y, min_z, max_z));
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

                // System.Diagnostics.Debug.WriteLine(string.Format("x: {0},{1},{2} y: {3},{4},{5} z: {6},{7},{8}",vertex0.x,vertex1.x,vertex2.x,vertex0.y,vertex1.y,vertex2.y,vertex0.z,vertex1.z,vertex2.z));

                Vector normal;
                Utils.GetTriangleNormal(vertex0, vertex1, vertex2, out normal);
                float angle_z = (float)Math.Sin(40.0 / 360.0 * Math.PI * 2); //
                if (Utils.abs(normal.z) > angle_z || IgnoreGradient)
                {

                    bool hasIntersected = false;

                    hasIntersected = Utils.SegmentTriangleIntersect(s0, s1, vertex0, vertex1, vertex2, out intersect);
                    if (!hasIntersected) hasIntersected = Utils.SegmentTriangleIntersect(s2, s3, vertex0, vertex1, vertex2, out intersect);
                    if (!hasIntersected) hasIntersected = Utils.SegmentTriangleIntersect(s4, s5, vertex0, vertex1, vertex2, out intersect);
                    if (!hasIntersected) hasIntersected = Utils.SegmentTriangleIntersect(s6, s7, vertex0, vertex1, vertex2, out intersect);
                    if (!hasIntersected) hasIntersected = Utils.SegmentTriangleIntersect(s8, s9, vertex0, vertex1, vertex2, out intersect);

                    if (hasIntersected)
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
                bool[] nearCliff = { true, true, true, true };

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

                    float[] dx = { minCliffD, -minCliffD, 0, 0 };
                    float[] dy = { 0, 0, minCliffD, -minCliffD };
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

        public bool FindStandableAt(float x, float y, float min_z, float max_z, out float z0, out int flags, float toonHeight, float toonSize, bool IgnoreGradient = false)
        {
            return FindStandableAt1(x, y, min_z, max_z, out z0, out flags, toonHeight, toonSize, IgnoreGradient);
        }
        public bool IsInWater(float x, float y, float min_z, float max_z)
        {
            TriangleCollection tc = GetChunkAt(x, y);
            ICollection<int> ts, tsm, tst;
            float minCliffD = 0.5f;

            ts = null;
            if (UseOctree)
            {
                TriangleOctree ot = tc.GetOctree();
                tst = ts = ot.FindTrianglesInBox(x - minCliffD, y - minCliffD, min_z, x + minCliffD, y + minCliffD, max_z);
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

                if (Utils.SegmentTriangleIntersect(s0, s1, vertex0, vertex1, vertex2, out intersect))
                {
                    if ((t_flags & TriangleFlagDeepWater) != 0)
                    {
                        return true;
                    }
                }

            }
            return false;
        }
        public int GradiantScore(float x, float y, float z, float range)
        {
            TriangleCollection tc = GetChunkAt(x, y);
            TriangleMatrix tm = tc.GetTriangleMatrix();

            float maxZ = float.MinValue;
            float minZ = float.MaxValue;

            foreach (int t in tm.GetAllCloseTo(x, y, range))
            {
                Vector vertex0;
                Vector vertex1;
                Vector vertex2;
                int t_flags;

                tc.GetTriangleVertices(t,
                        out vertex0.x, out vertex0.y, out vertex0.z,
                        out vertex1.x, out vertex1.y, out vertex1.z,
                        out vertex2.x, out vertex2.y, out vertex2.z, out t_flags);

                if (t_flags == 0)
                {
                    if (vertex0.z > maxZ) { maxZ = vertex0.z; }
                    if (vertex2.z > maxZ) { maxZ = vertex1.z; }
                    if (vertex1.z > maxZ) { maxZ = vertex2.z; }

                    if (vertex0.z < minZ) { minZ = vertex0.z; }
                    if (vertex2.z < minZ) { minZ = vertex1.z; }
                    if (vertex1.z < minZ) { minZ = vertex2.z; }
                }
            }
            int g = (int)((maxZ - minZ));
            if (g > 10)
            {
                g = 10;
            }
            return g;
        }

        public bool IsCloseToModel(float x, float y, float z, float range)
        {
            TriangleCollection tc = GetChunkAt(x, y);
            TriangleMatrix tm = tc.GetTriangleMatrix();

            foreach (int t in tm.GetAllCloseTo(x, y, range))
            {
                int v0, v1, v2;
                int t_flags;
                tc.GetTriangle(t, out v0, out v1, out v2, out t_flags);
                if ((t_flags & TriangleFlagObject) != 0) { return true; }
                if ((t_flags & TriangleFlagModel) != 0) { return true; }
            }
            return false;
        }

        public bool LineOfSightExists(PatherPath.Graph.Spot a, PatherPath.Graph.Spot b)
        {
            TriangleCollection tc = GetChunkAt(a.X, a.Y);
            ICollection<int> ts;

            ts = null;

            if (UseMatrix)
            {
                TriangleMatrix tm = tc.GetTriangleMatrix();
                ts = tm.GetAllCloseTo(a.X, a.Y, 1.0f);
            }

            Vector s0, s1;
            s0.x = a.X;
            s0.y = a.Y;
            s0.z = a.Z + 1;
            s1.x = b.X;
            s1.y = b.Y;
            s1.z = b.Z + 1;

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

                if (Utils.SegmentTriangleIntersect(s0, s1, vertex0, vertex1, vertex2, out intersect))
                {
                    return false;
                }
            }
            return true;
        }

        public bool FindStandableAt1(float x, float y, float min_z, float max_z, out float z0, out int flags, float toonHeight, float toonSize, bool IgnoreGradient)
        {
            TriangleCollection tc = GetChunkAt(x, y);
            ICollection<int> ts = null, tsm, tst;
            float minCliffD = 0.5f, best_z = -float.MaxValue;
            int best_flags = 0;
            bool found = false;

            if (UseOctree)
            {
                TriangleOctree ot = tc.GetOctree();
                tst = ts = ot.FindTrianglesInBox(x - minCliffD, y - minCliffD, min_z, x + minCliffD, y + minCliffD, max_z);
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

            foreach (int t in ts)
            {
                Vector vertex0, vertex1, vertex2, intersect;
                int t_flags;

                tc.GetTriangleVertices(t,
                        out vertex0.x, out vertex0.y, out vertex0.z,
                        out vertex1.x, out vertex1.y, out vertex1.z,
                        out vertex2.x, out vertex2.y, out vertex2.z, out t_flags);

                Vector normal;
                Utils.GetTriangleNormal(vertex0, vertex1, vertex2, out normal);
                float angle_z = (float)Math.Sin(45.0 / 360.0 * Math.PI * 2); //
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
                bool[] nearCliff = { true, true, true, true };

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

                    float[] dx = { minCliffD, -minCliffD, 0, 0 };
                    float[] dy = { 0, 0, minCliffD, -minCliffD };
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
            //if(!found)
            //found = FindStandableAt2(x, y, min_z, max_z, out z0, out flags, toonHeight, toonSize, IgnoreGradient);
            return found;
        }
    }




}
