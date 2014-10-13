using System;
using System.Diagnostics;
using System.Windows.Forms;
using GreyMagic;
using IceFlake.Client;
using IceFlake.Client.Collections;
using IceFlake.Client.Objects;
using IceFlake.Client.Patchables;
using IceFlake.Client.Scripts;
using IceFlake.DirectX;

namespace IceFlake
{
    public static class Manager
    {
        public static InProcessMemoryReader Memory { get; private set; }
        public static ObjectManager ObjectManager { get; private set; }
        public static EndSceneExecute ExecutionQueue { get; private set; }
        public static WoWDB DBC { get; private set; }
        public static Movement Movement { get; private set; }
        public static SpellCollection Spellbook { get; private set; }
        public static QuestCollection Quests { get; private set; }
        public static WoWInventory Inventory { get; private set; }
        public static WoWCamera Camera { get; private set; }
        public static WoWEvents Events { get; private set; }
        public static ScriptManager Scripts { get; private set; }
        public static WoWConsole Console { get; private set; }
        public static WoWClientServices ClientServices { get; private set; }

        public static WoWLocalPlayer LocalPlayer
        {
            get { return ObjectManager.LocalPlayer; }
        }

        public static void Initialize()
        {
            Memory = new InProcessMemoryReader(Process.GetCurrentProcess());

            Direct3D.OnFirstFrame += Start;
            Direct3D.OnLastFrame += Stop;
            Direct3D.Initialize();
        }

        public static void Start(object sender, EventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();

            Direct3D.RegisterCallbacks(
                ObjectManager = new ObjectManager(),
                ExecutionQueue = new EndSceneExecute(),
                Movement = new Movement(),
                Events = new WoWEvents(),
                Spellbook = new SpellCollection(),
                Scripts = new ScriptManager()
                );

            Helper.Initialize();
            Helper.FixInvalidPtrCheck();
            DBC = new WoWDB();
            Quests = new QuestCollection();
            Inventory = new WoWInventory();
            Camera = new WoWCamera();
            Console = new WoWConsole();
            ClientServices = new WoWClientServices();

            sw.Stop();
            Log.WriteLine(LogType.Good, "Initialization took {0} ms", sw.ElapsedMilliseconds);
        }

        public static void Stop(object sender, EventArgs e)
        {
            Log.WriteLine(LogType.Information, "Shutting down IceFlake");
        }

        public static void InvokeGUIThread(Action action)
        {
            EntryPoint.AppForm.Invoke(action);
        }
    }
}