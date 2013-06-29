using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.Client;
using IceFlake.Client.Objects;
using IceFlake.Client.Routines;
using IceFlake.Client.Routines.Combat;

namespace IceFlake.Routines
{
    public class StarterMageRoutine : RoutineBrain
    {
        public StarterMageRoutine()
        {
            AddAction(new HarmfulSpellRoutine(this, 1, "Fireball", 15f));
        }

        protected override void OnBeforeAction(RoutineAction action)
        {
        }

        protected override void OnAfterAction(RoutineAction action)
        {
        }
    }
}
