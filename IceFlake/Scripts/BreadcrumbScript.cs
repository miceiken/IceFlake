using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.Client;
using IceFlake.Client.Scripts;
using IceFlake.Client.Objects;

namespace IceFlake.Scripts
{
    public class BreadcrumbScript : Script
    {
        public BreadcrumbScript()
            : base("Breadcrumb", "Movement")
        {
            Locations = new List<Location>();
        }

        private WoWUnit FollowUnit;
        private float UnitFacing;
        private List<Location> Locations;
        private int LastIndex = 0;
        private Location? currentLocation;
        private DateTime LastAddition;

        //private Queue<Location> CreatePathFrom(Location start)
        //{
        //    var synclist = Locations;
        //    var position = synclist.FindIndex(l => l == start);
        //    var queue = new Queue<Location>(synclist);
        //    for (int i = 0; i < position; i++)
        //        queue.Dequeue();
        //    return queue;
        //}

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            LastIndex = 0;
            currentLocation = null;
            Locations.Clear();

            Print("Starting breadcrumb system!");

            var tank = Party.Members.OrderByDescending(m => m.MaxHealth).FirstOrDefault();
            if (tank.IsValid)
            {
                if (tank.InLoS)
                {
                    FollowUnit = tank;
                    UnitFacing = tank.Facing;
                    Locations.Add(FollowUnit.Location);
                    currentLocation = Locations[0];
                }
                else
                {
                    Print("Get in LoS of {0}.", tank.Name);
                    Stop();
                }
            }
            else
            {
                Print("No suitable unit to follow found, are you in group?");
                Stop();
            }
        }
        
        public override void OnTick()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            if (FollowUnit.IsValid)
            {
                var newFacing = FollowUnit.Facing;
                var span = DateTime.Now - LastAddition;
                if (Math.Abs(newFacing - UnitFacing) > 0.3f || (span.TotalSeconds > 2 && FollowUnit.Distance > 4f))
                {
                    Locations.Add(FollowUnit.Location);
                    UnitFacing = newFacing;
                    LastAddition = DateTime.Now;
                }
            }

            if (!Manager.LocalPlayer.IsCasting && !Manager.LocalPlayer.IsClickMoving)
            {
                currentLocation = Locations[LastIndex];
                if (currentLocation.HasValue && Manager.LocalPlayer.Location.DistanceTo(currentLocation.Value) < 3f && LastIndex + 1 < Locations.Count)
                    currentLocation = Locations[LastIndex++];

                if (currentLocation.HasValue && (FollowUnit.Distance > 6f || !FollowUnit.InLoS))
                {
                    Manager.LocalPlayer.ClickToMove(currentLocation.Value);
                }
            }
        }

        public override void OnTerminate()
        {
            Print("{0} breadcrumbs were recorded.", Locations.Count);
        }
    }
}
