#region Legal
/*
"PPather" Copyright 2008 Pontus Borg
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
#endregion Legal

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WowTriangles;
using PatherPath.Graph;
using Location = IceFlake.Client.Location;
using PathLoc = PatherPath.Graph.Location;

namespace IceFlake.Runtime
{
    public class Pather : IDisposable
    {
        public readonly string Continent;
        private PathGraph PG;
        public Pather(string continent)
        {
            Continent = continent;
            MPQTriangleSupplier mpq = new MPQTriangleSupplier();
            mpq.SetContinent(continent);
            ChunkedTriangleCollection triangleWorld = new ChunkedTriangleCollection(512);
            triangleWorld.SetMaxCached(64);
            triangleWorld.AddSupplier(mpq);
            PG = new PathGraph(continent, triangleWorld, (string s) => { Log.WriteLine(s); });
        }
        public List<Location> Search(Location start, Location end, double tolerance = 5.0)
        {
            var SearchTask = Task.Factory.StartNew<List<PatherPath.Graph.Location>>(() =>
            {
                return PG.CreatePath(Convert(start), Convert(end), (float)tolerance);
            }, TaskCreationOptions.LongRunning);
            Task.WaitAll(SearchTask);
            var path = SearchTask.Result;
            if (path != null)
                return path.ConvertAll<Location>(new Converter<PathLoc, Location>((loc) => { return Convert(loc); }));
            else return null;
        }

        public void Dispose()
        {
            PG.Save();
            PG.Clear();
            PG = null;
        }

        public PathLoc Convert(Location loc) { return new PathLoc(loc.X, loc.Y, loc.Z); }
        public Location Convert(PathLoc loc) { return new Location(loc.X, loc.Y, loc.Z); }
    }
}
