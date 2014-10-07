using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using IceFlake.Runtime;
using MeshGenLib;
using IceFlake.DirectX;

namespace IceFlake.Client
{
    public class Pather
    {
        private static readonly float TileSize = (533f + (1f / 3f));
        private static readonly float[] Origin = new[] { 32f * TileSize, 32f * TileSize, 0f };

        public Pather(string continent)
        {
            Continent = continent;
            DetourMesh = new NavMesh(continent, true);
        }

        public string Continent { get; private set; }
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
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Meshes\{0}_{1}_{2}.mesh", this.Continent, x, y));
        }

        public void GetTileByLocation(Vector3 loc, out int x, out int y)
        {
            float[] input = loc.ToFloatArray();
            float fx, fy;
            GetTileByLocation(input, out fx, out fy);
            x = (int)Math.Floor(fx);
            y = (int)Math.Floor(fy);
        }

        public static void GetTileByLocation(float[] loc, out float x, out float y)
        {
            x = (loc[0] - Origin[0]) / TileSize;
            y = (loc[1] - Origin[0]) / TileSize;
        }

        public void LoadAllTiles()
        {
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    if (!File.Exists(GetTilePath(x, y)))
                        continue;
                    DetourMesh.LoadTile(x, y);
                }
            }
        }

        public bool LoadAppropriateTiles(Vector3 start, Vector3 end)
        {
            const int extent = 3;

            bool failed = false;
            int tx, ty;
            // Start
            GetTileByLocation(start, out tx, out ty);
            for (int y = ty - extent; y <= ty + extent; y++)
            {
                for (int x = tx - extent; x <= tx + extent; x++)
                {
                    if (!DetourMesh.LoadTile(x, y))
                        failed = true;
                    Log.WriteLine("{0},{1}: {2}", x, y, failed);
                }
            }

            // End
            GetTileByLocation(end, out tx, out ty);
            for (int y = ty - extent; y <= ty + extent; y++)
            {
                for (int x = tx - extent; x <= tx + extent; x++)
                {
                    if (!DetourMesh.LoadTile(x, y))
                        failed = true;
                    Log.WriteLine("{0},{1}: {2}", x, y, failed);
                }
            }

            return !failed;
        }

        #endregion
    }
}
