using System.Collections.Generic;
using System.Linq;

namespace IceFlake.Client.API
{
    public class Trainer : Frame
    {
        public Trainer()
            : base("ClassTrainerFrame")
        {
        }

        public bool IsTradeskillTrainer
        {
            get { return WoWScript.Execute<bool>("IsTradeskillTrainer()"); }
        }

        public int NumServices
        {
            get { return WoWScript.Execute<int>("GetNumTrainerServices()"); }
        }

        public List<TrainerService> Services
        {
            get
            {
                WoWScript.ExecuteNoResults("SetTrainerServiceTypeFilter(\"available\", 1)");
                WoWScript.ExecuteNoResults("SetTrainerServiceTypeFilter(\"unavailable\", 0)");
                WoWScript.ExecuteNoResults("SetTrainerServiceTypeFilter(\"used\", 0)");
                var services = new List<TrainerService>();
                for (int i = 1; i <= NumServices; i++)
                    services.Add(GetServiceInfo(i));
                return services;
            }
        }

        public TrainerService GetServiceInfo(int index)
        {
            return new TrainerService(index);
        }

        public void BuyAllAvailable()
        {
            WoWScript.ExecuteNoResults("SetTrainerServiceTypeFilter(\"available\", 1)");
            foreach (TrainerService s in Services.Where(x => x.Available))
                s.Buy();
        }

        public void Close()
        {
            WoWScript.ExecuteNoResults("CloseTrainer()");
        }
    }

    public class TrainerService
    {
        public TrainerService(int index)
        {
            Index = index;
        }

        public int Index { get; private set; }

        public string Name
        {
            get { return WoWScript.Execute<string>("GetTrainerServiceInfo(" + Index + ")", 0); }
        }

        public string Rank
        {
            get { return WoWScript.Execute<string>("GetTrainerServiceInfo(" + Index + ")", 1); }
        }

        public string Category
        {
            get { return WoWScript.Execute<string>("GetTrainerServiceInfo(" + Index + ")", 2); }
        }

        public bool Expanded
        {
            get { return WoWScript.Execute<bool>("GetTrainerServiceInfo(" + Index + ")", 3); }
        }

        public bool Available
        {
            get { return Category != null && Category == "available"; }
        }

        public int Cost
        {
            get { return WoWScript.Execute<int>("GetTrainerServiceCost(" + Index + ")"); }
        }

        public string Description
        {
            get { return WoWScript.Execute<string>("GetTrainerServiceDescription(" + Index + ")"); }
        }

        public int LevelReq
        {
            get { return WoWScript.Execute<int>("GetTrainerServiceLevelReq(" + Index + ")"); }
        }

        public int SkillLine
        {
            get { return WoWScript.Execute<int>("GetTrainerServiceSkillLine(" + Index + ")"); }
        }

        public int SkillReq
        {
            get { return WoWScript.Execute<int>("GetTrainerServiceSkillReq(" + Index + ")"); }
        }

        public void Buy()
        {
            WoWScript.ExecuteNoResults("BuyTrainerService(" + Index + ")");
        }
    }
}