using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.Client;
using IceFlake.Client.API;
using IceFlake.Client.Patchables;
using IceFlake.Client.Objects;
using IceFlake.Client.Scripts;

namespace IceFlake.Scripts
{
    public class VendorScript : Script
    {
        public VendorScript()
            : base("Vendor", "Uncatalogued")
        { }

        private WoWUnit Vendor = null;

        public override void OnStart()
        {
            Print("This script is gonna find a vendor and sell all the gray items!");
        }

        public override void OnTick()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            Vendor = Manager.ObjectManager.Objects
                .Where(x => x.IsValid && x.IsUnit)
                .OfType<WoWUnit>()
                .Where(x => x.IsVendor)
                .OrderBy(x => x.Distance)
                .FirstOrDefault();

            if (Vendor == null || !Vendor.IsValid)
            {
                Print("Found no vendors");
                Stop();
                return;
            }

            if (Vendor.Distance > 6)
            {
                if (Manager.Movement.Destination == Vendor.Location)
                    Sleep(100);
                Manager.Movement.PathTo(Vendor.Location);
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
                API.Merchant.SellAll(ItemQuality.Common);
                if (Vendor.IsRepairer)
                    API.Merchant.RepairAll(); // ... and repair if it's a repairer
                Stop();
            }
        }
    }
}
