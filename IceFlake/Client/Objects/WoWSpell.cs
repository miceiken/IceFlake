using System;
using IceFlake.Client.Patchables;
using IceFlake.Client.Collections;

namespace IceFlake.Client.Objects
{
    public class WoWSpell
    {
        public WoWSpell(uint id)
        {
            Id = id;
            try
            {
                SpellRecord = Manager.DBC[ClientDB.Spell].GetLocalizedRow((int)id).GetStruct<SpellRec>();
            }
            catch
            {
                SpellRecord = default(SpellRec);
                Id = 0;
            }
        }

        private SpellRec SpellRecord { get; set; }

        public bool IsValid
        {
            get { return Id != 0; }
        }

        public uint Id { get; private set; }

        public string Name
        {
            get { return SpellRecord.SpellName; }
        }

        public void Cast()
        {
            Cast(Manager.LocalPlayer);
        }

        public void Cast(WoWUnit target)
        {
            if (!IsValid)
                return;

            if (target == null || !target.IsValid)
                return;

            //target.Select();
            //WoWScript.ExecuteNoResults("CastSpellByID(" + Id + ")");
            SpellCollection.CastSpell(Id, guid: target.Guid);
        }

        public float Cooldown
        {
            get
            {
                if (!IsValid)
                    return float.MaxValue;

                return SpellCollection.GetSpellCoolDown(Id);
            }
        }

        public bool IsReady
        {
            get { return Cooldown <= 0f; }
        }
    }
}