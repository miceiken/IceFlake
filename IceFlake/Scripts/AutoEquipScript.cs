using System;
using System.Collections.Generic;
using System.Linq;
using IceFlake.Client;
using IceFlake.Client.Scripts;
using IceFlake.Client.Patchables;
using IceFlake.Client.Objects;

namespace IceFlake.Scripts
{
    public class AutoEquipScript : Script
    {
        public AutoEquipScript()
            : base("AutoEquip", "Leveling")
        { }

        #region Fields

        private DateTime _lastPulse = DateTime.MinValue;
        private List<ulong> _blacklistedItems = new List<ulong>();

        private Dictionary<ItemStatType, double> _weightSet = new Dictionary<ItemStatType, double>()
        {
            // Restoration Druid
            //{ ItemStatType.SpellPower, 2.00 },
            //{ ItemStatType.HasteRating, 1.57 },
            //{ ItemStatType.Intellect, 1.51 },
            //{ ItemStatType.Spirit, 1.32 },
            //{ ItemStatType.CriticalStrikeRating, 1.11 },

            // Elemental Shaman
            //{ ItemStatType.HitRating, 2.00 },
            //{ ItemStatType.SpellPower, 1.60 },
            //{ ItemStatType.HasteRating, 1.56 },
            //{ ItemStatType.CriticalStrikeRating, 1.40 },
            //{ ItemStatType.Intellect, 1.11 },

            // Retribution Paladin
            { ItemStatType.DPS, 5.70 },
            { ItemStatType.HitRating, 2.00 },
            { ItemStatType.Strength, 1.80 },
            { ItemStatType.ExpertiseRating, 1.66 },
            { ItemStatType.CriticalStrikeRating, 1.40 },
            { ItemStatType.Agility, 1.32 },
            { ItemStatType.HasteRating, 1.30 },
            { ItemStatType.SpellPower, 1.09 },
        };

        private readonly List<string> ItemEvents = new List<string>()
        {
            "ACTIVE_TALENT_GROUP_CHANGED",
            "CHARACTER_POINTS_CHANGED",
            "PLAYER_ENTERING_WORLD",
            "START_LOOT_ROLL",
            "CONFIRM_DISENCHANT_ROLL",
            "CONFIRM_LOOT_ROLL",
            "ITEM_PUSH",
            "USE_BIND_CONFIRM",
            "LOOT_BIND_CONFIRM",
            "EQUIP_BIND_CONFIRM",
            "AUTOEQUIP_BIND_CONFIRM",
        };

        #endregion

        #region Start & Stop

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
            {
                Stop();
                return;
            }

            foreach (var ev in ItemEvents)
                Manager.Events.Register(ev, HandleEvent);
        }

        public override void OnTerminate()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            foreach (var ev in ItemEvents)
                Manager.Events.Remove(ev, HandleEvent);
        }

        #endregion

        #region OnTick

        public override void OnTick()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            if (_lastPulse + TimeSpan.FromMinutes(5) >= DateTime.Now)
                return;

            var items = Manager.LocalPlayer.InventoryItems;
            foreach (var item in items.Where(x => !_blacklistedItems.Contains(x.Guid)))
            {
                if (item.ItemInfo.Class != ItemClass.Armor && item.ItemInfo.Class != ItemClass.Weapon)
                {
                    //Print("{0} not equippable, blacklisting.", item.Name);
                    _blacklistedItems.Add(item.Guid);
                    continue;
                }

                GameError error;
                if (!Manager.LocalPlayer.CanUseItem(item, out error))
                {
                    Print("Cannot equip '{0}': {1}", item.Name, error);
                    if (error != GameError.ERR_CANT_EQUIP_LEVEL_I)
                        _blacklistedItems.Add(item.Guid);
                    continue;
                }

                var score = CalculateItemScore(item.ItemSparseInfo, item.ItemInfo);
                var wasReplaced = false;
                var goesIn = WoWItem.GetInventorySlotsByEquipSlot(item.ItemSparseInfo.InventoryType);
                foreach (var slot in goesIn)
                {
                    var currentItem = Manager.LocalPlayer.GetEquippedItem(slot);
                    if (!currentItem.IsValid)
                    {
                        if (slot == EquipSlot.OffHand)
                        {
                            var mh = Manager.LocalPlayer.GetEquippedItem(EquipSlot.MainHand);
                            if (mh.IsValid && mh.ItemSparseInfo.InventoryType == InventoryType.TwoHandedWeapon)
                                break;
                        }

                        Print("Equipping '{0}' (score: {1}) in empty slot", item.Name, score);
                        EquipItem(item, slot);
                        wasReplaced = true;
                        break;
                    }

                    var currScore = CalculateItemScore(currentItem.ItemSparseInfo, currentItem.ItemInfo);
                    // If suggested item is 2H and we have MH+OH compare both
                    if (item.ItemInfo.Class == ItemClass.Weapon)
                    {
                        if (item.ItemSparseInfo.InventoryType == InventoryType.TwoHandedWeapon
                            && currentItem.ItemSparseInfo.InventoryType == InventoryType.WeaponMainHand)
                        {
                            var weaponScore = currScore;
                            var offHand = Manager.LocalPlayer.GetEquippedItem(EquipSlot.OffHand);
                            if (offHand.IsValid)
                                weaponScore += CalculateItemScore(offHand.ItemSparseInfo, offHand.ItemInfo);

                            if (score > weaponScore)
                            {
                                Print("Equipping 2H '{0}' (score: {1}) over MH+OH (score: {2})", item.Name, score, weaponScore);
                                EquipItem(item, slot);
                                wasReplaced = true;
                            }
                            break;
                        }
                    }
                    

                    if (score > currScore)
                    {
                        Print("Equipping '{0}' (score: {1}) over '{2}' (score: {3})", item.Name, score, currentItem.Name, currScore);
                        EquipItem(item, slot);
                        wasReplaced = true;
                    }
                }

                if (!wasReplaced)
                    _blacklistedItems.Add(item.Guid);
            }
            _lastPulse = DateTime.Now;
        }

        #endregion

        #region EvaluateRollItem

        private void EvaluateRollItem(int rollId)
        {
            var itemString = WoWScript.Execute<string>("string.match(GetLootRollItemLink(" + rollId + "), 'item[%-?%d:]+')", 0).Trim();
            //Print("{0}: {1}", rollId, itemString);
            if (string.IsNullOrEmpty(itemString))
            {
                RollItem(rollId, RollType.Greed);
                return;
            }

            var splitItemString = itemString.Split(':');
            var itemId = Convert.ToUInt32(splitItemString.ElementAtOrDefault(1));
            if (itemId == 0)
            {
                RollItem(rollId, RollType.Greed);
                return;
            }
            //Print("{0}: id {1}", rollId, itemId);

            var itemRec = WoWItem.GetItemRecordFromId(itemId);
            var itemSparseRec = WoWItem.GetItemSparseRecordFromId(itemId);
            var itemName = itemSparseRec.Name;
            //Print("{0}: {1}", rollId, itemName);
            var itemSuffix = Convert.ToInt32(splitItemString.ElementAtOrDefault(7));
            if (itemSuffix > 0)
            {
                var suffix = Manager.DBC[ClientDB.ItemRandomProperties].GetRow(itemSuffix).GetField<string>(7);
                if (!string.IsNullOrEmpty(suffix))
                    itemName += " " + suffix;
            }

            GameError g_err;
            var equippable = Manager.LocalPlayer.CanUseItem(WoWItem.GetItemSparseRecordPointerFromId(itemId), out g_err);
            if (!equippable && g_err != GameError.ERR_CANT_EQUIP_LEVEL_I)
            {
                Print("Greeding '{0}' as it's not equippable ({1})", itemSparseRec.Name, g_err);
                RollItem(rollId, RollType.Greed);
                return;
            }

            var itemScore = CalculateItemScore(itemSparseRec, itemRec);
            var goesIn = WoWItem.GetInventorySlotsByEquipSlot(itemSparseRec.InventoryType);
            foreach (var slot in goesIn)
            {
                var currentItem = Manager.LocalPlayer.GetEquippedItem(slot);
                if (!currentItem.IsValid)
                {
                    if (slot == EquipSlot.OffHand)
                    { // No support for OH rolls yet
                        var mh = Manager.LocalPlayer.GetEquippedItem(EquipSlot.MainHand);
                        if (mh.IsValid && mh.ItemSparseInfo.InventoryType == InventoryType.TwoHandedWeapon)
                            break;
                    }

                    Print("Rolling Need for '{0}' (score: {1}) as we have an empty slot", itemSparseRec.Name, itemScore);
                    RollItem(rollId, RollType.Need);
                    return;
                }

                var currScore = CalculateItemScore(currentItem.ItemSparseInfo, currentItem.ItemInfo);

                // If suggested item is 2H and we have MH+OH compare both
                if (itemRec.Class == ItemClass.Weapon)
                {
                    if (itemSparseRec.InventoryType == InventoryType.TwoHandedWeapon
                        && currentItem.ItemSparseInfo.InventoryType == InventoryType.WeaponMainHand)
                    {
                        var weaponScore = currScore;
                        var offHand = Manager.LocalPlayer.GetEquippedItem(EquipSlot.OffHand);
                        if (offHand.IsValid)
                            weaponScore += CalculateItemScore(offHand.ItemSparseInfo, offHand.ItemInfo);

                        if (itemScore > weaponScore)
                        {
                            Print("Rolling need for 2H '{0}' (score: {1}) because it's better than MH+OH (score: {2})", itemSparseRec.Name, itemScore, weaponScore);
                            RollItem(rollId, RollType.Need);
                            return;
                        }
                    }
                }

                if (itemScore > currScore)
                {
                    Print("Rolling Need for '{0}' (score: {1}) as it's better than '{2}' (score: {3})", itemSparseRec.Name, itemScore, currentItem.Name, currScore);
                    RollItem(rollId, RollType.Need);
                    return;
                }
            }
            Print("Rolling greed for {0}", itemSparseRec.Name);
            RollItem(rollId, RollType.Greed);
        }

        #endregion

        #region CalculateItemScore

        private int CalculateItemScore(ItemInfo itemInfo)
        {
            var score = 0;
            // First we take care of the "special stats"
            //if (itemInfo.Class == ItemClass.Armor)
            //{
            //    if (_weightSet.ContainsKey(ItemStatType.Armor))
            //        score += (int)Math.Floor(itemInfo.Armor * _weightSet[ItemStatType.Armor]);
            //    else
            //        score += (int)itemInfo.Armor;
            //}
            //if (itemInfo.Class == ItemClass.Weapon)
            //{
            //    if (_weightSet.ContainsKey(ItemStatType.DPS))
            //        score += (int)Math.Floor(itemInfo.DPS * _weightSet[ItemStatType.DPS]);
            //    else
            //        score += (int)itemInfo.DPS;
            //}

            // Then the real ones
            foreach (var s in itemInfo.Stats)
            {
                if (_weightSet.ContainsKey(s.Key))
                    score += (int)Math.Floor(s.Value * _weightSet[s.Key]);
                else
                    score += s.Value;
            }
            return score;
        }

        #endregion

        #region Helpers

        private void RollItem(int rollId, RollType type)
        {
            WoWScript.ExecuteNoResults("RollOnLoot(" + rollId + "," + (int)type + ")");
        }

        private void EquipItem(WoWItem item, EquipSlot slot)
        {
            int bi, bs;
            item.GetSlotIndexes(out bi, out bs);
            WoWScript.ExecuteNoResults(string.Format("ClearCursor(); PickupContainerItem({0}, {1}); EquipCursorItem({2})", bi, bs, (int)slot));
        }

        #endregion

        #region Events

        private void HandleEvent(string ev, List<string> args)
        {
            switch (ev)
            {
                case "START_LOOT_ROLL":
                    EvaluateRollItem(Convert.ToInt32(args.ElementAtOrDefault(0)));
                    break;

                case "CONFIRM_DISENCHANT_ROLL":
                case "CONFIRM_LOOT_ROLL":
                    WoWScript.ExecuteNoResults("ConfirmLootRoll(" + args.ElementAtOrDefault(0) + "," + args.ElementAtOrDefault(1) + ")");
                    break;

                case "LOOT_BIND_CONFIRM":
                    WoWScript.ExecuteNoResults("ConfirmLootSlot(" + args.ElementAtOrDefault(0) + ")");
                    break;

                case "USE_BIND_CONFIRM":
                    WoWScript.ExecuteNoResults("ConfirmBindOnUse()");
                    break;

                case "EQUIP_BIND_CONFIRM":
                case "AUTOEQUIP_BIND_CONFIRM":
                    WoWScript.ExecuteNoResults("EquipPendingItem(" + args.ElementAtOrDefault(0) + ")");
                    break;

                case "ITEM_PUSH":
                    // New item in inventory.
                    _lastPulse = DateTime.MinValue;
                    break;

                case "ACTIVE_TALENT_GROUP_CHANGED":
                case "CHARACTER_POINTS_CHANGED":
                case "PLAYER_ENTERING_WORLD":
                    // Update weightsets
                    break;
            }
        }

        #endregion

        #region Enums

        public enum RollType
        {
            Pass = 0,
            Need = 1,
            Greed = 2,
            Disenchant = 3
        }

        #endregion
    }
}
