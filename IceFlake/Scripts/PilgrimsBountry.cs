using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.Client;
using IceFlake.Client.Scripts;
using IceFlake.Client.Objects;

namespace IceFlake.Scripts
{
    public class PilgrimsBountry : Script
    {
        public PilgrimsBountry()
            : base("Pilgrim's Bounty", "Holiday")
        { }

        private bool NeedTraining = false;
        private bool Initialized = false;
        private int CookingIndex = 0;
        private WoWUnit Trainer = null;

        public override void OnStart()
        {
            Initialized = false;
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
        }

        public override void OnTick()
        {
            if (!Initialized)
            {
                var profs = WoWScript.Execute("GetProfessions()");
                if (profs[3] == "nil")
                {
                    NeedTraining = true;
                    Print("No cooking skill found, need to train");
                }
                else
                {
                    CookingIndex = int.Parse(profs[3]);
                }
                Initialized = true;
            }
        }
    }
}
