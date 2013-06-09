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
    public class VendorScript : Script
    {
        public VendorScript()
            : base("Vendor", "Uncatalogued")
        { }

        private WoWUnit Vendor = WoWUnit.Invalid;

        public override void OnStart()
        {
            Print("This script is gonna find a vendor and sell all the gray items!");
        }

        public override void OnTick()
        {
            if (!Manager.IsInGame)
                return;

            Vendor = Manager.Objects
                .Where(x => x.IsValid && x.IsUnit)
                .OfType<WoWUnit>()
                .Where(x => x.IsVendor)
                .OrderBy(x => x.Distance)
                .FirstOrDefault() ?? WoWUnit.Invalid;

            if (Vendor == null || !Vendor.IsValid)
                Stop();

            if (Vendor.Distance > 6)
            {
                if (Mover.Destination == Vendor.Location)
                {
                    Mover.Pulse();
                    Sleep(100);
                }
                Mover.PathTo(Vendor.Location);
                Sleep(100);
            }

            if (!API.Gossip.IsShown && !API.Merchant.IsShown)
            {
                Vendor.Interact();
                Sleep(1000);
            }

            if (API.Gossip.IsShown)
            {
                var opt = API.Gossip.Options.FirstOrDefault(x => x.Gossip == GossipType.Vendor);
                if (opt == null) // Vendor doesn't have repairer gossip?
                    Stop();
                opt.Select();
                Sleep(1000);
            }

            if (API.Merchant.IsShown)
            {
                API.Merchant.SellAll(ItemQuality.Poor); // Sell all gray
                if (Vendor.IsRepairer)
                    API.Merchant.RepairAll(); // ... and repair if it's a repairer
                Sleep(2000);
            }
        }
    }
}
