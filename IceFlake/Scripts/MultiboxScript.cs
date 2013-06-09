using System.Linq;
using IceFlake.Client;
using IceFlake.Client.Scripts;
using IceFlake.Client.Objects;

namespace IceFlake.Scripts
{
    public class MultiboxScript : Script
    {
        public MultiboxScript()
            : base("Multiboxer", "Party")
        { }

        private WoWPlayer Leader;
        private bool FollowingLeader = false;

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            Leader = Party.Members.FirstOrDefault();
            if (Leader != null && Leader.IsValid)
            {
                Print("Leader set to {0}", Leader.Name);
            }
        }

        public override void OnTerminate()
        {
            Leader = null;
        }

        public override void OnTick()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            if (Leader != null && Leader.IsValid)
            {
                if (Helper.InCombat)
                {
                    if (FollowingLeader)
                    {
                        WoWScript.ExecuteNoResults("MoveForwardStop()");
                        FollowingLeader = false;
                    }
                    if (Leader.IsCasting && !Manager.LocalPlayer.IsCasting)
                    {
                        var id = Leader.CastingId;
                        if (id == 0)
                            id = Leader.ChanneledCastingId;
                        var spell = Manager.Spellbook[id];
                        if (spell.IsValid)
                        {
                            var target = Leader.Target;
                            if (target.IsValid)
                            {
                                if (Manager.LocalPlayer.Target != target)
                                    target.Select();
                                spell.Cast(target as WoWUnit);
                                Print("Casting {0} on {1}", spell.Name, target.Name);
                            }
                            else
                            {
                                spell.Cast(Manager.LocalPlayer);
                                Print("Casting {0} on self", spell.Name);
                            }
                        }
                    }
                }
                else
                {
                    if (!FollowingLeader)
                    {
                        Leader.Select();
                        WoWScript.ExecuteNoResults("FollowUnit(\"target\")");
                        FollowingLeader = true;
                    }
                }
            }
        }
    }
}
