/*
 *  Part of PPather
 *  Copyright Pontus Borg 2008
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;
using PatherPath.Graph;
using StormDll;
using File = System.IO.File;

namespace Wmo
{
    internal class Dbg
    {
        public static void Log(string s)
        {
            // Console.Write(s);
        }

        public static void LogLine(string s)
        {
            // PathGraph.Log(s);
        }
    }

    internal unsafe class ChunkReader
    {
        public static float TILESIZE = 533.33333f;
        public static float ZEROPOINT = (32.0f*(TILESIZE));
        public static float CHUNKSIZE = ((TILESIZE)/16.0f);
        public static float UNITSIZE = (CHUNKSIZE/8.0f);


        public static uint MWMO = ToBin("MWMO");
        public static uint MODF = ToBin("MODF");
        public static uint MAIN = ToBin("MAIN");
        public static uint MPHD = ToBin("MPHD");


        public static uint CBDW = ToBin("CBDW");

        public static uint MVER = ToBin("MVER");
        public static uint MOGI = ToBin("MOGI");
        public static uint MOHD = ToBin("MOHD");
        public static uint MOTX = ToBin("MOTX");
        public static uint MOMT = ToBin("MOMT");
        public static uint MOGN = ToBin("MOGN");
        public static uint MOLT = ToBin("MOLT");
        public static uint MODN = ToBin("MODN");
        public static uint MODS = ToBin("MODS");
        public static uint MODD = ToBin("MODD");
        public static uint MOSB = ToBin("MOSB");
        public static uint MOPV = ToBin("MOPV");
        public static uint MOPR = ToBin("MOPR");
        public static uint MFOG = ToBin("MFOG");

        public static uint MOGP = ToBin("MOGP");
        public static uint MOPY = ToBin("MOPY");
        public static uint MOVI = ToBin("MOVI");

        public static uint MOVT = ToBin("MOVT");
        public static uint MONR = ToBin("MONR");
        public static uint MOLR = ToBin("MOLR");
        public static uint MODR = ToBin("MODR");
        public static uint MOBA = ToBin("MOBA");
        public static uint MOCV = ToBin("MOCV");
        public static uint MLIQ = ToBin("MLIQ");
        public static uint MOBN = ToBin("MOBN");
        public static uint MOBR = ToBin("MOBR");


        public static uint MCIN = ToBin("MCIN");
        public static uint MTEX = ToBin("MTEX");
        public static uint MMDX = ToBin("MMDX");

        public static uint MDDF = ToBin("MDDF");
        public static uint MCNK = ToBin("MCNK");

        public static uint MCNR = ToBin("MCNR");
        public static uint MCRF = ToBin("MCRF");
        public static uint MCVT = ToBin("MCVT");
        public static uint MCLY = ToBin("MCLY");
        public static uint MCSH = ToBin("MCSH");
        public static uint MCAL = ToBin("MCAL");
        public static uint MCLQ = ToBin("MCLQ");
        public static uint MCSE = ToBin("MCSE");

        public static uint ToBin(String s)
        {
            char[] ca = s.ToCharArray();
            uint b0 = ca[0];
            uint b1 = ca[1];
            uint b2 = ca[2];
            uint b3 = ca[3];
            uint r = b3 | (b2 << 8) | (b1 << 16) | (b0 << 24);
            return r;
        }

        public static string ReadString(BinaryReader file)
        {
            var bytes = new char[1024];
            int len = 0;
            sbyte b = 0;
            do
            {
                b = file.ReadSByte();
                bytes[len] = (char) b;
                len++;
            } while (b != 0);

            var s = new string(bytes, 0, len - 1);
            return s;
        }


        public static string ExtractString(byte[] b, int off)
        {
            string s;
            fixed (byte* bp = b)
            {
                var sp = (sbyte*) bp;
                sp += off;
                s = new string(sp);
            }

            return s;
        }
    }

    public class DBC
    {
        public uint fieldCount;
        public uint[] rawRecords;
        public uint recordCount;
        public uint recordSize;
        public uint stringSize;

        public byte[] strings;

        public uint GetUint(int record, int id)
        {
            var recoff = (int) (record*fieldCount + id);
            return rawRecords[recoff];
        }

        public int GetInt(int record, int id)
        {
            var recoff = (int) (record*fieldCount + id);
            return (int) rawRecords[recoff];
        }

        public string GetString(int record, int id)
        {
            var recoff = (int) (record*fieldCount + id);
            return ChunkReader.ExtractString(strings, (int) rawRecords[recoff]);
        }
    }

    public class Vec3D
    {
        public float x, y, z;

        public Vec3D(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
        {
            return x + " " + y + " " + z;
        }
    }


    public class WMOManager : Manager<WMO>
    {
        private readonly ModelManager modelmanager;
        private readonly ArchiveSet set;

        public WMOManager(ArchiveSet set, ModelManager modelmanager, int maxItems)
            : base(maxItems)
        {
            this.set = set;
            this.modelmanager = modelmanager;
        }

        public override WMO Load(String path)
        {
            string localPath = "PPather\\wmo.tmp";
            Dbg.Log(" wmo");
            set.ExtractFile(path, localPath);
            var w = new WMO();
            w.fileName = path;

            var wrf = new WmoRootFile(localPath, w, modelmanager);

            for (int i = 0; i < w.groups.Length; i++)
            {
                string part = path.Substring(0, path.Length - 4);
                string gf = String.Format("{0}_{1,3:000}.wmo", part, i);
                Dbg.Log(" wmog");
                set.ExtractFile(gf, localPath);
                new WmoGroupFile(w.groups[i], localPath);
            }
            return w;
        }
    }

    public class WMOInstance
    {
        public int d2, d3;
        public Vec3D dir;
        public int doodadset;
        public int id;
        public Vec3D pos, pos2, pos3;
        public WMO wmo;


        public WMOInstance(WMO wmo, BinaryReader file)
        {
            // read X bytes from file
            this.wmo = wmo;

            id = file.ReadInt32();
            {
                float f0 = file.ReadSingle();
                float f1 = file.ReadSingle();
                float f2 = file.ReadSingle();
                pos = new Vec3D(f0, f1, f2);
            }
            {
                float f0 = file.ReadSingle();
                float f1 = file.ReadSingle();
                float f2 = file.ReadSingle();
                dir = new Vec3D(f0, f1, f2);
            }

            {
                float f0 = file.ReadSingle();
                float f1 = file.ReadSingle();
                float f2 = file.ReadSingle();
                pos2 = new Vec3D(f0, f1, f2);
            }
            {
                float f0 = file.ReadSingle();
                float f1 = file.ReadSingle();
                float f2 = file.ReadSingle();
                pos3 = new Vec3D(f0, f1, f2);
            }

            d2 = file.ReadInt32();
            doodadset = file.ReadInt16();
            short crap = file.ReadInt16();
        }
    }

    public struct DoodadSet
    {
        public uint firstInstance;
        public uint nInstances;
    }

    public class WMO
    {
        public byte[] MODNraw;
        public ModelInstance[] doodadInstances;
        public DoodadSet[] doodads;
        public string fileName = "";
        public WMOGroup[] groups;
        public uint nDoodadSets;
        public uint nDoodads;
        public uint nModels;
        //int nTextures, nGroups, nP, nLight nX;
        public Vec3D v1, v2; // bounding box

        //List<string> textures;
        //List<string> models;
        //Vector<ModelInstance> modelis;

        //Vector<WMOLight> lights;
        //List<WMOPV> pvs;
        //List<WMOPR> prs;

        //Vector<WMOFog> fogs;

        //Vector<WMODoodadSet> doodadsets;
    }


    public abstract class Manager<T>
    {
        private readonly Dictionary<string, T> items = new Dictionary<string, T>();
        private readonly Dictionary<string, int> items_LRU = new Dictionary<string, int>();

        private readonly int maxItems;
        private int NOW;

        public Manager(int maxItems)
        {
            this.maxItems = maxItems;
        }

        public abstract T Load(String path);

        private void EvictIfNeeded()
        {
            string toEvict = null;
            int toEvictLRU = Int32.MaxValue;
            if (items.Count > maxItems)
            {
                foreach (string path in items_LRU.Keys)
                {
                    int LRU = items_LRU[path];
                    if (LRU < toEvictLRU)
                    {
                        toEvictLRU = LRU;
                        toEvict = path;
                    }
                }
            }
            if (toEvict != null)
            {
                //                PathGraph.Log("Drop item : " + toEvict);
                items.Remove(toEvict);
                items_LRU.Remove(toEvict);
            }
        }

        public T AddAndLoadIfNeeded(string path)
        {
            path = path.ToLower();
            T w = Get(path);
            if (w == null)
            {
                EvictIfNeeded();
                w = Load(path);
                //Dbg.LogLine("need " + path); 
                if (w != null)
                    Add(path, w);
            }

            items_LRU.Remove(path);
            items_LRU.Add(path, NOW++);
            return w;
        }

        public void Add(string path, T wmo)
        {
            items.Add(path, wmo);
        }

        public T Get(string path)
        {
            T r;
            if (items.TryGetValue(path, out r))
                return r;
            return default(T);
        }
    }

    public class ModelManager : Manager<Model>
    {
        private readonly ArchiveSet set;

        public ModelManager(ArchiveSet set, int maxModels)
            : base(maxModels)
        {
            this.set = set;
        }

        public override Model Load(String path)
        {
            // change .mdx to .m2
            string file = path.Substring(0, path.Length - 4) + ".m2";

            //PathGraph.Log("Load model " + path);
            string localPath = "PPather\\model.tmp";
            Dbg.Log(" m");
            if (set.ExtractFile(file, localPath))
            {
                var w = new Model();
                w.fileName = file;
                var wrf = new ModelFile(localPath, w);
                return w;
            }
            return null;
        }
    }

    public class ModelInstance
    {
        public Vec3D dir;
        public Model model;
        public Vec3D pos;
        //bool w_is_set = false;
        public float sc;
        public float w;

        public ModelInstance(Model m, BinaryReader file)
        {
            model = m;
            uint d1 = file.ReadUInt32();
            {
                float f0 = file.ReadSingle();
                float f1 = file.ReadSingle();
                float f2 = file.ReadSingle();
                pos = new Vec3D(f0, f1, f2);
            }
            {
                float f0 = file.ReadSingle();
                float f1 = file.ReadSingle();
                float f2 = file.ReadSingle();
                dir = new Vec3D(f0, f1, f2);
            }
            uint scale = file.ReadUInt32();
            sc = scale/1024.0f;
        }

        public ModelInstance(Model m, Vec3D pos, Vec3D dir, float sc, float w)
        {
            model = m;
            this.pos = pos;
            this.dir = dir;
            this.sc = sc;
            this.w = w;
            //w_is_set = true;
        }
    }

    public class ModelView
    {
        public UInt16[] indexList;
        public uint offIndex;

        public uint offTriangle;
        public UInt16[] triangleList;
    }

    public class Model
    {
        public UInt16[] boundingTriangles;
        public float[] boundingVertices; // 3 per vertex
        public string fileName = "";
        // 4 bytes header
        // 4 bytes version
        //uint model_name_length; // (including \0); 
        //uint model_name_offset; 

        public float[] vertices; // 3 per vertex

        public ModelView[] view;
    }

    public class ModelFile
    {
        private readonly BinaryReader file;
        private readonly Model model;
        private readonly Stream stream;

        public ModelFile(string path, Model m)
        {
            model = m;
            stream = File.OpenRead(path);
            file = new BinaryReader(stream);
            try
            {
                ReadHeader();
            }
            catch (EndOfStreamException)
            {
            }
            file.Close();
            stream.Close();
        }

        private void ReadHeader()
        {
            uint header = file.ReadUInt32(); // (including \0); 
            if (header != 0x3032444d)
            {
                PathGraph.Log("Bad header in m2 file: " + header);
                return;
            }
            uint version = file.ReadUInt32(); // (including \0); 
            uint model_name_length = file.ReadUInt32(); // (including \0); 
            uint model_name_offset = file.ReadUInt32();
            uint Modeltype = file.ReadUInt32(); // ? always 0, 1 or 3 (mostly 0); 
            uint nGlobalSequences = file.ReadUInt32(); //  - number of global sequences; 
            uint ofsGlobalSequences = file.ReadUInt32(); //  - offset to global sequences; 
            uint nAnimations = file.ReadUInt32(); //  - number of animation sequences; 
            uint ofsAnimations = file.ReadUInt32(); //  - offset to animation sequences; 
            uint nC = file.ReadUInt32(); // ; 
            uint ofsC = file.ReadUInt32(); // ; 
            uint nD = file.ReadUInt32(); //  - always 201 or 203 depending on WoW client version; 
            uint ofsD = file.ReadUInt32();
            uint nBones = file.ReadUInt32(); //  - number of bones; 
            uint ofsBones = file.ReadUInt32(); //  - offset to bones; 
            uint nF = file.ReadUInt32(); //  - bone lookup table; 
            uint ofsF = file.ReadUInt32();
            uint nVertices = file.ReadUInt32(); //  - number of vertices; 
            uint ofsVertices = file.ReadUInt32(); //  - offset to vertices; 
            uint nViews = file.ReadUInt32(); //  - number of views (LOD versions?) 4 for every model; 
            uint ofsViews = file.ReadUInt32(); //  - offset to views; 
            uint nColors = file.ReadUInt32(); //  - number of color definitions; 
            uint ofsColors = file.ReadUInt32(); //  - offset to color definitions; 
            uint nTextures = file.ReadUInt32(); //  - number of textures; 
            uint ofsTextures = file.ReadUInt32(); //  - offset to texture definitions; 
            uint nTransparency = file.ReadUInt32(); //  - number of transparency definitions; 
            uint ofsTransparency = file.ReadUInt32(); //  - offset to transparency definitions; 
            uint nI = file.ReadUInt32(); //  - always 0; 
            uint ofsI = file.ReadUInt32();
            uint nTexAnims = file.ReadUInt32(); //  - number of texture animations; 
            uint ofsTexAnims = file.ReadUInt32(); //  - offset to texture animations; 
            uint nTexReplace = file.ReadUInt32();
            uint ofsTexReplace = file.ReadUInt32();
            uint nRenderFlags = file.ReadUInt32(); //  - number of blending mode definitions; 
            uint ofsRenderFlags = file.ReadUInt32(); //  - offset to blending mode definitions; 
            uint nY = file.ReadUInt32(); //  - bone lookup table; 
            uint ofsY = file.ReadUInt32();
            uint nTexLookup = file.ReadUInt32(); //  - number of texture lookup table entries; 
            uint ofsTexLookup = file.ReadUInt32(); //  - offset to texture lookup table; 
            uint nTexUnits = file.ReadUInt32(); //  - texture unit definitions?; 
            uint ofsTexUnits = file.ReadUInt32();
            uint nTransLookup = file.ReadUInt32(); //  - number of transparency lookup table entries; 
            uint ofsTransLookup = file.ReadUInt32(); //  - offset to transparency lookup table; 
            uint nTexAnimLookup = file.ReadUInt32(); //  - number of texture animation lookup table entries; 
            uint ofsTexAnimLookup = file.ReadUInt32(); //  - offset to texture animation lookup table; 
            for (int i = 0; i < 14; i++)
                file.ReadSingle();

            uint nBoundingTriangles = file.ReadUInt32();
            uint ofsBoundingTriangles = file.ReadUInt32();
            uint nBoundingVertices = file.ReadUInt32();
            uint ofsBoundingVertices = file.ReadUInt32();
            uint nBoundingNormals = file.ReadUInt32();
            uint ofsBoundingNormals = file.ReadUInt32();
            uint nAttachments = file.ReadUInt32();
            uint ofsAttachments = file.ReadUInt32();
            uint nAttachLookup = file.ReadUInt32();
            uint ofsAttachLookup = file.ReadUInt32();
            uint nAttachments_2 = file.ReadUInt32();
            uint ofsAttachments_2 = file.ReadUInt32();
            uint nLights = file.ReadUInt32(); //  - number of lights; 
            uint ofsLights = file.ReadUInt32(); //  - offset to lights; 
            uint nCameras = file.ReadUInt32(); //  - number of cameras; 
            uint ofsCameras = file.ReadUInt32(); //  - offset to cameras; 
            uint nCameraLookup = file.ReadUInt32();
            uint ofsCameraLookup = file.ReadUInt32();
            uint nRibbonEmitters = file.ReadUInt32(); //  - number of ribbon emitters; 
            uint ofsRibbonEmitters = file.ReadUInt32(); //  - offset to ribbon emitters; 
            uint nParticleEmitters = file.ReadUInt32(); //  - number of particle emitters; 
            uint ofsParticleEmitters = file.ReadUInt32(); //  - offset to particle emitters; 

            //model.views = new ModelView[nViews];
            model.view = null; // ReadViews(nViews, ofsViews);
            //model.nVertices = nVertices;
            model.vertices = ReadVertices(nVertices, ofsVertices);

            //model.nBoundingTriangles = nBoundingTriangles;
            model.boundingTriangles = ReadBoundingTriangles(nBoundingTriangles, ofsBoundingTriangles);
            //model.nBoundingVertices = nBoundingVertices;
            model.boundingVertices = ReadBoundingVertices(nBoundingVertices, ofsBoundingVertices);
        }

        private float[] ReadBoundingVertices(uint nVertices, uint ofsVertices)
        {
            if (nVertices == 0)
                return null;
            file.BaseStream.Seek(ofsVertices, SeekOrigin.Begin);
            var vertices = new float[nVertices*3];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = file.ReadSingle();
            }
            return vertices;
        }

        private UInt16[] ReadBoundingTriangles(uint nTriangles, uint ofsTriangles)
        {
            if (nTriangles == 0)
                return null;
            file.BaseStream.Seek(ofsTriangles, SeekOrigin.Begin);
            var triangles = new UInt16[nTriangles];
            for (int i = 0; i < triangles.Length; i++)
            {
                triangles[i] = file.ReadUInt16();
            }
            return triangles;
        }


        private float[] ReadVertices(uint nVertices, uint ofcVertices)
        {
            var vertices = new float[nVertices*3];
            file.BaseStream.Seek(ofcVertices, SeekOrigin.Begin);
            for (int i = 0; i < nVertices; i++)
            {
                vertices[i*3 + 0] = file.ReadSingle();
                vertices[i*3 + 1] = file.ReadSingle();
                vertices[i*3 + 2] = file.ReadSingle();
                file.ReadUInt32(); // bone weights
                file.ReadUInt32(); // bone indices

                file.ReadSingle(); // normal *3
                file.ReadSingle();
                file.ReadSingle();

                file.ReadSingle(); // texture coordinates
                file.ReadSingle();

                file.ReadSingle(); // some crap
                file.ReadSingle();
            }
            return vertices;
        }

        private ModelView[] ReadViews(uint nViews, uint offset)
        {
            file.BaseStream.Seek(offset, SeekOrigin.Begin);
            var views = new ModelView[nViews];
            for (uint i = 0; i < nViews; i++)
            {
                views[i] = new ModelView();
                uint nIndex = file.ReadUInt32();
                uint offIndex = file.ReadUInt32();
                views[i].offIndex = offIndex;
                views[i].indexList = new UInt16[nIndex];

                uint nTriangle = file.ReadUInt32();
                uint offTriangle = file.ReadUInt32();
                views[i].offTriangle = offTriangle;
                views[i].triangleList = new UInt16[nTriangle];

                uint nVertexProp = file.ReadUInt32();
                uint offVertexProp = file.ReadUInt32();

                uint nSubMesh = file.ReadUInt32();
                uint offSubMesh = file.ReadUInt32();

                uint nTexture = file.ReadUInt32();
                uint offTexture = file.ReadUInt32();

                file.ReadUInt32(); // some crap
            }
            for (uint i = 0; i < nViews; i++)
            {
                ReadView(views[i]);
            }
            return views;
        }


        private void ReadView(ModelView view)
        {
            file.BaseStream.Seek(view.offIndex, SeekOrigin.Begin);
            for (int i = 0; i < view.indexList.Length; i++)
            {
                view.indexList[i] = file.ReadUInt16();
            }
            file.BaseStream.Seek(view.offTriangle, SeekOrigin.Begin);
            for (int i = 0; i < view.triangleList.Length; i++)
            {
                view.triangleList[i] = file.ReadUInt16();
            }
        }
    }


    public class WMOGroup
    {
        public const UInt16 MAT_FLAG_NOCAMCOLLIDE = 0x001;
        public const UInt16 MAT_FLAG_DETAIL = 0x002;
        public const UInt16 MAT_FLAG_COLLISION = 0x004;
        public const UInt16 MAT_FLAG_HINT = 0x008;
        public const UInt16 MAT_FLAG_RENDER = 0x010;
        public const UInt16 MAT_FLAG_COLLIDE_HIT = 0x020;
        public UInt16 batchesA;
        public UInt16 batchesB;
        public UInt16 batchesC;
        public UInt16 batchesD;
        public uint flags;
        public uint id;
        public UInt16[] materials; // 1 per triangle


        public uint nTriangles;
        public uint nVertices;
        public uint nameStart, nameStart2;
        public UInt16 portalCount;
        public UInt16 portalStart;
        public UInt16[] triangles; // 3 per triangle
        public Vec3D v1;
        public Vec3D v2;
        public float[] vertices; // 3 per vertex
    }


    internal class WDT
    {
        public int gnWMO;
        public List<WMOInstance> gwmois = new List<WMOInstance>();
        public List<string> gwmos = new List<string>();
        public bool[,] maps = new bool[64,64];

        public MapTile[,] maptiles = new MapTile[64,64];
        public int nMaps;
    }

    internal class WDTFile
    {
        private readonly ArchiveSet archive;
        private readonly BinaryReader file;
        private readonly ModelManager modelmanager;
        private readonly string name;
        private readonly Stream stream;
        private readonly WDT wdt;
        private readonly WMOManager wmomanager;
        public bool loaded = false;


        public WDTFile(ArchiveSet archive, string name,
                       WDT wdt, WMOManager wmomanager, ModelManager modelmanager)
        {
            string wdtfile = "World\\Maps\\" + name + "\\" + name + ".wdt";
            Dbg.Log(" wdt");
            if (!archive.ExtractFile(wdtfile, "PPather\\wdt.tmp"))
                return;

            loaded = true;
            this.name = name;
            this.wdt = wdt;
            this.wmomanager = wmomanager;
            this.modelmanager = modelmanager;
            this.archive = archive;

            stream = File.OpenRead("PPather\\wdt.tmp");
            file = new BinaryReader(stream);

            bool done = false;
            do
            {
                try
                {
                    uint type = file.ReadUInt32();
                    uint size = file.ReadUInt32();
                    long curpos = file.BaseStream.Position;

                    if (type == ChunkReader.MVER)
                    {
                    }
                    else if (type == ChunkReader.MPHD)
                    {
                    }
                    else if (type == ChunkReader.MODF)
                    {
                        HandleMODF(size);
                    }
                    else if (type == ChunkReader.MWMO)
                    {
                        HandleMWMO(size);
                    }
                    else if (type == ChunkReader.MAIN)
                    {
                        HandleMAIN(size);
                    }
                    else
                    {
                        PathGraph.Log("WDT Unknown " + type);
                        //done = true; 
                    }
                    file.BaseStream.Seek(curpos + size, SeekOrigin.Begin);
                }
                catch (EndOfStreamException)
                {
                    done = true;
                }
            } while (!done);

            file.Close();
            stream.Close();

            // load map tiles
        }

        public void LoadMapTile(int x, int y)
        {
            if (wdt.maps[x, y])
            {
                var t = new MapTile();


                string filename = "World\\Maps\\" + name + "\\" + name + "_" + x + "_" + y + ".adt";
                Dbg.Log(" adt");
                if (archive.ExtractFile(filename, "PPather\\adt.tmp"))
                {
                    var f = new MapTileFile("PPather\\adt.tmp", t, wmomanager, modelmanager);
                    if (t.models.Count != 0 || t.wmos.Count != 0)
                    {
                        //PathGraph.Log(name + " " + x + " " + z + " models: " + t.models.Count + " wmos: " + t.wmos.Count);
                        // Weee
                    }
                    wdt.maptiles[x, y] = t;
                }
            }
        }

        private void HandleMWMO(uint size)
        {
            if (size != 0)
            {
                int l = 0;
                byte[] raw = file.ReadBytes((int) size);
                while (l < size)
                {
                    string s = ChunkReader.ExtractString(raw, l);
                    l += s.Length + 1;
                    wdt.gwmos.Add(s);
                }
            }
        }

        private void HandleMODF(uint size)
        {
            // global wmo instance data
            wdt.gnWMO = (int) size/64;
            for (uint i = 0; i < wdt.gnWMO; i++)
            {
                int id = file.ReadInt32();
                string path = wdt.gwmos[id];

                WMO wmo = wmomanager.AddAndLoadIfNeeded(path);

                var wmoi = new WMOInstance(wmo, file);
                wdt.gwmois.Add(wmoi);
            }
        }

        private void HandleMAIN(uint size)
        {
            // global map objects
            for (int j = 0; j < 64; j++)
            {
                for (int i = 0; i < 64; i++)
                {
                    int d = file.ReadInt32();
                    if (d != 0)
                    {
                        wdt.maps[i, j] = true;
                        wdt.nMaps++;
                    }
                    else
                        wdt.maps[i, j] = false;
                    file.ReadInt32(); // kasta
                }
            }
        }
    }

    internal class DBCFile
    {
        private readonly DBC dbc;
        private readonly BinaryReader file;
        private readonly Stream stream;

        public DBCFile(string name, DBC dbc)
        {
            this.dbc = dbc;
            stream = File.OpenRead(name);
            file = new BinaryReader(stream);

            bool done = false;
            do
            {
                try
                {
                    uint type = file.ReadUInt32();
                    //uint size = file.ReadUInt32();
                    //long curpos = file.BaseStream.Position;

                    if (type == ChunkReader.CBDW)
                    {
                        HandleWDBC();
                    }
                    else
                    {
                        PathGraph.Log("DBC Unknown " + type);
                        //done = true; 
                    }
                    //file.BaseStream.Seek(curpos + size, System.IO.SeekOrigin.Begin);
                }
                catch (EndOfStreamException)
                {
                    done = true;
                }
            } while (!done);

            file.Close();
            stream.Close();
        }

        private void HandleWDBC()
        {
            dbc.recordCount = file.ReadUInt32();

            dbc.fieldCount = file.ReadUInt32(); // words per record
            dbc.recordSize = file.ReadUInt32();
            dbc.stringSize = file.ReadUInt32();

            if (dbc.fieldCount*4 != dbc.recordSize)
            {
                // !!!
                PathGraph.Log("WOOT");
            }
            int off = 0;
            var raw = new uint[dbc.fieldCount*dbc.recordCount];
            for (uint i = 0; i < dbc.recordCount; i++)
            {
                for (int j = 0; j < dbc.fieldCount; j++)
                {
                    raw[off++] = file.ReadUInt32();
                }
            }
            dbc.rawRecords = raw;

            byte[] b = file.ReadBytes((int) dbc.stringSize);
            dbc.strings = b;
        }
    }

    internal class MapChunk
    {
        // public int nTextures;

        //public float r;

        private static readonly int[] holetab_h = new[] {0x1111, 0x2222, 0x4444, 0x8888};
        private static readonly int[] holetab_v = new[] {0x000F, 0x00F0, 0x0F00, 0xF000};
        public uint areaID;

        // public bool visible;
        public bool hasholes;
        public bool haswater;
        public uint holes;

        //public float waterlevel;

        //  0   1   2   3   4   5   6   7   8
        //    9  10  11  12  13  14  15  16
        // 17  18  19  20  21  22  23  24  25
        // ...
        public float[] vertices = new float[3*((9*9) + (8*8))];
        public byte[,] water_flags;
        public float[,] water_height;

        public float water_height1;
        public float water_height2;
        public float xbase, ybase, zbase;

        // 0 ..3, 0 ..3
        public bool isHole(int i, int j)
        {
            if (!hasholes)
                return false;
            i /= 2;
            j /= 2;
            if (i > 3 || j > 3)
                return false;
            //if(holes != 0)
            //    Console.Write("Someone checking for holes " + i + " " + j); 
            bool r = (holes & holetab_h[i] & holetab_v[j]) != 0;

            return r;
        }


        //TextureID textures[4];
        //TextureID alphamaps[3];
        //TextureID shadow, blend;
        //int animated[4];


        //short *strip;
        //int striplen;
        //Liquid *lq;
    }


    internal class MapTile
    {
        // public int x, z; // matches maps in WDT

        public MapChunk[,] chunks = new MapChunk[16,16];
        public List<ModelInstance> modelis = new List<ModelInstance>();
        public List<string> models = new List<string>();
        public List<WMOInstance> wmois = new List<WMOInstance>();

        public List<string> wmos = new List<string>();
    }

    internal class MapTileFile // adt file
    {
        private readonly BinaryReader file;
        private readonly int[] mcnk_offsets = new int[256];
        private readonly int[] mcnk_sizes = new int[256];
        private readonly ModelManager modelmanager;
        private readonly Stream stream;
        private readonly MapTile tile;
        private readonly WMOManager wmomanager;


        public MapTileFile(string name, MapTile tile, WMOManager wmomanager, ModelManager modelmanager)
        {
            this.tile = tile;
            this.wmomanager = wmomanager;
            this.modelmanager = modelmanager;
            stream = File.OpenRead(name);
            file = new BinaryReader(stream);
            bool done = false;
            do
            {
                try
                {
                    uint type = file.ReadUInt32();
                    uint size = file.ReadUInt32();
                    long curpos = file.BaseStream.Position;

                    if (type == ChunkReader.MVER)
                    {
                        HandleMVER(size);
                    }
                    if (type == ChunkReader.MCIN)
                    {
                        HandleMCIN(size);
                    }
                    else if (type == ChunkReader.MTEX)
                    {
                        HandleMTEX(size);
                    }
                    else if (type == ChunkReader.MMDX)
                    {
                        HandleMMDX(size);
                    }
                    else if (type == ChunkReader.MWMO)
                    {
                        HandleMWMO(size);
                    }
                    else if (type == ChunkReader.MDDF)
                    {
                        HandleMDDF(size);
                    }
                    else if (type == ChunkReader.MODF)
                    {
                        HandleMODF(size);
                    }
                    else if (type == ChunkReader.MCNK)
                    {
                        //HandleMCNK(size);
                    }
                    else
                    {
                        //PathGraph.Log("MapTile Unknown " + type);
                        //done = true; 
                    }
                    file.BaseStream.Seek(curpos + size, SeekOrigin.Begin);
                }
                catch (EndOfStreamException)
                {
                    done = true;
                }
            } while (!done);

            for (int j = 0; j < 16; j++)
            {
                for (int i = 0; i < 16; i++)
                {
                    int off = mcnk_offsets[j*16 + i];
                    file.BaseStream.Seek(off, SeekOrigin.Begin);
                    //PathGraph.Log("Chunk " + i + " " + j + " at off " + off); 
                    var chunk = new MapChunk();
                    ReadMapChunk(chunk);
                    tile.chunks[j, i] = chunk;
                }
            }
            file.Close();
            stream.Close();
        }

        private void HandleMVER(uint size)
        {
        }

        private void HandleMCIN(uint size)
        {
            for (int i = 0; i < 256; i++)
            {
                mcnk_offsets[i] = file.ReadInt32();
                mcnk_sizes[i] = file.ReadInt32();
                file.ReadInt32(); // crap
                file.ReadInt32(); // crap
            }
        }

        private void HandleMTEX(uint size)
        {
        }

        private void HandleMMDX(uint size)
        {
            if (size != 0)
            {
                int l = 0;
                byte[] raw = file.ReadBytes((int) size);
                while (l < size)
                {
                    string s = ChunkReader.ExtractString(raw, l);
                    l += s.Length + 1;

                    tile.models.Add(s);
                }
            }
        }

        private void HandleMWMO(uint size)
        {
            if (size != 0)
            {
                int l = 0;
                byte[] raw = file.ReadBytes((int) size);
                while (l < size)
                {
                    string s = ChunkReader.ExtractString(raw, l);
                    l += s.Length + 1;

                    tile.wmos.Add(s);
                }
            }
        }

        private void HandleMDDF(uint size)
        {
            int nMDX = (int) size/36;

            for (int i = 0; i < nMDX; i++)
            {
                int id = file.ReadInt32();
                Model model = modelmanager.AddAndLoadIfNeeded(tile.models[id]);

                var mi = new ModelInstance(model, file);
                tile.modelis.Add(mi);
            }
        }

        private void HandleMODF(uint size)
        {
            int nWMO = (int) size/64;
            for (int i = 0; i < nWMO; i++)
            {
                int id = file.ReadInt32();
                WMO wmo = wmomanager.AddAndLoadIfNeeded(tile.wmos[id]);

                var wi = new WMOInstance(wmo, file);
                tile.wmois.Add(wi);
            }
        }


        private void ReadMapChunk(MapChunk chunk)
        {
            // Read away header and size
            uint crap_head = file.ReadUInt32();
            uint crap_size = file.ReadUInt32();

            // Each map chunk has 9x9 vertices, 
            // and in between them 8x8 additional vertices, several texture layers, normal vectors, a shadow map, etc.

            uint flags = file.ReadUInt32();
            uint ix = file.ReadUInt32();
            uint iy = file.ReadUInt32();
            uint nLayers = file.ReadUInt32();
            uint nDoodadRefs = file.ReadUInt32();
            uint ofsHeight = file.ReadUInt32();
            uint ofsNormal = file.ReadUInt32();
            uint ofsLayer = file.ReadUInt32();
            uint ofsRefs = file.ReadUInt32();
            uint ofsAlpha = file.ReadUInt32();
            uint sizeAlpha = file.ReadUInt32();
            uint ofsShadow = file.ReadUInt32();
            uint sizeShadow = file.ReadUInt32();
            uint areaid = file.ReadUInt32();
            uint nMapObjRefs = file.ReadUInt32();
            uint holes = file.ReadUInt32();
            ushort s1 = file.ReadUInt16();
            ushort s2 = file.ReadUInt16();
            uint d1 = file.ReadUInt32();
            uint d2 = file.ReadUInt32();
            uint d3 = file.ReadUInt32();
            uint predTex = file.ReadUInt32();
            uint nEffectDoodad = file.ReadUInt32();
            uint ofsSndEmitters = file.ReadUInt32();
            uint nSndEmitters = file.ReadUInt32();
            uint ofsLiquid = file.ReadUInt32();
            uint sizeLiquid = file.ReadUInt32();
            float zpos = file.ReadSingle();
            float xpos = file.ReadSingle();
            float ypos = file.ReadSingle();
            uint textureId = file.ReadUInt32();
            uint props = file.ReadUInt32();
            uint effectId = file.ReadUInt32();

            chunk.areaID = areaid;

            chunk.zbase = zpos;
            chunk.xbase = xpos;
            chunk.ybase = ypos;

            // correct the x and z values 
            chunk.zbase = -chunk.zbase + ChunkReader.ZEROPOINT;
            chunk.xbase = -chunk.xbase + ChunkReader.ZEROPOINT;

            chunk.hasholes = (holes != 0);
            chunk.holes = holes;

            bool debug = false;
            //PathGraph.Log("  " + zpos + " " + xpos + " " + ypos);
            bool done = false;
            do
            {
                try
                {
                    uint type = file.ReadUInt32();
                    uint size = file.ReadUInt32();
                    long curpos = file.BaseStream.Position;

                    if (type == ChunkReader.MCNR)
                    {
                        size = 0x1C0; // WTF
                        if (debug)
                            PathGraph.Log("MCNR " + size);
                        HandleChunkMCNR(chunk, size);
                    }
                    else if (type == ChunkReader.MCVT)
                    {
                        if (debug)
                            PathGraph.Log("MCVT " + size);
                        HandleChunkMCVT(chunk, size);
                    }
                    else if (type == ChunkReader.MCRF)
                    {
                        if (debug)
                            PathGraph.Log("MCRF " + size);
                        HandleChunkMCRF(chunk, size);
                    }
                    else if (type == ChunkReader.MCLY)
                    {
                        if (debug)
                            PathGraph.Log("MCLY " + size);
                        HandleChunkMCLY(chunk, size);
                    }
                    else if (type == ChunkReader.MCSH)
                    {
                        if (debug)
                            PathGraph.Log("MCSH " + size);
                        HandleChunkMCSH(chunk, size);
                    }
                    else if (type == ChunkReader.MCAL)
                    {
                        if (debug)
                            PathGraph.Log("MCAL " + size);
                        HandleChunkMCAL(chunk, size);
                        // TODO rumors are that the size of this chunk is wrong sometimes
                    }
                    else if (type == ChunkReader.MCLQ)
                    {
                        size = sizeLiquid;
                        if (debug)
                            PathGraph.Log("MCLQ " + size);
                        if (sizeLiquid != 8)
                        {
                            chunk.haswater = true;
                            HandleChunkMCLQ(chunk, size);
                            //done = true; // size if fucked up, give up
                        }
                    }
                    else if (type == ChunkReader.MCSE)
                    {
                        if (debug)
                            PathGraph.Log("MCSE " + size);
                        HandleChunkMCSE(chunk, size);
                    }
                    else if (type == ChunkReader.MCNK)
                    {
                        //PathGraph.Log("MCNK " + size);
                        done = true; // found next
                        //HandleChunkMCSE(chunk, size);
                    }
                    else
                    {
                        //PathGraph.Log("MapChunk Unknown " + type);
                        //done = true; 
                    }
                    file.BaseStream.Seek(curpos + size, SeekOrigin.Begin);
                }
                catch (EndOfStreamException)
                {
                    done = true;
                }
            } while (!done);
        }

        private void HandleChunkMCNR(MapChunk chunk, uint size)
        {
            // Normals
        }

        private void HandleChunkMCVT(MapChunk chunk, uint size)
        {
            // vertices
            int off = 0;
            for (int j = 0; j < 17; j++)
            {
                for (int i = 0; i < ((j%2 != 0) ? 8 : 9); i++)
                {
                    float h, xpos, zpos;
                    h = file.ReadSingle();
                    xpos = i*ChunkReader.UNITSIZE;
                    zpos = j*0.5f*ChunkReader.UNITSIZE;
                    if (j%2 != 0)
                    {
                        xpos += ChunkReader.UNITSIZE*0.5f;
                    }
                    float x = chunk.xbase + xpos;
                    float y = chunk.ybase + h;
                    float z = chunk.zbase + zpos;

                    chunk.vertices[off++] = x;
                    chunk.vertices[off++] = y;
                    chunk.vertices[off++] = z;
                }
            }
        }

        private void HandleChunkMCRF(MapChunk chunk, uint size)
        {
        }

        private void HandleChunkMCLY(MapChunk chunk, uint size)
        {
            // texture info
        }

        private void HandleChunkMCSH(MapChunk chunk, uint size)
        {
            // shadow map 64 x 64
        }


        private void HandleChunkMCAL(MapChunk chunk, uint size)
        {
            // alpha maps  64 x 64
        }

        private void HandleChunkMCLQ(MapChunk chunk, uint size)
        {
            chunk.water_height1 = file.ReadSingle();
            chunk.water_height2 = file.ReadSingle();

            chunk.water_height = new float[9,9];
            chunk.water_flags = new byte[8,8];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    UInt32 word1 = file.ReadUInt32();
                    // UInt32 word2 = file.ReadUInt32(); 
                    //Int16 unk1 = file.ReadInt16(); // ??
                    //Int16 unk2 = file.ReadInt16(); // ??
                    chunk.water_height[i, j] = file.ReadSingle(); //  word1 + word2; //  file.ReadSingle();
                }
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    chunk.water_flags[i, j] = file.ReadByte();
                }
            }
        }

        private void HandleChunkMCSE(MapChunk chunk, uint size)
        {
            // Sound emitters
        }
    }

    internal class WmoRootFile
    {
        private readonly BinaryReader file;
        private readonly ModelManager modelmanager;
        private readonly Stream stream;

        public WMO wmo;
        //   string groupnames; 

        public WmoRootFile(string name, WMO wmo, ModelManager modelmanager)
        {
            this.wmo = wmo;
            this.modelmanager = modelmanager;
            stream = File.OpenRead(name);
            file = new BinaryReader(stream);
            bool done = false;
            do
            {
                try
                {
                    uint type = file.ReadUInt32();
                    uint size = file.ReadUInt32();
                    long curpos = file.BaseStream.Position;

                    if (type == ChunkReader.MVER)
                    {
                        HandleMVER(size);
                    }
                    if (type == ChunkReader.MOHD)
                    {
                        HandleMOHD(size);
                    }
                    else if (type == ChunkReader.MOGP)
                    {
                        HandleMOGP(size);
                    }
                    else if (type == ChunkReader.MOGI)
                    {
                        HandleMOGI(size);
                    }
                    else if (type == ChunkReader.MODS)
                    {
                        HandleMODS(size);
                    }
                    else if (type == ChunkReader.MODD)
                    {
                        HandleMODD(size);
                    }
                    else if (type == ChunkReader.MODN)
                    {
                        HandleMODN(size);
                    }
                    else
                    {
                        //PathGraph.Log("Root Unknown " + type);
                        //done = true; 
                    }
                    file.BaseStream.Seek(curpos + size, SeekOrigin.Begin);
                }
                catch (EndOfStreamException)
                {
                    done = true;
                }
            } while (!done);
            file.Close();
            stream.Close();
        }

        private void HandleMVER(uint size)
        {
        }

        private void HandleMOHD(uint size)
        {
            uint nTextures = file.ReadUInt32();
            uint nGroups = file.ReadUInt32();
            uint nP = file.ReadUInt32();
            uint nLights = file.ReadUInt32();
            wmo.nModels = file.ReadUInt32();
            wmo.nDoodads = file.ReadUInt32();
            wmo.nDoodadSets = file.ReadUInt32();

            uint col = file.ReadUInt32();
            uint nX = file.ReadUInt32();

            float f0 = file.ReadSingle();
            float f1 = file.ReadSingle();
            float f2 = file.ReadSingle();
            wmo.v1 = new Vec3D(f0, f1, f2);

            float f3 = file.ReadSingle();
            float f4 = file.ReadSingle();
            float f5 = file.ReadSingle();
            wmo.v2 = new Vec3D(f3, f4, f5);

            wmo.groups = new WMOGroup[nGroups];
        }

        private void HandleMOGN(uint size)
        {
            // group name
            // groupnames = ChunkReader.ReadString(file);                       
        }

        private void HandleMODS(uint size)
        {
            wmo.doodads = new DoodadSet[wmo.nDoodadSets];
            for (int i = 0; i < wmo.nDoodadSets; i++)
            {
                byte[] name = file.ReadBytes(20); // set name; 
                wmo.doodads[i].firstInstance = file.ReadUInt32();
                wmo.doodads[i].nInstances = file.ReadUInt32();
                file.ReadUInt32();
            }
        }

        private void HandleMODD(uint size)
        {
            // 40 bytes per doodad instance, nDoodads entries.
            // While WMOs and models (M2s) in a map tile are rotated along the axes, 
            //  doodads within a WMO are oriented using quaternions! Hooray for consistency!
            /*
0x00 	uint32 		Offset to the start of the model's filename in the MODN chunk.
0x04 	3 * float 	Position (X,Z,-Y)
0x10 	float 		W component of the orientation quaternion
0x14 	3 * float 	X, Y, Z components of the orientaton quaternion
0x20 	float 		Scale factor
0x24 	4 * uint8 	(B,G,R,A) color. Unknown. It is often (0,0,0,255). (something to do with lighting maybe?)
             */

            uint sets = size/0x28;
            wmo.doodadInstances = new ModelInstance[wmo.nDoodads];
            for (int i = 0; i < sets /*wmo.nDoodads*/; i++)
            {
                uint nameOffsetInMODN = file.ReadUInt32(); // 0x00
                float posx = file.ReadSingle(); // 0x04
                float posz = file.ReadSingle(); // 0x08
                float posy = -file.ReadSingle(); // 0x0c

                float quatw = file.ReadSingle(); // 0x10

                float quatx = file.ReadSingle(); // 0x14
                float quaty = file.ReadSingle(); // 0x18
                float quatz = file.ReadSingle(); // 0x1c


                float scale = file.ReadSingle(); // 0x20

                file.ReadUInt32(); // lighning crap 0x24
                // float last = file.ReadSingle(); // 0x28


                String name = ChunkReader.ExtractString(wmo.MODNraw, (int) nameOffsetInMODN);
                Model m = modelmanager.AddAndLoadIfNeeded(name);

                var pos = new Vec3D(posx, posy, posz);
                var dir = new Vec3D(quatz, quaty, quatz);


                var mi = new ModelInstance(m, pos, dir, scale, quatw);
                wmo.doodadInstances[i] = mi;
            }
        }

        private void HandleMODN(uint size)
        {
            wmo.MODNraw = file.ReadBytes((int) size);
            // List of filenames for M2 (mdx) models that appear in this WMO.
        }

        private void HandleMOGP(uint size)
        {
        }

        private void HandleMOGI(uint size)
        {
            for (int i = 0; i < wmo.groups.Length; i++)
            {
                var g = new WMOGroup();
                wmo.groups[i] = g;

                g.flags = file.ReadUInt32();
                float f0 = file.ReadSingle();
                float f1 = file.ReadSingle();
                float f2 = file.ReadSingle();
                g.v1 = new Vec3D(f0, f1, f2);

                float f3 = file.ReadSingle();
                float f4 = file.ReadSingle();
                float f5 = file.ReadSingle();
                g.v2 = new Vec3D(f3, f4, f5);

                uint nameOfs = file.ReadUInt32();
            }
        }
    }

    internal class WmoGroupFile
    {
        private readonly BinaryReader file;
        private readonly WMOGroup g;
        private readonly Stream stream;
        //long indicesFileMarker; 
        public WmoGroupFile(WMOGroup group, string name)
        {
            g = group;
            stream = File.OpenRead(name);
            file = new BinaryReader(stream);

            file.BaseStream.Seek(0x14, SeekOrigin.Begin);
            HandleMOGP(11);

            file.BaseStream.Seek(0x58, SeekOrigin.Begin); // first chunk

            bool done = false;
            do
            {
                try
                {
                    uint type = file.ReadUInt32();
                    uint size = file.ReadUInt32();
                    long curpos = file.BaseStream.Position;
                    uint MVER = ChunkReader.ToBin("MVER");
                    if (type == ChunkReader.MVER)
                    {
                        HandleMVER(size);
                    }
                    if (type == ChunkReader.MOPY)
                    {
                        HandleMOPY(size);
                    }
                    else if (type == ChunkReader.MOVI)
                    {
                        HandleMOVI(size);
                    }
                    else if (type == ChunkReader.MOVT)
                    {
                        HandleMOVT(size);
                    }
                    else if (type == ChunkReader.MONR)
                    {
                        HandleMONR(size);
                    }
                    else if (type == ChunkReader.MOLR)
                    {
                        HandleMOLR(size);
                    }
                    else if (type == ChunkReader.MODR)
                    {
                        HandleMODR(size);
                    }
                    else if (type == ChunkReader.MOBA)
                    {
                        HandleMOBA(size);
                    }
                    else if (type == ChunkReader.MOCV)
                    {
                        HandleMOCV(size);
                    }
                    else if (type == ChunkReader.MLIQ)
                    {
                        HandleMLIQ(size);
                    }
                    else if (type == ChunkReader.MOBN)
                    {
                        HandleMOBN(size);
                    }
                    else if (type == ChunkReader.MOBR)
                    {
                        HandleMOBR(size);
                    }


                    else
                    {
                        //PathGraph.Log("Group Unknown " + type);
                        //done = true; 
                    }
                    file.BaseStream.Seek(curpos + size, SeekOrigin.Begin);
                }
                catch (EndOfStreamException)
                {
                    done = true;
                }
            } while (!done);


            file.Close();
            stream.Close();
        }

        private void HandleMVER(uint size)
        {
        }

        private void HandleMOPY(uint size)
        {
            g.nTriangles = size/2;
            // materials
            /*  0x01 - inside small houses and paths leading indoors
             *  0x02 - ???
             *  0x04 - set on indoor things and ruins
             *  0x08 - ???
             *  0x10 - ???
             *  0x20 - Always set?
             *  0x40 - sometimes set- 
             *  0x80 - ??? never set
             * 
             */

            g.materials = new ushort[g.nTriangles];

            for (int i = 0; i < g.nTriangles; i++)
            {
                g.materials[i] = file.ReadUInt16();
            }
        }

        private void HandleMOVI(uint size)
        {
            //indicesFileMarker = file.BaseStream.Position; 
            g.triangles = new UInt16[g.nTriangles*3];
            for (uint i = 0; i < g.nTriangles; i++)
            {
                uint off = i*3;
                g.triangles[off + 0] = file.ReadUInt16();
                g.triangles[off + 1] = file.ReadUInt16();
                g.triangles[off + 2] = file.ReadUInt16();
            }
        }

        private void HandleMOVT(uint size)
        {
            g.nVertices = size/12;
            // let's hope it's padded to 12 bytes, not 16...
            g.vertices = new float[g.nVertices*3];
            for (uint i = 0; i < g.nVertices; i++)
            {
                float f0 = file.ReadSingle();
                float f1 = file.ReadSingle();
                float f2 = file.ReadSingle();
                uint off = i*3;
                g.vertices[off + 0] = f0;
                g.vertices[off + 1] = f1;
                g.vertices[off + 2] = f2;
            }
        }

        private void HandleMONR(uint size)
        {
        }

        private void HandleMOLR(uint size)
        {
        }

        private void HandleMODR(uint size)
        {
        }

        private void HandleMOBA(uint size)
        {
        }

        private void HandleMOCV(uint size)
        {
        }

        private void HandleMLIQ(uint size)
        {
        }

        /*
        struct t_BSP_NODE
        {	
            public UInt16 planetype;          // unsure
            public Int16 child0;        // index of bsp child node(right in this array)   
            public Int16 child1;
            public UInt16 numfaces;  // num of triangle faces
            public UInt16 firstface; // index of the first triangle index(in  MOBR)
            public UInt16 nUnk;	          // 0
            public float fDist;    
        };*/

        private void HandleMOBN(uint size)
        {
            /*
            t_BSP_NODE bsp;
            uint items = size / 16;
            for (int i = 0; i < items; i++)
            {
                bsp.planetype = file.ReadUInt16();
                bsp.child0 = file.ReadInt16();
                bsp.child1 = file.ReadInt16();
                bsp.numfaces = file.ReadUInt16();
                bsp.firstface = file.ReadUInt16();
                bsp.nUnk = file.ReadUInt16();
                bsp.fDist = file.ReadSingle();

                PathGraph.Log("BSP node type: " + bsp.planetype);
                if (bsp.child0 == -1)
                {
                    PathGraph.Log("  faces: " + bsp.firstface + " " + bsp.numfaces);
                    
                }
                else
                {
                    PathGraph.Log("  children: " + bsp.child0 + " " + bsp.child1 + " dist "+ bsp.fDist); 
                }
            }*/
        }

        private void HandleMOBR(uint size)
        {
        }


        private void HandleMOGP(uint size)
        {
            g.nameStart = file.ReadUInt32();
            g.nameStart2 = file.ReadUInt32();
            g.flags = file.ReadUInt32();

            float bound1X = file.ReadSingle();
            float bound1Y = file.ReadSingle();
            float bound1Z = file.ReadSingle();
            g.v1 = new Vec3D(bound1X, bound1Y, bound1Z);

            float bound2X = file.ReadSingle();
            float bound2Y = file.ReadSingle();
            float bound2Z = file.ReadSingle();
            g.v2 = new Vec3D(bound1X, bound1Y, bound1Z);

            g.portalStart = file.ReadUInt16();
            g.portalCount = file.ReadUInt16();
            g.batchesA = file.ReadUInt16();
            g.batchesB = file.ReadUInt16();
            g.batchesC = file.ReadUInt16();
            g.batchesD = file.ReadUInt16();

            uint fogCrap = file.ReadUInt32();

            uint unknown1 = file.ReadUInt32();
            g.id = file.ReadUInt32();
            uint unknown2 = file.ReadUInt32();
            uint unknown3 = file.ReadUInt32();
        }
    }
}