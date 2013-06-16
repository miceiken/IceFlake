using IceFlake.Client;
using IceFlake.Client.Objects;
using IceFlake.Client.Patchables;
using IceFlake.Client.Scripts;
using System;
using System.Linq;

namespace IceFlake.Scripts
{
    public class DebugScript : Script
    {
        public DebugScript()
            : base("Debug", "Script")
        { }

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            //var con = new WoWConsole();
            //con.Write("This is a test!", WoWConsoleColor.DEFAULT_COLOR);
        }
    }
}
