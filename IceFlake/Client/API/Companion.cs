using System;
using System.Collections.Generic;
using System.Linq;

namespace IceFlake.Client.API
{
    public class Companion : Frame
    {
        private List<WoWCompanion> CachedCritters = new List<WoWCompanion>();
        private List<WoWCompanion> CachedMounts = new List<WoWCompanion>();

        public Companion()
            // TODO: Find real frame name
            : base("CompanionFrame")
        {
        }

        public int NumMounts
        {
            get { return WoWScript.Execute<int>("GetNumCompanions(\"MOUNT\")", 0); }
        }

        public bool CanMount
        {
            get { return NumMounts > 0; }
        }

        public List<WoWCompanion> Mounts
        {
            get
            {
                if (CachedMounts.Count > 0)
                    return CachedMounts;
                CachedMounts = Enumerable.Range(1, NumMounts).Select(i => new WoWCompanion("MOUNT", i)).ToList();
                return CachedMounts;
            }
        }

        public int NumCritters
        {
            get { return WoWScript.Execute<int>("GetNumCompanions(\"CRITTER\")", 0); }
        }

        public List<WoWCompanion> Critters
        {
            get
            {
                if (CachedCritters.Count > 0)
                    return CachedCritters;
                CachedCritters = Enumerable.Range(1, NumCritters).Select(i => new WoWCompanion("CRITTER", i)).ToList();
                return CachedCritters;
            }
        }

        public bool RandomMount()
        {
            var r = new Random();
            List<WoWCompanion> mounts = Mounts;
            if (mounts.Count > 0)
            {
                WoWCompanion mount = mounts[r.Next(0, mounts.Count)];
                mount.Summon();
                return true;
            }
            //else
            //{
            //    if (Core.LocalPlayer.Class == WoWClass.Druid && WoWSpellCollection.GetSpell("Travel Form").IsValid)
            //    {
            //        WoWSpellCollection.GetSpell("Travel Form").Cast();
            //        return true;
            //    }
            //    else if (Core.LocalPlayer.Class == WoWClass.Shaman && WoWSpellCollection.GetSpell("Ghost Wolf").IsValid)
            //    {
            //        WoWSpellCollection.GetSpell("Ghost Wolf").Cast();
            //        return true;
            //    }
            //    else if (Core.LocalPlayer.Race == WoWRace.Worgen && WoWSpellCollection.GetSpell("Running Wild").IsValid)
            //    {
            //        WoWSpellCollection.GetSpell("Running Wild").Cast();
            //        return true;
            //    }
            //}
            return false;
        }

        public void ClearCache()
        {
            CachedMounts.Clear();
            CachedCritters.Clear();
        }
    }

    public class WoWCompanion
    {
        public WoWCompanion(string type, int index)
        {
            List<string> ret = WoWScript.Execute("GetCompanionInfo(\"" + type + "\", " + index + ")");
            Type = type;
            Index = index;
            if (ret.Count < 5)
                return;

            CreatureId = int.Parse(ret[0]);
            Name = ret[1];
            SpellId = int.Parse(ret[2]);
            Active = !(ret[4] == "false" || ret[4] == "0" || ret[4] == "nil");
            Flags = int.Parse(ret[5]);
        }

        private string Type { get; set; }
        private int Index { get; set; }

        public int CreatureId { get; private set; }
        public string Name { get; private set; }
        public int SpellId { get; private set; }
        public bool Active { get; private set; }
        public int Flags { get; private set; }

        public bool IsMount
        {
            get { return Type.ToLower().Equals("mount"); }
        }

        public bool IsGround
        {
            get { return IsMount && (Flags & 0x01) != 0; }
        }

        public bool IsFlying
        {
            get { return IsMount && (Flags & 0x02) != 0; }
        }

        public bool IsSwimming
        {
            get { return IsMount && (Flags & 0x08) != 0; }
        }

        public void Summon()
        {
            WoWScript.ExecuteNoResults("CallCompanion(\"MOUNT\", " + Index + ")");
        }
    }
}