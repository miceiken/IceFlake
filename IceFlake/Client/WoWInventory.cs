using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.Client.Objects;
using IceFlake.Client.Collections;
using IceFlake.Client.Patchables;

namespace IceFlake.Client
{
    public class WoWInventory
    {
        private WoWLocalPlayer Me { get { return Manager.ObjectManager.LocalPlayer; } }

        public WoWInventory()
        { }

        #region Items

        public IEnumerable<WoWItem> Items
        {
            get
            {
                return Manager.ObjectManager.Objects
                    .Where(obj => obj.IsValid && obj.IsItem)
                    .OfType<WoWItem>()
                    .Where(item => item.OwnerGuid == Me.Guid);
            }
        }

        public IEnumerable<WoWItem> BackpackItems
        {
            get { return Enumerable.Range(0, 16).Select(i => Me.GetBackpackItem(i)).Where(item => item != null && item.IsValid); }
        }

        public IEnumerable<WoWItem> InventoryItems
        {
            get { return BackpackItems.Concat(InventoryContainers.SelectMany(container => container.Items)); }
        }

        public IEnumerable<WoWItem> BankedItems
        {
            get
            {
                var bankItems = Enumerable.Range(0, 28).Select(i => Me.GetBankedItem(i)).Where(item => item != null && item.IsValid);
                return bankItems.Concat(BankContainers.SelectMany(container => container.Items));
            }
        }

        public IEnumerable<WoWItem> EquippedItems
        {
            get { return Enumerable.Range((int)EquipSlot.Start, (int)EquipSlot.End + 1).Select(eq => Me.GetEquippedItem(eq)).Where(item => item != null && item.IsValid); }
        }

        #endregion

        #region Containers

        public IEnumerable<WoWContainer> InventoryContainers
        {
            get
            {
                return
                    Enumerable.Range((int)BagSlot.Bag1, (int)BagSlot.Bag4 + 1).Select(bs => WoWContainer.GetBagByIndex(bs))
                        .Where(container => container != null && container.IsValid);
            }
        }

        public IEnumerable<WoWContainer> BankContainers
        {
            get
            {
                return
                    Enumerable.Range((int)BagSlot.Bank1, (int)BagSlot.Bank7 + 1).Select(
                        bs => WoWContainer.GetBagByIndex(bs)).Where(container => container != null && container.IsValid);
            }
        }

        public IEnumerable<WoWContainer> AllContainers
        {
            get
            {
                return
                    Enumerable.Range((int)BagSlot.Bag1, (int)BagSlot.Bank7 + 1).Select(
                        bs => WoWContainer.GetBagByIndex(bs)).Where(container => container != null && container.IsValid);
            }
        }

        #endregion
    }
}
