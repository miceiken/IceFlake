using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.Client;
using IceFlake.Client.Scripts;

namespace IceFlake.Scripts
{
    public class GuildRecruitmentScript : Script
    {
        public GuildRecruitmentScript()
            : base("Recruitment", "Guild")
        { }

        public Dictionary<string, WhoCharacterInfo> InvitedCharacters = new Dictionary<string, WhoCharacterInfo>();
        //public List<string> Classes = new List<string>
        //{
        //    "Warrior",
        //    "Warlock",
        //    "Hunter",
        //    "Druid",
        //    "Rogue",
        //    "Priest",
        //    "Shaman",
        //    "Paladin",
        //    "Death Knight",
        //    "Mage"
        //};

        private int TotalSearches;
        private int CurrentLevelSearch;

        private GRState CurrentState;
        private DateTime LastSearch;

        private string RecruitMessage = "";

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            WoWScript.ExecuteNoResults("SetWhoToUI(1)");
            Manager.Events.Register("WHO_LIST_UPDATE", HandleEvent);
            TotalSearches = 0;
            CurrentLevelSearch = 1;
            CurrentState = GRState.Search;
        }

        public override void OnTick()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            switch (CurrentState)
            {
                case GRState.Search:
                    Print("Starting a new search for level {0}", CurrentLevelSearch);
                    LastSearch = DateTime.Now;
                    WoWScript.ExecuteNoResults("SendWho(\"" + CurrentLevelSearch + "\")");
                    TotalSearches++;

                    CurrentState = GRState.Searching;
                    break;

                case GRState.Searching:
                    var span = DateTime.Now - LastSearch;
                    if (span.TotalSeconds > 5)
                    {
                        Print("Last search didn't give any match after 5 seconds, maybe there are no characters in this level?");
                        CurrentLevelSearch++;
                        CurrentState = GRState.Search;
                    }
                    break;

                case GRState.Invite:
                    var searchResult = WoWScript.Execute("GetNumWhoResults()");
                    var shownResults = int.Parse(searchResult[0]);
                    var totalResults = int.Parse(searchResult[1]);
                    Print("Search returned {0} visible matches out of a total {1}", shownResults, totalResults);
                    for (int i = 1; i <= shownResults; i++)
                    {
                        var searchInfo = WoWScript.Execute("GetWhoInfo(" + i + ")");
                        var charInfo = new WhoCharacterInfo()
                        {
                            Name = searchInfo[0],
                            Guild = searchInfo[1],
                            Level = int.Parse(searchInfo[2]),
                            Race = searchInfo[3],
                            Class = searchInfo[4],
                            Zone = searchInfo[5]
                        };

                        if (string.IsNullOrEmpty(charInfo.Guild) && !InvitedCharacters.ContainsKey(charInfo.Name))
                        {
                            Print("Inviting {0} (Level {1} {2} {3})", charInfo.Name, charInfo.Level, charInfo.Race, charInfo.Class);
                            //WoWScript.ExecuteNoResults("SendChatMessage(\"" + RecruitMessage + "\", \"WHISPER\", nil, \"" + charInfo.Name + "\")");
                            WoWScript.ExecuteNoResults("GuildInvite(\"" + charInfo.Name + "\")");
                            InvitedCharacters.Add(charInfo.Name, charInfo);
                        }
                    }

                    CurrentState = GRState.Search;
                    Sleep(5000);
                    break;

                case GRState.Completed:
                    Print("Stopping after {0} searches and {1} invites", TotalSearches, InvitedCharacters.Count);
                    Stop();
                    break;
            }

            Sleep(500);
        }

        public override void OnTerminate()
        {
            Manager.Events.Remove("WHO_LIST_UPDATE", HandleEvent);
        }

        private void HandleEvent(string ev, List<string> args)
        {
            CurrentState = (CurrentLevelSearch <= 85 ? GRState.Invite : GRState.Completed);
            CurrentLevelSearch++;
        }

        public class WhoCharacterInfo
        {
            public string Name;
            public string Guild;
            public int Level;
            public string Race;
            public string Class;
            public string Zone;
        }

        enum GRState
        {
            Search,
            Searching,
            Invite,
            Completed
        }
    }
}
