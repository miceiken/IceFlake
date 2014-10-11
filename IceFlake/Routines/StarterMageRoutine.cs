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
            CheckForAndApplyBuff(Manager.LocalPlayer, "Arcane Intellect");
            CheckForAndApplyBuff(Manager.LocalPlayer, "Frost Armor");
        }

        protected override void OnAfterAction(RoutineAction action)
        {
        }

        protected void CheckForAndApplyBuff(WoWUnit target, string name, bool sleep = true)
        {
            WoWAura buff = target.Auras[name];
            if (buff != null && buff.IsValid)
                return;

            WoWSpell spell = Manager.Spellbook[name];
            if (spell == null || !spell.IsValid)
                return;

            Print("Buffing {0} with {1}", target.Name, name);
            spell.Cast(target);
            if (sleep)
                Sleep(1000);
        }
    }
}