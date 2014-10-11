using IceFlake.Client.Patchables;
using IceFlake.Client.Scripts;

namespace IceFlake.Scripts
{
    public class QuestHelperScript : Script
    {
        private bool dumpedActiveQuests;
        private bool dumpedCompletedQuests;

        public QuestHelperScript()
            : base("Quest", "Helper")
        {
        }

        public override void OnStart()
        {
            dumpedCompletedQuests = false;
            dumpedActiveQuests = false;
            //Manager.Quests.QueryQuestsCompleted();
        }

        public override void OnTick()
        {
            //if (!dumpedCompletedQuests && Manager.Quests.CompletedQuestIds.Count() > 0)
            //{
            //    Print("Completed Quests:");
            //    foreach (var q in Manager.Quests.CompletedQuestIds)
            //    {
            //        if (q == 0) continue;
            //        var qcr = new WoWQuest(q).CachedEntry;
            //        Print("\t{0}: {1}", q, qcr.Name);
            //    }
            //    dumpedCompletedQuests = true;
            //}

            if (!dumpedActiveQuests)
            {
                Print("Active Quests:");
                // Loop through all active quests from quest log
                foreach (QuestLogEntry q in Manager.Quests.QuestLog)
                {
                    // Get the Quest Cache Record
                    QuestCacheRecord qcr = q.AsWoWQuest().CachedEntry;
                    Print("{0}: {1}:", qcr.Id, qcr.Name);
                    unsafe
                    {
                        // There are 4 ObjectiveIds and ObjectiveRequiredCounts
                        for (int i = 0; i < 4; i++)
                        {
                            // Get Objective ID i
                            int objectiveId = qcr.ObjectiveId[i];
                            if (objectiveId == 0) continue;
                            int objectiveReq = qcr.ObjectiveRequiredCount[i];
                            Print("\tObjective id {0} req {1}", objectiveId, objectiveReq);
                        }

                        // There are 6 CollectItemIds and CollectItemCounts
                        for (int i = 0; i < 6; i++)
                        {
                            int collectItemId = qcr.CollectItemId[i];
                            if (collectItemId == 0) continue;
                            int collectItemCount = qcr.CollectItemCount[i];
                            Print("\tCollectItem id {0} req {1}", collectItemId, collectItemCount);
                        }

                        // There are 4 IntermediateItemIds and IntermediateItemCounts
                        for (int i = 0; i < 4; i++)
                        {
                            int intermediateItemId = qcr.IntermediateItemId[i];
                            if (intermediateItemId == 0) continue;
                            int intermediateItemCount = qcr.IntermediateItemCount[i];
                            Print("\tIntermediateItem id {0} req {1}", intermediateItemId, intermediateItemCount);
                        }
                    }
                }
                dumpedActiveQuests = true;
            }
        }

        public override void OnTerminate()
        {
        }
    }
}