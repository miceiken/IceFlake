using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.Client.Objects;

namespace IceFlake.Client.Routines.Combat
{
    public class HelpfulSpellRoutine : SpellRoutine
    {
        public HelpfulSpellRoutine(RoutineBrain brain, int priority, string spellName, float range)
            : base(brain, priority, spellName, range)
        { }

        public override void Execute()
        {
            ExecuteOnUnit(Brain.HelpfulTarget);
        }

        public void ExecuteOnUnit(WoWUnit unit)
        {
            Print("Casting {0} on {1}", SpellName, unit.Name);
            Manager.LocalPlayer.LookAt(unit.Location);
            Spell.Cast(unit);
            Sleep(1000);
        }

        public override bool IsWanted
        {
            get { return base.IsWanted && (Brain.HelpfulTarget != null && Brain.HelpfulTarget.IsValid) && Brain.HelpfulTarget.InLoS; }
        }
    }
}
