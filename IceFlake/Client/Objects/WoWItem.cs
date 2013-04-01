using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using IceFlake.Client.Patchables;

namespace IceFlake.Client.Objects
{
    public class WoWItem : WoWObject
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void UseItemDelegate(IntPtr thisObj, ref ulong guid, int unkZero);
        private static UseItemDelegate _useItem;

        // int __thiscall WowClientDB2::ItemRec_C::GetRow(int this, int a2, int a3, int a4, int a5, char a6)
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr GetItemRecPointerFromId(IntPtr wowdb2, uint itemid, ref ulong guid, IntPtr callback, IntPtr param, bool unk = false);
        private static GetItemRecPointerFromId _getItemRecPtrFromId;

        // int __thiscall WowClientDB2::ItemRecSparse_C::GetRow(int this, int a2, int a3, int a4, int a5, char a6)
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr GetItemSparseRecPointerFromId(IntPtr wowdb2, uint itemid, ref ulong guid, IntPtr callback, IntPtr param, bool unk = false);
        private static GetItemSparseRecPointerFromId _getItemSparseRecFromId;

        public WoWItem(IntPtr pointer)
            : base(pointer)
        {
            if (IsValid)
            {
                //ItemInfo = WoWItem.GetItemRecordFromId(this.Entry);
                //ItemSparseInfo = WoWItem.GetItemSparseRecordFromId(this.Entry);
            }
        }

        public ulong OwnerGuid
        {
            get { return GetDescriptor<ulong>(WoWItemFields.ITEM_FIELD_OWNER); }
        }

        public ulong CreatorGuid
        {
            get { return GetDescriptor<ulong>(WoWItemFields.ITEM_FIELD_CREATOR); }
        }

        public uint StackCount
        {
            get { return GetDescriptor<uint>(WoWItemFields.ITEM_FIELD_STACK_COUNT); }
        }

        public ItemFlags Flags
        {
            get { return (ItemFlags)GetDescriptor<uint>(WoWItemFields.ITEM_FIELD_FLAGS); }
        }

        public uint RandomPropertiesId
        {
            get { return GetDescriptor<uint>(WoWItemFields.ITEM_FIELD_RANDOM_PROPERTIES_ID); }
        }

        public uint Durability
        {
            get { return GetDescriptor<uint>(WoWItemFields.ITEM_FIELD_DURABILITY); }
        }

        public uint MaxDurability
        {
            get { return GetDescriptor<uint>(WoWItemFields.ITEM_FIELD_MAXDURABILITY); }
        }

        public IEnumerable<ItemEnchantment> Enchants
        {
            get
            {
                for (var i = 0; i < 12; i++)
                    if (GetDescriptor<uint>((int)WoWItemFields.ITEM_FIELD_ENCHANTMENT_1_1 + (i * 12)) > 0)
                        yield return GetDescriptor<ItemEnchantment>((int)WoWItemFields.ITEM_FIELD_ENCHANTMENT_1_1 + (i * 12));
            }
        }

        public void Use()
        {
            Use(Manager.LocalPlayer);
        }

        public void Use(WoWObject target)
        {
            if (_useItem == null)
                _useItem = Manager.Memory.RegisterDelegate<UseItemDelegate>((IntPtr)Pointers.Item.UseItem);
            var guid = target.Guid;
            _useItem(Manager.LocalPlayer.Pointer, ref guid, 0);
        }

        public bool IsSoulbound
        {
            get { return ((uint)this.Flags & 1u) != 0; }
        }

        public ItemRec ItemInfo
        {
            get;
            private set;
        }

        public ItemSparseRec ItemSparseInfo
        {
            get;
            private set;
        }

        public bool GetSlotIndexes(out int container, out int slot)
        {
            // Remember that lua is 1 indexed based!
            container = -1;
            slot = -1;

            // Backpack
            for (var i = 0; i < 16; i++)
            {
                var item = Manager.LocalPlayer.GetBackpackItem(i);
                if (item == null) continue;
                Log.WriteLine("B0S{0}={1}", i, item.Name);
                if (item.Guid == this.Guid)
                {
                    container = 0;
                    slot = i + 1;
                    return true;
                }
            }

            // All the bags
            for (var i = (int)BagSlot.Bag1; i < (int)BagSlot.Bank7; i++)
            {
                var bag = WoWContainer.GetBagByIndex(i);
                if (bag == null) continue;
                for (var x = 0; x < bag.Slots; x++)
                {
                    if (bag.GetItemGuid(x) == 0ul) continue;
                    Log.WriteLine("B{0}S{1}={2}", i, x, bag.GetItem(x).Name);
                    if (bag.GetItemGuid(x) == this.Guid)
                    {
                        container = i + 1;
                        slot = x + 1;
                        return true;
                    }
                }
            }
            return false;
        }

        //public static IntPtr GetItemRecordPointerFromId(uint id)
        //{
        //    if (_getItemRecPtrFromId == null)
        //        _getItemRecPtrFromId = Core.Memory.RegisterDelegate<GetItemRecPointerFromId>((IntPtr)Pointers.Item.GetItemRecPtr);

        //    ulong guid = 0ul;
        //    return _getItemRecPtrFromId(Pointers.Item.ItemRecDB, id, ref guid, IntPtr.Zero, IntPtr.Zero);
        //}

        //public static ItemRec GetItemRecordFromId(uint id)
        //{
        //    var ptr = GetItemRecordPointerFromId(id);
        //    if (ptr == IntPtr.Zero)
        //        return default(ItemRec);
        //    return Core.Memory.Read<ItemRec>(ptr);
        //}

        //public static IntPtr GetItemSparseRecordPointerFromId(uint id)
        //{
        //    if (_getItemSparseRecFromId == null)
        //        _getItemSparseRecFromId = Core.Memory.RegisterDelegate<GetItemSparseRecPointerFromId>((IntPtr)Pointers.Item.GetItemSparseRecPtr);

        //    ulong guid = 0ul;
        //    return _getItemRecPtrFromId(Pointers.Item.ItemSparseRecDB, id, ref guid, IntPtr.Zero, IntPtr.Zero);
        //}

        //public static ItemSparseRec GetItemSparseRecordFromId(uint id)
        //{
        //    var ptr = GetItemSparseRecordPointerFromId(id);
        //    if (ptr == IntPtr.Zero)
        //        return default(ItemSparseRec);

        //    return Core.Memory.Read<ItemSparseRec>(ptr);
        //}

        public static IEnumerable<EquipSlot> GetInventorySlotsByEquipSlot(InventoryType type)
        {
            switch (type)
            {
                case InventoryType.Head:
                    yield return EquipSlot.Head;
                    break;
                case InventoryType.Neck:
                    yield return EquipSlot.Neck;
                    break;
                case InventoryType.Shoulders:
                    yield return EquipSlot.Shoulders;
                    break;
                case InventoryType.Body:
                    yield return EquipSlot.Body;
                    break;
                case InventoryType.Chest:
                case InventoryType.Robe:
                    yield return EquipSlot.Chest;
                    break;
                case InventoryType.Waist:
                    yield return EquipSlot.Waist;
                    break;
                case InventoryType.Legs:
                    yield return EquipSlot.Legs;
                    break;
                case InventoryType.Feet:
                    yield return EquipSlot.Feet;
                    break;
                case InventoryType.Wrists:
                    yield return EquipSlot.Wrists;
                    break;
                case InventoryType.Hands:
                    yield return EquipSlot.Hands;
                    break;
                case InventoryType.Finger:
                    yield return EquipSlot.Finger1;
                    yield return EquipSlot.Finger2;
                    break;
                case InventoryType.Trinket:
                    yield return EquipSlot.Trinket1;
                    yield return EquipSlot.Trinket2;
                    break;
                case InventoryType.Weapon:
                    yield return EquipSlot.MainHand;
                    yield return EquipSlot.OffHand;
                    break;
                case InventoryType.Shield:
                case InventoryType.WeaponOffHand:
                case InventoryType.Holdable:
                    yield return EquipSlot.OffHand;
                    break;
                case InventoryType.Ranged:
                case InventoryType.Thrown:
                case InventoryType.RangedRight:
                case InventoryType.Relic:
                    yield return EquipSlot.Ranged;
                    break;
                case InventoryType.Cloak:
                    yield return EquipSlot.Back;
                    break;
                case InventoryType.TwoHandedWeapon:
                    {
                        yield return EquipSlot.MainHand;
                        // TODO: Check for titan's grip
                        //bool flag = Manager.LocalPlayer.Class == WoWClass.Warrior && WoWScript.Execute<int>(InventoryManager.#a(61464), 4u) > 0;
                        //var mainHand = Manager.LocalPlayer.GetEquippedItem(EquipSlot.MainHand);
                        //if (flag && mainHand != null && mainHand.ItemInfo.InventoryType == InventoryType.TwoHandedWeapon)
                        //{
                        //    yield return EquipSlot.OffHand;
                        //}
                        break;
                    }
                case InventoryType.Bag:
                case InventoryType.Quiver:
                    yield return EquipSlot.Bag1;
                    yield return EquipSlot.Bag2;
                    yield return EquipSlot.Bag3;
                    yield return EquipSlot.Bag4;
                    break;
                case InventoryType.Tabard:
                    yield return EquipSlot.Tabard;
                    break;
                case InventoryType.WeaponMainHand:
                    yield return EquipSlot.MainHand;
                    break;
            }
        }
    }
}