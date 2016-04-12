using IceFlake.Client.Collections;
using IceFlake.Client.Patchables;
using System.Collections.Generic;

namespace IceFlake.Client.Objects
{
    public class WoWSpell
    {
        public WoWSpell(uint id)
        {
            Id = id;
            try
            {
                SpellRecord = Manager.DBC[ClientDB.Spell].GetLocalizedRow((int) id).GetStruct<SpellRec>();
            }
            catch
            {
                SpellRecord = default(SpellRec);
                Id = 0;
            }
        }

        private SpellRec SpellRecord { get; set; }
        private LuaSpellInfo _spellinfo = null;
        private int lastspellinfoframe = -1;

        public LuaSpellInfo SpellInfo {
            get {
                if (_spellinfo == null || lastspellinfoframe != DirectX.Direct3D.FrameCount) {
                    List<string> luares = WoWScript.Execute("GetSpellInfo(" + this.Id + ")");
                    _spellinfo = new LuaSpellInfo();
                    int.TryParse(luares[3], out _spellinfo.PowerCost);
                    bool.TryParse(luares[4], out _spellinfo.IsFunnel);
                    uint.TryParse(luares[6], out _spellinfo.CastTime);
                    float.TryParse(luares[7], out _spellinfo.MinRange);
                    float.TryParse(luares[8], out _spellinfo.MaxRange);
                }
                return _spellinfo;
            }
        }

        public bool IsValid
        {
            get { return Id != 0; }
        }

        public uint Id { get; private set; }

        public string Name
        {
            get { return SpellRecord.SpellName; }
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

        public int PowerCost {
            get { return SpellInfo.PowerCost; }
        }

        // Returns true if ability is being used, useful for "on next swing"-abilities e.g. Runic Strike for DKs
        public bool IsUsing {
            get { return WoWScript.Execute("IsCurrentSpell(\"" + Name + "\")")[0].Equals("1"); }
        }

        public bool IsMeleeRange {
            get {
                return SpellRecord.rangeIndex == 2u;
            }
        }

        public bool IsRanged {
            get {
                return MaxRange > 0;
            }
        }

        public float MinRange {
            get {
                return SpellInfo.MinRange;
            }
        }

        // Returns 0 for spells without range and melee range abilities
        public float MaxRange {
            get {
                return SpellInfo.MaxRange;
            }
        }

        public bool FacingRequired {
            get { return SpellRecord.FacingCasterFlags == 1; }
        }

        public bool CanCast(WoWUnit target = null, bool checkrange = false) {
            WoWLocalPlayer Me = Manager.LocalPlayer;
            if (!IsReady) return false;
            if (Me.IsCasting) return false;
            if (IsUsing) return false;
            var r = WoWScript.Execute("IsUsableSpell(select(1, GetSpellInfo(" + this.Id + ")))");
            if (r.Count <= 0 || !r[0].Equals("1")) return false;
            if (checkrange && target != null) {
                if (IsMeleeRange && !Me.IsWithinMeleeRangeOf(target)) {
                    return false;
                }
                else if (IsRanged && (target.Distance < MinRange || target.Distance > MaxRange)) {
                    return false;
                }
            }
            return true;
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
    }
}