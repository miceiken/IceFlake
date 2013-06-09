//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.IO;
//using System.Windows.Forms;
//using IceFlake.Client;
//using IceFlake.Client.Scripts;
//using IceFlake.Client.API;

//namespace IceFlake.Scripts
//{
//    public class TalentScript : Script
//    {
//        public TalentScript()
//            : base("Talent", "Leveling")
//        { }

//        private TalentTree TalentBuild = null;

//        public override void OnStart()
//        {
//            var ret = SelectTalentFile();
//            if (!string.IsNullOrEmpty(ret) && File.Exists(ret))
//            {
//                TalentBuild = TalentTree.Load(ret);
//            }
//        }

//        public override void OnTick()
//        {
//            if (TalentBuild == null ||  TalentBuild.Talents == null)
//            {
//                Print("No talent build selected");
//                Stop();
//                return;
//            }

//            //if (!API.Talent.HasLearnedMajorTree)
//            //    WoWScript.ExecuteNoResults("SetPreviewPrimaryTalentTree(" + TalentBuild.Specialization + ", GetActiveTalentGroup())");

//            var unspentTalents = API.Talent.UnspentPoints;

//            if (unspentTalents < 1)
//                Sleep(30000);

//            foreach (var t in TalentBuild.Talents)
//            {
//                var tab = API.Talent.Tabs[TalentBuild.Specialization - 1];
//                foreach (var tb in tab.Talents)
//                {
//                    if (tb.Index == t.Index || tb.Name == t.Name)
//                        tb.Learn(t.Count);
//                }
//            }

//            /*

//            // Find the tab where most points are spent
//            var bestTab = API.Talent.Tabs.OrderBy(x => x.PointsSpent).FirstOrDefault();
//            // If no points are placed, get the talent specialization (lua is 1-indexed)
//            if (bestTab.PointsSpent == 0)
//                bestTab = API.Talent.Tabs[TalentBuild.Specialization - 1];
//            Print("Put talents in {0}", bestTab.Name);
//            // Find all talents that goes in that tab
//            var talentNodes = TalentBuild.Talents.Where(x => x.Tab == bestTab.Index);
//            Print("{0} talents goes into {1}", talentNodes.Count(), bestTab.Name);

//            var learnedTalents = bestTab.Talents.Where(x => x.Count > 0);
//            foreach (var tn in talentNodes)
//            {
//                if (unspentTalents == 0)
//                    Sleep(30000);

//                var lt = learnedTalents.FirstOrDefault(x => (x.Name == tn.Name) || (x.Index == tn.Index && x.Tab == tn.Tab));
//                if (lt == null) continue;

//                lt.Learn(tn.Count);
//                Print("Learning {0}x{1}", tn.Name, tn.Count);

//                //var numLearned = lt.Count;

//                //while (unspentTalents > 0 && numLearned < tn.Count)
//                //{
//                //    WoWScript.ExecuteNoResults("AddPreviewTalentPoints(" + tn.Tab + ", " + tn.Index + ")");
//                //    numLearned++;
//                //    unspentTalents--;
//                //}
//            }

//            //WoWScript.ExecuteNoResults("LearnPreviewTalents()");
//            */
//        }

//        public override void OnTerminate()
//        {
//            TalentBuild = null;
//        }

//        public string SelectTalentFile()
//        {
//            var fileDlg = new OpenFileDialog();
//            fileDlg.InitialDirectory = Path.Combine(Program.Directory, "Talents");
//            fileDlg.Filter = "Profiles|*.xml";
//            var fileName = Program.SelectFile(fileDlg);
//            return fileName;
//        }
//    }
//}
