using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;
using cleanLayer.Library.Scripts;

namespace cleanLayer.Scripts
{
    public class PilgrimsBountry : Script
    {
        public PilgrimsBountry()
            : base("Pilgrim's Bounty", "Holiday")
        { }

        private bool NeedTraining = false;
        private bool Initialized = false;
        private int CookingIndex = 0;
        private WoWUnit Trainer = WoWUnit.Invalid;

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
