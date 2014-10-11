using System;
using System.Collections.Generic;
using System.Linq;
using IceFlake.Client.Patchables;

namespace IceFlake.Client.API
{
    public class Loot : Frame
    {
        public Loot()
            : base("LootFrame")
        {
        }

        public bool IsFishingLoot
        {
            get { return WoWScript.Execute<bool>("IsFishingLoot()"); }
        }

        public int NumLootItems
        {
            get { return WoWScript.Execute<int>("GetNumLootItems()"); }
        }

        public IEnumerable<LootSlotInfo> LootSlots
        {
            get { return Enumerable.Range(1, NumLootItems).Select(i => new LootSlotInfo(i)); }
        }

        public void Close()
        {
            WoWScript.ExecuteNoResults("CloseLoot()");
        }
    }

    public class LootSlotInfo
    {
        public LootSlotInfo(int slot)
        {
            Slot = slot;
        }

        public int Slot { get; private set; }

        public string Texture
        {
            get { return WoWScript.Execute<string>("GetLootSlotInfo(" + Slot + ")", 0); }
        }

        public string Name
        {
            get { return WoWScript.Execute<string>("GetLootSlotInfo(" + Slot + ")", 1).Replace('\n', ':'); }
        }

        public int Quantity
        {
            get { return WoWScript.Execute<int>("GetLootSlotInfo(" + Slot + ")", 2); }
        }

        public ItemQuality Quality
        {
            get { return (ItemQuality) WoWScript.Execute<int>("GetLootSlotInfo(" + Slot + ")", 3); }
        }

        public bool Locked
        {
            get { return WoWScript.Execute<bool>("GetLootSlotInfo(" + Slot + ")", 4); }
        }

        public string ItemLink
        {
            get { return WoWScript.Execute<string>("GetLootSlotLink(" + Slot + ")"); }
        }

        public bool IsCoin
        {
            get { return WoWScript.Execute<bool>("LootSlotIsCoin(" + Slot + ")"); }
        }

        public bool IsItem
        {
            get { return WoWScript.Execute<bool>("LootSlotIsItem(" + Slot + ")"); }
        }

        public void Loot()
        {
            WoWScript.ExecuteNoResults("LootSlot(" + Slot + ")");
        }

        public void ConfirmLoot()
        {
            WoWScript.ExecuteNoResults("ConfirmLootSlot(" + Slot + ")");
        }
    }

    public class LootRollItemInfo
    {
        public LootRollItemInfo(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }

        public string Texture
        {
            get { return WoWScript.Execute<string>("GetLootRollItemInfo(" + Id + ")", 0); }
        }

        public string Name
        {
            get { return WoWScript.Execute<string>("GetLootRollItemInfo(" + Id + ")", 1); }
        }

        public int Quantity
        {
            get { return WoWScript.Execute<int>("GetLootRollItemInfo(" + Id + ")", 2); }
        }

        public ItemQuality Quality
        {
            get { return (ItemQuality) WoWScript.Execute<int>("GetLootRollItemInfo(" + Id + ")", 3); }
        }

        public bool BindsOnPickup
        {
            get { return WoWScript.Execute<bool>("GetLootRollItemInfo(" + Id + ")", 4); }
        }

        public string ItemLink
        {
            get { return WoWScript.Execute<string>("GetLootRollItemLink(" + Id + ")"); }
        }

        // Returned in milliseconds
        public int TimeLeft
        {
            get { return WoWScript.Execute<int>("GetLootRollTimeLeft(" + Id + ")"); }
        }

        public DateTime Expires
        {
            get { return DateTime.Now.AddMilliseconds(TimeLeft); }
        }

        public void Roll(LootRoll roll)
        {
            WoWScript.ExecuteNoResults("RollOnLoot(" + Id + ", " + roll.ToString().ToLower() + ")");
        }

        public void ConfirmRoll(LootRoll roll)
        {
            WoWScript.ExecuteNoResults("ConfirmLootRoll(" + Id + ", " + roll.ToString().ToLower() + ")");
        }
    }

    public enum LootType
    {
        Items = 1,
        Money,
        Currency, // Archeology?
    }

    public enum LootRoll
    {
        Pass = 0,
        Need,
        Greed,
        Disenchant,
    }
}