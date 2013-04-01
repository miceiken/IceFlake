namespace IceFlake.Client.API
{
    public class QuestGossip : Frame
    {
        public QuestGossip()
            : base("QuestFrame")
        {
        }

        public string Title
        {
            get { return WoWScript.Execute<string>("GetTitleText()"); }
        }

        public int ID
        {
            get { return WoWScript.Execute<int>("GetQuestID()"); }
        }

        public bool Completable
        {
            get { return WoWScript.Execute<bool>("IsQuestCompletable()"); }
        }

        public bool IsDaily
        {
            get { return WoWScript.Execute<bool>("QuestIsDaily()"); }
        }

        public bool IsWeekly
        {
            get { return WoWScript.Execute<bool>("QuestIsWeekly()"); }
        }

        public void Accept()
        {
            WoWScript.ExecuteNoResults("AcceptQuest()");
        }

        public void Decline()
        {
            WoWScript.ExecuteNoResults("DeclineQuest()");
        }

        public void Complete()
        {
            WoWScript.ExecuteNoResults("CompleteQuest()");
        }

        public void Close()
        {
            WoWScript.ExecuteNoResults("CloseQuest()");
        }
    }
}