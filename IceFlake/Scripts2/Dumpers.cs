using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;
using cleanLayer.Library.Scripts;

namespace cleanLayer.Scripts
{
    public class PlayerDumperScript : Script
    {
        public PlayerDumperScript()
            : base("Players", "Dumper")
        { }

        public override void OnStart()
        {
            if (!Manager.IsInGame)
                return;

            //Print("Facing: {0}", Manager.LocalPlayer.Facing);
            foreach (var p in Manager.Objects.Where(x => x.IsPlayer).Cast<WoWPlayer>())
            {
                Print("-- {0}", p.Name);
                Print("\tGUID: 0x{0}", p.Guid.ToString("X8"));
                Print("\tPosition: {0}", p.Location);
            }
        }
    }

    public class PartyDumperScript : Script
    {
        public PartyDumperScript()
            : base("Party", "Dumper")
        { }

        public override void OnStart()
        {
            if (!Manager.IsInGame)
                return;

            foreach (var p in WoWParty.Members)
            {
                Print("-- {0}", p.Name);
                Print("\tHealth: {0}/{1} ({2}%)", p.Health, p.MaxHealth, p.HealthPercentage);
                Print("\tClass: {0}", p.Class);
                Print("\tLevel: {0}", p.Level);
                Print("\tLocation: {0} ({1} yards)", p.Location, p.Distance);
                Print("\tLoS: {0}", p.InLoS);
                Print("\tReaction: {0}", p.Reaction);
            }
        }
    }
}
