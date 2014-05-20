using IceFlake.Client;
using IceFlake.Client.Patchables;
using IceFlake.Client.Routines;
using IceFlake.Client.Routines.Combat;
using System.Linq;

namespace IceFlake.Routines
{
    public class ElementalShamanRoutine : RoutineBrain
    {
        public ElementalShamanRoutine()
        {
            AddAction(new Thunderstorm(this, 11));
            AddAction(new EarthShock(this, 10));
            AddAction(new FlameShock(this, 9));
            AddAction(new HarmfulSpellRoutine(this, 8, "Lava Burst", 30));
            AddAction(new Shield(this, 7));
            AddAction(new HarmfulSpellRoutine(this, 6, "Lightning Bolt", 30));
        }

        private bool _totemsSet = false;
        protected override void OnBeforeAction(RoutineAction action)
        {
            if (!_totemsSet)
            {
                SetTotems();
                _totemsSet = true;
            }

            if (Helper.InCombat && Manager.LocalPlayer.Totems.Count() == 0)
            {
                if (TotemHelper.CallTotems())
                    Sleep(600);
            }

            // TODO: fix
            //var ft = Manager.Spellbook["Flametongue Weapon"];
            //if (ft != null && ft.IsValid && ft.IsReady) // 5 appears to be flametongue weapon
            //{
            //    var mainhand = Manager.LocalPlayer.GetEquippedItem(EquipSlot.MainHand);
            //    if (mainhand.IsValid && mainhand.Enchants.Count(x => x.Id == (uint)WeaponEnchantments.Flametongue) == 0)
            //    {
            //        Log.WriteLine("Applying Flametongue Weapon to Main Hand weapon");
            //        ft.Cast();
            //        Sleep(600);
            //    }
            //}

            // Instant Lava Burst
            if (action is HarmfulSpellRoutine)
            {
                var haction = action as HarmfulSpellRoutine;
                if (haction.SpellName == "Lava Burst")
                {
                    var cd = Manager.Spellbook["Elemental Mastery"];
                    if (cd != null && cd.IsValid && cd.IsReady)
                    {
                        Log.WriteLine("Popping Elemental Mastery for instant Lava Burst");
                        cd.Cast();
                        Sleep(600);
                    }
                }
            }
        }

        protected override void OnAfterAction(RoutineAction action)
        {
            // Remove Totems
            if (!Helper.InCombat)
            {
                if (TotemHelper.RecallTotems())
                    Sleep(600);
            }
        }

        protected class Thunderstorm : HarmfulSpellRoutine
        {
            public Thunderstorm(RoutineBrain brain, int priority)
                : base(brain, priority, "Thunderstorm", 12)
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && (Manager.LocalPlayer.PowerPercentage < 70 || Brain.HarmfulTargets.Count(x => x.Distance < 8) > 2); }
            }
        }

        // Proc it!
        protected class EarthShock : HarmfulSpellRoutine
        {
            public EarthShock(RoutineBrain brain, int priority)
                : base(brain, priority, "Earth Shock", 25)
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && Manager.LocalPlayer.Auras.HasAura("Elemental Overload"); }
            }
        }

        protected class FlameShock : HarmfulSpellRoutine
        {
            public FlameShock(RoutineBrain brain, int priority)
                : base(brain, priority, "Flame Shock", 25)
            { }

            // TODO: Fix the aura issue where CasterGuid gives "Arithmetic operation resulted in an overflow."
            public override bool IsWanted
            {
                get { return base.IsWanted && Brain.HarmfulTarget.Auras.Where(x => x.IsValid && x.Name == "Flame Shock" && x.CasterGuid == Manager.LocalPlayer.Guid).Count() == 0; }
            }
        }

        // Decide what shield to use
        protected class Shield : SpellRoutine
        {
            public Shield(RoutineBrain brain, int priority)
                : base(brain, priority, "Lightning Shield", 6)
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && !Manager.LocalPlayer.Auras.HasAura(SpellName); }
            }

            public override string SpellName
            {
                get { return (Manager.LocalPlayer.PowerPercentage < 40 ? "Water Shield" : "Lightning Shield"); }
            }
        }

        protected void SetTotems()
        {
            TotemHelper.SetTotemSlot(MultiCastSlot.ElementsEarth, Totem.Stoneskin);
            TotemHelper.SetTotemSlot(MultiCastSlot.ElementsFire, Totem.Searing); // totem of wrath?
            TotemHelper.SetTotemSlot(MultiCastSlot.ElementsWater, Totem.HealingStream);
            TotemHelper.SetTotemSlot(MultiCastSlot.ElementsAir, Totem.WrathOfAir);
        }
    }
}
