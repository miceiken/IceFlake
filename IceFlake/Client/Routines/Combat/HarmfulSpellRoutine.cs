using IceFlake.Client.Objects;

namespace IceFlake.Client.Routines.Combat
{
    public class HarmfulSpellRoutine : SpellRoutine
    {
        public HarmfulSpellRoutine(RoutineBrain brain, int priority, string spellName, float range)
            : base(brain, priority, spellName, range)
        {
        }

        public override bool IsWanted
        {
            get
            {
                return base.IsWanted && (Brain.HarmfulTarget != null && Brain.HarmfulTarget.IsValid) &&
                       Brain.HarmfulTarget.InLoS;
            }
        }

        public override void Execute()
        {
            ExecuteOnUnit(Brain.HarmfulTarget);
        }

        public void ExecuteOnUnit(WoWUnit unit)
        {
            Print("Casting {0} on {1}", SpellName, unit.Name);
            Manager.LocalPlayer.LookAt(unit.Location);
            Spell.Cast(unit);
            Sleep(1000);
        }
    }
}