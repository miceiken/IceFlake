using System.Collections.Generic;
using System.Linq;

namespace IceFlake.Client.API
{
    public class Talent : Frame
    {
        public Talent()
            : base("PlayerTalentFrame")
        {
        }

        public int UnspentPoints
        {
            get { return WoWScript.Execute<int>("GetUnspentTalentPoints()"); }
        }

        public int NumTabs
        {
            get { return WoWScript.Execute<int>("GetNumTalentTabs()"); }
        }

        public List<TalentTab> Tabs
        {
            get { return Enumerable.Range(1, NumTabs).Select(i => new TalentTab(i)).ToList(); }
        }

        public bool HasLearnedMajorTree
        {
            get { return WoWScript.Execute<int>("GetPrimaryTalentTree()") > 0; }
        }

        public void Show()
        {
            WoWScript.ExecuteNoResults(Name + ":Show()");
        }
    }


    public class TalentTab
    {
        public TalentTab(int index)
        {
            Index = index;
        }

        public int Index { get; private set; }

        public string Name
        {
            get { return WoWScript.Execute<string>("GetTalentTabInfo(" + Index + ")", 0); }
        }

        public string Icon
        {
            get { return WoWScript.Execute<string>("GetTalentTabInfo(" + Index + ")", 1); }
        }

        public string Description
        {
            get { return WoWScript.Execute<string>("GetTalentTabInfo(" + Index + ")", 3); }
        }

        public int PointsSpent
        {
            get { return WoWScript.Execute<int>("GetTalentTabInfo(" + Index + ")", 2); }
        }

        public bool IsUnlocked
        {
            get { return WoWScript.Execute<bool>("GetTalentTabInfo(" + Index + ")", 4); }
        }

        public int NumTalents
        {
            get { return WoWScript.Execute<int>("GetNumTalents(" + Index + ")"); }
        }

        public List<TalentNode> Talents
        {
            get { return Enumerable.Range(1, NumTalents).Select(i => new TalentNode(Index, i)).ToList(); }
        }
    }

    public class TalentNode
    {
        public TalentNode(int tab, int index)
        {
            Tab = tab;
            Index = index;
        }

        public int Tab { get; private set; }
        public int Index { get; private set; }

        public string Name
        {
            get { return WoWScript.Execute<string>("GetTalentInfo(" + Tab + ", " + Index + ")", 0); }
        }

        public int Tier
        {
            get { return WoWScript.Execute<int>("GetTalentInfo(" + Tab + ", " + Index + ")", 2); }
        }

        public int Column
        {
            get { return WoWScript.Execute<int>("GetTalentInfo(" + Tab + ", " + Index + ")", 3); }
        }

        public int Count
        {
            get { return WoWScript.Execute<int>("GetTalentInfo(" + Tab + ", " + Index + ")", 4); }
        }

        public int MaxCount
        {
            get { return WoWScript.Execute<int>("GetTalentInfo(" + Tab + ", " + Index + ")", 5); }
        }

        public bool IsExceptional
        {
            get { return WoWScript.Execute<bool>("GetTalentInfo(" + Tab + ", " + Index + ")", 6); }
        }

        public bool MeetsPreReq
        {
            get { return WoWScript.Execute<bool>("GetTalentInfo(" + Tab + ", " + Index + ")", 7); }
        }

        public void Learn(int count = 1)
        {
            for (int i = 0; i < count; i++)
                WoWScript.ExecuteNoResults("LearnTalent(" + Tab + ", " + Index + ")");
        }
    }
}