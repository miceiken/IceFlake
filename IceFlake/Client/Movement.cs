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
            if (!Manager.ObjectManager.IsInGame)
                return;

            CurrentMap = World.CurrentMap;
            PatherInstance = new Pather(CurrentMap);
        }

        private string CurrentMap
        {
            get;
            set;
        }

        private Pather PatherInstance
        {
            get;
            set;
        }

        private DateTime SleepTime
        {
            get;
            set;
        }

        public Location Destination
        {
            get;
            private set;
        }

        public Location CurrentLocation
        {
            get;
            private set;
        }

        public bool PathTo(Location pos)
        {
            if (!Manager.ObjectManager.IsInGame)
                return false;

            if (CurrentMap != World.CurrentMap)
            {
                CurrentMap = World.CurrentMap;
                PatherInstance = new Pather(CurrentMap);
            }

            if (PatherInstance == null)
                return false;

            Destination = pos;
            var path = PatherInstance.Search(Manager.LocalPlayer.Location, pos);
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

        public void Stop()
        {
            generatedPath.Clear();
            Destination = default(Location);
            Manager.LocalPlayer.StopCTM();
        }

        [EndSceneHandler]
        public void Direct3D_EndScene()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            if (generatedPath == null)
                return;

            if (generatedPath.Count == 0)
            {
                generatedPath = null;
                return;
            }

            if (SleepTime >= DateTime.Now)
                return;

            while (generatedPath.Count > 0 && (CurrentLocation == default(Location) || Manager.LocalPlayer.Location.DistanceTo(CurrentLocation) < 3f))
                CurrentLocation = generatedPath.Dequeue();

            MoveTo(CurrentLocation, 3f);

            SleepTime = DateTime.Now + TimeSpan.FromMilliseconds(100);
        }

        public bool MoveTo(Location loc, double tolerance = 5)
        {
            Manager.LocalPlayer.ClickToMove(loc);
            return Manager.LocalPlayer.Location.DistanceTo(loc) <= tolerance;
        }
    }
}
