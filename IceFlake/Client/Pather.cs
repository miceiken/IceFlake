using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using IceFlake.Runtime;
using SlimDX;
using RecastManaged;
using DetourManaged;

namespace IceFlake.Client
{
    public class Pather
    {
        private static readonly float TileSize = (533f + (1f / 3f));
        private static readonly float[] Origin = new[] { 32f * TileSize, 32f * TileSize, 0f };

        public Pather(string continent)
        {
            Continent = continent;

            DetourMesh = new NavMesh();
            DetourMesh.Init(Origin, TileSize, TileSize, 2048, 16384, 4194304);
        }

        public string Continent { get; private set; }
        public QueryFilter Filter { get; private set; }
        public NavMesh DetourMesh { get; private set; }

        #region Memory Management

        public int MemoryPressure { get; private set; }

        private void AddMemoryPressure(int bytes)
        {
            GC.AddMemoryPressure(bytes);
            MemoryPressure += bytes;
        }

        #endregion

        #region NavMesh Helpers

        public string GetTilePath(int x, int y)
        {
            return string.Format(@"Meshes\{0}_{1}_{2}.mesh", this.Continent, x, y);
        }

        public void GetTileByLocation(Vector3 loc, out int x, out int y)
        {
            var input = loc.ToRecast().ToFloatArray();
            float fx, fy;
            GetTileByLocation(input, out fx, out fy);
            x = (int)Math.Floor(fx);
            y = (int)Math.Floor(fy);
        }

        public static void GetTileByLocation(float[] loc, out float x, out float y)
        {
            x = (loc[0] - Origin[0]) / TileSize;
            y = (loc[1] - Origin[1]) / TileSize;
        }

        public void LoadAllTiles()
        {
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    if (!File.Exists(GetTilePath(x, y)))
                        continue;

                    LoadTile(x, y);
                }
            }
        }

        public void LoadAround(Vector3 loc, int extent)
        {
            int tx, ty;
            GetTileByLocation(loc, out tx, out ty);
            for (int y = ty - extent; y <= ty + extent; y++)
            {
                for (int x = tx - extent; x <= tx + extent; x++)
                {
                    LoadTile(x, y);
                }
            }
        }

        public bool LoadTile(int x, int y, byte[] data)
        {
            if (!DetourMesh.AddTileAt(x, y, data))
                return false;
            AddMemoryPressure(data.Length);
            //HandleConnections(tile);
            return true;
        }

        public bool LoadTile(int x, int y)
        {
            if (DetourMesh.GetTileAt(x, y) != null)
                return true;
            var path = GetTilePath(x, y);
            if (!File.Exists(path))
                return false;
            var data = File.ReadAllBytes(path);
            return LoadTile(x, y, data);
        }

        public void RemoveTile(int x, int y, out byte[] tileData)
        {
            DetourMesh.RemoveTileAt(x, y, out tileData);
        }

        public void RemoveTile(int x, int y)
        {
            DetourMesh.RemoveTileAt(x, y);
        }

        #endregion

        #region FindPath

        public IEnumerable<Vector3> FindPath(Location start, Location end, bool hardFail)
        {
            return FindPath(start.ToVector3(), end.ToVector3(), hardFail);
        }

        public IEnumerable<Vector3> FindPath(Vector3 start, Vector3 end, bool hardFail)
        {
            LoadAllTiles();
            return FindPath(start.ToFloatArray(), end.ToFloatArray(), hardFail); // .ToRecast() ?
        }

        public IEnumerable<Vector3> FindPath(float[] start, float[] end, bool hardFail)
        {
            return FindPath(start, end, QueryFilter.Default, hardFail);
        }

        public IEnumerable<Vector3> FindPath(float[] start, float[] end, QueryFilter filter, bool hardFail)
        {
            float[] extents = new[] { 0.5f, 0.5f, 0.5f };
            PolygonReference startRef, endRef;


            float[] transformedStart = new[] { start[0], start[1], start[2] };
            float[] transformedEnd = new[] { end[0], end[1], end[2] };

            RecastManaged.Helper.Transform(ref transformedStart);
            RecastManaged.Helper.Transform(ref transformedEnd);

            while ((startRef = DetourMesh.FindNearestPolygon(transformedStart, extents, filter)).PolyIndex == 0)
            {
                if (extents[0] > 100.0f)
                    throw new Exception("Extents got too huge");

                extents[0] += 0.5f;
                extents[1] += 0.5f;
                extents[2] += 0.5f;
            }

            extents = new[] { 0.5f, 0.5f, 0.5f };
            while ((endRef = DetourMesh.FindNearestPolygon(transformedEnd, extents, filter)).PolyIndex == 0)
            {
                if (extents[0] > 100.0f)
                    throw new Exception("Extents got too huge");

                extents[0] += 0.5f;
                extents[1] += 0.5f;
                extents[2] += 0.5f;
            }

            var path = DetourMesh.FindPath(startRef, endRef, transformedStart, transformedEnd, filter);

            if (path.Length <= 0)
                return null;

            // if the last poly in the path is not the end poly, a path was not found
            if (hardFail && path[path.Length - 1].PolyIndex != endRef.PolyIndex)
                return null;

            StraightPathFlags[] flags;
            PolygonReference[] straightPathRefs;
            var straightPath = DetourMesh.FindStraightPath(transformedStart, transformedEnd, path, out flags, out straightPathRefs);

            RecastManaged.Helper.InverseTransform(ref straightPath);

            List<Vector3> ret = new List<Vector3>(straightPath.Length / 3);
            for (int i = 0; i < straightPath.Length / 3; i++)
                ret.Add(new Vector3(straightPath[i * 3 + 0], straightPath[i * 3 + 1], straightPath[i * 3 + 2]));

            return ret;
        }

        #endregion
    }
}
