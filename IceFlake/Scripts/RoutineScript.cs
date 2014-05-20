using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.Client;
using IceFlake.Client.API;
using IceFlake.Client.Patchables;
using IceFlake.Client.Objects;
using IceFlake.Client.Scripts;
using IceFlake.Client.Routines;
using IceFlake.Routines;

namespace IceFlake.Scripts
{
    public class StarterMageRoutineScript : Script
    {
        public StarterMageRoutineScript()
            : base("Starter Mage", "Routine")
        {
            routine = new StarterMageRoutine();
        }

        private RoutineBrain routine;

        public override void OnStart()
        {
            if (Manager.LocalPlayer.Class != WoWClass.Mage)
                Stop();
            routine.Start();
        }

        public override void OnTick()
        {
        }

        public override void OnTerminate()
        {
            routine.Stop();
        }
    }

    public class ElementalShamanRoutineScript : Script
    {
        public ElementalShamanRoutineScript()
            : base("Elemental Shaman", "Routine")
        {
            routine = new ElementalShamanRoutine();
        }

        private RoutineBrain routine;

        public override void OnStart()
        {
            if (Manager.LocalPlayer.Class != WoWClass.Mage)
                Stop();
            routine.Start();
        }

        public override void OnTick()
        {
        }

        public override void OnTerminate()
        {
            routine.Stop();
        }
    }
}
