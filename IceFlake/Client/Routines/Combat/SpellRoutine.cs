using IceFlake.Client.Objects;

namespace IceFlake.Client.Routines.Combat
{
    public abstract class SpellRoutine : RoutineAction
    {
        public SpellRoutine(RoutineBrain brain, int priority, string spellName, float range = 5f)
            : base(brain, priority)
        {
            SpellName = spellName;
            Range = range;
        }

        public override bool IsWanted
        {
            get { return base.IsWanted && Manager.ObjectManager.IsInGame && !Manager.LocalPlayer.IsCasting; }
        }

        public override bool IsReady
        {
            get { return base.IsReady && (Spell != null && Spell.IsValid) && Spell.IsReady; }
        }

        public WoWSpell Spell
        {
            get { return Manager.Spellbook[SpellName]; }
        }

        public virtual string SpellName { get; private set; }

        public virtual float Range { get; private set; }

        public override void Execute()
        {
            Print("Casting {0}", SpellName);
            Spell.Cast();
            Sleep(1000);
        }
    }
}