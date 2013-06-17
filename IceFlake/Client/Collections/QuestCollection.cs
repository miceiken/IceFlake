using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.Client.Patchables;

namespace IceFlake.Client.Collections
{
    public class QuestCollection
    {
        public IEnumerable<QuestLogEntry> QuestLog
        {
            get
            {
                var descriptorArray = Manager.Memory.Read<uint>(new IntPtr(Manager.LocalPlayer.Pointer.ToInt64() + 0x8));
                for (int i = 0; i < 25; i++)
                {
                    var qlPtr = new IntPtr(descriptorArray + (int)WoWPlayerFields.PLAYER_QUEST_LOG_1_1 * 0x4 + (i * 0x14));
                    if (Manager.Memory.Read<uint>(qlPtr) > 0)
                        yield return Manager.Memory.Read<QuestLogEntry>(qlPtr);
                }
            }
        }

        // TODO: Fix this
        public IEnumerable<int> CompletedQuests
        {
            get
            {
                var currentQuest = Manager.Memory.Read<uint>((IntPtr)Pointers.LocalPlayer.CompletedQuests);
                while ((currentQuest & 1) == 0 && currentQuest > 0)
                {
                    yield return Manager.Memory.Read<int>(new IntPtr(currentQuest + 2 * 4));
                    currentQuest = Manager.Memory.Read<uint>(new IntPtr(currentQuest + 4));
                }
            }
        }

        public WoWQuest this[int id]
        {
            get { return new WoWQuest(id); }
        }
    }
}
