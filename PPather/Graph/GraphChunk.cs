using System;
using System.Collections.Generic;
using System.IO;

namespace PatherPath.Graph
{
    public class GraphChunk
    {
        public const int CHUNK_SIZE = 512;
        private const uint FILE_MAGIC = 0x12341234;
        private const uint FILE_ENDMAGIC = 0x43214321;
        private const uint SPOT_MAGIC = 0x53504f54;

        private readonly float base_x;
        private readonly float base_y;
        public long LRU = 0;
        public int ix, iy;
        public bool modified = false;

        private Spot[,] spots;

        public GraphChunk(float base_x, float base_y, int ix, int iy)
        {
            this.base_x = base_x;
            this.base_y = base_y;
            this.ix = ix;
            this.iy = iy;
            spots = new Spot[CHUNK_SIZE,CHUNK_SIZE];
            modified = false;
        }

        public void Clear()
        {
            foreach (Spot s in spots)
                if (s != null)
                    s.traceBack = null;

            spots = null;
        }

        private void LocalCoords(float x, float y, out int ix, out int iy)
        {
            ix = (int) (x - base_x);
            iy = (int) (y - base_y);
        }

        public Spot GetSpot2D(float x, float y)
        {
            int ix, iy;
            LocalCoords(x, y, out ix, out iy);
            Spot s = spots[ix, iy];
            return s;
        }

        public Spot GetSpot(float x, float y, float z)
        {
            Spot s = GetSpot2D(x, y);

            while (s != null && !s.IsCloseZ(z))
            {
                s = s.next;
            }

            return s;
        }

        // return old spot at conflicting poision
        // or the same as passed the function if all was ok
        public Spot AddSpot(Spot s)
        {
            Spot old = GetSpot(s.X, s.Y, s.Z);
            if (old != null)
                return old;
            int x, y;

            s.chunk = this;

            LocalCoords(s.X, s.Y, out x, out y);

            s.next = spots[x, y];
            spots[x, y] = s;
            modified = true;
            return s;
        }


        public List<Spot> GetAllSpots()
        {
            var l = new List<Spot>();
            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                for (int y = 0; y < CHUNK_SIZE; y++)
                {
                    Spot s = spots[x, y];
                    while (s != null)
                    {
                        l.Add(s);
                        s = s.next;
                    }
                }
            }
            return l;
        }

        private string FileName()
        {
            return String.Format("c_{0,3:000}_{1,3:000}.bin", ix, iy);
        }


        // Per spot: 
        // uint32 magic
        // uint32 reserved;
        // uint32 flags;
        // float x;
        // float y;
        // float z;
        // uint32 no_paths
        //   for each path
        //     float x;
        //     float y;
        //     float z;


        public bool Load(string baseDir)
        {
            string fileName = FileName();
            string filenamebin = baseDir + fileName;

            Stream stream = null;
            BinaryReader file = null;
            int n_spots = 0;
            int n_steps = 0;
            try
            {
                stream = File.OpenRead(filenamebin);
                if (stream != null)
                {
                    file = new BinaryReader(stream);
                    if (file != null)
                    {
                        uint magic = file.ReadUInt32();
                        if (magic == FILE_MAGIC)
                        {
                            uint type;
                            while ((type = file.ReadUInt32()) != FILE_ENDMAGIC)
                            {
                                n_spots++;
                                uint reserved = file.ReadUInt32();
                                uint flags = file.ReadUInt32();
                                float x = file.ReadSingle();
                                float y = file.ReadSingle();
                                float z = file.ReadSingle();
                                uint n_paths = file.ReadUInt32();
                                var s = new Spot(x, y, z);
                                s.flags = flags;

                                for (uint i = 0; i < n_paths; i++)
                                {
                                    n_steps++;
                                    float sx = file.ReadSingle();
                                    float sy = file.ReadSingle();
                                    float sz = file.ReadSingle();
                                    s.AddPathTo(sx, sy, sz);
                                }
                                AddSpot(s);
                            }
                        }
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                PathGraph.Log(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                PathGraph.Log(e.Message);
            }

            if (file != null)
            {
                file.Close();
            }
            if (stream != null)
            {
                stream.Close();
            }


            Log("Loaded " + fileName + " " + n_spots + " spots " + n_steps + " steps");

            modified = false;
            return false;
        }


        public bool Save(string baseDir)
        {
            if (!modified)
                return true; // doh

            string fileName = FileName();
            string filename = baseDir + fileName;

            Stream fileout = null;
            BinaryWriter file = null;

            //try {
            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir);
            //} catch { };

            int n_spots = 0;
            int n_steps = 0;
            try
            {
                fileout = File.Create(filename + ".new");

                if (fileout != null)
                {
                    file = new BinaryWriter(fileout);

                    if (file != null)
                    {
                        file.Write(FILE_MAGIC);

                        List<Spot> spots = GetAllSpots();
                        foreach (Spot s in spots)
                        {
                            file.Write(SPOT_MAGIC);
                            file.Write((uint) 0); // reserved
                            file.Write(s.flags);
                            file.Write(s.X);
                            file.Write(s.Y);
                            file.Write(s.Z);
                            var n_paths = (uint) s.n_paths;
                            file.Write(n_paths);
                            for (uint i = 0; i < n_paths; i++)
                            {
                                uint off = i*3;
                                file.Write(s.paths[off]);
                                file.Write(s.paths[off + 1]);
                                file.Write(s.paths[off + 2]);
                                n_steps++;
                            }
                            n_spots++;
                        }
                        file.Write(FILE_ENDMAGIC);
                    }

                    if (file != null)
                    {
                        file.Close();
                        file = null;
                    }

                    if (fileout != null)
                    {
                        fileout.Close();
                        fileout = null;
                    }

                    String old = filename + ".bak";

                    if (File.Exists(old))
                        File.Delete(old);
                    if (File.Exists(filename))
                        File.Move(filename, old);
                    File.Move(filename + ".new", filename);
                    if (File.Exists(old))
                        File.Delete(old);

                    modified = false;
                }
                else
                {
                    Log("Save failed");
                }
                Log("Saved " + fileName + " " + n_spots + " spots " + n_steps + " steps");
            }
            catch (Exception e)
            {
                Log("Save failed " + e);
            }

            if (file != null)
            {
                file.Close();
                file = null;
            }

            if (fileout != null)
            {
                fileout.Close();
                fileout = null;
            }


            return false;
        }

        private void Log(String s)
        {
            //PathGraph.Log(s);
            PathGraph.Log(s);
        }
    }
}