using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.Runtime;
using IceFlake.Client.Objects;

namespace IceFlake.Client
{
    public class Movement
    {
        public Movement()
        {
            Pather = new Pather(World.CurrentMap);
        }

        private Pather Pather;

        public bool PathTo(Location pos)
        {
            var path = Pather.Search(Manager.LocalPlayer.Location, pos);
            if (path == null)
            {
                Log.WriteLine("Could not path to {0}", pos);
                return false;
            }
            FollowPath(path);
            return true;
        }

        public void FollowPath(List<Location> Path, Action<int, int> AfterSpot = null, double tolerance = 3D, double face_tolerance = 0.5D)
        {
            foreach (var spot in Path)
                Log.WriteLine("\t[{0}, {1}, {2}]", spot.X, spot.Y, spot.Z);
        }

        public bool MoveTo(Location loc, double tolerance = 5)
        {
            Manager.LocalPlayer.ClickToMove(loc);
            return Manager.LocalPlayer.Location.DistanceTo(loc) <= tolerance;
        }
    }
}
