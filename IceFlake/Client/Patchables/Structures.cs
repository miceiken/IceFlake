using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace IceFlake.Client.Patchables
{
    #region SpellBookRec

    public struct SpellBookRec
    {
        public uint ID;
        public KnownSpellType Knowledgde;
        public uint RequiredLevel;
        public byte Unk1;
        public byte Unk2;
        public uint Unk_TabType;

        public bool IsKnown
        {
            get { return Knowledgde == KnownSpellType.Learned; }
        }

        public bool IsAvailable
        {
            get { return RequiredLevel <= Manager.LocalPlayer.Level; }
        }
    }

    #endregion

    #region SpellRec

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SpellRec
    {
        public uint Id;
        public uint Category;
        public uint Dispel;
        public int Mechanic;

        public uint Attributes; //group of flags
        public uint AttributesEx;
        public uint AttributesEx2;
        public uint AttributesEx3;
        public uint AttributesEx4;
        public uint AttributesEx5;
        public uint AttributesEx6;
        public uint AttributesEx7;

        public uint Stances;
        public uint unk_320_2;
        public uint StancesNot;
        public uint unk_320_3;
        public uint Targets;
        public uint TargetCreatureType;
        public uint RequiresSpellFocus;
        public uint FacingCasterFlags;
        public uint CasterAuraState;
        public uint TargetAuraState;
        public uint CasterAuraStateNot;
        public uint TargetAuraStateNot;
        public uint casterAuraSpell;
        public uint targetAuraSpell;
        public uint excludeCasterAuraSpell;
        public uint excludeTargetAuraSpell;
        public uint CastingTimeIndex;
        public uint RecoveryTime;
        public uint CategoryRecoveryTime;
        public uint InterruptFlags;
        public uint AuraInterruptFlags;
        public uint ChannelInterruptFlags;
        public uint procFlags;
        public uint procChance;
        public uint procCharges;
        public uint maxLevel;
        public uint baseLevel;
        public uint spellLevel;
        public uint DurationIndex;
        public int powerType;
        public uint manaCost;
        public uint manaCostPerlevel;
        public uint manaPerSecond;
        public uint manaPerSecondPerLevel;
        public uint rangeIndex;
        public float speed;
        public uint modalNextSpell;
        public uint StackAmount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] Totem;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I4)]
        public int[] Reagent;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.U4)]
        public uint[] ReagentCount;
        public int EquippedItemClass;
        public int EquippedItemSubClassMask;
        public int EquippedItemInventoryTypeMask;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public int[] Effect;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I4)]
        public int[] EffectDieSides;
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I4)]
        //public int[] EffectBaseDice;
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.R4)]
        //public float[] EffectDicePerLevel;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.R4)]
        public float[] EffectRealPointsPerLevel;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I4)]
        public int[] EffectBasePoints;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public uint[] EffectMechanic;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public uint[] EffectImplicitTargetA;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public uint[] EffectImplicitTargetB;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public uint[] EffectRadiusIndex;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public uint[] EffectApplyAuraName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public uint[] EffectAmplitude;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.R4)]
        public float[] EffectMultipleValue;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public uint[] EffectChainTarget;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public uint[] EffectItemType;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I4)]
        public int[] EffectMiscValue;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I4)]
        public int[] EffectMiscValueB;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public uint[] EffectTriggerSpell;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.R4)]
        public float[] EffectPointsPerComboPoint;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.Struct)]
        public Flag96[] EffectSpellClassMask;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] SpellVisual;
        public uint SpellIconID;
        public uint activeIconID;
        public uint spellPriority;

        [MarshalAs(UnmanagedType.LPStr)]
        public string SpellName;
        [MarshalAs(UnmanagedType.LPStr)]
        public string Rank;
        [MarshalAs(UnmanagedType.LPStr)]
        public string Description;
        [MarshalAs(UnmanagedType.LPStr)]
        public string ToolTip;

        public uint ManaCostPercentage;
        public uint StartRecoveryCategory;
        public uint StartRecoveryTime;
        public uint MaxTargetLevel;
        public uint SpellFamilyName;
        public Flag96 SpellFamilyFlags;
        public uint MaxAffectedTargets;
        public uint DmgClass;
        public uint PreventionType;
        public uint StanceBarOrder;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.R4)]
        public float[] DmgMultiplier;
        public uint MinFactionId;
        public uint MinReputation;
        public uint RequiredAuraVision;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] TotemCategory;
        public int AreaGroupId;
        public int SchoolMask;
        public uint runeCostID;
        public uint spellMissileID;
        public uint PowerDisplayId;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.R4)]
        public float[] unk_320_4;
        public uint spellDescriptionVariableID;
        public uint SpellDifficultyId;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Flag96
    {
        public uint flag1;
        public uint flag2;
        public uint flag3;
    }

    #endregion

    #region SpellEffectRec

    [StructLayout(LayoutKind.Sequential)]
    public struct SpellEffectRec
    {
        public uint ID;
        public uint Effect;
        public float EffectAmplitude;
        public uint EffectAura;
        public uint EffectAuraPeriod;
        public int EffectBasePoints;
        public float EffectUnk_320;
        public float EffectChainAmplitude;
        public uint EffectChainTargets;
        public uint EffectDieSides;
        public uint EffectItemType;
        public uint EffectMechanic;
        public int EffectMiscValue;
        public int EffectMiscValueB;
        public float EffectPointsPerCombo;
        public uint EffectRadiusIndex;
        public uint EffectRadiusMaxIndex;
        public float EffectRealPointsPerLevel;
        public uint EffectSpellClassMask1;
        public uint EffectSpellClassMask2;
        public uint EffectSpellClassMask3;
        public int EffectTriggerSpell;
        public uint ImplicitTargetA;
        public uint ImplicitTargetB;
        public uint EffectSpellId;
        public uint EffectIndex;
        public int EffectUnk1;
    }

    #endregion

    #region AuraRec

    [StructLayout(LayoutKind.Sequential)]
    public struct AuraRec
    {
        public ulong CreatorGuid;
        public uint AuraId;
        public AuraFlags Flags;
        public byte Level;
        public ushort StackCount;
        public uint Duration;
        public uint EndTime;
    }

    #endregion

    #region FactionTemplateRec

    [StructLayout(LayoutKind.Sequential)]
    public struct FactionTemplateRec // sizeof(0x38)
    {
        public uint m_ID; // 0
        public uint m_faction; // 1
        public uint m_flags; // 2
        public uint m_factionGroup; // 3
        public uint m_friendGroup; // 4
        public uint m_enemyGroup; // 5
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public uint[] m_enemies; // 6-9
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public uint[] m_friend; // 10-13
    }

    #endregion

    #region AreaTableRec

    [StructLayout(LayoutKind.Sequential)]
    public struct AreaTableRec
    {
        public uint m_ID; // 0
        public uint m_ContinentID; // 1
        public uint m_ParentAreaID; // 2
        public uint m_AreaBit; // 3
        public uint m_flags; // 4
        public uint m_SoundProviderPref; // 5
        public uint m_SoundProviderPrefUnderwater; // 6
        public uint m_AmbienceID; // 7
        public uint m_ZoneMusic; // 8
        public uint m_IntroSound; // 9
        public uint m_ExplorationLevel; // 10
        private IntPtr m_AreaName_lang; // 11
        public uint m_factionGroupMask; // 12
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public uint[] m_liquidTypeID; // 13-16
        public float m_minElevation; // 17
        public float m_ambient_multiplier; // 18
        public uint m_lightid; // 19
        public uint m_field20; // 20
        public uint m_field21; // 21
        public uint m_field22; // 22
        public uint m_field23; // 23
        public uint m_field24; // 24

        public string AreaName
        {
            get { return Manager.Memory.ReadString(m_AreaName_lang); }
        }
    }

    #endregion

    #region ItemRec

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ItemCacheRecord
    {
        public int ID;
        public ItemClass Class;
        public int SubClassId;
        public int Unk0;
        public int DisplayInfoId;
        public ItemQuality Quality;
        public ItemFlags TypeFlags;
        public int BuyPrice;
        public int Faction;
        public int SellPrice;
        public InventoryType InventoryType;
        public int AllowedClasses;
        public int AllowedRaces;
        public int ItemLevel;
        public int RequiredLevel;
        public int RequiredSkill;
        public int RequiredSkillLevel;
        public int RequireSpell;
        public int RequiredHonorRank;
        public int RequiredCityRank;
        public int RequiredReputationFaction;
        public int RequiredReputationRank;
        public int UniqueCount;
        public int MaxStackSize;
        public int BagSlots;
        public int NumberOfStats;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.I4)]
        public int[] StatTypes;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.I4)]
        public int[] StatValues;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I4)]
        public int[] Unk1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.R4)]
        public float[] DamageMin;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.R4)]
        public float[] DamageMax;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I4)]
        public int[] DamageType;
        public int ResistPhysical_Armor;
        public int ResistHoly;
        public int ResistFire;
        public int ResistNature;
        public int ResistFrost;
        public int ResistShadow;
        public int ResistArcane;
        public int WeaponDelay;
        public int AmmoType;
        public float RangeModifier;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I4)]
        public int[] SpellId;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I4)]
        public int[] SpellTriggerId;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I4)]
        public int[] SpellCharges;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I4)]
        public int[] SpellCooldown;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I4)]
        public int[] SpellCategory;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I4)]
        public int[] SpellCategoryCooldown;
        public ItemBondType Bonding;
        [MarshalAs(UnmanagedType.LPStr)]
        public string Description;
        public int BookTextId;
        public int BookPages;
        public int BookStationaryId;
        public int BeginQuestId;
        public int LockPickSkillRequired;
        public int MaterialId;
        public int SheathId;
        public int RandomPropertyId;
        public int RandomSuffixId;
        public int BlockValue;
        public int ItemSetId;
        public int DurabilityValue;
        public int ItemAreaId;
        public int ItemMapId;
        public int BagFamily;
        public int totemCategory;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I4)]
        public int[] SocketColor;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I4)]
        public int[] SocketContent;
        public int SocketBonus;
        public int GemProperties;
        public int DisenchantSkillLevel;
        public float ArmorDamageModifier;
        public int ItemExtendedCost;
        public int ItemLimitId;
        public int Unk2;
        [MarshalAs(UnmanagedType.LPStr)]
        public string Name;

        public ItemArmorClass ArmorClass
        {
            get { return (ItemArmorClass)SubClassId; }
        }

        public ItemWeaponClass WeaponClass
        {
            get { return (ItemWeaponClass)SubClassId; }
        }

        public ItemGemClass GemClass
        {
            get { return (ItemGemClass)SubClassId; }
        }

        public bool IsFoodDrink
        {
            get
            {

                return Class == ItemClass.Consumable &&
                       (ItemSubclassConsumable)SubClassId == ItemSubclassConsumable.FoodDrink;
            }
        }

        //public uint Armor
        //{
        //    get
        //    {
        //        if (ID == 0)
        //            return 0;

        //        if (Rarity == ItemQuality.Heirloom)
        //            return 0;

        //        if (Class != ItemClass.Armor)
        //            return 0;

        //        if (Class == ItemClass.Armor && ArmorClass == ItemArmorClass.Shield)
        //        {
        //            var ias = Manager.DBC[ClientDB.ItemArmorShield].GetRow(ItemLevel);
        //            if (ias == null)
        //                return 0;
        //            return (uint)(ias.GetField<float>(1 + (uint)Quality) + 0.5f);
        //        }

        //        var iaq = Manager.DBC[ClientDB.ItemArmorQuality].GetRow(ItemLevel);
        //        var iat = Manager.DBC[ClientDB.ItemArmorTotal].GetRow(ItemLevel);
        //        if (iaq == null || iat == null)
        //            return 0;

        //        WoWDB.DbTable.Row al = null;
        //        if (InventoryType == InventoryType.Robe)
        //            al = Manager.DBC[ClientDB.ArmorLocation].GetRow((uint)InventoryType.Chest);
        //        else
        //            al = Manager.DBC[ClientDB.ArmorLocation].GetRow((uint)InventoryType);
        //        if (al == null)
        //            return 0;

        //        float iatMult, alMult;
        //        switch (ArmorClass)
        //        {
        //            case ItemArmorClass.Cloth:
        //                iatMult = iat.GetField<float>(2);
        //                alMult = al.GetField<float>(2);
        //                break;
        //            case ItemArmorClass.Leather:
        //                iatMult = iat.GetField<float>(3);
        //                alMult = al.GetField<float>(3);
        //                break;
        //            case ItemArmorClass.Mail:
        //                iatMult = iat.GetField<float>(4);
        //                alMult = al.GetField<float>(4);
        //                break;
        //            case ItemArmorClass.Plate:
        //                iatMult = iat.GetField<float>(5);
        //                alMult = al.GetField<float>(5);
        //                break;
        //            default:
        //                return 0;
        //        }

        //        return (uint)(iaq.GetField<float>(1 + (uint)Rarity) * iatMult * alMult + 0.5f);
        //    }
        //}

        //public float DPS
        //{
        //    get
        //    {
        //        if (ID == 0)
        //            return 0f;

        //        if (Class != ItemClass.Weapon)
        //            return 0f;

        //        if (Rarity == ItemQuality.Heirloom)
        //            return 0f;

        //        WoWDB.DbTable table = null;

        //        switch (InventoryType)
        //        {
        //            case InventoryType.Weapon:
        //            case InventoryType.WeaponMainHand:
        //            case InventoryType.WeaponOffHand:
        //                if (Flags2.HasFlag(ItemFlags2.CasterWeapon))
        //                    table = WoWDB.GetTable(ClientDB.ItemDamageOneHandCaster);
        //                else
        //                    table = WoWDB.GetTable(ClientDB.ItemDamageOneHand);
        //                break;
        //            case InventoryType.TwoHandedWeapon:
        //                if (Flags2.HasFlag(ItemFlags2.CasterWeapon))
        //                    table = WoWDB.GetTable(ClientDB.ItemDamageTwoHandCaster);
        //                else
        //                    table = WoWDB.GetTable(ClientDB.ItemDamageTwoHand);
        //                break;
        //            case InventoryType.Ammo:
        //                table = WoWDB.GetTable(ClientDB.ItemDamageAmmo);
        //                break;
        //            case InventoryType.Ranged:
        //            case InventoryType.Thrown:
        //            case InventoryType.RangedRight:
        //                switch (WeaponClass)
        //                {
        //                    case ItemWeaponClass.Bow:
        //                    case ItemWeaponClass.Gun:
        //                    case ItemWeaponClass.Crossbow:
        //                        table = WoWDB.GetTable(ClientDB.ItemDamageRanged);
        //                        break;
        //                    case ItemWeaponClass.Thrown:
        //                        table = WoWDB.GetTable(ClientDB.ItemDamageThrown);
        //                        break;
        //                    case ItemWeaponClass.Wand:
        //                        table = WoWDB.GetTable(ClientDB.ItemDamageWand);
        //                        break;
        //                }
        //                break;
        //        }

        //        if (table == null)
        //            return 0f;

        //        var row = table.GetRow(ItemLevel);
        //        if (row == null)
        //            return 0f;

        //        return (float)Math.Round(row.GetField<float>(1 + (uint)Rarity), 1);
        //    }
        //}

        public Dictionary<ItemStatType, int> Stats
        {
            get
            {
                // TODO: Fix this
                //if (RandomProperty > 0 || RandomSuffix > 0)
                //    return RandomStats;

                var stats = new Dictionary<ItemStatType, int>();
                for (int i = 0; i < 10; i++)
                {
                    if (StatTypes[i] == 0 || StatValues[i] == 0) continue;
                    stats.Add((ItemStatType)StatTypes[i], StatValues[i]);
                }
                return stats;
            }
        }

        public Dictionary<ItemStatType, int> RandomStats
        {
            get
            {
                if (RandomPropertyId == 0 && RandomSuffixId == 0)
                    return null;

                var sieEntries = new List<SpellItemEnchantmentRec>();
                var sieStorage = Manager.DBC[ClientDB.SpellItemEnchantment];
                if (RandomPropertyId > 0)
                { // ItemRandomProperty
                    var irp = Manager.DBC[ClientDB.ItemRandomProperties].GetRow(RandomPropertyId);
                    for (uint i = 2; i < 6; i++)
                    {
                        var sieRef = irp.GetField<int>(i);
                        if (sieRef == 0) continue;
                        sieEntries.Add(sieStorage.GetRow(sieRef).GetStruct<SpellItemEnchantmentRec>());
                    }
                }
                else
                { // ItemRandomSuffix
                    var irs = Manager.DBC[ClientDB.ItemRandomSuffix].GetRow(RandomSuffixId);
                    for (uint i = 3; i < 7; i++)
                    {
                        var sieRef = irs.GetField<int>(i);
                        if (sieRef == 0) continue;
                        sieEntries.Add(sieStorage.GetRow(sieRef).GetStruct<SpellItemEnchantmentRec>());
                    }
                }

                var stats = new Dictionary<ItemStatType, int>();
                foreach (var sie in sieEntries)
                {
                    for (var i = 0; i < 3; i++)
                    {
                        if (sie.EffectID[i] == 0
                            || (sie.EffectValueMin[i] == 0 && sie.EffectValueMax[i] == 0))
                            continue;

                        var stat = (ItemStatType)sie.EffectID[i];
                        var value = (sie.EffectValueMin[i] + sie.EffectValueMax[i]) / 2; // get average
                        if (stats.ContainsKey(stat))
                            stats[stat] += value;
                        else
                            stats.Add(stat, value);
                    }
                }

                return stats;
            }
        }

        public Dictionary<SocketColor, int> Sockets
        {
            get
            {
                var ret = new Dictionary<SocketColor, int>();
                for (int i = 0; i < 3; i++)
                {
                    if (SocketColor[i] == 0) continue;
                    ret.Add((SocketColor)SocketColor[i], SocketContent[i]);
                }
                return ret;
            }
        }

        private const int MAX_SPELL_EFFECTS = 3;

        public bool Eatable
        {
            get
            {
                if (!IsFoodDrink)
                    return false;

                for (var i = 0; i < MAX_SPELL_EFFECTS; i++)
                {
                    //var effect = WoWSpellCollection.GetSpellEffectRecord(SpellId[0], i);
                    //var auraType = (AuraType)effect.EffectAura;
                    //if (auraType == AuraType.OBS_MOD_HEALTH || auraType == AuraType.MOD_REGEN)
                    //    return true;
                }
                return false;
            }
        }

        public bool Drinkable
        {
            get
            {
                if (!IsFoodDrink)
                    return false;

                for (var i = 0; i < MAX_SPELL_EFFECTS; i++)
                {
                    //var effect = WoWSpellCollection.GetSpellEffectRecord(SpellId[0], i);
                    //var auraType = (AuraType)effect.EffectAura;
                    //if (auraType == AuraType.OBS_MOD_MANA || (auraType == AuraType.MOD_POWER_REGEN && (PowerType)effect.EffectMiscValue == PowerType.Mana))
                    //    return true;
                }
                return false;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SpellItemEnchantmentRec
    {
        public uint ID;
        public uint Charges;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public uint[] EffectID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public int[] EffectValueMin;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public int[] EffectValueMax;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public int[] EffectArg;
        private readonly IntPtr _Name;

        public string Name
        {
            get { return _Name != IntPtr.Zero ? Manager.Memory.ReadString(_Name) : string.Empty; }
        }
    }

    #endregion

    #region ItemEnchantment

    [StructLayout(LayoutKind.Sequential)]
    public struct ItemEnchantment
    {
        public int Id;
        public int Duration;
        public int Charges;
    }

    #endregion

    #region MapRec

    [StructLayout(LayoutKind.Sequential)]
    internal struct MapRec
    {
        public uint m_ID; // 0
        public IntPtr _m_Directory; // 1
        public InstanceType m_InstanceType; // 2 
        public MapFlags m_Flags; // 3
        public uint m_Unk4; // 4  Unknown, values seem to be only 1,2 or 3
        public IntPtr _m_MapName_lang; // 5
        public uint m_areaTableID; // 6
        public IntPtr _m_MapDescription0_lang; // 7
        public IntPtr _m_MapDescription1_lang; // 8
        public uint m_LoadingScreenID; // 9
        public float m_minimapIconScale; // 10
        public uint m_corpseMapID; // 11
        public float m_corpseX; // 12
        public float m_corpseY; // 13
        public uint m_timeOfDayOverride; // 14
        public uint m_expansionID; // 15 (Vanilla: 0, BC: 1, WotLK: 2, Cata: 3, MOP: 4)
        public uint m_raidOffset; // 16
        public uint m_maxPlayers; // 17
        public uint m_parentMapID; // 18
        // read area name using our static memory reading helper class
        public string m_Directory
        {
            get { return Manager.Memory.ReadString(_m_Directory); }
        }

        public string m_MapName_lang
        {
            get { return Manager.Memory.ReadString(_m_MapName_lang); }
        }

        public string m_MapDescription0_lang
        {
            get { return Manager.Memory.ReadString(_m_MapDescription0_lang); }
        }

        public string m_MapDescription1_lang
        {
            get { return Manager.Memory.ReadString(_m_MapDescription1_lang); }
        }
    };

    #endregion

    #region AuctionEntry

    [StructLayout(LayoutKind.Sequential)]
    public struct AuctionEntry
    {
        private uint unk0;
        public uint Id;
        public uint Entry;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public ItemEnchantment[] Enchants;
        public uint RandomProperty;
        public uint RandomSuffix;
        public uint StackSize;
        public uint Charges;
        public uint ItemFlags;
        public ulong Owner;
        public ulong MinBid;
        public ulong MinIncrement;
        public ulong BuyoutPrice;
        public uint TimeLeft;
        private readonly uint unk1;
        public ulong Bidder;
        public ulong BidAmount;
        public int SaleStatus;
        private readonly uint unk2;

        public DateTime Expires
        {
            get { return DateTime.Now.AddMilliseconds(TimeLeft - Helper.PerformanceCount); }
        }

        public ItemCacheRecord ItemInfo
        {
            get { return IceFlake.Client.Objects.WoWItem.GetItemRecordFromId(Id); }
        }

        public string BuyoutPriceInCurrency
        {
            get { return BuyoutPrice.ToWowCurrency(); }
        }

        public static readonly int Size = Marshal.SizeOf(typeof(AuctionEntry));
    }

    #endregion

    #region Quest

    [StructLayout(LayoutKind.Sequential)]
    public struct QuestLogEntry
    {
        public int ID;
        public QuestState State;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public short[] Objectives;
        public int Time;

        public WoWQuest AsWoWQuest()
        {
            return new WoWQuest(this.ID);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct QuestCacheRecord
    {
        public int Id;
        public int Method;
        public int Level;
        public int RequiredLevel;
        public int AreaIdOrSortId;
        public int InfoId;
        public int SuggestedPlayers;
        public int FriendlyFactionID;
        public int FriendlyFactionAmount;
        public int HostileFactionID;
        public int HostileFactionAmount;
        public int NextQuestId;
        public int XPId;
        public int RewardMoney;
        public int RewardMoneyInsteadOfXp;
        public int RewardSpellId;
        public int EffectOnPlayer;
        public int RewardHonor;
        public float RewardHonorBonus;
        public int StartingItemId;
        public int Flags;
        public fixed int RewardItem[4];
        public fixed int RewardItemCount[4];
        public fixed int RewardChoiceItem[6];
        public fixed int RewardChoiceItemCount[6];
        public int PointMapID;
        public float PointX;
        public float PointY;
        public int PointOptional;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string Name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3000)]
        public string ObjectiveText;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3000)]
        public string Description;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string EndText;
        public fixed int ObjectiveId[4];
        public fixed int ObjectiveRequiredCount[4];
        public fixed int CollectItemId[6];
        public fixed int CollectItemCount[6];
        public fixed int IntermediateItemId[4];
        public fixed int IntermediateItemCount[4];
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        //public fixed byte OverrideObjectiveText[4][256]; // 4 * 256
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string OverrideObjectiveText1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string OverrideObjectiveText2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string OverrideObjectiveText3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string OverrideObjectiveText4;
        public int RewardTitleId;
        public int RequiredPlayersKilled;
        public int RewardTalentPoints;
        public int RewardArenaPoints;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2048)]
        public string CompletionText;
        public fixed int RewardReputationFaction[5];
        public fixed int FactionRewardID[5];
        public fixed int RewardReputationOverride[5];
        public int Unk17;
    }

    public enum QuestState
    {
        None = 0,
        Complete = 1,
        Fail = 2,
    }

    public struct QuestCache
    {
        public uint Id;
        public uint Method;
        public int QuestLevel;
        public uint MinLevel;
        public int ZoneOrSort;
        public uint Type;
        public int SuggestedPlayers;
        public uint RepObjectiveFaction1;
        public int RepObjectiveValue1;
        public uint RepObjectiveFaction2;
        public int RepObjectiveValue2;
        public uint NextQuestInChain;
        public int RewardXpId;
        public uint RewardMoneyMaxLevel;
        public uint RewardSpell;
        public uint RewardSpellCast;
        public uint RewardHonorAddition;
        public float RewardHonorMultiplier;
        public uint SourceItemId;
        public uint QuestFlags;
        public uint CharTitleId;
        public int PlayersSlain;
        public int BonusTalents;
        public int BonusArenaPoints;
        public int Unk2;

        public uint RewardItemId1;
        public uint RewardItemCount1;
        public uint RewardItemId2;
        public uint RewardItemCount2;
        public uint RewardItemId3;
        public uint RewardItemCount3;
        public uint RewardItemId4;
        public uint RewardItemCount4;

        public uint RewardChoiceItemId1;
        public uint RewardChoiceItemCount1;
        public uint RewardChoiceItemId2;
        public uint RewardChoiceItemCount2;
        public uint RewardChoiceItemId3;
        public uint RewardChoiceItemCount3;
        public uint RewardChoiceItemId4;
        public uint RewardChoiceItemCount4;
        public uint RewardChoiceItemId5;
        public uint RewardChoiceItemCount5;
        public uint RewardChoiceItemId6;
        public uint RewardChoiceItemCount6;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public uint[] RewardReputationFactions;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public int[] RewardReputationFactionsValue;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public uint[] RewardReputationValue;

        public uint PointMapId;
        public float PointX;
        public float PointY;
        public int Unk3;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string Title;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string Objectives;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string Description;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string ToDoText;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string EndText;

        public int ReqCreatureOrGOId1;
        public int ReqCreatureOrGoCount1;
        public int ReqSourceId1;
        public int ReqSourceIdMaxCount1;

        public int ReqCreatureOrGOId2;
        public int ReqCreatureOrGoCount2;
        public int ReqSourceId2;
        public int ReqSourceIdMaxCount2;

        public int ReqCreatureOrGOId3;
        public int ReqCreatureOrGoCount3;
        public int ReqSourceId3;
        public int ReqSourceIdMaxCount3;

        public int ReqCreatureOrGOId4;
        public int ReqCreatureOrGoCount4;
        public int ReqSourceId4;
        public int ReqSourceIdMaxCount4;

        public int ReqItemId1;
        public int ReqItemCount1;

        public int ReqItemId2;
        public int ReqItemCount2;

        public int ReqItemId3;
        public int ReqItemCount3;

        public int ReqItemId4;
        public int ReqItemCount4;

        public int ReqItemId5;
        public int ReqItemCount5;

        public int ReqItemId6;
        public int ReqItemCount6;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string OverrideObjectiveText1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string OverrideObjectiveText2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string OverrideObjectiveText3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string OverrideObjectiveText4;
    }

    #endregion

    #region TerrainClickEvent

    [StructLayout(LayoutKind.Sequential)]
    public struct TerrainClickEvent
    {
        public ulong GUID;
        public Location Position;
        [MarshalAs(UnmanagedType.U4)]
        public MouseButton Button;
    }

    [Flags]
    public enum MouseButton : uint
    {
        Left = 1,
        Middle = 2,
        None = 0,
        Right = 4,
        XButton1 = 8,
        XButton2 = 0x10
    }

    #endregion

    #region DBC

    [StructLayout(LayoutKind.Sequential)]
    internal struct WoWClientDB
    {
        public IntPtr VTable; // pointer to vtable
        public uint NumRows; // number of rows
        public uint MaxIndex; // maximal row index
        public uint MinIndex; // minimal row index
        public IntPtr Data; // pointer to actual dbc file data
        public IntPtr FirstRow; // pointer to first row
        public IntPtr Rows; // pointer to rows array - not anymore?
        public IntPtr Unk1; // ptr
        public uint Unk2; // 1
        public IntPtr Unk3; // ptr
        public uint RowEntrySize; // 2 or 4
    };

    [StructLayout(LayoutKind.Sequential)]
    internal struct DBCFile
    {
        public uint Magic;
        public int RecordsCount;
        public int FieldsCount;
        public int RecordSize;
        public int StringTableSize;
    }

    #endregion

    #region Camera

    public unsafe struct CameraInfo
    {
        fixed int unk0[2];
        public SlimDX.Vector3 Position;
        public fixed float Facing[9];
        public float NearPlane;
        public float FarPlane;
        public float FieldOfView;
        public float Aspect;
    }

    #endregion
}