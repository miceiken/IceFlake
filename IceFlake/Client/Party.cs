using System;
using System.Collections.Generic;
using Core.Client.Objects;
using Core.Client.Patchables;

namespace Core.Client
{
    public static class Party
    {
        public static int NumPartyMembers
        {
            get
            {
                int ret = 0;
                for (int i = 0; i < 5; i++)
                {
                    if (GetPartyMemberGuid(i) != 0)
                        ret++;
                }
                return ret;
            }
        }

        public static List<WoWPlayer> Members
        {
            get
            {
                var ret = new List<WoWPlayer>();
                for (int i = 0; i < 4; i++)
                {
                    var unit = GetPartyMember(i) as WoWPlayer;
                    if (unit != null && unit.IsValid)
                        ret.Add(unit);
                }
                return ret;
            }
        }

        public static WoWObject GetPartyMember(int index)
        {
            return WoWCore.ObjectManager.GetObjectByGuid(GetPartyMemberGuid(index));
        }

        public static ulong GetPartyMemberGuid(int index)
        {
            return WoWCore.Memory.Read<ulong>(new IntPtr(Pointers.Party.PartyArray + (index*8)), true);
        }
    }
}