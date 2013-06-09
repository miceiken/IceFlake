using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;
using cleanCore.API;
using cleanLayer.Library.Scripts;
using System.Runtime.InteropServices;

namespace cleanLayer.Scripts
{
    public class AHTestScript : Script
    {
        public AHTestScript()
            : base("AH", "Uncatalogued")
        { }

        public override void OnStart()
        {
            firstRun = true;
        }

        private bool firstRun = true;
        public override void OnTick()
        {
            if (firstRun)
            {
                var count = Helper.Magic.Read<int>(Offsets.AuctionHouse.ListCount);
                if (count == 0) return;

                var ptr = Helper.Magic.Read<IntPtr>(Offsets.AuctionHouse.ListBase);
                if (ptr == IntPtr.Zero) return;

                for (var i = 0; i < count; i++)
                {
                    var auc = Helper.Magic.ReadStruct<AuctionEntry>(ptr + (i * AuctionEntry.Size));

                    Print("#{0} | buyout {1} | expires on {2}", auc.Id, Helper.MoneyString((int)auc.BuyoutPrice), auc.Expires);
                }

                firstRun = false;
            }
        }

        public override void OnTerminate()
        {
        }
    }

    public class TradeSkillScript : Script
    {
        public TradeSkillScript()
            : base("TradeSkill", "Test")
        { }

        public override void OnStart()
        {
            if (!API.TradeSkill.IsOpen)
            {
                Print("No tradeskill window open, aborting");
                Stop();
            }

            Print("TradeSkill: {0} (lvl {1}/{2})", API.TradeSkill.TradeSkillName, API.TradeSkill.Level, API.TradeSkill.MaxLevel);
            var recipes = API.TradeSkill.Recipes;
            Print("{0} recipes:", recipes.Count());
            foreach (var rec in recipes)
                Print("\t{0} | T: {1} | ST: {2} | Avail: {3} | SkillUps: {4}", rec.Name, rec.Type, rec.ServiceType, rec.NumAvailable, rec.NumSkillUps);

            Stop();
        }
    }
}
