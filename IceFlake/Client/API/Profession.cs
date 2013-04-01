namespace IceFlake.Client.API
{
    public class Profession
    {
        // TODO: null checks!
        public ProfessionInfo Profession1
        {
            get { return new ProfessionInfo(WoWScript.Execute<int>("GetProfessions()", 0)); }
        }

        public ProfessionInfo Profession2
        {
            get { return new ProfessionInfo(WoWScript.Execute<int>("GetProfessions()", 1)); }
        }

        public ProfessionInfo Archaeology
        {
            get { return new ProfessionInfo(WoWScript.Execute<int>("GetProfessions()", 2)); }
        }

        public ProfessionInfo Fishing
        {
            get { return new ProfessionInfo(WoWScript.Execute<int>("GetProfessions()", 3)); }
        }

        public ProfessionInfo Cooking
        {
            get { return new ProfessionInfo(WoWScript.Execute<int>("GetProfessions()", 4)); }
        }

        public ProfessionInfo FirstAid
        {
            get { return new ProfessionInfo(WoWScript.Execute<int>("GetProfessions()", 5)); }
        }
    }

    public class ProfessionInfo
    {
        public ProfessionInfo(int idx)
        {
            Index = idx;
        }

        public int Index { get; private set; }

        public string Name
        {
            get { return WoWScript.Execute<string>("GetProfessionInfo(" + Index + ")", 0); }
        }

        public int Level
        {
            get { return WoWScript.Execute<int>("GetProfessionInfo(" + Index + ")", 2); }
        }

        public int MaxLevel
        {
            get { return WoWScript.Execute<int>("GetProfessionInfo(" + Index + ")", 3); }
        }

        public int SkillLine
        {
            get { return WoWScript.Execute<int>("GetProfessionInfo(" + Index + ")", 6); }
        }

        public int RankModifier
        {
            get { return WoWScript.Execute<int>("GetProfessionInfo(" + Index + ")", 7); }
        }
    }
}