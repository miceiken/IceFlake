using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using IceFlake.Client.Objects;
using IceFlake.Client.Patchables;
using IceFlake.DirectX;

namespace IceFlake.Client.Collections
{
    public class SpellCollection : IEnumerable<WoWSpell>
    {
        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //private delegate IntPtr GetSpellEffectRec(uint spellId, int effectIdx);
        //private static GetSpellEffectRec _getSpellEffectRec;

        private static CastSpellDelegate _castSpell;

        private static GetSpellCooldownDelegate _getSpellCooldown;

        public SpellCollection()
        {
            _castSpell = Manager.Memory.RegisterDelegate<CastSpellDelegate>((IntPtr)Pointers.Spell.CastSpell);
            _getSpellCooldown =
                Manager.Memory.RegisterDelegate<GetSpellCooldownDelegate>((IntPtr)Pointers.Spell.GetSpellCooldown);

            KnownSpells = new List<WoWSpell>();
            Update = true;
        }

        private IEnumerable<WoWSpell> KnownSpells { get; set; }

        public bool Update { get; set; }

        public WoWSpell this[uint index]
        {
            get { return KnownSpells.FirstOrDefault(s => s.Id == index); }
        }

        public WoWSpell this[string name]
        {
            get { return KnownSpells.FirstOrDefault(s => s.Name == name); }
        }

        #region IEnumerable<WoWSpell> Members

        IEnumerator<WoWSpell> IEnumerable<WoWSpell>.GetEnumerator()
        {
            return KnownSpells.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return KnownSpells.GetEnumerator();
        }

        #endregion
        public Queue<WoWSpell> CastQueue = new Queue<WoWSpell>(); // Debugging only
        [EndSceneHandler]
        public void Direct3D_EndScene()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            // --- DEBUG ----
            if (CastQueue.Count > 0 && this.Count() > 0)
            {
                if (!Manager.LocalPlayer.IsCasting)
                {
                    var spell = CastQueue.Peek();
                    //if (spell.IsReady)
                    //{
                        spell.Cast();
                        CastQueue.Dequeue();
                    //}
                }
            }
            // --------------

            if (!Update)
                return;

            var knownspells = new List<WoWSpell>();
            for (var i = 0; i < Manager.Memory.Read<int>((IntPtr)Pointers.Spell.SpellCount); i++)
            {
                var spellId = Manager.Memory.Read<uint>((IntPtr)(Pointers.Spell.SpellBook + (i * 4)));
                knownspells.Add(new WoWSpell(spellId));
            }            

            if (KnownSpells.Count() == 0)
            {
                Log.WriteLine("SpellBook: {0} spells", knownspells.Count());
            }

            KnownSpells = knownspells;

            Update = false;
        }

        public static void CastSpell(uint spellId, int itemId = 0, ulong guid = 0ul, int isTrade = 0)
        {
            _castSpell(spellId, itemId, guid, isTrade);
        }

        public static float GetSpellCoolDown(uint id)
        {
            int start = 0;
            int duration = 0;
            bool isReady = false;
            int unk0 = 0;

            _getSpellCooldown(id, false, ref duration, ref start, ref isReady, ref unk0);

            int result = start + duration - (int)Helper.PerformanceCount;
            return isReady ? (result > 0 ? result / 1000f : 0f) : float.MaxValue;
        }

        #region Nested type: CastSpellDelegate

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int CastSpellDelegate(
            uint spellId, int itemId = 0, ulong guid = 0ul, int isTrade = 0, int a6 = 0, int a7 = 0, int a8 = 0);

        #endregion

        #region Nested type: GetSpellCooldownDelegate

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool GetSpellCooldownDelegate(
            uint spellId, bool isPet, ref int duration, ref int start, ref bool isEnabled, ref int unk0);

        #endregion
    }
}