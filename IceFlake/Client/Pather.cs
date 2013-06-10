using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WowTriangles;
using PatherPath.Graph;
using System.IO;
using Location = IceFlake.Client.Location;
using PathLoc = PatherPath.Graph.Location;

namespace IceFlake.Client
{
    public class Pather : IDisposable
    {
        public readonly string Continent;
        private PathGraph PG;
        public Pather(string continent)
        {
            Continent = continent;
            var mpq = new MPQTriangleSupplier();
            mpq.SetContinent(continent);
            var triangleWorld = new ChunkedTriangleCollection(512);
            triangleWorld.SetMaxCached(64);
            triangleWorld.AddSupplier(mpq);
            PG = new PathGraph(continent, triangleWorld, null, (string s) => { Log.WriteLine(s); });
        }

        public List<Location> Search(Location start, Location end, double tolerance = 5.0)
        {
            //var SearchTask = Task.Factory.StartNew<List<PatherPath.Graph.Location>>(() =>
            //{
            //    return PG.CreatePath(Convert(start), Convert(end), (float)tolerance);
            //}, TaskCreationOptions.LongRunning);
            //Task.WaitAll(SearchTask);
            //var path = SearchTask.Result;
            var path = PG.CreatePath(Convert(start), Convert(end), (float)tolerance);
            if (path != null && path.Locations != null)
                return path.Locations.ConvertAll<Location>(new Converter<PathLoc, Location>((loc) => { return Convert(loc); }));
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
