using System;
using System.Linq;
using IceFlake.Client.Patchables;

namespace IceFlake.Client.Objects
{
    public class WoWPlayer : WoWUnit
    {
        public WoWPlayer(IntPtr pointer)
            : base(pointer)
        {
        }

        public uint Experience
        {
            get { return GetDescriptor<uint>(WoWPlayerFields.PLAYER_XP); }
        }

        public uint NextLevelExperience
        {
            get { return GetDescriptor<uint>(WoWPlayerFields.PLAYER_NEXT_LEVEL_XP); }
        }

        public uint GuildRank
        {
            get { return GetDescriptor<uint>(WoWPlayerFields.PLAYER_GUILDRANK); }
        }

        public float BlockPercentage
        {
            get { return GetDescriptor<float>(WoWPlayerFields.PLAYER_BLOCK_PERCENTAGE); }
        }

        public float DodgePercentage
        {
            get { return GetDescriptor<float>(WoWPlayerFields.PLAYER_DODGE_PERCENTAGE); }
        }

        public float ParryPercentage
        {
            get { return GetDescriptor<float>(WoWPlayerFields.PLAYER_PARRY_PERCENTAGE); }
        }

        public uint Expertise
        {
            get { return GetDescriptor<uint>(WoWPlayerFields.PLAYER_EXPERTISE); }
        }

        public uint OffhandExpertise
        {
            get { return GetDescriptor<uint>(WoWPlayerFields.PLAYER_OFFHAND_EXPERTISE); }
        }

        public float CritPercentage
        {
            get { return GetDescriptor<float>(WoWPlayerFields.PLAYER_CRIT_PERCENTAGE); }
        }

        public float RangedCritPercentage
        {
            get { return GetDescriptor<float>(WoWPlayerFields.PLAYER_RANGED_CRIT_PERCENTAGE); }
        }

        public float OffhandCritPercentage
        {
            get { return GetDescriptor<float>(WoWPlayerFields.PLAYER_OFFHAND_CRIT_PERCENTAGE); }
        }

        public uint RestedExperience
        {
            get { return GetDescriptor<uint>(WoWPlayerFields.PLAYER_REST_STATE_EXPERIENCE); }
        }

        public ulong Coinage
        {
            get { return GetDescriptor<ulong>(WoWPlayerFields.PLAYER_FIELD_COINAGE); }
        }

        public ulong PetGuid
        {
            get
            {
                return
                    Manager.ObjectManager.Objects.Where(x => x.IsValid && x.IsUnit).OfType<WoWUnit>().Where(
                        x => x.SummonedBy == Guid).Select(x => x.Guid).FirstOrDefault();
            }
        }

        public uint PlayerFlags
        {
            get { return GetDescriptor<uint>(WoWPlayerFields.PLAYER_FLAGS); }
        }

        public bool IsGhost
        {
            get { return (GetDescriptor<uint>(WoWPlayerFields.PLAYER_FLAGS) & (1 << 4)) > 0; }
        }

        public WoWUnit Pet
        {
            get
            {
                return
                    Manager.ObjectManager.Objects.Where(x => x.IsValid && x.IsUnit).OfType<WoWUnit>().FirstOrDefault(x => x.SummonedBy == Guid);
            }
        }
    }
}