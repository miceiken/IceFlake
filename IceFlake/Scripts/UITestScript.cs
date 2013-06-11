using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.Client;
using IceFlake.Client.API;
using IceFlake.Client.Scripts;

namespace IceFlake.Scripts
{
    public class UITestScript : Script
    {
        public UITestScript()
            : base("User Interface", "Dumper")
        { }

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            if (API.Gossip.IsShown)
            {
                Print("Dumping gossip");
                var opts = API.Gossip.Options;
                foreach (var opt in opts)
                {
                    Print("\tT: {0} - G: {1}", opt.Title, opt.Gossip);
                }
            }
            if (API.Merchant.IsShown)
            {
                Print("Dumping merchant items");
                var items = API.Merchant.Items;
                foreach (var i in items)
                {
                    Print("\tN: {0} - P: {1}c - MS: {2}", i.Name, i.Price, i.MaxStack);
                    i.Buy(); // Will buy Quantity of each
                }
            }
            if (API.Trainer.IsShown)
            {
                Print("Dumping trainer services");
                var services = API.Trainer.Services;
                foreach (var s in services)
                {
                    Print("\tN: {0} - A: {1} - C: {2}c", s.Name, s.Available, s.Cost);
                }
                API.Trainer.BuyAllAvailable();
            }
            if (API.Talent.IsShown)
            {
                Print("Dumping talents");
                var tabs = API.Talent.Tabs;
                foreach (var tab in tabs)
                {
                    Print("Dumping {0}-talents", tab.Name);
                    foreach (var t in tab.Talents)
                        Print("\tTier {0}, Col {1} - {2} ({3}/{4})", t.Tier, t.Column, t.Name, t.Count, t.MaxCount);
                }
            }
            if (API.Taxi.IsShown)
            {
                Print("Dumping taxi nodes - current: {0}", API.Taxi.CurrentNode.Name);
                var nodes = API.Taxi.Nodes;
                foreach (var n in nodes.Where(x => x.Type != "NONE"))
                {
                    Print("\tN: {0} - C: {1}c - T: {2}", n.Name, n.Cost, n.Type);
                }
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
