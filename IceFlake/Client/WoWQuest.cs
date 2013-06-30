using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.Client.Patchables;

namespace IceFlake.Client
{
    public class WoWQuest
    {
        public WoWQuest(int id)
        {
            this.ID = id;
            if (CachedEntry.Id == 0)
                CachedEntry = Manager.LocalPlayer.GetQuestRecordFromId(id);
        }

        public WoWQuest(QuestLogEntry ql)
            : this(ql.ID)
        {
            this.QuestLogEntry = ql;
        }

        public WoWQuest(QuestCacheRecord qc)
            : this(qc.Id)
        {
            this.CachedEntry = qc;
        }

        public int ID
        {
            get;
            private set;
        }

        public QuestLogEntry QuestLogEntry
        {
            get;
            private set;
        }

        public QuestCacheRecord CachedEntry
        {
            get;
            private set;
        }

        public bool PlayerIsOnQuest
        {
            get { return Manager.Quests.QuestLog.Count(x => x.ID == this.ID) > 0; }
        }

        public bool PlayerHasCompletedQuest
        {
            get { return Manager.Quests.CompletedQuestIds.Count(x => x == this.ID) > 0; }
        }

        public bool PlayerIsSuitableForQuest
        {
            get { return Manager.LocalPlayer.Level >= CachedEntry.RequiredLevel; }
        }
    }
}
