using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
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

        /*
        public const float IndoorsWantedStepLength = 1.5f;
        public const float IndoorsMaxStepLength = 2.5f;
        */

        public const float CHUNK_BASE = 100000.0f; // Always keep positive
        public static Action<string> LogDelegate;
        private readonly List<GraphChunk> ActiveChunks = new List<GraphChunk>();
        private readonly string Continent;
        public string BaseDir = "PPather\\PathInfo";
        private long LRU;
        private SparseMatrix2D<GraphChunk> chunks;


        public TriangleCollection paint;
        private int searchID;
        public ChunkedTriangleCollection triangleWorld;


        public PathGraph(string continent,
                         ChunkedTriangleCollection triangles,
                         TriangleCollection paint,
                         Action<string> logDelegate)
        {
            LogDelegate = logDelegate;
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            Continent = continent;
            triangleWorld = triangles;
            this.paint = paint;
            Clear();
        }

        public void Close()
        {
            triangleWorld.Close();
        }

        public void Clear()
        {
            chunks = new SparseMatrix2D<GraphChunk>(8);
        }

        private void GetChunkCoord(float x, float y, out int ix, out int iy)
        {
            ix = (int) ((CHUNK_BASE + x)/GraphChunk.CHUNK_SIZE);
            iy = (int) ((CHUNK_BASE + y)/GraphChunk.CHUNK_SIZE);
        }

        private void GetChunkBase(int ix, int iy, out float bx, out float by)
        {
            bx = (float) ix*GraphChunk.CHUNK_SIZE - CHUNK_BASE;
            by = (float) iy*GraphChunk.CHUNK_SIZE - CHUNK_BASE;
        }

        private GraphChunk GetChunkAt(float x, float y)
        {
            int ix, iy;
            GetChunkCoord(x, y, out ix, out iy);
            GraphChunk c = chunks.Get(ix, iy);
            if (c != null)
                c.LRU = LRU++;
            return c;
        }

        private void CheckForChunkEvict()
        {
            lock (this)
            {
                if (ActiveChunks.Count < 25)
                    return;

                GraphChunk evict = null;
                foreach (GraphChunk gc in ActiveChunks)
                {
                    if (evict == null || gc.LRU < evict.LRU)
                    {
                        evict = gc;
                    }
                }

                // It is full!
                evict.Save(BaseDir + "\\" + Continent + "\\");
                ActiveChunks.Remove(evict);
                chunks.Clear(evict.ix, evict.iy);
                evict.Clear();
            }
        }


        public void Save()
        {
            lock (this)
            {
                ICollection<GraphChunk> l = chunks.GetAllElements();
                foreach (GraphChunk gc in l)
                {
                    if (gc.modified)
                    {
                        gc.Save(BaseDir + "\\" + Continent + "\\");
                    }
                }
            }
        }

        // Create and load from file if exisiting
        private void LoadChunk(float x, float y)
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
            List<Spot> close = FindAllSpots(s.location, MaxStepLength);
            if (!s.GetFlag(Spot.FLAG_MPQ_MAPPED))
            {
                foreach (Spot cs in close)
                {
                    if (cs.HasPathTo(this, s) && s.HasPathTo(this, cs) ||
                        cs.IsBlocked())
                    {
                    }
                    else if (!triangleWorld.IsStepBlocked(s.X, s.Y, s.Z, cs.X, cs.Y, cs.Z,
                                                          toonHeight, toonSize, null))
                    {
                        float mid_x = (s.X + cs.X)/2;
                        float mid_y = (s.Y + cs.Y)/2;
                        float mid_z = (s.Z + cs.Z)/2;
                        float stand_z;
                        int flags;
                        if (triangleWorld.FindStandableAt(mid_x, mid_y,
                                                          mid_z - WantedStepLength*.75f, mid_z + WantedStepLength*.75f,
                                                          out stand_z, out flags, toonHeight, toonSize))
                        {
                            s.AddPathTo(cs);
                            cs.AddPathTo(s);
                        }
                    }
                }
            }
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
            if (l == null)
                return null;
            return GetSpot(l.X, l.Y, l.Z);
        }


        // this can be slow...

        public Spot FindClosestSpot(Location l_d)
        {
            return FindClosestSpot(l_d, 30.0f, null);
        }

        public Spot FindClosestSpot(Location l_d, Set<Spot> Not)
        {
            return FindClosestSpot(l_d, 30.0f, Not);
        }


        public Spot FindClosestSpot(Location l, float max_d)
        {
            return FindClosestSpot(l, max_d, null);
        }

        // this can be slow...
        public Spot FindClosestSpot(Location l, float max_d, Set<Spot> Not)
        {
            Spot closest = null;
            float closest_d = 1E30f;
            int d = 0;
            while (d <= max_d + 0.1f)
            {
                for (int i = -d; i <= d; i++)
                {
                    float x_up = l.X + d;
                    float x_dn = l.X - d;
                    float y_up = l.Y + d;
                    float y_dn = l.Y - d;

                    Spot s0 = GetSpot2D(x_up, l.Y + i);
                    Spot s2 = GetSpot2D(x_dn, l.Y + i);

                    Spot s1 = GetSpot2D(l.X + i, y_dn);
                    Spot s3 = GetSpot2D(l.X + i, y_up);
                    Spot[] sv = {s0, s1, s2, s3};
                    foreach (Spot s in sv)
                    {
                        Spot ss = s;
                        while (ss != null)
                        {
                            float di = ss.GetDistanceTo(l);
                            if (di < max_d && !ss.IsBlocked() &&
                                (di < closest_d))
                            {
                                closest = ss;
                                closest_d = di;
                            }
                            ss = ss.next;
                        }
                    }
                }

                if (closest_d < d) // can't get better
                {
                    //Log("Closest2 spot to " + l + " is " + closest);
                    return closest;
                }
                d++;
            }
            //Log("Closest1 spot to " + l + " is " + closest);
            return closest;
        }

        public List<Spot> FindAllSpots(Location l, float max_d)
        {
            var sl = new List<Spot>();

            int d = 0;
            while (d <= max_d + 0.1f)
            {
                for (int i = -d; i <= d; i++)
                {
                    float x_up = l.X + d;
                    float x_dn = l.X - d;
                    float y_up = l.Y + d;
                    float y_dn = l.Y - d;

                    Spot s0 = GetSpot2D(x_up, l.Y + i);
                    Spot s2 = GetSpot2D(x_dn, l.Y + i);

                    Spot s1 = GetSpot2D(l.X + i, y_dn);
                    Spot s3 = GetSpot2D(l.X + i, y_up);
                    Spot[] sv = {s0, s1, s2, s3};
                    foreach (Spot s in sv)
                    {
                        Spot ss = s;
                        while (ss != null)
                        {
                            float di = ss.GetDistanceTo(l);
                            if (di < max_d)
                            {
                                sl.Add(ss);
                            }
                            ss = ss.next;
                        }
                    }
                }
                d++;
            }
            return sl;
        }

        public List<Spot> FindAllSpots(float min_x, float min_y, float max_x, float max_y)
        {
            // hmm, do it per chunk
            var l = new List<Spot>();
            for (float mx = min_x; mx <= max_x + GraphChunk.CHUNK_SIZE - 1; mx += GraphChunk.CHUNK_SIZE)
            {
                for (float my = min_y; my <= max_y + GraphChunk.CHUNK_SIZE - 1; my += GraphChunk.CHUNK_SIZE)
                {
                    LoadChunk(mx, my);
                    GraphChunk gc = GetChunkAt(mx, my);
                    List<Spot> sl = gc.GetAllSpots();
                    foreach (Spot s in sl)
                    {
                        if (s.X >= min_x && s.X <= max_x &&
                            s.Y >= min_y && s.Y <= max_y)
                        {
                            l.Add(s);
                        }
                    }
                }
            }
            return l;
        }


        public Spot TryAddSpot(Spot wasAt, Location isAt)
        {
            Spot isAtSpot = FindClosestSpot(isAt, WantedStepLength);
            if (isAtSpot == null)
            {
                isAtSpot = GetSpot(isAt);
                if (isAtSpot == null)
                {
                    var s = new Spot(isAt);
                    s = AddSpot(s);
                    isAtSpot = s;
                }
                if (isAtSpot.GetFlag(Spot.FLAG_BLOCKED))
                {
                    isAtSpot.SetFlag(Spot.FLAG_BLOCKED, false);
                    Log("Cleared blocked flag");
                }
                if (wasAt != null)
                {
                    wasAt.AddPathTo(isAtSpot);
                    isAtSpot.AddPathTo(wasAt);
                }

                List<Spot> sl = FindAllSpots(isAtSpot.location, MaxStepLength);
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
                Log("Learned a new spot at " + isAtSpot.location + " connected to " + connected + " other spots");
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
                (((point.X - line0.X)*(line1.X - line0.X)) +
                 ((point.Y - line0.Y)*(line1.Y - line0.Y)) +
                 ((point.Z - line0.Z)*(line1.Z - line0.Z)))/
                (LineMag*LineMag);

            if (U < 0.0f || U > 1.0f)
                return false;

            float InterX = line0.X + U*(line1.X - line0.X);
            float InterY = line0.Y + U*(line1.Y - line0.Y);
            float InterZ = line0.Z + U*(line1.Z - line0.Z);

            float Distance = point.GetDistanceTo(new Location(InterX, InterY, InterZ));
            if (Distance < 0.5f)
                return true;
            return false;
        }

        public void MarkBlockedAt(Location loc)
        {
            var s = new Spot(loc);
            s = AddSpot(s);
            s.SetFlag(Spot.FLAG_BLOCKED, true);
            // Find all paths leading though this one

            List<Spot> sl = FindAllSpots(loc, 5.0f);
            foreach (Spot sp in sl)
            {
                List<Location> paths = sp.GetPaths();
                foreach (Location to in paths)
                {
                    if (LineCrosses(sp.location, to, loc))
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


        private float TurnCost(Spot from, Spot to)
        {
            Spot prev = from.traceBack;
            if (prev == null)
                return 0.0f;
            return TurnCost(prev.X, prev.Y, prev.Z, from.X, from.Y, from.Z, to.X, to.Y, to.Z);
        }

        private float TurnCost(float x0, float y0, float z0, float x1, float y1, float z1, float x2, float y2, float z2)
        {
            float v1x = x1 - x0;
            float v1y = y1 - y0;
            float v1z = z1 - z0;
            var v1l = (float) Math.Sqrt(v1x*v1x + v1y*v1y + v1z*v1z);
            v1x /= v1l;
            v1y /= v1l;
            v1z /= v1l;

            float v2x = x2 - x1;
            float v2y = y2 - y1;
            float v2z = z2 - z1;
            var v2l = (float) Math.Sqrt(v2x*v2x + v2y*v2y + v2z*v2z);
            v2x /= v2l;
            v2y /= v2l;
            v2z /= v2l;

            float ddx = v1x - v2x;
            float ddy = v1y - v2y;
            float ddz = v1z - v2z;
            return (float) Math.Sqrt(ddx*ddx + ddy*ddy + ddz*ddz);
        }

        // return null if failed or the last spot in the path found

        private Spot search(Spot src, Spot dst,
                            Location realDst,
                            float minHowClose, bool AllowInvented,
                            ILocationHeuristics locationHeuristics)
        {
            searchID++;
            int count = 0;
            int prevCount = 0;
            int currentSearchID = searchID;
            float heuristicsFactor = 1.3f;
            DateTime pre = DateTime.Now;
            DateTime lastSpam = pre;

            // lowest first queue
            var q = new PriorityQueue<Spot, float>(); // (new SpotSearchComparer(dst, score)); ;
            q.Enqueue(src, -src.GetDistanceTo(dst)*heuristicsFactor);
            Spot BestSpot = null;

            //Set<Spot> closed      = new Set<Spot>();
            //SpotData<float> score = new SpotData<float>();

            src.SearchScoreSet(currentSearchID, 0.0f);
            src.traceBack = null;

            // A* -ish algorithm

            while (q.Count != 0) // && count < 100000)
            {
                float prio;
                Spot spot = q.Dequeue(out prio); // .Value; 
                //q.Remove(spot);


                if (spot.SearchIsClosed(currentSearchID))
                    continue;
                spot.SearchClose(currentSearchID);

                if (count%100 == 0)
                {
                    TimeSpan span = DateTime.Now.Subtract(lastSpam);
                    if (span.Seconds != 0 && BestSpot != null)
                    {
                        Thread.Sleep(50); // give glider a chance to stop us
                        int t = span.Seconds*1000 + span.Milliseconds;
                        if (t == 0)
                            Log("searching.... " + (count + 1) + " d: " + BestSpot.location.GetDistanceTo(realDst));
                        else
                            Log("searching.... " + (count + 1) + " d: " + BestSpot.location.GetDistanceTo(realDst) + " " +
                                (count - prevCount)*1000/t + " steps/s");
                        lastSpam = DateTime.Now;
                        prevCount = count;
                    }
                }
                count++;


                if (spot.Equals(dst) || spot.location.GetDistanceTo(realDst) <= minHowClose)
                {
                    TimeSpan ts = DateTime.Now.Subtract(pre);
                    int t = ts.Seconds*1000 + ts.Milliseconds;
                    /*if(t == 0)
                        Log("  search found the way there. " + count); 
                    else
                        Log("  search found the way there. " + count + " " + (count * 1000) / t + " steps/s");
                      */
                    return spot; // got there
                }

                if (BestSpot == null ||
                    spot.location.GetDistanceTo(realDst) < BestSpot.location.GetDistanceTo(realDst))
                {
                    BestSpot = spot;
                }
                {
                    TimeSpan ts = DateTime.Now.Subtract(pre);
                    if (ts.Seconds > 15)
                    {
                        Log("too long search, aborting");
                        break;
                    }
                }

                float src_score = spot.SearchScoreGet(currentSearchID);

                //PathGraph.Log("inspect: " + c + " score " + s);


                int new_found = 0;
                List<Spot> ll = spot.GetPathsToSpots(this);
                foreach (Spot to in ll)
                {
                    //Spot to = GetSpot(l);

                    if (to != null && !to.IsBlocked() && !to.SearchIsClosed(currentSearchID))
                    {
                        float old_score = 1E30f;

                        float new_score = src_score + spot.GetDistanceTo(to) + TurnCost(spot, to);
                        if (locationHeuristics != null)
                            new_score += locationHeuristics.Score(spot.X, spot.Y, spot.Z);
                        if (to.GetFlag(Spot.FLAG_WATER))
                            new_score += 30;

                        if (to.SearchScoreIsSet(currentSearchID))
                        {
                            old_score = to.SearchScoreGet(currentSearchID);
                        }

                        if (new_score < old_score)
                        {
                            // shorter path to here found
                            to.traceBack = spot;
                            //if (q.Contains(to)) 
                            //   q.Remove(to); // very sloppy to not dequeue it
                            to.SearchScoreSet(currentSearchID, new_score);
                            q.Enqueue(to, -(new_score + to.GetDistanceTo(dst)*heuristicsFactor));
                            new_found++;
                        }
                    }
                }

                //hmm search the triangles :p
                if (!spot.GetFlag(Spot.FLAG_MPQ_MAPPED))
                {
                    var PI = (float) Math.PI;

                    spot.SetFlag(Spot.FLAG_MPQ_MAPPED, true);
                    for (float a = 0; a < PI*2; a += PI/8)
                    {
                        float nx = spot.X + (float) Math.Sin(a)*WantedStepLength; // *0.8f;
                        float ny = spot.Y + (float) Math.Cos(a)*WantedStepLength; // *0.8f;
                        Spot s = GetSpot(nx, ny, spot.Z);
                        if (s == null)
                            s = FindClosestSpot(new Location(nx, ny, spot.Z), MinStepLength); // TODO: this is slow
                        if (s != null)
                        {
                            // hmm, they should already be connected 
                        }
                        else
                        {
                            float new_z;
                            int flags;
                            // gogo find a new one
                            //PathGraph.Log("gogo brave new world");
                            if (!triangleWorld.FindStandableAt(nx, ny,
                                                               spot.Z - WantedStepLength*.75f,
                                                               spot.Z + WantedStepLength*.75f,
                                                               out new_z, out flags, toonHeight, toonSize))
                            {
                                //Spot blocked = new Spot(nx, ny, spot.Z);
                                //blocked.SetFlag(Spot.FLAG_BLOCKED, true);
                                //AddSpot(blocked);
                            }
                            else
                            {
                                s = FindClosestSpot(new Location(nx, ny, new_z), MinStepLength);
                                if (s == null)
                                {
                                    if (!triangleWorld.IsStepBlocked(spot.X, spot.Y, spot.Z, nx, ny, new_z,
                                                                     toonHeight, toonSize, null))
                                    {
                                        var n = new Spot(nx, ny, new_z);
                                        Spot to = AddAndConnectSpot(n);
                                        if ((flags & ChunkedTriangleCollection.TriangleFlagDeepWater) != 0)
                                        {
                                            to.SetFlag(Spot.FLAG_WATER, true);
                                        }
                                        if (((flags & ChunkedTriangleCollection.TriangleFlagModel) != 0) ||
                                            ((flags & ChunkedTriangleCollection.TriangleFlagObject) != 0))
                                        {
                                            to.SetFlag(Spot.FLAG_INDOORS, true);
                                        }
                                        if (to != n || to.SearchIsClosed(currentSearchID))
                                        {
                                            // PathGraph.Log("/sigh");
                                        }
                                        else
                                        {
                                            // There should be a path from source to this one now
                                            if (spot.HasPathTo(to.location))
                                            {
                                                float old_score = 1E30f;

                                                float new_score = src_score + spot.GetDistanceTo(to) +
                                                                  TurnCost(spot, to);
                                                if (locationHeuristics != null)
                                                    new_score += locationHeuristics.Score(spot.X, spot.Y, spot.Z);


                                                if (to.GetFlag(Spot.FLAG_WATER))
                                                    new_score += 30;

                                                if (to.SearchScoreIsSet(currentSearchID))
                                                {
                                                    old_score = to.SearchScoreGet(currentSearchID);
                                                }

                                                if (new_score < old_score)
                                                {
                                                    // shorter path to here found
                                                    to.traceBack = spot;
                                                    //if (q.Contains(to)) 
                                                    //    q.Remove(to);
                                                    to.SearchScoreSet(currentSearchID, new_score);
                                                    q.Enqueue(to, -(new_score + to.GetDistanceTo(dst)*heuristicsFactor));
                                                    new_found++;
                                                }
                                            }
                                            else
                                            {
                                                // woot! I added a new one and it is not connected!?!?
                                                //PathGraph.Log("/cry");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            {
                TimeSpan ts = DateTime.Now.Subtract(pre);
                int t = ts.Seconds*1000 + ts.Milliseconds;
                if (t == 0)
                    t = 1;
                Log("  search failed. " + (count*1000)/t + " steps/s");
            }
            return BestSpot; // :(
        }


        private List<Spot> FollowTraceBack(Spot from, Spot to)
        {
            var path = new List<Spot>();
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
                        r = null; // fount source
                }
                else
                    r = null;
                count++;
            }
            path.Insert(0, from); // add first
            return path;
        }

        public bool IsUnderwaterOrInAir(Location l)
        {
            int flags;
            float z;
            if (triangleWorld.FindStandableAt(l.X, l.Y, l.Z - 50.0f, l.Z + 5.0f, out z, out flags, toonHeight, toonSize))
            {
                if ((flags & ChunkedTriangleCollection.TriangleFlagDeepWater) != 0)
                    return true;
                else
                    return false;
            }
            //return true; 
            return false;
        }

        public Path CreatePath(Spot from, Spot to, Location realDst,
                               float minHowClose, bool AllowInvented,
                               ILocationHeuristics locationHeuristics)
        {
            Spot newTo = search(from, to, realDst, minHowClose, AllowInvented,
                                locationHeuristics);
            if (newTo != null)
            {
                if (newTo.GetDistanceTo(to) <= minHowClose)
                {
                    List<Spot> path = FollowTraceBack(from, newTo);
                    return new Path(path);
                }
            }
            return null;
        }

        public Path CreatePath(Location fromLoc, Location toLoc,
                               float howClose)
        {
            return CreatePath(fromLoc, toLoc, howClose, null);
        }

        public Path CreatePath(Location fromLoc, Location toLoc,
                               float howClose,
                               ILocationHeuristics locationHeuristics)
        {
            Stopwatch t = Stopwatch.StartNew();
            Spot from = FindClosestSpot(fromLoc, MinStepLength);
            Spot to = FindClosestSpot(toLoc, MinStepLength);

            if (from == null)
            {
                from = AddAndConnectSpot(new Spot(fromLoc));
            }
            if (to == null)
            {
                to = AddAndConnectSpot(new Spot(toLoc));
            }

            Path rawPath = CreatePath(from, to, to.location, howClose, true, locationHeuristics);


            if (rawPath != null && paint != null)
            {
                Location prev = null;
                for (int i = 0; i < rawPath.Count(); i++)
                {
                    Location l = rawPath.Get(i);
                    paint.AddBigMarker(l.X, l.Y, l.Z);
                    if (prev != null)
                    {
                        paint.PaintPath(l.X, l.Y, l.Z + 3, prev.X, prev.Y, prev.Z + 3);
                    }
                    prev = l;
                }
            }
            t.Stop();
            Log("CreatePath took " + t.Elapsed);
            if (rawPath == null)
            {
                return null;
            }
            else
            {
                Location last = rawPath.GetLast();
                if (last.GetDistanceTo(toLoc) > 1.0)
                    rawPath.AddLast(toLoc);
            }
            return rawPath;
        }

        public static void Log(String s)
        {
            if (LogDelegate != null)
                LogDelegate.Invoke(s);
            //PathGraph.Log(s);
        }
    }
}