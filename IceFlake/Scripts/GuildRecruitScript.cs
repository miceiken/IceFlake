using System;
using System.Collections.Generic;
using IceFlake.Client;
using IceFlake.Client.Scripts;

namespace IceFlake.Scripts
{
    public class GuildRecruitmentScript : Script
    {
        private const bool SendMessage = false;
        private int CurrentLevelSearch;

        private GRState CurrentState;
        public Dictionary<string, WhoCharacterInfo> InvitedCharacters = new Dictionary<string, WhoCharacterInfo>();
        private DateTime LastSearch;

        private string RecruitMessage = "";
        private int TotalSearches;

        public GuildRecruitmentScript()
            : base("Recruitment", "Guild")
        {
        }

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
                    TimeSpan span = DateTime.Now - LastSearch;
                    if (span.TotalSeconds > 5)
                    {
                        Print(
                            "Last search didn't give any match after 5 seconds, maybe there are no characters in this level?");
                        CurrentLevelSearch++;
                        CurrentState = GRState.Search;
                    }
                    break;

                case GRState.Invite:
                    List<string> searchResult = WoWScript.Execute("GetNumWhoResults()");
                    int shownResults = int.Parse(searchResult[0]);
                    int totalResults = int.Parse(searchResult[1]);
                    Print("Search returned {0} visible matches out of a total {1}", shownResults, totalResults);
                    for (int i = 1; i <= shownResults; i++)
                    {
                        List<string> searchInfo = WoWScript.Execute("GetWhoInfo(" + i + ")");
                        var charInfo = new WhoCharacterInfo
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
                            Print("Inviting {0} (Level {1} {2} {3})", charInfo.Name, charInfo.Level, charInfo.Race,
                                charInfo.Class);
                            if (SendMessage)
                                WoWScript.ExecuteNoResults("SendChatMessage(\"" + RecruitMessage +
                                                           "\", \"WHISPER\", nil, \"" + charInfo.Name + "\")");
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

        private enum GRState
        {
            Search,
            Searching,
            Invite,
            Completed
        }

        public class WhoCharacterInfo
        {
            public string Class;
            public string Guild;
            public int Level;
            public string Name;
            public string Race;
            public string Zone;
        }
    }
}