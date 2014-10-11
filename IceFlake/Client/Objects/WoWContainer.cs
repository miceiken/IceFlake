using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using IceFlake.Client.Patchables;

namespace IceFlake.Client.Objects
{
    public class WoWContainer : WoWItem
    {
        #region Typedefs & Delegates

        private static GetBagAtIndexDelegate GetBagAtIndex;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ulong GetBagAtIndexDelegate(int index);

        #endregion

        public WoWContainer(IntPtr pointer)
            : base(pointer)
        {
        }

        public int Slots
        {
            get { return GetDescriptor<int>(WoWContainerFields.CONTAINER_FIELD_NUM_SLOTS); }
        }

        public List<WoWItem> Items
        {
            get
            {
                return Enumerable.Range(0, Slots).Select(i => GetItem(i)).Where(i => i != null && i.IsValid).ToList();
            }
        }

        public static WoWContainer GetBagByIndex(int slot)
        {
            if (GetBagAtIndex == null)
                GetBagAtIndex =
                    Manager.Memory.RegisterDelegate<GetBagAtIndexDelegate>((IntPtr) Pointers.Container.GetBagAtIndex);
            return Manager.ObjectManager.GetObjectByGuid(GetBagAtIndex(slot)) as WoWContainer;
        }

        public ulong GetItemGuid(int index)
        {
            if (index > 35 || index > Slots || index < 0)
                return 0;

            return GetAbsoluteDescriptor<ulong>((int) WoWContainerFields.CONTAINER_FIELD_SLOT_1*0x4 + (index*8));
        }

        public WoWItem GetItem(int index)
        {
            return Manager.ObjectManager.GetObjectByGuid(GetItemGuid(index)) as WoWItem;
        }
    }
}