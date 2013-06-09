using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.Runtime;
using IceFlake.Client.Objects;
using IceFlake.DirectX;

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

        private Queue<Location> generatedPath;
        public void FollowPath(List<Location> Path, Action<int, int> AfterSpot = null, double tolerance = 3D, double face_tolerance = 0.5D)
        {
            generatedPath = new Queue<Location>();
            foreach (var spot in Path)
                generatedPath.Enqueue(spot);
        }

        private DateTime SleepTime;
        private Location currentLocation;
        [EndSceneHandler]
        public void Direct3D_EndScene()
        {
            if (generatedPath == null)
                return;

            if (generatedPath.Count == 0)
            {
                generatedPath = null;
                return;
            }

            if (SleepTime >= DateTime.Now)
                return;

            while (generatedPath.Count > 0 && (currentLocation == default(Location) || Manager.LocalPlayer.Location.DistanceTo(currentLocation) < 3f))
                currentLocation = generatedPath.Dequeue();

            MoveTo(currentLocation, 3f);

            SleepTime = DateTime.Now + TimeSpan.FromMilliseconds(100);
        }

        public bool MoveTo(Location loc, double tolerance = 5)
        {
            Manager.LocalPlayer.ClickToMove(loc);
            return Manager.LocalPlayer.Location.DistanceTo(loc) <= tolerance;
        }
    }
}
