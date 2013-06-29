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
    public class RoutineScript : Script
    {
        public RoutineScript()
            : base("Starter Mage", "Routine")
        {
            routine = new StarterMageRoutine();
        }

        private RoutineBrain routine;

        public override void OnStart()
        {
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
