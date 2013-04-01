using System.Collections.Generic;
using System.Linq;
using IceFlake.Client.Objects;

namespace IceFlake.Client.API
{
    public class TradeSkill : Frame
    {
        public TradeSkill()
            : base("TradeskillWindow") // TODO: Find the real window name.
        {
        }

        public bool IsOpen
        {
            get { return TradeSkillName != "UNKNOWN"; }
        }

        public IEnumerable<TradeSkillInfo> Recipes
        {
            get
            {
                ResetFilter();
                var ret = new List<TradeSkillInfo>();
                for (int i = 1; i <= WoWScript.Execute<int>("GetNumTradeSkills()"); i++)
                    ret.Add(new TradeSkillInfo(i));
                return ret;
            }
        }

        public string TradeSkillName
        {
            get { return WoWScript.Execute<string>("GetTradeSkillLine()", 0); }
        }

        public int Level
        {
            get { return WoWScript.Execute<int>("GetTradeSkillLine()", 1); }
        }

        public int MaxLevel
        {
            get { return WoWScript.Execute<int>("GetTradeSkillLine()", 2); }
        }

        private void ResetFilter()
        {
            if (!IsOpen)
                return;

            WoWScript.ExecuteNoResults(
                "TradeSkillOnlyShowMakeable(false)" +
                "TradeSkillOnlyShowSkillUps(false)" +
                "SetTradeSkillSubClassFilter(0)" +
                "SetTradeSkillInvSlotFilter(0)" +
                "SetTradeSkillItemNameFilter(\"\")"
                ); // No filters!

            for (int i = 1; i <= WoWScript.Execute<int>("GetNumTradeSkills()"); i++)
            {
                WoWScript.ExecuteNoResults("ExpandTradeSkillSubClass(" + i + ")");
            }
        }

        public void Close()
        {
            WoWScript.ExecuteNoResults("CloseTradeSkill()");
        }
    }

    public class TradeSkillInfo
    {
        // We should probably read all the info when instantiated and hook up the prof-level-up events to update the list.
        public TradeSkillInfo(int index)
        {
            Index = index;
        }

        public int Index { get; private set; }

        public string Name
        {
            get { return WoWScript.Execute<string>("GetTradeSkillInfo(" + Index + ")", 0); }
        }

        public string Type
        {
            get { return WoWScript.Execute<string>("GetTradeSkillInfo(" + Index + ")", 1); }
        }

        // easy, header, medium, optimal, trivial
        public int NumAvailable
        {
            get { return WoWScript.Execute<int>("GetTradeSkillInfo(" + Index + ")", 2); }
        }

        public bool IsExpanded
        {
            get { return WoWScript.Execute<bool>("GetTradeSkillInfo(" + Index + ")", 3); }
        }

        public string ServiceType
        {
            get { return WoWScript.Execute<string>("GetTradeSkillInfo(" + Index + ")", 4); }
        }

        // Emboss, Embrodier, Enchant, Engrave, Inscribe, Modify, Tinker, nil (produce item)
        public int NumSkillUps
        {
            get { return WoWScript.Execute<int>("GetTradeSkillInfo(" + Index + ")", 5); }
        }

        public bool Craftable
        {
            get { return NumAvailable > 0 && Tools.All(x => x.HasTool); }
        }

        public IEnumerable<TradeSkillTool> Tools
        {
            get
            {
                var ret = new List<TradeSkillTool>();
                List<string> tools = WoWScript.Execute("GetTradeSkillTools(" + Index + ")");
                for (int i = 0; i < tools.Count/2; i++)
                {
                    string ht = tools[i + 1];
                    ret.Add(new TradeSkillTool
                                {
                                    Name = tools[i],
                                    HasTool = !(ht == "false" || ht == "0" || ht == "nil")
                                });
                }
                return ret;
            }
        }

        public void Select()
        {
            WoWScript.ExecuteNoResults("SelectTradeSkill(" + Index + ")");
        }

        public void Execute(int repeat = 1)
        {
            WoWScript.ExecuteNoResults("DoTradeSkill(" + Index + ", " + repeat + ")");
        }

        public void ApplyToItem(WoWItem item)
        {
            ApplyToItem(item.Entry);
        }

        public void ApplyToItem(uint itemid)
        {
            Execute();
            WoWScript.ExecuteNoResults("SpellTargetItem(" + itemid + ") BindEnchant() ReplaceEnchant()");
        }

        #region Nested type: TradeSkillTool

        public class TradeSkillTool
        {
            public bool HasTool;
            public string Name;
        }

        #endregion
    }
}