using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.Client;
using IceFlake.Client.Patchables;

namespace IceFlake.Routines
{
    public static class TotemHelper
    {
        public static void SetTotemSlot(MultiCastSlot slot, Totem spellID)
        {
            WoWScript.ExecuteNoResults("SetMultiCastSpell(" + (int)slot + ", " + (int)spellID + ")");
        }

        public static bool CallTotems()
        {
            var CotE = Manager.Spellbook["Call of the Elements"];
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
            var spell = Manager.Spellbook[((uint)totem)];
            if (spell.IsValid && spell.IsReady)
            {
                Log.WriteLine("Calling {0} totem", totem.ToString());
                spell.Cast();
                return true;
            }
            return false;
        }

        public static bool NeedToRecall
        {
            get { return Manager.LocalPlayer.Totems.Count() > 0; }
        }

        public static bool RecallTotems()
        {
            if (NeedToRecall)
            {
                var tr = Manager.Spellbook["Totemic Recall"];
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
