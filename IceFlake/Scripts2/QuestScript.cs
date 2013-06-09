using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;
using cleanCore.API;
using cleanLayer.Library;
using cleanLayer.Library.Scripts;

namespace cleanLayer.Scripts
{
    public class QuestScript : Script
    {
        public QuestScript()
            : base("Quest Test", "Uncatalogued")
        { }

        public override void OnStart()
        {
            if (!Manager.IsInGame)
            {
                Stop();
                return;
            }

            var qs = Manager.LocalPlayer.Quests;
            Print("{0} quests", qs.Count());
            foreach (var q in qs)
            {
                Print("\t{0} - {1} - {2}", q.ID, q.State, q.Time);
                foreach (var o in q.Objectives)
                    Print("\t\t{0}", o);
            }

            Stop();    
        }

        public override void OnTerminate()
        {            
        }

        public override void OnTick()
        {
            //if (!Manager.IsInGame)
            //    return;
        }
    }
}
