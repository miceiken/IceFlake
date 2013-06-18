using IceFlake.Client;
using IceFlake.Client.Objects;
using IceFlake.Client.Patchables;
using IceFlake.Client.Scripts;
using System;
using System.Linq;
using IceFlake.Client.Patchables;

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

            Print("Death Knight Runes:");
            var runeStates = Manager.Memory.Read<uint>((IntPtr)Pointers.LocalPlayer.RuneState);
            for (var i = 0; i < 6; i++)
            {
                var runeType = Manager.Memory.Read<int>(new IntPtr(Pointers.LocalPlayer.RuneType + i * 4));
                var runeReady = (runeStates & (1 << i)) > 0;
                var runeCooldown = Manager.Memory.Read<int>(new IntPtr(Pointers.LocalPlayer.RuneCooldown + i * 4));
                Print("\t {0}: {1} {2}", (RuneType)runeType, runeReady ? "Ready" : "Not ready", runeCooldown);
            }
        }
    }
}