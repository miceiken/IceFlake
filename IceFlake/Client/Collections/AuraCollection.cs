using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IceFlake.Client.Objects;

namespace IceFlake.Client.Collections
{
    public class AuraCollection : IEnumerable<WoWAura>
    {
        private readonly Dictionary<IntPtr, WoWAura> Auras;
        private readonly WoWUnit Unit;

        public AuraCollection(WoWUnit unit)
        {
            Unit = unit;
            Auras = new Dictionary<IntPtr, WoWAura>();
        }

        public WoWAura this[int id]
        {
            get { return Auras.Values.FirstOrDefault(o => o.ID == id); }
        }

        public WoWAura this[string name]
        {
            get { return Auras.Values.FirstOrDefault(o => o.Name == name); }
        }

        #region IEnumerable<WoWAura> Members

        IEnumerator<WoWAura> IEnumerable<WoWAura>.GetEnumerator()
        {
            return Auras.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Auras.Values.GetEnumerator();
        }

        #endregion

        internal void Update()
        {
            if (!Unit.IsValid)
            {
                Auras.Clear();
                return;
            }

            foreach (var oldAura in Auras)
                oldAura.Value.Invalidate();

            for (int i = 0; i < Unit.GetAuraCount; i++)
            {
                IntPtr ptr = Unit.GetAuraPointer(i);
                if (Auras.ContainsKey(ptr))
                    Auras[ptr].Validate(ptr);
                else
                    Auras.Add(ptr, new WoWAura(ptr));
            }

            Auras.Where(a => !a.Value.IsValid).ToList().ForEach(a => Auras.Remove(a.Key));
        }

        public bool HasAura(string name)
        {
            return Auras.Values.Count(o => o.Name == name && o.IsValid) > 0;
        }
    }
}