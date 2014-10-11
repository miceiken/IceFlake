namespace IceFlake.Client.Patchables
{
    public enum WoWObjectFields
    {
        OBJECT_FIELD_GUID = 0x0000, // Size: 2, Type: LONG, Flags: PUBLIC
        OBJECT_FIELD_TYPE = 0x0002, // Size: 1, Type: INT, Flags: PUBLIC
        OBJECT_FIELD_ENTRY = 0x0003, // Size: 1, Type: INT, Flags: PUBLIC
        OBJECT_FIELD_SCALE_X = 0x0004, // Size: 1, Type: FLOAT, Flags: PUBLIC
        OBJECT_FIELD_PADDING = 0x0005, // Size: 1, Type: INT, Flags: NONE
        OBJECT_END = 0x0006,
    };

    public enum WoWItemFields
    {
        ITEM_FIELD_OWNER = WoWObjectFields.OBJECT_END + 0x0000, // Size: 2, Type: LONG, Flags: PUBLIC
        ITEM_FIELD_CONTAINED = WoWObjectFields.OBJECT_END + 0x0002, // Size: 2, Type: LONG, Flags: PUBLIC
        ITEM_FIELD_CREATOR = WoWObjectFields.OBJECT_END + 0x0004, // Size: 2, Type: LONG, Flags: PUBLIC
        ITEM_FIELD_GIFTCREATOR = WoWObjectFields.OBJECT_END + 0x0006, // Size: 2, Type: LONG, Flags: PUBLIC
        ITEM_FIELD_STACK_COUNT = WoWObjectFields.OBJECT_END + 0x0008, // Size: 1, Type: INT, Flags: OWNER, ITEM_OWNER
        ITEM_FIELD_DURATION = WoWObjectFields.OBJECT_END + 0x0009, // Size: 1, Type: INT, Flags: OWNER, ITEM_OWNER
        ITEM_FIELD_SPELL_CHARGES = WoWObjectFields.OBJECT_END + 0x000A, // Size: 5, Type: INT, Flags: OWNER, ITEM_OWNER
        ITEM_FIELD_FLAGS = WoWObjectFields.OBJECT_END + 0x000F, // Size: 1, Type: INT, Flags: PUBLIC
        ITEM_FIELD_ENCHANTMENT_1_1 = WoWObjectFields.OBJECT_END + 0x0010, // Size: 2, Type: INT, Flags: PUBLIC
        ITEM_FIELD_ENCHANTMENT_1_3 = WoWObjectFields.OBJECT_END + 0x0012, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        ITEM_FIELD_ENCHANTMENT_2_1 = WoWObjectFields.OBJECT_END + 0x0013, // Size: 2, Type: INT, Flags: PUBLIC
        ITEM_FIELD_ENCHANTMENT_2_3 = WoWObjectFields.OBJECT_END + 0x0015, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        ITEM_FIELD_ENCHANTMENT_3_1 = WoWObjectFields.OBJECT_END + 0x0016, // Size: 2, Type: INT, Flags: PUBLIC
        ITEM_FIELD_ENCHANTMENT_3_3 = WoWObjectFields.OBJECT_END + 0x0018, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        ITEM_FIELD_ENCHANTMENT_4_1 = WoWObjectFields.OBJECT_END + 0x0019, // Size: 2, Type: INT, Flags: PUBLIC
        ITEM_FIELD_ENCHANTMENT_4_3 = WoWObjectFields.OBJECT_END + 0x001B, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        ITEM_FIELD_ENCHANTMENT_5_1 = WoWObjectFields.OBJECT_END + 0x001C, // Size: 2, Type: INT, Flags: PUBLIC
        ITEM_FIELD_ENCHANTMENT_5_3 = WoWObjectFields.OBJECT_END + 0x001E, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        ITEM_FIELD_ENCHANTMENT_6_1 = WoWObjectFields.OBJECT_END + 0x001F, // Size: 2, Type: INT, Flags: PUBLIC
        ITEM_FIELD_ENCHANTMENT_6_3 = WoWObjectFields.OBJECT_END + 0x0021, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        ITEM_FIELD_ENCHANTMENT_7_1 = WoWObjectFields.OBJECT_END + 0x0022, // Size: 2, Type: INT, Flags: PUBLIC
        ITEM_FIELD_ENCHANTMENT_7_3 = WoWObjectFields.OBJECT_END + 0x0024, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        ITEM_FIELD_ENCHANTMENT_8_1 = WoWObjectFields.OBJECT_END + 0x0025, // Size: 2, Type: INT, Flags: PUBLIC
        ITEM_FIELD_ENCHANTMENT_8_3 = WoWObjectFields.OBJECT_END + 0x0027, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        ITEM_FIELD_ENCHANTMENT_9_1 = WoWObjectFields.OBJECT_END + 0x0028, // Size: 2, Type: INT, Flags: PUBLIC
        ITEM_FIELD_ENCHANTMENT_9_3 = WoWObjectFields.OBJECT_END + 0x002A, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        ITEM_FIELD_ENCHANTMENT_10_1 = WoWObjectFields.OBJECT_END + 0x002B, // Size: 2, Type: INT, Flags: PUBLIC
        ITEM_FIELD_ENCHANTMENT_10_3 = WoWObjectFields.OBJECT_END + 0x002D, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        ITEM_FIELD_ENCHANTMENT_11_1 = WoWObjectFields.OBJECT_END + 0x002E, // Size: 2, Type: INT, Flags: PUBLIC
        ITEM_FIELD_ENCHANTMENT_11_3 = WoWObjectFields.OBJECT_END + 0x0030, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        ITEM_FIELD_ENCHANTMENT_12_1 = WoWObjectFields.OBJECT_END + 0x0031, // Size: 2, Type: INT, Flags: PUBLIC
        ITEM_FIELD_ENCHANTMENT_12_3 = WoWObjectFields.OBJECT_END + 0x0033, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        ITEM_FIELD_PROPERTY_SEED = WoWObjectFields.OBJECT_END + 0x0034, // Size: 1, Type: INT, Flags: PUBLIC
        ITEM_FIELD_RANDOM_PROPERTIES_ID = WoWObjectFields.OBJECT_END + 0x0035, // Size: 1, Type: INT, Flags: PUBLIC
        ITEM_FIELD_DURABILITY = WoWObjectFields.OBJECT_END + 0x0036, // Size: 1, Type: INT, Flags: OWNER, ITEM_OWNER
        ITEM_FIELD_MAXDURABILITY = WoWObjectFields.OBJECT_END + 0x0037, // Size: 1, Type: INT, Flags: OWNER, ITEM_OWNER
        ITEM_FIELD_CREATE_PLAYED_TIME = WoWObjectFields.OBJECT_END + 0x0038, // Size: 1, Type: INT, Flags: PUBLIC
        ITEM_FIELD_PAD = WoWObjectFields.OBJECT_END + 0x0039, // Size: 1, Type: INT, Flags: NONE
        ITEM_END = WoWObjectFields.OBJECT_END + 0x003A,
    };

    public enum WoWContainerFields
    {
        CONTAINER_FIELD_NUM_SLOTS = WoWItemFields.ITEM_END + 0x0000, // Size: 1, Type: INT, Flags: PUBLIC
        CONTAINER_ALIGN_PAD = WoWItemFields.ITEM_END + 0x0001, // Size: 1, Type: BYTES, Flags: NONE
        CONTAINER_FIELD_SLOT_1 = WoWItemFields.ITEM_END + 0x0002, // Size: 72, Type: LONG, Flags: PUBLIC
        CONTAINER_END = WoWItemFields.ITEM_END + 0x004A,
    };

    public enum WoWUnitFields
    {
        UNIT_FIELD_CHARM = WoWObjectFields.OBJECT_END + 0x0000, // Size: 2, Type: LONG, Flags: PUBLIC
        UNIT_FIELD_SUMMON = WoWObjectFields.OBJECT_END + 0x0002, // Size: 2, Type: LONG, Flags: PUBLIC
        UNIT_FIELD_CRITTER = WoWObjectFields.OBJECT_END + 0x0004, // Size: 2, Type: LONG, Flags: PRIVATE
        UNIT_FIELD_CHARMEDBY = WoWObjectFields.OBJECT_END + 0x0006, // Size: 2, Type: LONG, Flags: PUBLIC
        UNIT_FIELD_SUMMONEDBY = WoWObjectFields.OBJECT_END + 0x0008, // Size: 2, Type: LONG, Flags: PUBLIC
        UNIT_FIELD_CREATEDBY = WoWObjectFields.OBJECT_END + 0x000A, // Size: 2, Type: LONG, Flags: PUBLIC
        UNIT_FIELD_TARGET = WoWObjectFields.OBJECT_END + 0x000C, // Size: 2, Type: LONG, Flags: PUBLIC
        UNIT_FIELD_CHANNEL_OBJECT = WoWObjectFields.OBJECT_END + 0x000E, // Size: 2, Type: LONG, Flags: PUBLIC
        UNIT_CHANNEL_SPELL = WoWObjectFields.OBJECT_END + 0x0010, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_FIELD_BYTES_0 = WoWObjectFields.OBJECT_END + 0x0011, // Size: 1, Type: BYTES, Flags: PUBLIC
        UNIT_FIELD_HEALTH = WoWObjectFields.OBJECT_END + 0x0012, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_FIELD_POWER1 = WoWObjectFields.OBJECT_END + 0x0013, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_FIELD_POWER2 = WoWObjectFields.OBJECT_END + 0x0014, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_FIELD_POWER3 = WoWObjectFields.OBJECT_END + 0x0015, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_FIELD_POWER4 = WoWObjectFields.OBJECT_END + 0x0016, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_FIELD_POWER5 = WoWObjectFields.OBJECT_END + 0x0017, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_FIELD_POWER6 = WoWObjectFields.OBJECT_END + 0x0018, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_FIELD_POWER7 = WoWObjectFields.OBJECT_END + 0x0019, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_FIELD_MAXHEALTH = WoWObjectFields.OBJECT_END + 0x001A, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_FIELD_MAXPOWER1 = WoWObjectFields.OBJECT_END + 0x001B, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_FIELD_MAXPOWER2 = WoWObjectFields.OBJECT_END + 0x001C, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_FIELD_MAXPOWER3 = WoWObjectFields.OBJECT_END + 0x001D, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_FIELD_MAXPOWER4 = WoWObjectFields.OBJECT_END + 0x001E, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_FIELD_MAXPOWER5 = WoWObjectFields.OBJECT_END + 0x001F, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_FIELD_MAXPOWER6 = WoWObjectFields.OBJECT_END + 0x0020, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_FIELD_MAXPOWER7 = WoWObjectFields.OBJECT_END + 0x0021, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_FIELD_POWER_REGEN_FLAT_MODIFIER = WoWObjectFields.OBJECT_END + 0x0022,
        // Size: 7, Type: FLOAT, Flags: PRIVATE, OWNER
        UNIT_FIELD_POWER_REGEN_INTERRUPTED_FLAT_MODIFIER = WoWObjectFields.OBJECT_END + 0x0029,
        // Size: 7, Type: FLOAT, Flags: PRIVATE, OWNER
        UNIT_FIELD_LEVEL = WoWObjectFields.OBJECT_END + 0x0030, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_FIELD_FACTIONTEMPLATE = WoWObjectFields.OBJECT_END + 0x0031, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_VIRTUAL_ITEM_SLOT_ID = WoWObjectFields.OBJECT_END + 0x0032, // Size: 3, Type: INT, Flags: PUBLIC
        UNIT_FIELD_FLAGS = WoWObjectFields.OBJECT_END + 0x0035, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_FIELD_FLAGS_2 = WoWObjectFields.OBJECT_END + 0x0036, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_FIELD_AURASTATE = WoWObjectFields.OBJECT_END + 0x0037, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_FIELD_BASEATTACKTIME = WoWObjectFields.OBJECT_END + 0x0038, // Size: 2, Type: INT, Flags: PUBLIC
        UNIT_FIELD_RANGEDATTACKTIME = WoWObjectFields.OBJECT_END + 0x003A, // Size: 1, Type: INT, Flags: PRIVATE
        UNIT_FIELD_BOUNDINGRADIUS = WoWObjectFields.OBJECT_END + 0x003B, // Size: 1, Type: FLOAT, Flags: PUBLIC
        UNIT_FIELD_COMBATREACH = WoWObjectFields.OBJECT_END + 0x003C, // Size: 1, Type: FLOAT, Flags: PUBLIC
        UNIT_FIELD_DISPLAYID = WoWObjectFields.OBJECT_END + 0x003D, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_FIELD_NATIVEDISPLAYID = WoWObjectFields.OBJECT_END + 0x003E, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_FIELD_MOUNTDISPLAYID = WoWObjectFields.OBJECT_END + 0x003F, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_FIELD_MINDAMAGE = WoWObjectFields.OBJECT_END + 0x0040,
        // Size: 1, Type: FLOAT, Flags: PRIVATE, OWNER, PARTY_LEADER
        UNIT_FIELD_MAXDAMAGE = WoWObjectFields.OBJECT_END + 0x0041,
        // Size: 1, Type: FLOAT, Flags: PRIVATE, OWNER, PARTY_LEADER
        UNIT_FIELD_MINOFFHANDDAMAGE = WoWObjectFields.OBJECT_END + 0x0042,
        // Size: 1, Type: FLOAT, Flags: PRIVATE, OWNER, PARTY_LEADER
        UNIT_FIELD_MAXOFFHANDDAMAGE = WoWObjectFields.OBJECT_END + 0x0043,
        // Size: 1, Type: FLOAT, Flags: PRIVATE, OWNER, PARTY_LEADER
        UNIT_FIELD_BYTES_1 = WoWObjectFields.OBJECT_END + 0x0044, // Size: 1, Type: BYTES, Flags: PUBLIC
        UNIT_FIELD_PETNUMBER = WoWObjectFields.OBJECT_END + 0x0045, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_FIELD_PET_NAME_TIMESTAMP = WoWObjectFields.OBJECT_END + 0x0046, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_FIELD_PETEXPERIENCE = WoWObjectFields.OBJECT_END + 0x0047, // Size: 1, Type: INT, Flags: OWNER
        UNIT_FIELD_PETNEXTLEVELEXP = WoWObjectFields.OBJECT_END + 0x0048, // Size: 1, Type: INT, Flags: OWNER
        UNIT_DYNAMIC_FLAGS = WoWObjectFields.OBJECT_END + 0x0049, // Size: 1, Type: INT, Flags: DYNAMIC
        UNIT_MOD_CAST_SPEED = WoWObjectFields.OBJECT_END + 0x004A, // Size: 1, Type: FLOAT, Flags: PUBLIC
        UNIT_CREATED_BY_SPELL = WoWObjectFields.OBJECT_END + 0x004B, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_NPC_FLAGS = WoWObjectFields.OBJECT_END + 0x004C, // Size: 1, Type: INT, Flags: DYNAMIC
        UNIT_NPC_EMOTESTATE = WoWObjectFields.OBJECT_END + 0x004D, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_FIELD_STAT0 = WoWObjectFields.OBJECT_END + 0x004E, // Size: 1, Type: INT, Flags: PRIVATE, OWNER
        UNIT_FIELD_STAT1 = WoWObjectFields.OBJECT_END + 0x004F, // Size: 1, Type: INT, Flags: PRIVATE, OWNER
        UNIT_FIELD_STAT2 = WoWObjectFields.OBJECT_END + 0x0050, // Size: 1, Type: INT, Flags: PRIVATE, OWNER
        UNIT_FIELD_STAT3 = WoWObjectFields.OBJECT_END + 0x0051, // Size: 1, Type: INT, Flags: PRIVATE, OWNER
        UNIT_FIELD_STAT4 = WoWObjectFields.OBJECT_END + 0x0052, // Size: 1, Type: INT, Flags: PRIVATE, OWNER
        UNIT_FIELD_POSSTAT0 = WoWObjectFields.OBJECT_END + 0x0053, // Size: 1, Type: INT, Flags: PRIVATE, OWNER
        UNIT_FIELD_POSSTAT1 = WoWObjectFields.OBJECT_END + 0x0054, // Size: 1, Type: INT, Flags: PRIVATE, OWNER
        UNIT_FIELD_POSSTAT2 = WoWObjectFields.OBJECT_END + 0x0055, // Size: 1, Type: INT, Flags: PRIVATE, OWNER
        UNIT_FIELD_POSSTAT3 = WoWObjectFields.OBJECT_END + 0x0056, // Size: 1, Type: INT, Flags: PRIVATE, OWNER
        UNIT_FIELD_POSSTAT4 = WoWObjectFields.OBJECT_END + 0x0057, // Size: 1, Type: INT, Flags: PRIVATE, OWNER
        UNIT_FIELD_NEGSTAT0 = WoWObjectFields.OBJECT_END + 0x0058, // Size: 1, Type: INT, Flags: PRIVATE, OWNER
        UNIT_FIELD_NEGSTAT1 = WoWObjectFields.OBJECT_END + 0x0059, // Size: 1, Type: INT, Flags: PRIVATE, OWNER
        UNIT_FIELD_NEGSTAT2 = WoWObjectFields.OBJECT_END + 0x005A, // Size: 1, Type: INT, Flags: PRIVATE, OWNER
        UNIT_FIELD_NEGSTAT3 = WoWObjectFields.OBJECT_END + 0x005B, // Size: 1, Type: INT, Flags: PRIVATE, OWNER
        UNIT_FIELD_NEGSTAT4 = WoWObjectFields.OBJECT_END + 0x005C, // Size: 1, Type: INT, Flags: PRIVATE, OWNER
        UNIT_FIELD_RESISTANCES = WoWObjectFields.OBJECT_END + 0x005D,
        // Size: 7, Type: INT, Flags: PRIVATE, OWNER, PARTY_LEADER
        UNIT_FIELD_RESISTANCEBUFFMODSPOSITIVE = WoWObjectFields.OBJECT_END + 0x0064,
        // Size: 7, Type: INT, Flags: PRIVATE, OWNER
        UNIT_FIELD_RESISTANCEBUFFMODSNEGATIVE = WoWObjectFields.OBJECT_END + 0x006B,
        // Size: 7, Type: INT, Flags: PRIVATE, OWNER
        UNIT_FIELD_BASE_MANA = WoWObjectFields.OBJECT_END + 0x0072, // Size: 1, Type: INT, Flags: PUBLIC
        UNIT_FIELD_BASE_HEALTH = WoWObjectFields.OBJECT_END + 0x0073, // Size: 1, Type: INT, Flags: PRIVATE, OWNER
        UNIT_FIELD_BYTES_2 = WoWObjectFields.OBJECT_END + 0x0074, // Size: 1, Type: BYTES, Flags: PUBLIC
        UNIT_FIELD_ATTACK_POWER = WoWObjectFields.OBJECT_END + 0x0075, // Size: 1, Type: INT, Flags: PRIVATE, OWNER
        UNIT_FIELD_ATTACK_POWER_MODS = WoWObjectFields.OBJECT_END + 0x0076,
        // Size: 1, Type: TWO_SHORT, Flags: PRIVATE, OWNER
        UNIT_FIELD_ATTACK_POWER_MULTIPLIER = WoWObjectFields.OBJECT_END + 0x0077,
        // Size: 1, Type: FLOAT, Flags: PRIVATE, OWNER
        UNIT_FIELD_RANGED_ATTACK_POWER = WoWObjectFields.OBJECT_END + 0x0078,
        // Size: 1, Type: INT, Flags: PRIVATE, OWNER
        UNIT_FIELD_RANGED_ATTACK_POWER_MODS = WoWObjectFields.OBJECT_END + 0x0079,
        // Size: 1, Type: TWO_SHORT, Flags: PRIVATE, OWNER
        UNIT_FIELD_RANGED_ATTACK_POWER_MULTIPLIER = WoWObjectFields.OBJECT_END + 0x007A,
        // Size: 1, Type: FLOAT, Flags: PRIVATE, OWNER
        UNIT_FIELD_MINRANGEDDAMAGE = WoWObjectFields.OBJECT_END + 0x007B, // Size: 1, Type: FLOAT, Flags: PRIVATE, OWNER
        UNIT_FIELD_MAXRANGEDDAMAGE = WoWObjectFields.OBJECT_END + 0x007C, // Size: 1, Type: FLOAT, Flags: PRIVATE, OWNER
        UNIT_FIELD_POWER_COST_MODIFIER = WoWObjectFields.OBJECT_END + 0x007D,
        // Size: 7, Type: INT, Flags: PRIVATE, OWNER
        UNIT_FIELD_POWER_COST_MULTIPLIER = WoWObjectFields.OBJECT_END + 0x0084,
        // Size: 7, Type: FLOAT, Flags: PRIVATE, OWNER
        UNIT_FIELD_MAXHEALTHMODIFIER = WoWObjectFields.OBJECT_END + 0x008B,
        // Size: 1, Type: FLOAT, Flags: PRIVATE, OWNER
        UNIT_FIELD_HOVERHEIGHT = WoWObjectFields.OBJECT_END + 0x008C, // Size: 1, Type: FLOAT, Flags: PUBLIC
        UNIT_FIELD_PADDING = WoWObjectFields.OBJECT_END + 0x008D, // Size: 1, Type: INT, Flags: NONE
        UNIT_END = WoWObjectFields.OBJECT_END + 0x008E,
    };

    public enum WoWPlayerFields
    {
        PLAYER_DUEL_ARBITER = WoWUnitFields.UNIT_END + 0x0000, // Size: 2, Type: LONG, Flags: PUBLIC
        PLAYER_FLAGS = WoWUnitFields.UNIT_END + 0x0002, // Size: 1, Type: INT, Flags: PUBLIC
        PLAYER_GUILDID = WoWUnitFields.UNIT_END + 0x0003, // Size: 1, Type: INT, Flags: PUBLIC
        PLAYER_GUILDRANK = WoWUnitFields.UNIT_END + 0x0004, // Size: 1, Type: INT, Flags: PUBLIC
        PLAYER_BYTES = WoWUnitFields.UNIT_END + 0x0005, // Size: 1, Type: BYTES, Flags: PUBLIC
        PLAYER_BYTES_2 = WoWUnitFields.UNIT_END + 0x0006, // Size: 1, Type: BYTES, Flags: PUBLIC
        PLAYER_BYTES_3 = WoWUnitFields.UNIT_END + 0x0007, // Size: 1, Type: BYTES, Flags: PUBLIC
        PLAYER_DUEL_TEAM = WoWUnitFields.UNIT_END + 0x0008, // Size: 1, Type: INT, Flags: PUBLIC
        PLAYER_GUILD_TIMESTAMP = WoWUnitFields.UNIT_END + 0x0009, // Size: 1, Type: INT, Flags: PUBLIC
        PLAYER_QUEST_LOG_1_1 = WoWUnitFields.UNIT_END + 0x000A, // Size: 1, Type: INT, Flags: PARTY_MEMBER
        PLAYER_QUEST_LOG_1_2 = WoWUnitFields.UNIT_END + 0x000B, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_1_3 = WoWUnitFields.UNIT_END + 0x000C, // Size: 2, Type: TWO_SHORT, Flags: PRIVATE
        PLAYER_QUEST_LOG_1_4 = WoWUnitFields.UNIT_END + 0x000E, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_2_1 = WoWUnitFields.UNIT_END + 0x000F, // Size: 1, Type: INT, Flags: PARTY_MEMBER
        PLAYER_QUEST_LOG_2_2 = WoWUnitFields.UNIT_END + 0x0010, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_2_3 = WoWUnitFields.UNIT_END + 0x0011, // Size: 2, Type: TWO_SHORT, Flags: PRIVATE
        PLAYER_QUEST_LOG_2_5 = WoWUnitFields.UNIT_END + 0x0013, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_3_1 = WoWUnitFields.UNIT_END + 0x0014, // Size: 1, Type: INT, Flags: PARTY_MEMBER
        PLAYER_QUEST_LOG_3_2 = WoWUnitFields.UNIT_END + 0x0015, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_3_3 = WoWUnitFields.UNIT_END + 0x0016, // Size: 2, Type: TWO_SHORT, Flags: PRIVATE
        PLAYER_QUEST_LOG_3_5 = WoWUnitFields.UNIT_END + 0x0018, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_4_1 = WoWUnitFields.UNIT_END + 0x0019, // Size: 1, Type: INT, Flags: PARTY_MEMBER
        PLAYER_QUEST_LOG_4_2 = WoWUnitFields.UNIT_END + 0x001A, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_4_3 = WoWUnitFields.UNIT_END + 0x001B, // Size: 2, Type: TWO_SHORT, Flags: PRIVATE
        PLAYER_QUEST_LOG_4_5 = WoWUnitFields.UNIT_END + 0x001D, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_5_1 = WoWUnitFields.UNIT_END + 0x001E, // Size: 1, Type: INT, Flags: PARTY_MEMBER
        PLAYER_QUEST_LOG_5_2 = WoWUnitFields.UNIT_END + 0x001F, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_5_3 = WoWUnitFields.UNIT_END + 0x0020, // Size: 2, Type: TWO_SHORT, Flags: PRIVATE
        PLAYER_QUEST_LOG_5_5 = WoWUnitFields.UNIT_END + 0x0022, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_6_1 = WoWUnitFields.UNIT_END + 0x0023, // Size: 1, Type: INT, Flags: PARTY_MEMBER
        PLAYER_QUEST_LOG_6_2 = WoWUnitFields.UNIT_END + 0x0024, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_6_3 = WoWUnitFields.UNIT_END + 0x0025, // Size: 2, Type: TWO_SHORT, Flags: PRIVATE
        PLAYER_QUEST_LOG_6_5 = WoWUnitFields.UNIT_END + 0x0027, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_7_1 = WoWUnitFields.UNIT_END + 0x0028, // Size: 1, Type: INT, Flags: PARTY_MEMBER
        PLAYER_QUEST_LOG_7_2 = WoWUnitFields.UNIT_END + 0x0029, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_7_3 = WoWUnitFields.UNIT_END + 0x002A, // Size: 2, Type: TWO_SHORT, Flags: PRIVATE
        PLAYER_QUEST_LOG_7_5 = WoWUnitFields.UNIT_END + 0x002C, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_8_1 = WoWUnitFields.UNIT_END + 0x002D, // Size: 1, Type: INT, Flags: PARTY_MEMBER
        PLAYER_QUEST_LOG_8_2 = WoWUnitFields.UNIT_END + 0x002E, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_8_3 = WoWUnitFields.UNIT_END + 0x002F, // Size: 2, Type: TWO_SHORT, Flags: PRIVATE
        PLAYER_QUEST_LOG_8_5 = WoWUnitFields.UNIT_END + 0x0031, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_9_1 = WoWUnitFields.UNIT_END + 0x0032, // Size: 1, Type: INT, Flags: PARTY_MEMBER
        PLAYER_QUEST_LOG_9_2 = WoWUnitFields.UNIT_END + 0x0033, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_9_3 = WoWUnitFields.UNIT_END + 0x0034, // Size: 2, Type: TWO_SHORT, Flags: PRIVATE
        PLAYER_QUEST_LOG_9_5 = WoWUnitFields.UNIT_END + 0x0036, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_10_1 = WoWUnitFields.UNIT_END + 0x0037, // Size: 1, Type: INT, Flags: PARTY_MEMBER
        PLAYER_QUEST_LOG_10_2 = WoWUnitFields.UNIT_END + 0x0038, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_10_3 = WoWUnitFields.UNIT_END + 0x0039, // Size: 2, Type: TWO_SHORT, Flags: PRIVATE
        PLAYER_QUEST_LOG_10_5 = WoWUnitFields.UNIT_END + 0x003B, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_11_1 = WoWUnitFields.UNIT_END + 0x003C, // Size: 1, Type: INT, Flags: PARTY_MEMBER
        PLAYER_QUEST_LOG_11_2 = WoWUnitFields.UNIT_END + 0x003D, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_11_3 = WoWUnitFields.UNIT_END + 0x003E, // Size: 2, Type: TWO_SHORT, Flags: PRIVATE
        PLAYER_QUEST_LOG_11_5 = WoWUnitFields.UNIT_END + 0x0040, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_12_1 = WoWUnitFields.UNIT_END + 0x0041, // Size: 1, Type: INT, Flags: PARTY_MEMBER
        PLAYER_QUEST_LOG_12_2 = WoWUnitFields.UNIT_END + 0x0042, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_12_3 = WoWUnitFields.UNIT_END + 0x0043, // Size: 2, Type: TWO_SHORT, Flags: PRIVATE
        PLAYER_QUEST_LOG_12_5 = WoWUnitFields.UNIT_END + 0x0045, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_13_1 = WoWUnitFields.UNIT_END + 0x0046, // Size: 1, Type: INT, Flags: PARTY_MEMBER
        PLAYER_QUEST_LOG_13_2 = WoWUnitFields.UNIT_END + 0x0047, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_13_3 = WoWUnitFields.UNIT_END + 0x0048, // Size: 2, Type: TWO_SHORT, Flags: PRIVATE
        PLAYER_QUEST_LOG_13_5 = WoWUnitFields.UNIT_END + 0x004A, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_14_1 = WoWUnitFields.UNIT_END + 0x004B, // Size: 1, Type: INT, Flags: PARTY_MEMBER
        PLAYER_QUEST_LOG_14_2 = WoWUnitFields.UNIT_END + 0x004C, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_14_3 = WoWUnitFields.UNIT_END + 0x004D, // Size: 2, Type: TWO_SHORT, Flags: PRIVATE
        PLAYER_QUEST_LOG_14_5 = WoWUnitFields.UNIT_END + 0x004F, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_15_1 = WoWUnitFields.UNIT_END + 0x0050, // Size: 1, Type: INT, Flags: PARTY_MEMBER
        PLAYER_QUEST_LOG_15_2 = WoWUnitFields.UNIT_END + 0x0051, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_15_3 = WoWUnitFields.UNIT_END + 0x0052, // Size: 2, Type: TWO_SHORT, Flags: PRIVATE
        PLAYER_QUEST_LOG_15_5 = WoWUnitFields.UNIT_END + 0x0054, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_16_1 = WoWUnitFields.UNIT_END + 0x0055, // Size: 1, Type: INT, Flags: PARTY_MEMBER
        PLAYER_QUEST_LOG_16_2 = WoWUnitFields.UNIT_END + 0x0056, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_16_3 = WoWUnitFields.UNIT_END + 0x0057, // Size: 2, Type: TWO_SHORT, Flags: PRIVATE
        PLAYER_QUEST_LOG_16_5 = WoWUnitFields.UNIT_END + 0x0059, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_17_1 = WoWUnitFields.UNIT_END + 0x005A, // Size: 1, Type: INT, Flags: PARTY_MEMBER
        PLAYER_QUEST_LOG_17_2 = WoWUnitFields.UNIT_END + 0x005B, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_17_3 = WoWUnitFields.UNIT_END + 0x005C, // Size: 2, Type: TWO_SHORT, Flags: PRIVATE
        PLAYER_QUEST_LOG_17_5 = WoWUnitFields.UNIT_END + 0x005E, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_18_1 = WoWUnitFields.UNIT_END + 0x005F, // Size: 1, Type: INT, Flags: PARTY_MEMBER
        PLAYER_QUEST_LOG_18_2 = WoWUnitFields.UNIT_END + 0x0060, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_18_3 = WoWUnitFields.UNIT_END + 0x0061, // Size: 2, Type: TWO_SHORT, Flags: PRIVATE
        PLAYER_QUEST_LOG_18_5 = WoWUnitFields.UNIT_END + 0x0063, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_19_1 = WoWUnitFields.UNIT_END + 0x0064, // Size: 1, Type: INT, Flags: PARTY_MEMBER
        PLAYER_QUEST_LOG_19_2 = WoWUnitFields.UNIT_END + 0x0065, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_19_3 = WoWUnitFields.UNIT_END + 0x0066, // Size: 2, Type: TWO_SHORT, Flags: PRIVATE
        PLAYER_QUEST_LOG_19_5 = WoWUnitFields.UNIT_END + 0x0068, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_20_1 = WoWUnitFields.UNIT_END + 0x0069, // Size: 1, Type: INT, Flags: PARTY_MEMBER
        PLAYER_QUEST_LOG_20_2 = WoWUnitFields.UNIT_END + 0x006A, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_20_3 = WoWUnitFields.UNIT_END + 0x006B, // Size: 2, Type: TWO_SHORT, Flags: PRIVATE
        PLAYER_QUEST_LOG_20_5 = WoWUnitFields.UNIT_END + 0x006D, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_21_1 = WoWUnitFields.UNIT_END + 0x006E, // Size: 1, Type: INT, Flags: PARTY_MEMBER
        PLAYER_QUEST_LOG_21_2 = WoWUnitFields.UNIT_END + 0x006F, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_21_3 = WoWUnitFields.UNIT_END + 0x0070, // Size: 2, Type: TWO_SHORT, Flags: PRIVATE
        PLAYER_QUEST_LOG_21_5 = WoWUnitFields.UNIT_END + 0x0072, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_22_1 = WoWUnitFields.UNIT_END + 0x0073, // Size: 1, Type: INT, Flags: PARTY_MEMBER
        PLAYER_QUEST_LOG_22_2 = WoWUnitFields.UNIT_END + 0x0074, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_22_3 = WoWUnitFields.UNIT_END + 0x0075, // Size: 2, Type: TWO_SHORT, Flags: PRIVATE
        PLAYER_QUEST_LOG_22_5 = WoWUnitFields.UNIT_END + 0x0077, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_23_1 = WoWUnitFields.UNIT_END + 0x0078, // Size: 1, Type: INT, Flags: PARTY_MEMBER
        PLAYER_QUEST_LOG_23_2 = WoWUnitFields.UNIT_END + 0x0079, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_23_3 = WoWUnitFields.UNIT_END + 0x007A, // Size: 2, Type: TWO_SHORT, Flags: PRIVATE
        PLAYER_QUEST_LOG_23_5 = WoWUnitFields.UNIT_END + 0x007C, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_24_1 = WoWUnitFields.UNIT_END + 0x007D, // Size: 1, Type: INT, Flags: PARTY_MEMBER
        PLAYER_QUEST_LOG_24_2 = WoWUnitFields.UNIT_END + 0x007E, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_24_3 = WoWUnitFields.UNIT_END + 0x007F, // Size: 2, Type: TWO_SHORT, Flags: PRIVATE
        PLAYER_QUEST_LOG_24_5 = WoWUnitFields.UNIT_END + 0x0081, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_25_1 = WoWUnitFields.UNIT_END + 0x0082, // Size: 1, Type: INT, Flags: PARTY_MEMBER
        PLAYER_QUEST_LOG_25_2 = WoWUnitFields.UNIT_END + 0x0083, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_QUEST_LOG_25_3 = WoWUnitFields.UNIT_END + 0x0084, // Size: 2, Type: TWO_SHORT, Flags: PRIVATE
        PLAYER_QUEST_LOG_25_5 = WoWUnitFields.UNIT_END + 0x0086, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_VISIBLE_ITEM_1_ENTRYID = WoWUnitFields.UNIT_END + 0x0087, // Size: 1, Type: INT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_1_ENCHANTMENT = WoWUnitFields.UNIT_END + 0x0088, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_2_ENTRYID = WoWUnitFields.UNIT_END + 0x0089, // Size: 1, Type: INT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_2_ENCHANTMENT = WoWUnitFields.UNIT_END + 0x008A, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_3_ENTRYID = WoWUnitFields.UNIT_END + 0x008B, // Size: 1, Type: INT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_3_ENCHANTMENT = WoWUnitFields.UNIT_END + 0x008C, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_4_ENTRYID = WoWUnitFields.UNIT_END + 0x008D, // Size: 1, Type: INT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_4_ENCHANTMENT = WoWUnitFields.UNIT_END + 0x008E, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_5_ENTRYID = WoWUnitFields.UNIT_END + 0x008F, // Size: 1, Type: INT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_5_ENCHANTMENT = WoWUnitFields.UNIT_END + 0x0090, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_6_ENTRYID = WoWUnitFields.UNIT_END + 0x0091, // Size: 1, Type: INT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_6_ENCHANTMENT = WoWUnitFields.UNIT_END + 0x0092, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_7_ENTRYID = WoWUnitFields.UNIT_END + 0x0093, // Size: 1, Type: INT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_7_ENCHANTMENT = WoWUnitFields.UNIT_END + 0x0094, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_8_ENTRYID = WoWUnitFields.UNIT_END + 0x0095, // Size: 1, Type: INT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_8_ENCHANTMENT = WoWUnitFields.UNIT_END + 0x0096, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_9_ENTRYID = WoWUnitFields.UNIT_END + 0x0097, // Size: 1, Type: INT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_9_ENCHANTMENT = WoWUnitFields.UNIT_END + 0x0098, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_10_ENTRYID = WoWUnitFields.UNIT_END + 0x0099, // Size: 1, Type: INT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_10_ENCHANTMENT = WoWUnitFields.UNIT_END + 0x009A, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_11_ENTRYID = WoWUnitFields.UNIT_END + 0x009B, // Size: 1, Type: INT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_11_ENCHANTMENT = WoWUnitFields.UNIT_END + 0x009C, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_12_ENTRYID = WoWUnitFields.UNIT_END + 0x009D, // Size: 1, Type: INT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_12_ENCHANTMENT = WoWUnitFields.UNIT_END + 0x009E, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_13_ENTRYID = WoWUnitFields.UNIT_END + 0x009F, // Size: 1, Type: INT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_13_ENCHANTMENT = WoWUnitFields.UNIT_END + 0x00A0, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_14_ENTRYID = WoWUnitFields.UNIT_END + 0x00A1, // Size: 1, Type: INT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_14_ENCHANTMENT = WoWUnitFields.UNIT_END + 0x00A2, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_15_ENTRYID = WoWUnitFields.UNIT_END + 0x00A3, // Size: 1, Type: INT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_15_ENCHANTMENT = WoWUnitFields.UNIT_END + 0x00A4, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_16_ENTRYID = WoWUnitFields.UNIT_END + 0x00A5, // Size: 1, Type: INT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_16_ENCHANTMENT = WoWUnitFields.UNIT_END + 0x00A6, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_17_ENTRYID = WoWUnitFields.UNIT_END + 0x00A7, // Size: 1, Type: INT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_17_ENCHANTMENT = WoWUnitFields.UNIT_END + 0x00A8, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_18_ENTRYID = WoWUnitFields.UNIT_END + 0x00A9, // Size: 1, Type: INT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_18_ENCHANTMENT = WoWUnitFields.UNIT_END + 0x00AA, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_19_ENTRYID = WoWUnitFields.UNIT_END + 0x00AB, // Size: 1, Type: INT, Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_19_ENCHANTMENT = WoWUnitFields.UNIT_END + 0x00AC, // Size: 1, Type: TWO_SHORT, Flags: PUBLIC
        PLAYER_CHOSEN_TITLE = WoWUnitFields.UNIT_END + 0x00AD, // Size: 1, Type: INT, Flags: PUBLIC
        PLAYER_FAKE_INEBRIATION = WoWUnitFields.UNIT_END + 0x00AE, // Size: 1, Type: INT, Flags: PUBLIC
        PLAYER_FIELD_PAD_0 = WoWUnitFields.UNIT_END + 0x00AF, // Size: 1, Type: INT, Flags: NONE
        PLAYER_FIELD_INV_SLOT_HEAD = WoWUnitFields.UNIT_END + 0x00B0, // Size: 46, Type: LONG, Flags: PRIVATE
        PLAYER_FIELD_PACK_SLOT_1 = WoWUnitFields.UNIT_END + 0x00DE, // Size: 32, Type: LONG, Flags: PRIVATE
        PLAYER_FIELD_BANK_SLOT_1 = WoWUnitFields.UNIT_END + 0x00FE, // Size: 56, Type: LONG, Flags: PRIVATE
        PLAYER_FIELD_BANKBAG_SLOT_1 = WoWUnitFields.UNIT_END + 0x0136, // Size: 14, Type: LONG, Flags: PRIVATE
        PLAYER_FIELD_VENDORBUYBACK_SLOT_1 = WoWUnitFields.UNIT_END + 0x0144, // Size: 24, Type: LONG, Flags: PRIVATE
        PLAYER_FIELD_KEYRING_SLOT_1 = WoWUnitFields.UNIT_END + 0x015C, // Size: 64, Type: LONG, Flags: PRIVATE
        PLAYER_FIELD_CURRENCYTOKEN_SLOT_1 = WoWUnitFields.UNIT_END + 0x019C, // Size: 64, Type: LONG, Flags: PRIVATE
        PLAYER_FARSIGHT = WoWUnitFields.UNIT_END + 0x01DC, // Size: 2, Type: LONG, Flags: PRIVATE
        PLAYER__FIELD_KNOWN_TITLES = WoWUnitFields.UNIT_END + 0x01DE, // Size: 2, Type: LONG, Flags: PRIVATE
        PLAYER__FIELD_KNOWN_TITLES1 = WoWUnitFields.UNIT_END + 0x01E0, // Size: 2, Type: LONG, Flags: PRIVATE
        PLAYER__FIELD_KNOWN_TITLES2 = WoWUnitFields.UNIT_END + 0x01E2, // Size: 2, Type: LONG, Flags: PRIVATE
        PLAYER_FIELD_KNOWN_CURRENCIES = WoWUnitFields.UNIT_END + 0x01E4, // Size: 2, Type: LONG, Flags: PRIVATE
        PLAYER_XP = WoWUnitFields.UNIT_END + 0x01E6, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_NEXT_LEVEL_XP = WoWUnitFields.UNIT_END + 0x01E7, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_SKILL_INFO_1_1 = WoWUnitFields.UNIT_END + 0x01E8, // Size: 384, Type: TWO_SHORT, Flags: PRIVATE
        PLAYER_CHARACTER_POINTS1 = WoWUnitFields.UNIT_END + 0x0368, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_CHARACTER_POINTS2 = WoWUnitFields.UNIT_END + 0x0369, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_TRACK_CREATURES = WoWUnitFields.UNIT_END + 0x036A, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_TRACK_RESOURCES = WoWUnitFields.UNIT_END + 0x036B, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_BLOCK_PERCENTAGE = WoWUnitFields.UNIT_END + 0x036C, // Size: 1, Type: FLOAT, Flags: PRIVATE
        PLAYER_DODGE_PERCENTAGE = WoWUnitFields.UNIT_END + 0x036D, // Size: 1, Type: FLOAT, Flags: PRIVATE
        PLAYER_PARRY_PERCENTAGE = WoWUnitFields.UNIT_END + 0x036E, // Size: 1, Type: FLOAT, Flags: PRIVATE
        PLAYER_EXPERTISE = WoWUnitFields.UNIT_END + 0x036F, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_OFFHAND_EXPERTISE = WoWUnitFields.UNIT_END + 0x0370, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_CRIT_PERCENTAGE = WoWUnitFields.UNIT_END + 0x0371, // Size: 1, Type: FLOAT, Flags: PRIVATE
        PLAYER_RANGED_CRIT_PERCENTAGE = WoWUnitFields.UNIT_END + 0x0372, // Size: 1, Type: FLOAT, Flags: PRIVATE
        PLAYER_OFFHAND_CRIT_PERCENTAGE = WoWUnitFields.UNIT_END + 0x0373, // Size: 1, Type: FLOAT, Flags: PRIVATE
        PLAYER_SPELL_CRIT_PERCENTAGE1 = WoWUnitFields.UNIT_END + 0x0374, // Size: 7, Type: FLOAT, Flags: PRIVATE
        PLAYER_SHIELD_BLOCK = WoWUnitFields.UNIT_END + 0x037B, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_SHIELD_BLOCK_CRIT_PERCENTAGE = WoWUnitFields.UNIT_END + 0x037C, // Size: 1, Type: FLOAT, Flags: PRIVATE
        PLAYER_EXPLORED_ZONES_1 = WoWUnitFields.UNIT_END + 0x037D, // Size: 128, Type: BYTES, Flags: PRIVATE
        PLAYER_REST_STATE_EXPERIENCE = WoWUnitFields.UNIT_END + 0x03FD, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_FIELD_COINAGE = WoWUnitFields.UNIT_END + 0x03FE, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_FIELD_MOD_DAMAGE_DONE_POS = WoWUnitFields.UNIT_END + 0x03FF, // Size: 7, Type: INT, Flags: PRIVATE
        PLAYER_FIELD_MOD_DAMAGE_DONE_NEG = WoWUnitFields.UNIT_END + 0x0406, // Size: 7, Type: INT, Flags: PRIVATE
        PLAYER_FIELD_MOD_DAMAGE_DONE_PCT = WoWUnitFields.UNIT_END + 0x040D, // Size: 7, Type: INT, Flags: PRIVATE
        PLAYER_FIELD_MOD_HEALING_DONE_POS = WoWUnitFields.UNIT_END + 0x0414, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_FIELD_MOD_HEALING_PCT = WoWUnitFields.UNIT_END + 0x0415, // Size: 1, Type: FLOAT, Flags: PRIVATE
        PLAYER_FIELD_MOD_HEALING_DONE_PCT = WoWUnitFields.UNIT_END + 0x0416, // Size: 1, Type: FLOAT, Flags: PRIVATE
        PLAYER_FIELD_MOD_TARGET_RESISTANCE = WoWUnitFields.UNIT_END + 0x0417, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_FIELD_MOD_TARGET_PHYSICAL_RESISTANCE = WoWUnitFields.UNIT_END + 0x0418,
        // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_FIELD_BYTES = WoWUnitFields.UNIT_END + 0x0419, // Size: 1, Type: BYTES, Flags: PRIVATE
        PLAYER_AMMO_ID = WoWUnitFields.UNIT_END + 0x041A, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_SELF_RES_SPELL = WoWUnitFields.UNIT_END + 0x041B, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_FIELD_PVP_MEDALS = WoWUnitFields.UNIT_END + 0x041C, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_FIELD_BUYBACK_PRICE_1 = WoWUnitFields.UNIT_END + 0x041D, // Size: 12, Type: INT, Flags: PRIVATE
        PLAYER_FIELD_BUYBACK_TIMESTAMP_1 = WoWUnitFields.UNIT_END + 0x0429, // Size: 12, Type: INT, Flags: PRIVATE
        PLAYER_FIELD_KILLS = WoWUnitFields.UNIT_END + 0x0435, // Size: 1, Type: TWO_SHORT, Flags: PRIVATE
        PLAYER_FIELD_TODAY_CONTRIBUTION = WoWUnitFields.UNIT_END + 0x0436, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_FIELD_YESTERDAY_CONTRIBUTION = WoWUnitFields.UNIT_END + 0x0437, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_FIELD_LIFETIME_HONORBALE_KILLS = WoWUnitFields.UNIT_END + 0x0438, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_FIELD_BYTES2 = WoWUnitFields.UNIT_END + 0x0439, // Size: 1, Type: 6, Flags: PRIVATE
        PLAYER_FIELD_WATCHED_FACTION_INDEX = WoWUnitFields.UNIT_END + 0x043A, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_FIELD_COMBAT_RATING_1 = WoWUnitFields.UNIT_END + 0x043B, // Size: 25, Type: INT, Flags: PRIVATE
        PLAYER_FIELD_ARENA_TEAM_INFO_1_1 = WoWUnitFields.UNIT_END + 0x0454, // Size: 21, Type: INT, Flags: PRIVATE
        PLAYER_FIELD_HONOR_CURRENCY = WoWUnitFields.UNIT_END + 0x0469, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_FIELD_ARENA_CURRENCY = WoWUnitFields.UNIT_END + 0x046A, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_FIELD_MAX_LEVEL = WoWUnitFields.UNIT_END + 0x046B, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_FIELD_DAILY_QUESTS_1 = WoWUnitFields.UNIT_END + 0x046C, // Size: 25, Type: INT, Flags: PRIVATE
        PLAYER_RUNE_REGEN_1 = WoWUnitFields.UNIT_END + 0x0485, // Size: 4, Type: FLOAT, Flags: PRIVATE
        PLAYER_NO_REAGENT_COST_1 = WoWUnitFields.UNIT_END + 0x0489, // Size: 3, Type: INT, Flags: PRIVATE
        PLAYER_FIELD_GLYPH_SLOTS_1 = WoWUnitFields.UNIT_END + 0x048C, // Size: 6, Type: INT, Flags: PRIVATE
        PLAYER_FIELD_GLYPHS_1 = WoWUnitFields.UNIT_END + 0x0492, // Size: 6, Type: INT, Flags: PRIVATE
        PLAYER_GLYPHS_ENABLED = WoWUnitFields.UNIT_END + 0x0498, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_PET_SPELL_POWER = WoWUnitFields.UNIT_END + 0x0499, // Size: 1, Type: INT, Flags: PRIVATE
        PLAYER_END = WoWUnitFields.UNIT_END + 0x049A,
    };

    public enum WoWGameObjectFields
    {
        OBJECT_FIELD_CREATED_BY = WoWObjectFields.OBJECT_END + 0x0000, // Size: 2, Type: LONG, Flags: PUBLIC
        GAMEOBJECT_DISPLAYID = WoWObjectFields.OBJECT_END + 0x0002, // Size: 1, Type: INT, Flags: PUBLIC
        GAMEOBJECT_FLAGS = WoWObjectFields.OBJECT_END + 0x0003, // Size: 1, Type: INT, Flags: PUBLIC
        GAMEOBJECT_PARENTROTATION = WoWObjectFields.OBJECT_END + 0x0004, // Size: 4, Type: FLOAT, Flags: PUBLIC
        GAMEOBJECT_DYNAMIC = WoWObjectFields.OBJECT_END + 0x0008, // Size: 1, Type: TWO_SHORT, Flags: DYNAMIC
        GAMEOBJECT_FACTION = WoWObjectFields.OBJECT_END + 0x0009, // Size: 1, Type: INT, Flags: PUBLIC
        GAMEOBJECT_LEVEL = WoWObjectFields.OBJECT_END + 0x000A, // Size: 1, Type: INT, Flags: PUBLIC
        GAMEOBJECT_BYTES_1 = WoWObjectFields.OBJECT_END + 0x000B, // Size: 1, Type: BYTES, Flags: PUBLIC
        GAMEOBJECT_END = WoWObjectFields.OBJECT_END + 0x000C,
    };

    public enum WoWDynamicObjectFields
    {
        DYNAMICOBJECT_CASTER = WoWObjectFields.OBJECT_END + 0x0000, // Size: 2, Type: LONG, Flags: PUBLIC
        DYNAMICOBJECT_BYTES = WoWObjectFields.OBJECT_END + 0x0002, // Size: 1, Type: BYTES, Flags: PUBLIC
        DYNAMICOBJECT_SPELLID = WoWObjectFields.OBJECT_END + 0x0003, // Size: 1, Type: INT, Flags: PUBLIC
        DYNAMICOBJECT_RADIUS = WoWObjectFields.OBJECT_END + 0x0004, // Size: 1, Type: FLOAT, Flags: PUBLIC
        DYNAMICOBJECT_CASTTIME = WoWObjectFields.OBJECT_END + 0x0005, // Size: 1, Type: INT, Flags: PUBLIC
        DYNAMICOBJECT_END = WoWObjectFields.OBJECT_END + 0x0006,
    };

    public enum WoWCorpseFields
    {
        CORPSE_FIELD_OWNER = WoWObjectFields.OBJECT_END + 0x0000, // Size: 2, Type: LONG, Flags: PUBLIC
        CORPSE_FIELD_PARTY = WoWObjectFields.OBJECT_END + 0x0002, // Size: 2, Type: LONG, Flags: PUBLIC
        CORPSE_FIELD_DISPLAY_ID = WoWObjectFields.OBJECT_END + 0x0004, // Size: 1, Type: INT, Flags: PUBLIC
        CORPSE_FIELD_ITEM = WoWObjectFields.OBJECT_END + 0x0005, // Size: 19, Type: INT, Flags: PUBLIC
        CORPSE_FIELD_BYTES_1 = WoWObjectFields.OBJECT_END + 0x0018, // Size: 1, Type: BYTES, Flags: PUBLIC
        CORPSE_FIELD_BYTES_2 = WoWObjectFields.OBJECT_END + 0x0019, // Size: 1, Type: BYTES, Flags: PUBLIC
        CORPSE_FIELD_GUILD = WoWObjectFields.OBJECT_END + 0x001A, // Size: 1, Type: INT, Flags: PUBLIC
        CORPSE_FIELD_FLAGS = WoWObjectFields.OBJECT_END + 0x001B, // Size: 1, Type: INT, Flags: PUBLIC
        CORPSE_FIELD_DYNAMIC_FLAGS = WoWObjectFields.OBJECT_END + 0x001C, // Size: 1, Type: INT, Flags: DYNAMIC
        CORPSE_FIELD_PAD = WoWObjectFields.OBJECT_END + 0x001D, // Size: 1, Type: INT, Flags: NONE
        CORPSE_END = WoWObjectFields.OBJECT_END + 0x001E,
    };
}