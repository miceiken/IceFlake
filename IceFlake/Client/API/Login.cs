namespace IceFlake.Client.API
{
    public class Login
    {
        public bool AccountSelectVisible
        {
            get { return WoWScript.Execute<bool>("WoWAccountSelectDialog ~= nil and WoWAccountSelectDialog:IsShown()"); }
        }

        public bool LoginScreenVisible
        {
            get { return WoWScript.Execute<bool>("AccountLoginUI ~= nil and AccountLoginUI:IsVisible()"); }
        }

        public bool RealmFrameVisible
        {
            get { return WoWScript.Execute<bool>("RealmList ~= nil and RealmList:IsVisible()"); }
        }

        public bool CharSelectVisible
        {
            get { return WoWScript.Execute<bool>("CharacterSelectUI ~= nil and CharacterSelectUI:IsVisible()"); }
        }

        public void DoLogin(string account, string password)
        {
            WoWScript.ExecuteNoResults(string.Format("DefaultServerLogin('{0}', '{1}')", account, password));
        }

        public void SelectGameAccount(string account)
        {
            WoWScript.ExecuteNoResults(
                string.Format(
                    "for i = 0, GetNumGameAccounts(), 1 do local name = GetGameAccountInfo(i) " +
                    "if (name == '{0}') then SetGameAccount(i) end end",
                    account));
        }

        public void SelectRealm(string realm)
        {
            if (RealmFrameVisible)
            {
                var curRealm = WoWScript.Execute<string>("GetServerName()");
                if (!string.IsNullOrEmpty(curRealm) && curRealm == realm)
                {
                    WoWScript.ExecuteNoResults(
                        string.Format(
                            "for i = 1, select('#', GetRealmCategories()), 1 do local numRealms = GetNumRealms(i)" +
                            "for j = 1, numRealms, 1 do local name, numCharacters = GetRealmInfo(i, j)" +
                            "if (name ~= nil and name == '{0}') RealmList:Hide() ChangeRealm(i,j) end end end",
                            realm));
                }
                else
                {
                    WoWScript.ExecuteNoResults("RequestRealmList(1)");
                }
            }
        }

        public void SelectCharacter(string character)
        {
            if (CharSelectVisible)
            {
                WoWScript.ExecuteNoResults(
                    string.Format(
                        "for i=0,GetNumCharacters(),1 do local name = GetCharacterInfo(i)" +
                        "if (name ~= nil and name == '{0}') then CharacterSelect_SelectCharacter(i) end end",
                        character));
            }
        }

        public void EnterWorld()
        {
            WoWScript.ExecuteNoResults("EnterWorld()");
        }
    }
}