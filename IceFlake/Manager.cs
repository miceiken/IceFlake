using GreyMagic;
using IceFlake.Client;
using IceFlake.Client.Objects;
using IceFlake.Client.Collections;
using IceFlake.Client.Scripts;
using IceFlake.DirectX;
using IceFlake.Runtime;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;

namespace IceFlake
{
    internal static class Manager
    {
        internal static void Initialize()
        {
            Memory = new InProcessMemoryReader(Process.GetCurrentProcess());

            Direct3D.OnFirstFrame += Start;
            Direct3D.OnLastFrame += Stop;
            Direct3D.Initialize();
        }

        internal static void Start(object sender, EventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();

            AssemblyAnalyzer.RegisterTargets(
                ObjectManager = new ObjectManager(),
                ExecutionQueue = new EndSceneExecute(),
                Movement = new Movement(),
                Events = new Events(),
                Spellbook = new SpellCollection(),
                Scripts = new ScriptManager()
                );
            AssemblyAnalyzer.Analyze(Assembly.GetExecutingAssembly());

            Helper.Initialize();
            DBC = new WoWDB();            
            Camera = new Camera();            

            sw.Stop();
            Log.WriteLine("Initialization took {0} ms", sw.ElapsedMilliseconds);
        }

        internal static void Stop(object sender, EventArgs e)
        {
            Log.WriteLine("Shutting down IceFlake");
            Events = null;
            Spellbook = null;
            Movement = null;
            DBC = null;
            ObjectManager = null;
            ExecutionQueue = null;

            Memory.Detours.RemoveAll();
            Memory.Patches.RemoveAll();

            Memory = null;

            GC.Collect();
            //Environment.Exit(1);
        }

        internal static InProcessMemoryReader Memory { get; private set; }

        internal static ObjectManager ObjectManager { get; private set; }

        internal static WoWLocalPlayer LocalPlayer
        {
            get { return ObjectManager.LocalPlayer; }
        }

        internal static EndSceneExecute ExecutionQueue { get; private set; }

        internal static WoWDB DBC { get; private set; }

        internal static Movement Movement { get; private set; }

        internal static SpellCollection Spellbook { get; private set; }

        internal static Camera Camera { get; private set; }

        internal static Events Events { get; private set; }

        internal static ScriptManager Scripts { get; private set; }
    }
}
