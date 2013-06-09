using System.Linq;
using cleanCore;
using cleanLayer.Library.Scripts;

namespace cleanLayer.Scripts
{
    public class MultiboxScript : Script
    {
        public MultiboxScript()
            : base("Multiboxer", "Party")
        { }

        private WoWPlayer Leader = WoWPlayer.Invalid;
        private bool FollowingLeader = false;

        public override void OnStart()
        {
            if (!Manager.IsInGame)
                return;

            Leader = WoWParty.Members.FirstOrDefault() ?? WoWPlayer.Invalid;
            if (Leader.IsValid)
            {
                Print("Leader set to {0}", Leader.Name);
            }
        }

        public override void OnTerminate()
        {
            Leader = WoWPlayer.Invalid;
        }

        public override void OnTick()
        {
            if (!Manager.IsInGame)
                return;

            if (Leader.IsValid)
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
                        var spell = WoWSpellCollection.GetSpell(id);
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
