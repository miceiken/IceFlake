using System.Linq;
using IceFlake.Client;
using IceFlake.Client.Objects;
using IceFlake.Client.Patchables;

namespace IceFlake.Routines
{
    public static class TotemHelper
    {
        public static bool NeedToRecall
        {
            get { return Manager.LocalPlayer.Totems.Count() > 0; }
        }

        public static void SetTotemSlot(MultiCastSlot slot, Totem spellID)
        {
            WoWScript.ExecuteNoResults("SetMultiCastSpell(" + (int) slot + ", " + (int) spellID + ")");
        }

        public static bool CallTotems()
        {
            WoWSpell CotE = Manager.Spellbook["Call of the Elements"];
            if (CotE.IsValid && CotE.IsReady)
            {
                Log.WriteLine("Call of the Elements");
                CotE.Cast();
                return true;
            }
            return false;
        }

        public static bool CallTotem(Totem totem)
        {
            WoWSpell spell = Manager.Spellbook[((uint) totem)];
            if (spell.IsValid && spell.IsReady)
            {
                Log.WriteLine("Calling {0} totem", totem.ToString());
                spell.Cast();
                return true;
            }
            return false;
        }

        public static bool RecallTotems()
        {
            if (NeedToRecall)
            {
                WoWSpell tr = Manager.Spellbook["Totemic Recall"];
                if (tr.IsValid && tr.IsReady)
                {
                    Log.WriteLine("Recalling totems");
                    tr.Cast();
                    return true;
                }
            }
            return false;
        }
    }
}