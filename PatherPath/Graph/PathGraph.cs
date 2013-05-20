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
using System.Linq;
using Par = System.Threading.Tasks.Parallel;
using WowTriangles;
namespace PatherPath.Graph
{
    public class PathGraph
    {
        public const float toonHeight = 2.0f;
        public const float toonSize = 0.5f;

        public const float MinStepLength = 2f;
        public const float WantedStepLength = 3f;
        public const float MaxStepLength = 5f;
        public const float StepLength = WantedStepLength * .75f;

        /*
        public const float IndoorsWantedStepLength = 1.5f;
        public const float IndoorsMaxStepLength = 2.5f;
        */

        public const float CHUNK_BASE = 100000.0f; // Always keep positive
        public string BaseDir = "Maps";
        public readonly string Continent;
        private SparseMatrix2D<GraphChunk> chunks;
        public ChunkedTriangleCollection triangleWorld;

        private List<GraphChunk> ActiveChunks = new List<GraphChunk>();
        private long LRU = 0;

        public const float PI = (float)Math.PI;

        public static Action<string> LogDelegate;

        public PathGraph(string continent, ChunkedTriangleCollection triangles, Action<string> LogDelegate)
        {
            this.Continent = continent;
            this.triangleWorld = triangles;
            PathGraph.LogDelegate = LogDelegate;
            Clear();
        }

        public void Clear()
        {
            chunks = new SparseMatrix2D<GraphChunk>(8);
        }
        private void GetChunkCoord(float x, float y, out int ix, out int iy)
        {
            ix = (int)((CHUNK_BASE + x) / GraphChunk.CHUNK_SIZE);
            iy = (int)((CHUNK_BASE + y) / GraphChunk.CHUNK_SIZE);
        }

        private void GetChunkBase(int ix, int iy, out float bx, out float by)
        {
            bx = (float)ix * GraphChunk.CHUNK_SIZE - CHUNK_BASE;
            by = (float)iy * GraphChunk.CHUNK_SIZE - CHUNK_BASE;
        }

        private GraphChunk GetChunkAt(float x, float y)
        {
            int ix, iy;
            GetChunkCoord(x, y, out ix, out iy);
            GraphChunk c = chunks.Get(ix, iy);
            if (c != null) c.LRU = LRU++;
            return c;
        }

        private void CheckForChunkEvict()
        {
            lock (chunks)
            {
                if (ActiveChunks.Count < 512)
                    return;

                GraphChunk evict = null;
                foreach (GraphChunk gc in ActiveChunks)
                    if (evict == null || gc.LRU < evict.LRU)
                        evict = gc;

                // It is full!
                evict.Save(BaseDir + "\\" + Continent + "\\");
                ActiveChunks.Remove(evict);
                chunks.Clear(evict.ix, evict.iy);
                evict.Clear();
            }
        }

        public void Save()
        {
            lock (chunks)
            {
                Logger.Log("Saving pathing data");
                ICollection<GraphChunk> l = chunks.GetAllElements();
                Par.ForEach<GraphChunk>(l, (gc) =>
                {
                    if (gc.modified)
                        gc.Save(BaseDir + "\\" + Continent + "\\");
                });
            }
        }

        // Create and load from file if exisiting
        private void LoadChunk(float x, float y)
        {
            lock (chunks)
            {
                GraphChunk gc = GetChunkAt(x, y);
                if (gc == null)
                {
                    int ix, iy;
                    GetChunkCoord(x, y, out ix, out iy);

                    float base_x, base_y;
                    GetChunkBase(ix, iy, out base_x, out base_y);

                    gc = new GraphChunk(base_x, base_y, ix, iy);
                    gc.LRU = LRU++;

                    CheckForChunkEvict();

                    gc.Load(BaseDir + "\\" + Continent + "\\");
                    chunks.Set(ix, iy, gc);
                    ActiveChunks.Add(gc);
                }
            }
        }

        public Spot AddSpot(Spot s)
        {
            LoadChunk(s.X, s.Y);
            GraphChunk gc = GetChunkAt(s.X, s.Y);
            return gc.AddSpot(s);
        }

        // Connect according to MPQ data
        public Spot AddAndConnectSpot(Spot s)
        {
            s = AddSpot(s);
            if (s.Flags.HasFlag(SpotFlags.FLAG_MPQ_MAPPED)) return s;

            List<Spot> close = FindAllSpots(s, MaxStepLength);
            /*
            Benchmark: AddAndConnectSpot-Par
            MarkTime : 00:00:00.0005821
            
            Benchmark: AddAndConnectSpot
            MarkTime : 00:00:00.0007017
            */
            Par.ForEach<Spot>(close, (cs) =>
            {
                if (!(cs.HasPathTo(this, s) &&
                        s.HasPathTo(this, cs) ||
                        cs.IsBlocked()) &&
                        !triangleWorld.IsStepBlocked(s.X, s.Y, s.Z, cs.X, cs.Y, cs.Z, toonHeight, toonSize))
                {
                    float mid_x = (s.X + cs.X) / 2, mid_y = (s.Y + cs.Y) / 2, mid_z = (s.Z + cs.Z) / 2, stand_z;
                    int flags;
                    if (triangleWorld.FindStandableAt(mid_x, mid_y, mid_z - StepLength, mid_z + StepLength, out stand_z, out flags, toonHeight, toonSize))
                    {
                        s.AddPathTo(cs);
                        cs.AddPathTo(s);
                    }
                }
            });
            return s;
        }

        public Spot GetSpot(float x, float y, float z)
        {
            LoadChunk(x, y);
            GraphChunk gc = GetChunkAt(x, y);
            return gc.GetSpot(x, y, z);
        }
        public Spot GetSpot2D(float x, float y)
        {
            LoadChunk(x, y);
            GraphChunk gc = GetChunkAt(x, y);
            return gc.GetSpot2D(x, y);
        }
        public Spot GetSpot(Location l)
        {
            return GetSpot(l.X, l.Y, l.Z);
        }

        public Spot FindClosestSpot(Location l_d)
        {
            return FindClosestSpot(l_d, 30.0f, null);
        }
        public Spot FindClosestSpot(Location l_d, List<Spot> Not)
        {
            return FindClosestSpot(l_d, 30.0f, Not);
        }
        public Spot FindClosestSpot(Location l, float max_d)
        {
            return FindClosestSpot(l, max_d, null);
        }

        // this can be slow...
        public Spot FindClosestSpot(Location l, float max_d, List<Spot> Not)
        {
            Spot closest = null;
            float closest_d = float.MaxValue;
            int d = 0;

            /*
            Benchmark: FindClosestSpot
            MarkTime : 00:00:00.0000097
            */
            while (d <= max_d)
            {
                for (int i = -d; i <= d; i++)
                {
                    Spot[] sv = {
                        GetSpot2D(l.X + d, l.Y + i), //0
                        GetSpot2D(l.X + i, l.Y - d), //1
                        GetSpot2D(l.X - d, l.Y + i), //2
                        GetSpot2D(l.X + i, l.Y + d)};//3
                    foreach (Spot spot in sv)
                    {
                        var s = spot;
                        while (s != null)
                        {
                            float di = s.GetDistanceTo(l);
                            if (di < max_d && !s.IsBlocked() && (di < closest_d))
                            {
                                closest = s;
                                closest_d = di;
                            }
                            s = s.next;
                        }
                    }
                }
                if (closest_d < d) // can't get better
                    //Log("Closest2 spot to " + l + " is " + closest);
                    return closest;
                d++;
            }
            //Log("Closest1 spot to " + l + " is " + closest);
            return closest;
        }

        public List<Spot> FindAllSpots(Location l, float max_d)
        {
            List<Spot> sl = new List<Spot>();
            float d = 0;

            /*
            Benchmark: FindAllSpots
            MarkTime : 00:00:00.0000335
            */
            while (d <= max_d + 0.1f)
            {
                for (var i = -d; i <= d; i++)
                {
                    Spot[] sv = {
                        GetSpot2D(l.X + d, l.Y + i),
                        GetSpot2D(l.X + i, l.Y - d),
                        GetSpot2D(l.X - d, l.Y + i),
                        GetSpot2D(l.X + i, l.Y + d)};
                    foreach (Spot s in sv)
                    {
                        Spot ss = s;
                        while (ss != null)
                        {
                            if (ss.GetDistanceTo(l) < max_d)
                                sl.Add(ss);
                            ss = ss.next;
                        }
                    }
                }
                d++;
            }
            return sl;
        }
        public Spot TryAddSpot(Spot wasAt, Location isAt)
        {
            //if (IsUnderwaterOrInAir(isAt)) { return wasAt; }
            Spot isAtSpot = FindClosestSpot(isAt, WantedStepLength);
            if (isAtSpot == null)
            {
                isAtSpot = GetSpot(isAt);
                if (isAtSpot == null)
                {
                    Spot s = new Spot(isAt);
                    s = AddSpot(s);
                    isAtSpot = s;
                }
                if (isAtSpot.Flags.HasFlag(SpotFlags.FLAG_BLOCKED))
                {
                    isAtSpot.Flags ^= SpotFlags.FLAG_BLOCKED;
                    Logger.Log("Cleared blocked flag");
                }
                if (wasAt != null)
                {
                    wasAt.AddPathTo(isAtSpot);
                    isAtSpot.AddPathTo(wasAt);
                }

                List<Spot> sl = FindAllSpots(isAtSpot, MaxStepLength);
                int connected = 0;
                foreach (Spot other in sl)
                {
                    if (other != isAtSpot)
                    {
                        other.AddPathTo(isAtSpot);
                        isAtSpot.AddPathTo(other);
                        connected++;
                        // Log("  connect to " + other.location);
                    }
                }
                Logger.Log("Learned a new spot at " + isAtSpot + " connected to " + connected + " other spots");
                wasAt = isAtSpot;
            }
            else
            {
                if (wasAt != null && wasAt != isAtSpot)
                {
                    // moved to an old spot, make sure they are connected
                    wasAt.AddPathTo(isAtSpot);
                    isAtSpot.AddPathTo(wasAt);
                }
                wasAt = isAtSpot;
            }

            return wasAt;
        }

        private bool LineCrosses(Location line0, Location line1, Location point)
        {
            float LineMag = line0.GetDistanceTo(line1); // Magnitude( LineEnd, LineStart );

            float U =
                (((point.X - line0.X) * (line1.X - line0.X)) +
                  ((point.Y - line0.Y) * (line1.Y - line0.Y)) +
                  ((point.Z - line0.Z) * (line1.Z - line0.Z))) /
                (LineMag * LineMag);

            if (U < 0.0f || U > 1.0f)
                return false;

            float InterX = line0.X + U * (line1.X - line0.X);
            float InterY = line0.Y + U * (line1.Y - line0.Y);
            float InterZ = line0.Z + U * (line1.Z - line0.Z);

            float Distance = point.GetDistanceTo(new Location(InterX, InterY, InterZ));
            if (Distance < 0.5f)
                return true;
            return false;
        }

        public void MarkBlockedAt(Location loc)
        {
            Spot s = new Spot(loc);
            s = AddSpot(s);
            s.Flags |= SpotFlags.FLAG_BLOCKED;
            // Find all paths leading though this one

            List<Spot> sl = FindAllSpots(loc, 5.0f);
            foreach (Spot sp in sl)
            {
                foreach (Location to in sp.paths)
                {
                    if (LineCrosses(sp, to, loc))
                    {
                        sp.RemovePathTo(to);
                    }
                }
            }

        }

        public void BlacklistStep(Location from, Location to)
        {
            Spot froms = GetSpot(from);
            if (froms != null)
                froms.RemovePathTo(to);
        }

        public void MarkStuckAt(Location loc, float heading)
        {
            // TODO another day...
            Location inf = loc.InFrontOf(heading, 1.0f);
            MarkBlockedAt(inf);

            // TODO
        }


        //////////////////////////////////////////////////////
        // Searching
        //////////////////////////////////////////////////////


        public Spot currentSearchStartSpot = null;
        public Spot currentSearchSpot = null;


        float TurnCost(Spot from, Spot to)
        {
            Spot prev = from.traceBack;
            if (prev == null) { return 0.0f; }
            return TurnCost(prev.X, prev.Y, prev.Z, from.X, from.Y, from.Z, to.X, to.Y, to.Z);

        }

        float TurnCost(float x0, float y0, float z0, float x1, float y1, float z1, float x2, float y2, float z2)
        {
            float v1x = x1 - x0;
            float v1y = y1 - y0;
            float v1z = z1 - z0;

            float v1l = (float)Math.Sqrt(v1x * v1x + v1y * v1y + v1z * v1z);
            v1x /= v1l;
            v1y /= v1l;
            v1z /= v1l;

            float v2x = x2 - x1;
            float v2y = y2 - y1;
            float v2z = z2 - z1;

            float v2l = (float)Math.Sqrt(v2x * v2x + v2y * v2y + v2z * v2z);
            v2x /= v2l;
            v2y /= v2l;
            v2z /= v2l;

            float ddx = v1x - v2x;
            float ddy = v1y - v2y;
            float ddz = v1z - v2z;
            return (float)Math.Sqrt(ddx * ddx + ddy * ddy + ddz * ddz);
        }

        // return null if failed or the last spot in the path found


        public SearchProgress searchProgress { get; private set; }
        int searchID = 0;
        float heuristicsFactor = 1.5f;
        private Spot Search(Spot fromSpot, Spot destinationSpot, float minHowClose)
        {
            currentSearchStartSpot = fromSpot;
            searchID++;
            int currentSearchID = searchID;
            searchProgress = new SearchProgress(fromSpot, destinationSpot, searchID);

            // lowest first queue
            var prioritySpotQueue = new PriorityQueue<Spot>(); // (new SpotSearchComparer(dst, score)); ;
            prioritySpotQueue.Enqueue(fromSpot.GetDistanceTo(destinationSpot) * heuristicsFactor, fromSpot);

            fromSpot.SearchScoreSet(currentSearchID, 0.0);
            fromSpot.traceBack = null;
            fromSpot.traceBackDistance = 0;

            System.Diagnostics.Stopwatch SaveTimer = new System.Diagnostics.Stopwatch();
            SaveTimer.Start();

            while (prioritySpotQueue.Count > 0)
            {
                if (SaveTimer.Elapsed.TotalSeconds >= 20)
                {
                    Save();
                    SaveTimer.Restart();
                }

                currentSearchSpot = prioritySpotQueue.Dequeue();

                TriangleCollection tc = triangleWorld.GetChunkAt(currentSearchSpot.X, currentSearchSpot.Y); // force the world to be loaded

                if (currentSearchSpot.SearchIsClosed(currentSearchID)) continue;
                currentSearchSpot.SearchClose(currentSearchID);

                //update status
                if (!searchProgress.CheckProgress(currentSearchSpot)) break;

                // are we there?
                if (currentSearchSpot.GetDistanceTo(destinationSpot) <= minHowClose) return currentSearchSpot; // got there

                CreateSpotsAroundSpot(currentSearchSpot);
                //Find spots to link to
                Par.ForEach(currentSearchSpot.GetPathsToSpots(this), (spotLinkedToCurrent) =>
                {
                    if (spotLinkedToCurrent != null && !spotLinkedToCurrent.SearchIsClosed(currentSearchID))
                        ScoreSpot(spotLinkedToCurrent, destinationSpot, currentSearchID, prioritySpotQueue);
                });
            }
            //we ran out of spots to search
            searchProgress.LogStatus("  search failed. ");
            return null;
        }

        private void ScoreSpot(Spot spotLinkedToCurrent, Spot destinationSpot, int currentSearchID, PriorityQueue<Spot> prioritySpotQueue)
        {
            // the movement cost to move from the starting point A to a given square on the grid, following the path generated to get there.  
            double G_Score = currentSearchSpot.traceBackDistance + currentSearchSpot.GetDistanceTo(spotLinkedToCurrent);
            //the estimated movement cost to move from that given square on the grid to the final destination, point B.
            //This is often referred to as the heuristic, which can be a bit confusing. The reason why it is called that
            //is because it is a guess. We really don’t know the actual distance until we find the path, because all sorts
            //of things can be in the way (walls, water, etc.)
            double H_Score = spotLinkedToCurrent.GetDistanceTo(destinationSpot) * heuristicsFactor;
            double F_Score = G_Score + H_Score;

            if (spotLinkedToCurrent.Flags.HasFlag(SpotFlags.FLAG_WATER)) F_Score += 50;
            if (spotLinkedToCurrent.Flags.HasFlag(SpotFlags.FLAG_CLOSETOMODEL)) F_Score += 25;

            if (!spotLinkedToCurrent.SearchScoreIsSet(currentSearchID) || F_Score < spotLinkedToCurrent.SearchScoreGet(currentSearchID))
            {
                // shorter path to here found
                spotLinkedToCurrent.traceBack = currentSearchSpot;
                spotLinkedToCurrent.traceBackDistance = G_Score;
                spotLinkedToCurrent.SearchScoreSet(currentSearchID, F_Score);
                prioritySpotQueue.Enqueue(F_Score, spotLinkedToCurrent);
            }
        }

        private void CreateSpotsAroundSpot(Spot currentSearchSpot)
        {
            if (currentSearchSpot.Flags.HasFlag(SpotFlags.FLAG_MPQ_MAPPED)) return;
            currentSearchSpot.Flags |= SpotFlags.FLAG_MPQ_MAPPED; //mark as mapped

            //loop through the spots in a circle around the current search spot
            for (float radianAngle = 0; radianAngle < PI * 2; radianAngle += PI / 8)
            {
                //calculate the location of the spot at the angle
                float
                    nx = currentSearchSpot.X + (float)Math.Sin(radianAngle) * WantedStepLength,
                    ny = currentSearchSpot.Y + (float)Math.Cos(radianAngle) * WantedStepLength,
                    zmin = currentSearchSpot.Z - StepLength,
                    zmax = currentSearchSpot.Z + StepLength,
                    nz;
                int flags;

                if (!triangleWorld.FindStandableAt(nx, ny, zmin, zmax, out nz, out flags, toonHeight, toonSize)) continue;
                if (triangleWorld.IsStepBlocked(currentSearchSpot.X, currentSearchSpot.Y, currentSearchSpot.Z, nx, ny, nz, toonHeight, toonSize)) continue;
                if (GetSpot(nx, ny, nz) != null) continue; //found a spot so don't create a new one
                if (FindClosestSpot(new Location(nx, ny, nz), MinStepLength) != null) continue; //see if there is a close spot, stop if there is, TODO: this is slow

                //create a new spot and connect it
                Spot newSpot = AddAndConnectSpot(new Spot(nx, ny, nz));

                if ((flags & ChunkedTriangleCollection.TriangleFlagDeepWater) != 0)
                    newSpot.Flags |= SpotFlags.FLAG_WATER;
                if (((flags & ChunkedTriangleCollection.TriangleFlagModel) != 0) || ((flags & ChunkedTriangleCollection.TriangleFlagObject) != 0))
                    newSpot.Flags |= SpotFlags.FLAG_INDOORS;
                if (triangleWorld.IsCloseToModel(newSpot.X, newSpot.Y, newSpot.Z, MaxStepLength))
                    newSpot.Flags |= SpotFlags.FLAG_CLOSETOMODEL;
            }
        }
        public List<Spot> CurrentSearchPath()
        {
            return FollowTraceBack(currentSearchStartSpot, currentSearchSpot);
        }

        private List<Spot> FollowTraceBack(Spot from, Spot to)
        {
            List<Spot> path = new List<Spot>();
            int count = 0;

            Spot r = to;
            path.Insert(0, to); // add last
            while (r != null)
            {
                Spot s = r.traceBack;

                if (s != null)
                {
                    path.Insert(0, s); // add first
                    r = s;
                    if (r == from)
                    {
                        r = null;  // found source
                    }
                }
                else
                {
                    r = null;
                }
                count++;
            }
            path.Insert(0, from); // add first
            return path;
        }

        public bool IsUnderwaterOrInAir(Location l)
        {
            int flags;
            float z;
            if (triangleWorld.FindStandableAt(l.X, l.Y, l.Z - 50.0f, l.Z + 5.0f, out z, out  flags, toonHeight, toonSize))
            {
                if ((flags & ChunkedTriangleCollection.TriangleFlagDeepWater) != 0)
                    return true;
                else
                    return false;
            }
            //return true; 
            return false;
        }
        public bool IsUnderwaterOrInAir(Spot s)
        {
            return IsUnderwaterOrInAir(s);
        }

        public bool IsInABuilding(Location l)
        {
            //int flags;
            //float z;
            //if (triangleWorld.FindStandableAt(l.X, l.Y, l.Z +12.0f, l.Z + 50.0f, out z, out  flags, toonHeight, toonSize))
            //{
            //   return true;
            //    //return false;
            //}
            //return triangleWorld.IsCloseToModel(l.X,l.Y,l.Z,1);
            //return true; 
            return false;
        }

        public List<Location> LastPath = null;
        public List<Location> CreatePath(Spot from, Spot to, float minHowClose)
        {
            Spot newTo = Search(from, to, minHowClose);
            if (newTo != null)
            {
                if (newTo.GetDistanceTo(to) <= minHowClose)
                {
                    List<Spot> path = FollowTraceBack(from, newTo);
                    LastPath = new List<Location>(path);
                    return LastPath;
                }
            }
            return null;
        }

        public List<Location> CreatePath(Location fromLoc, Location toLoc, float howClose)
        {
            Logger.Log("Creating Path from " + fromLoc.ToString() + " to " + toLoc.ToString());

            //GSpellTimer t = new GSpellTimer(0);
            Spot from = FindClosestSpot(fromLoc, MinStepLength);
            Spot to = FindClosestSpot(toLoc, MinStepLength);

            if (from == null)
                from = AddAndConnectSpot(new Spot(fromLoc));
            if (to == null)
                to = AddAndConnectSpot(new Spot(toLoc));

            List<Location> rawPath = CreatePath(from, to, howClose);
            if (rawPath == null) return null;
            else
            {
                Location last = rawPath.Last();
                if (last.GetDistanceTo(toLoc) > 1.0) { rawPath.Add(toLoc); }
            }
            LastPath = rawPath;
            return rawPath;
        }
    }
}
