#region Legal
/*
Copyright 2009 scorpion

This file is part of N2.

N2 is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

N2 is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with N2.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace PatherPath.Graph
{
    public class SpotPaths : IList<Location>
    {
        private List<Location> Paths = new List<Location>();

        public int IndexOf(Location item)
        {
            lock (Paths)
            {
                return Paths.IndexOf(item);
            }
        }
        public void Insert(int index, Location item)
        {
            lock (Paths)
            {
                Paths.Insert(index, item);
            }
        }
        public void RemoveAt(int index)
        {
            lock (Paths)
            {
                Paths.RemoveAt(index);
            }
        }
        public Location this[int index]
        {
            get
            {
                lock (Paths)
                {
                    return Paths[index];
                }
            }
            set
            {
                lock (Paths)
                {
                    Paths[index] = value;
                }
            }
        }
        public void Add(Location item)
        {
            lock (Paths)
            {
                Paths.Add(item);
            }
        }
        public void Clear()
        {
            lock (Paths)
            {
                Paths.Clear();
            }
        }
        public bool Contains(Location item)
        {
            lock (Paths)
            {
                return Paths.Contains(item);
            }
        }
        public void CopyTo(Location[] array, int arrayIndex)
        {
            lock (Paths)
            {
                Paths.CopyTo(array, arrayIndex);
            }
        }
        public int Count
        {
            get
            {
                lock (Paths)
                {
                    return Paths.Count;
                }
            }
        }
        public bool Remove(Location item)
        {
            lock (Paths)
            {
                return Paths.Remove(item);
            }
        }
        public IEnumerator<Location> GetEnumerator()
        {
            lock (Paths)
            {
                return new List<Location>(Paths).GetEnumerator();
            }
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            lock (Paths)
            {
                return new List<Location>(Paths).GetEnumerator();
            }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }
    }
}
