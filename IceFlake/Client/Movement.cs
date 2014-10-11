using System;
using System.Collections.Generic;
using IceFlake.DirectX;

//using meshPather;

namespace IceFlake.Client
{
    public class Movement : IPulsable
    {
        public Queue<Location> generatedPath;

        public Movement()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            CurrentMap = WoWWorld.CurrentMap;
            //PatherInstance = new Pather(CurrentMap);
        }

        private string CurrentMap { get; set; }

        //public Pather PatherInstance
        //{
        //    get;
        //    set;
        //}

        private DateTime SleepTime { get; set; }

        public Location Destination { get; private set; }

        public Location CurrentLocation { get; private set; }

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

            while (generatedPath.Count > 0 &&
                   (CurrentLocation == default(Location) ||
                    Manager.LocalPlayer.Location.DistanceTo(CurrentLocation) < 3f))
                CurrentLocation = generatedPath.Dequeue();

            MoveTo(CurrentLocation, 3f);

            SleepTime = DateTime.Now + TimeSpan.FromMilliseconds(100);
        }

        public bool PathTo(Location pos)
        {
            if (!Manager.ObjectManager.IsInGame)
                return false;

            //if (CurrentMap != WoWWorld.CurrentMap)
            //{
            //    CurrentMap = WoWWorld.CurrentMap;
            //    PatherInstance = new Pather(CurrentMap);
            //}

            //if (PatherInstance == null)
            //    return false;

            //Destination = pos;
            //var path = PatherInstance.FindPath(Manager.LocalPlayer.Location, pos, false);
            ////var path = PatherInstance.FindPath(Manager.LocalPlayer.Location, pos.ToVector3());
            //if (path == null)
            //{
            //    Log.WriteLine("Could not path to {0}", pos);
            //    return false;
            //}

            //FollowPath(path.ToLocation());

            return true;
        }

        public void FollowPath(IEnumerable<Location> Path)
        {
            generatedPath = new Queue<Location>();
            foreach (Location spot in Path)
                Log.WriteLine("\t{0}", spot);
            //generatedPath.Enqueue(spot);
        }

        public void Stop()
        {
            generatedPath.Clear();
            Destination = default(Location);
            Manager.LocalPlayer.StopCTM();
        }

        public bool MoveTo(Location loc, double tolerance = 5)
        {
            Manager.LocalPlayer.ClickToMove(loc);
            return Manager.LocalPlayer.Location.DistanceTo(loc) <= tolerance;
        }
    }
}