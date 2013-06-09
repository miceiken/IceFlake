using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using cleanCore;
using cleanCore.D3D;
using cleanLayer.Library.Scripts;
using cleanLayer.Library.Modules;
using cleanLayer.Modules;

namespace cleanLayer.Scripts
{
    public class DrawPath : Script
    {
        public DrawPath()
            : base("Draw Path", "Uncatalogued")
        { }

        private List<Location> locs = new List<Location>();

        public override void OnStart()
        {
            if (!Manager.IsInGame)
                return;

            locs = SPatrol.Locations;

            if (locs == null || locs.Count == 0)
                Stop();
        }

        public override void OnTick()
        {
            if (!Manager.IsInGame)
                return;

            var prevLoc = locs[0];
            foreach (var loc in locs.Skip(1))
            {
                //Rendering.DrawCircle(loc, 1f, Color.Yellow, Color.Yellow);
                //Rendering.DrawLine(prevLoc, loc, Color.Blue);
                prevLoc = loc;
            }
        }
    }
}
