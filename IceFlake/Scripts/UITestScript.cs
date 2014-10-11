using System.Collections.Generic;
using System.Linq;
using IceFlake.Client.API;
using IceFlake.Client.Scripts;

namespace IceFlake.Scripts
{
    public class UITestScript : Script
    {
        public UITestScript()
            : base("User Interface", "Dumper")
        {
        }

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            if (API.Gossip.IsShown)
            {
                Print("Dumping gossip");
                IEnumerable<GossipOption> opts = API.Gossip.Options;
                foreach (GossipOption opt in opts)
                {
                    Print("\tT: {0} - G: {1}", opt.Title, opt.Gossip);
                }
            }
            if (API.Merchant.IsShown)
            {
                Print("Dumping merchant items");
                IEnumerable<MerchantItem> items = API.Merchant.Items;
                foreach (MerchantItem i in items)
                {
                    Print("\tN: {0} - P: {1}c - MS: {2}", i.Name, i.Price, i.MaxStack);
                    i.Buy(); // Will buy Quantity of each
                }
            }
            if (API.Trainer.IsShown)
            {
                Print("Dumping trainer services");
                IEnumerable<TrainerService> services = API.Trainer.Services;
                foreach (TrainerService s in services)
                {
                    Print("\tN: {0} - A: {1} - C: {2}c", s.Name, s.Available, s.Cost);
                }
                API.Trainer.BuyAllAvailable();
            }
            if (API.Talent.IsShown)
            {
                Print("Dumping talents");
                IEnumerable<TalentTab> tabs = API.Talent.Tabs;
                foreach (TalentTab tab in tabs)
                {
                    Print("Dumping {0}-talents", tab.Name);
                    foreach (TalentNode t in tab.Talents)
                        Print("\tTier {0}, Col {1} - {2} ({3}/{4})", t.Tier, t.Column, t.Name, t.Count, t.MaxCount);
                }
            }
            if (API.Taxi.IsShown)
            {
                Print("Dumping taxi nodes - current: {0}", API.Taxi.CurrentNode.Name);
                IEnumerable<TaxiNode> nodes = API.Taxi.Nodes;
                foreach (TaxiNode n in nodes.Where(x => x.Type != "NONE"))
                {
                    Print("\tN: {0} - C: {1}c - T: {2}", n.Name, n.Cost, n.Type);
                }
            }
            if (API.Loot.IsShown)
            {
                Print("Dumping loot");
                IEnumerable<LootSlotInfo> slots = API.Loot.LootSlots;
                foreach (LootSlotInfo s in slots)
                {
                    Print("\tLoot #{0}: [{1}]{2}", s.Slot, s.Name, s.Quantity > 1 ? "x" + s.Quantity : "");
                    if (s.IsItem)
                        Print("\t{0} ({1})", s.Quality, s.ItemLink);

                    if (s.Locked)
                        Print("Not lootable");
                    else
                        s.Loot();
                }
            }
            if (API.Companion.IsShown)
            {
                Print("Dumping mounts");
                List<WoWCompanion> mounts = API.Companion.Mounts;
                foreach (WoWCompanion mount in mounts)
                    Print("\t{0}: {1}{2}", mount.SpellId, mount.Name, mount.Active ? " (Active)" : "");
            }

            Stop();
        }

        public override void OnTerminate()
        {
        }

        public override void OnTick()
        {
        }
    }
}