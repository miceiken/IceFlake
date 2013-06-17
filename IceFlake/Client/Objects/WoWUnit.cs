using System;
using System.Runtime.InteropServices;
using IceFlake.Client.Collections;
using IceFlake.Client.Patchables;

namespace IceFlake.Client.Objects
{
    public class WoWUnit : WoWObject
    {

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int CreatureTypeDelegate(IntPtr thisObj);
        private static CreatureTypeDelegate _creatureType;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int CreatureRankDelegate(IntPtr thisObj);
        private static CreatureRankDelegate _creatureRank;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr GetAuraDelegate(IntPtr thisObj, int index);

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int GetShapeshiftFormIdDelegate(IntPtr thisObj);
        private static GetShapeshiftFormIdDelegate _getShapeshiftFormId;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int UnitReactionDelegate(IntPtr thisObj, IntPtr unitToCompare);
        private static UnitReactionDelegate _unitReaction;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int UnitThreatInfoDelegate(
            IntPtr pThis, IntPtr guid, ref IntPtr threatStatus, ref IntPtr threatPct, ref IntPtr rawPct,
            ref int threatValue);
        private static UnitThreatInfoDelegate _unitThreatInfo;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int GetAuraCountDelegate(IntPtr thisObj);
        private static GetAuraCountDelegate _getAuraCount;
        private static GetAuraDelegate _getAura;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate bool HasAuraDelegate(IntPtr thisObj, int spellId);
        private static HasAuraDelegate _hasAura;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int HandleTrackingFacingDelegate(IntPtr thisObj, int a2, float a3);
        private HandleTrackingFacingDelegate _handleTrackingFacing;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int SetPlayerPitchDelegate(IntPtr thisObj, float a2, int a3, float a4);
        private SetPlayerPitchDelegate _setPlayerPitch;

        private readonly AuraCollection _auras;

        public WoWUnit(IntPtr pointer)
            : base(pointer)
        {
            _auras = new AuraCollection(this);
        }

        public UnitReaction Reaction
        {
            get
            {
                if (_unitReaction == null)
                    _unitReaction =
                        Manager.Memory.RegisterDelegate<UnitReactionDelegate>((IntPtr)Pointers.Unit.UnitReaction);
                return (UnitReaction)_unitReaction(Pointer, Manager.LocalPlayer.Pointer);
            }
        }

        public bool IsFriendly
        {
            get { return (int)Reaction > (int)UnitReaction.Neutral; }
        }

        public bool IsNeutral
        {
            get { return Reaction == UnitReaction.Neutral; }
        }

        public bool IsHostile
        {
            get { return (int)Reaction < (int)UnitReaction.Neutral; }
        }

        public ulong TargetGuid
        {
            get { return GetDescriptor<ulong>(WoWUnitFields.UNIT_FIELD_TARGET); }
        }

        public WoWObject Target
        {
            get { return Manager.ObjectManager.GetObjectByGuid(TargetGuid); }
        }

        public bool IsDead
        {
            get { return Health <= 0 || HasFlag(WoWUnitFields.UNIT_DYNAMIC_FLAGS, UnitDynamicFlags.Dead); }
        }

        private byte[] DisplayPower
        {
            get { return BitConverter.GetBytes(GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_POWER1)); }
        }

        private byte[] UnitBytes0
        {
            get { return BitConverter.GetBytes(GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_BYTES_0)); }
        }

        private byte[] UnitBytes1
        {
            get { return BitConverter.GetBytes(GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_BYTES_1)); }
        }

        private byte[] UnitBytes2
        {
            get { return BitConverter.GetBytes(GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_BYTES_2)); }
        }

        public WoWRace Race
        {
            get { return (WoWRace)UnitBytes0[0]; }
        }

        public WoWClass Class
        {
            get { return (WoWClass)UnitBytes0[1]; }
        }

        public WoWGender Gender
        {
            get { return (WoWGender)UnitBytes0[2]; }
        }

        public WoWPowerType PowerType
        {
            get { return (WoWPowerType)UnitBytes0[3]; }
        }

        public bool IsLootable
        {
            get { return HasFlag(WoWUnitFields.UNIT_DYNAMIC_FLAGS, UnitDynamicFlags.Lootable); }
        }

        public bool IsTapped
        {
            get { return HasFlag(WoWUnitFields.UNIT_DYNAMIC_FLAGS, UnitDynamicFlags.TaggedByOther); }
        }

        public bool IsTappedByMe
        {
            get { return HasFlag(WoWUnitFields.UNIT_DYNAMIC_FLAGS, UnitDynamicFlags.TaggedByMe); }
        }

        public bool IsInCombat
        {
            get { return HasFlag(WoWUnitFields.UNIT_FIELD_FLAGS, UnitFlags.Combat); }
        }

        public bool IsLooting
        {
            get { return HasFlag(WoWUnitFields.UNIT_FIELD_FLAGS, UnitFlags.Looting); }
        }

        public uint Health
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_HEALTH); }
        }

        public uint MaxHealth
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MAXHEALTH); }
        }

        public double HealthPercentage
        {
            get { return (Health / (double)MaxHealth) * 100; }
        }

        public uint Level
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_LEVEL); }
        }

        public UnitFlags Flags
        {
            get { return (UnitFlags)GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_FLAGS); }
        }

        public UnitFlags2 Flags2
        {
            get { return GetDescriptor<UnitFlags2>(WoWUnitFields.UNIT_FIELD_FLAGS_2); }
        }

        public UnitDynamicFlags DynamicFlags
        {
            get { return GetDescriptor<UnitDynamicFlags>(WoWUnitFields.UNIT_DYNAMIC_FLAGS); }
        }

        public UnitNPCFlags NpcFlags
        {
            get { return GetDescriptor<UnitNPCFlags>(WoWUnitFields.UNIT_NPC_FLAGS); }
        }

        public bool IsVendor
        {
            get { return HasFlag(WoWUnitFields.UNIT_NPC_FLAGS, UnitNPCFlags.UNIT_NPC_FLAG_VENDOR); }
        }

        public bool IsRepairer
        {
            get { return HasFlag(WoWUnitFields.UNIT_NPC_FLAGS, UnitNPCFlags.UNIT_NPC_FLAG_REPAIR); }
        }

        public bool IsClassTrainer
        {
            get { return HasFlag(WoWUnitFields.UNIT_NPC_FLAGS, UnitNPCFlags.UNIT_NPC_FLAG_TRAINER_CLASS); }
        }

        public bool IsProfessionTrainer
        {
            get { return HasFlag(WoWUnitFields.UNIT_NPC_FLAGS, UnitNPCFlags.UNIT_NPC_FLAG_TRAINER_PROFESSION); }
        }

        public bool IsFlightmaster
        {
            get { return HasFlag(WoWUnitFields.UNIT_NPC_FLAGS, UnitNPCFlags.UNIT_NPC_FLAG_FLIGHTMASTER); }
        }

        public bool IsInnkeeper
        {
            get { return HasFlag(WoWUnitFields.UNIT_NPC_FLAGS, UnitNPCFlags.UNIT_NPC_FLAG_INNKEEPER); }
        }

        public bool IsAuctioneer
        {
            get { return HasFlag(WoWUnitFields.UNIT_NPC_FLAGS, UnitNPCFlags.UNIT_NPC_FLAG_AUCTIONEER); }
        }

        public uint Faction
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_FACTIONTEMPLATE); }
        }

        public uint BaseAttackTime
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_BASEATTACKTIME); }
        }

        public uint RangedAttackTime
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_RANGEDATTACKTIME); }
        }

        public float BoundingRadius
        {
            get { return GetDescriptor<float>(WoWUnitFields.UNIT_FIELD_BOUNDINGRADIUS); }
        }

        public float CombatReach
        {
            get { return GetDescriptor<float>(WoWUnitFields.UNIT_FIELD_COMBATREACH); }
        }

        public uint DisplayId
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_DISPLAYID); }
        }

        public uint MountDisplayId
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MOUNTDISPLAYID); }
        }

        public uint NativeDisplayId
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_NATIVEDISPLAYID); }
        }

        public uint MinDamage
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MINDAMAGE); }
        }

        public uint MaxDamage
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MAXDAMAGE); }
        }

        public uint MinOffhandDamage
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MINOFFHANDDAMAGE); }
        }

        public uint MaxOffhandDamage
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MAXOFFHANDDAMAGE); }
        }

        public uint PetExperience
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_PETEXPERIENCE); }
        }

        public uint PetNextLevelExperience
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_PETNEXTLEVELEXP); }
        }

        public uint BaseMana
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_BASE_MANA); }
        }

        public uint BaseHealth
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_BASE_HEALTH); }
        }

        public uint AttackPower
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_ATTACK_POWER); }
        }

        public uint RangedAttackPower
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_RANGED_ATTACK_POWER); }
        }

        public uint MinRangedDamage
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MINRANGEDDAMAGE); }
        }

        public uint MaxRangedDamage
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MAXRANGEDDAMAGE); }
        }

        private uint GetPowerByType(WoWPowerType power)
        {
            switch (power)
            {
                default:
                case WoWPowerType.Mana:
                    return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_POWER1);
                case WoWPowerType.Rage:
                    return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_POWER2);
                case WoWPowerType.Energy:
                    return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_POWER3);
                case WoWPowerType.Focus:
                    return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_POWER4);
                case WoWPowerType.Happiness:
                    return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_POWER5);
                case WoWPowerType.RunicPower:
                    return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_POWER7);
            }
        }

        private uint GetMaxPowerByType(WoWPowerType power)
        {
            switch (power)
            {
                default:
                case WoWPowerType.Mana:
                    return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MAXPOWER1);
                case WoWPowerType.Rage:
                    return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MAXPOWER2);
                case WoWPowerType.Energy:
                    return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MAXPOWER3);
                case WoWPowerType.Focus:
                    return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MAXPOWER4);
                case WoWPowerType.Happiness:
                    return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MAXPOWER5);
                case WoWPowerType.RunicPower:
                    return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MAXPOWER7);
            }
        }

        public uint Power
        {
            get { return GetPowerByType(PowerType); }
        }

        public uint MaxPower
        {
            get { return GetMaxPowerByType(PowerType); }
        }

        public double PowerPercentage
        {
            get { return (Power / (double)MaxPower) * 100; }
        }

        public ulong SummonedBy
        {
            get { return GetDescriptor<ulong>(WoWUnitFields.UNIT_FIELD_SUMMONEDBY); }
        }

        public ulong CreatedBy
        {
            get { return GetDescriptor<ulong>(WoWUnitFields.UNIT_FIELD_CREATEDBY); }
        }

        public uint ChanneledCastingId
        {
            get { return Manager.Memory.Read<uint>(new IntPtr(Pointer.ToInt64() + Pointers.Unit.ChanneledCastingId)); }
        }

        public uint CastingId
        {
            get { return Manager.Memory.Read<uint>(new IntPtr(Pointer.ToInt64() + Pointers.Unit.CastingId)); }
        }

        public bool IsCasting
        {
            get { return (ChanneledCastingId != 0 || CastingId != 0); }
        }

        public int GetAuraCount
        {
            get
            {
                if (_getAuraCount == null)
                    _getAuraCount =
                        Manager.Memory.RegisterDelegate<GetAuraCountDelegate>((IntPtr)Pointers.Unit.GetAuraCount);
                return _getAuraCount(Pointer);
            }
        }

        public AuraCollection Auras
        {
            get
            {
                _auras.Update();
                return _auras;
            }
        }

        public UnitClassificationType Classification
        {
            get
            {
                if (_creatureRank == null)
                    _creatureRank =
                        Manager.Memory.RegisterDelegate<CreatureRankDelegate>((IntPtr)Pointers.Unit.GetCreatureRank);
                return (UnitClassificationType)_creatureRank(Pointer);
            }
        }

        public CreatureType CreatureType
        {
            get
            {
                if (_creatureType == null)
                    _creatureType =
                        Manager.Memory.RegisterDelegate<CreatureTypeDelegate>((IntPtr)Pointers.Unit.GetCreatureType);
                return (CreatureType)_creatureType(Pointer);
            }
        }

        public ShapeshiftForm Shapeshift
        {
            get
            {
                if (_getShapeshiftFormId == null)
                    _getShapeshiftFormId =
                        Manager.Memory.RegisterDelegate<GetShapeshiftFormIdDelegate>(
                            (IntPtr)Pointers.Unit.ShapeshiftFormId);
                return (ShapeshiftForm)_getShapeshiftFormId(Pointer);
            }
        }

        public bool IsStealthed
        {
            get { return Shapeshift == ShapeshiftForm.Stealth; }
        }

        public bool IsTotem
        {
            get { return CreatureType == CreatureType.Totem; }
        }

        public int CalculateThreat
        {
            get
            {
                if (_unitThreatInfo == null)
                    _unitThreatInfo =
                        Manager.Memory.RegisterDelegate<UnitThreatInfoDelegate>((IntPtr)Pointers.Unit.CalculateThreat);

                var threatStatus = new IntPtr();
                var threatPct = new IntPtr();
                var threatRawPct = new IntPtr();
                int threatValue = 0;
                var storageField = Manager.Memory.Read<IntPtr>(Manager.LocalPlayer.Pointer + 0x08);
                _unitThreatInfo(Pointer, storageField, ref threatStatus, ref threatPct, ref threatRawPct,
                                ref threatValue);

                return (int)threatStatus;
            }
        }

        public bool HasAura(int spellId)
        {
            if (_hasAura == null)
                _hasAura = Manager.Memory.RegisterDelegate<HasAuraDelegate>((IntPtr)Pointers.Unit.HasAuraBySpellId);
            return _hasAura(Pointer, spellId);
        }

        public IntPtr GetAuraPointer(int index)
        {
            if (_getAura == null)
                _getAura = Manager.Memory.RegisterDelegate<GetAuraDelegate>((IntPtr)Pointers.Unit.GetAura);
            return _getAura(Pointer, index);
        }

        //public override string ToString()
        //{
        //    return "[\"" + Name + "\", Distance = " + (int)Distance + ", Type = " + Type + ", React = " + Reaction + "]";
        //}
    }
}