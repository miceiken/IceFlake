using System;
using System.Runtime.InteropServices;
//using IceFlake.Client.Collections;
using IceFlake.Client.Patchables;

namespace IceFlake.Client.Objects
{
    public class WoWUnit : WoWObject
    {
        private static HasAuraDelegate _hasAura;

        private static UnitReactionDelegate _unitReaction;

        private static UnitThreatInfoDelegate _unitThreatInfo;

        private static CreatureTypeDelegate _creatureType;

        private static GetShapeshiftFormIdDelegate _getShapeshiftFormId;

        private static GetAuraCountDelegate _getAuraCount;

        private static GetAuraDelegate _getAura;
        //private readonly AuraCollection _auras;

        public WoWUnit(IntPtr pointer)
            : base(pointer)
        {
            //_auras = new AuraCollection(this);
        }

        public UnitReaction Reaction
        {
            get
            {
                if (_unitReaction == null)
                    _unitReaction =
                        Core.Memory.RegisterDelegate<UnitReactionDelegate>((IntPtr)Pointers.Unit.UnitReaction);
                return (UnitReaction)_unitReaction(Pointer, Core.LocalPlayer.Pointer);
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
            get { return Core.ObjectManager.GetObjectByGuid(TargetGuid); }
        }

        public bool IsDead
        {
            get { return Health <= 0 || HasFlag(WoWUnitFields.UNIT_DYNAMIC_FLAGS, UnitDynamicFlags.Dead); }
        }

        private byte[] DisplayPower
        {
            get { return BitConverter.GetBytes(GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_POWER1)); }
        }

        public WoWRace Race
        {
            get { return (WoWRace)DisplayPower[0]; }
        }

        public WoWClass Class
        {
            get { return (WoWClass)DisplayPower[1]; }
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

        public double PowerPercentage
        {
            get { return (Power / (double)MaxPower) * 100; }
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

        public uint Power
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_POWER1); }
        }

        public uint MaxPower
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MAXPOWER1); }
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
            get { return Core.Memory.Read<uint>(new IntPtr(Pointer.ToInt64() + Pointers.Unit.ChanneledCastingId)); }
        }

        public uint CastingId
        {
            get { return Core.Memory.Read<uint>(new IntPtr(Pointer.ToInt64() + Pointers.Unit.CastingId)); }
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
                        Core.Memory.RegisterDelegate<GetAuraCountDelegate>((IntPtr)Pointers.Unit.GetAuraCount);
                return _getAuraCount(Pointer);
            }
        }

        //public AuraCollection Auras
        //{
        //    get
        //    {
        //        _auras.Update();
        //        return _auras;
        //    }
        //}

        public CreatureType CreatureType
        {
            get
            {
                if (_creatureType == null)
                    _creatureType =
                        Core.Memory.RegisterDelegate<CreatureTypeDelegate>((IntPtr)Pointers.Unit.GetCreatureType);
                return (CreatureType)_creatureType(Pointer);
            }
        }

        public ShapeshiftForm Shapeshift
        {
            get
            {
                if (_getShapeshiftFormId == null)
                    _getShapeshiftFormId =
                        Core.Memory.RegisterDelegate<GetShapeshiftFormIdDelegate>(
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
                        Core.Memory.RegisterDelegate<UnitThreatInfoDelegate>((IntPtr)Pointers.Unit.CalculateThreat);

                var threatStatus = new IntPtr();
                var threatPct = new IntPtr();
                var threatRawPct = new IntPtr();
                int threatValue = 0;
                var storageField = Core.Memory.Read<IntPtr>(Core.LocalPlayer.Pointer + 0x08);
                _unitThreatInfo(Pointer, storageField, ref threatStatus, ref threatPct, ref threatRawPct,
                                ref threatValue);

                return (int)threatStatus;
            }
        }

        public bool HasAura(int spellId)
        {
            if (_hasAura == null)
                _hasAura = Core.Memory.RegisterDelegate<HasAuraDelegate>((IntPtr)Pointers.Unit.HasAuraBySpellId);
            return _hasAura(Pointer, spellId);
        }

        public IntPtr GetAuraPointer(int index)
        {
            if (_getAura == null)
                _getAura = Core.Memory.RegisterDelegate<GetAuraDelegate>((IntPtr)Pointers.Unit.GetAura);
            return _getAura(Pointer, index);
        }

        #region Nested type: CreatureTypeDelegate

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int CreatureTypeDelegate(IntPtr thisObj);

        #endregion

        #region Nested type: GetAuraCountDelegate

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int GetAuraCountDelegate(IntPtr thisObj);

        #endregion

        #region Nested type: GetAuraDelegate

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr GetAuraDelegate(IntPtr thisObj, int index);

        #endregion

        #region Nested type: GetShapeshiftFormIdDelegate

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int GetShapeshiftFormIdDelegate(IntPtr thisObj);

        #endregion

        #region Nested type: HasAuraDelegate

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate bool HasAuraDelegate(IntPtr thisObj, int spellId);

        #endregion

        #region Nested type: UnitReactionDelegate

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int UnitReactionDelegate(IntPtr thisObj, IntPtr unitToCompare);

        #endregion

        #region Nested type: UnitThreatInfoDelegate

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int UnitThreatInfoDelegate(
            IntPtr pThis, IntPtr guid, ref IntPtr threatStatus, ref IntPtr threatPct, ref IntPtr rawPct,
            ref int threatValue);

        #endregion

        //public override string ToString()
        //{
        //    return "[\"" + Name + "\", Distance = " + (int)Distance + ", Type = " + Type + ", React = " + Reaction + "]";
        //}
    }
}