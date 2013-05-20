/*
  This file is part of ppather.

    PPather is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    PPather is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with ppather.  If not, see <http://www.gnu.org/licenses/>.

*/
using System;
using System.Collections.Generic;
namespace PatherPath.Graph
{
    [Flags]
    public enum SpotFlags
    {
        FLAG_VISITED = 0x0001,
        FLAG_BLOCKED = 0x0002,
        FLAG_MPQ_MAPPED = 0x0004,
        FLAG_WATER = 0x0008,
        FLAG_INDOORS = 0x0010,
        FLAG_CLOSETOMODEL = 0x0020,
    }
    public class Spot : Location
    {
        public const float Z_RESOLUTION = 2.0f; // Z spots max this close

        public SpotFlags Flags;
        public SpotPaths paths = new SpotPaths();

        public GraphChunk chunk = null;
        public Spot next;  // list on same x,y, used by chunk

        public int searchID = 0;
        public Spot traceBack; // Used by search
        public double traceBackDistance = 0; // Used by search
        public double score; // Used by search
        public bool closed, scoreSet;

        public Spot(float x, float y, float z)
            : base(x, y, z)
        {

        }
        public Spot(Location l)
            : base(l)
        {

        }

        public bool IsBlocked()
        {
            return Flags.HasFlag(SpotFlags.FLAG_BLOCKED);
        }
        public bool IsInWater()
        {
            return Flags.HasFlag(SpotFlags.FLAG_WATER);
        }

        public bool IsCloseZ(float z)
        {
            float dz = z - this.Z;
            if (dz > Z_RESOLUTION)
                return false;
            if (dz < -Z_RESOLUTION)
                return false;
            return true;
        }

        public bool GetPath(int i, out float x, out float y, out float z)
        {
            x = 0F;
            y = 0F;
            z = 0F;

            var Loc = paths[i];
            if (Loc == null) return false;

            x = Loc.X;
            y = Loc.Y;
            z = Loc.Z;
            return true;
        }

        public Spot GetToSpot(PathGraph pg, int i)
        {
            float x, y, z;
            GetPath(i, out x, out y, out z);
            return pg.GetSpot(x, y, z);
        }

        public List<Spot> GetPathsToSpots(PathGraph pg)
        {
            List<Spot> list = new List<Spot>(paths.Count);
            for (int i = 0; i < paths.Count; i++)
            {
                Spot spot = GetToSpot(pg, i);
                if (spot != null)
                    list.Add(spot);
            }
            return list;
        }

        public bool HasPathTo(PathGraph pg, Spot s)
        {
            for (int i = 0; i < paths.Count; i++)
            {
                if (GetToSpot(pg, i) == s)
                    return true;
            }
            return false;
        }

        public bool HasPathTo(Location l)
        {
            return HasPathTo(l.X, l.Y, l.Z);
        }

        public bool HasPathTo(float x, float y, float z)
        {
            return paths.Contains(new Location(x, y, z));
        }

        public void AddPathTo(Spot s)
        {
            AddPathTo(s.X, s.Y, s.Z);
        }

        public void AddPathTo(Location l)
        {
            AddPathTo(l.X, l.Y, l.Z);
        }

        public void AddPathTo(float x, float y, float z)
        {
            if (HasPathTo(x, y, z)) return;
            paths.Add(new Location(x, y, z));
            if (chunk != null)
                chunk.modified = true;
        }

        public void RemovePathTo(Location l)
        {
            RemovePathTo(l.X, l.Y, l.Z);
        }

        public void RemovePathTo(float x, float y, float z)
        {
            var testLoc = new Location(x, y, z);
            paths.Remove(testLoc);
        }

        // search stuff

        public bool SetSearchID(int id)
        {
            if (searchID != id)
            {
                closed = false;
                scoreSet = false;
                searchID = id;
                return true;
            }
            return false;
        }

        public bool SearchIsClosed(int id)
        {
            if (id == searchID)
                return closed;
            return false;
        }

        public void SearchClose(int id)
        {
            SetSearchID(id);
            closed = true;
        }

        public bool SearchScoreIsSet(int id)
        {
            if (id == searchID)
            {
                return scoreSet;
            }
            return false;
        }

        public double SearchScoreGet(int id)
        {
            if (id == searchID)
            {
                return score;
            }
            return float.MaxValue;
        }

        public void SearchScoreSet(int id, double score)
        {
            SetSearchID(id);
            this.score = score;
            scoreSet = true;
        }
    }
}