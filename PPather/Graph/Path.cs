using System;
using System.Collections.Generic;

namespace PatherPath.Graph
{
    public class Path
    {
        public List<Location> Locations = new List<Location>();

        public Path()
        {
        }

        public Path(List<Spot> steps)
        {
            foreach (Spot s in steps)
            {
                AddLast(s.location);
            }
        }

        public int Count()
        {
            return Locations.Count;
        }

        public Location GetFirst()
        {
            return Get(0);
        }

        public Location GetSecond()
        {
            if (Locations.Count > 1)
                return Get(1);
            return null;
        }

        public Location GetRandom()
        {
            if (Locations.Count < 2)
                return null;
            var r = new Random();
            return Locations[r.Next(0, (Locations.Count - 1))];
        }

        public Location GetLast()
        {
            return Locations[Locations.Count - 1];
        }

        public Location RemoveFirst()
        {
            Location l = Get(0);
            Locations.RemoveAt(0);
            return l;
        }

        public Location Get(int index)
        {
            return Locations[index];
        }

        public void AddFirst(Location l)
        {
            Locations.Insert(0, l);
        }

        public void AddFirst(Path l)
        {
            Locations.InsertRange(0, l.Locations);
        }

        public void AddLast(Location l)
        {
            Locations.Add(l);
        }

        public void AddLast(Path l)
        {
            Locations.AddRange(l.Locations);
        }
    }
}