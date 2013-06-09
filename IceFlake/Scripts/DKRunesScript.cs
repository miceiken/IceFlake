using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.Library;
using IceFlake.Client.Scripts;
using IceFlake.Client;

namespace IceFlake.Scripts
{
    public class DKRunesScript : Script
    {
        public DKRunesScript()
            : base("DK Runes", "Uncatalogued")
        { }

        public override void OnStart()
        {
            if (!Manager.IsInGame || Manager.LocalPlayer.Class != WoWClass.DeathKnight)
                Stop();
        }

        public override void OnTick()
        {
            Print("Blood: {0}", Manager.LocalPlayer.BloodRunesReady);
            Print("Frost: {0}", Manager.LocalPlayer.FrostRunesReady);
            Print("Unholy: {0}", Manager.LocalPlayer.UnholyRunesReady);
            Sleep(1000);
        }
    }
}
