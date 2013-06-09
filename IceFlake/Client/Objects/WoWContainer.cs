using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using IceFlake.Client.Patchables;
using System.Linq;

namespace IceFlake.Client.Objects
{
    public class WoWContainer : WoWItem
    {
        private static GetBagAtIndexDelegate GetBagAtIndex;

        public WoWContainer(IntPtr pointer)
            : base(pointer)
        {
        }

        public int Slots
        {
            get { return GetDescriptor<int>(WoWContainerFields.CONTAINER_FIELD_NUM_SLOTS); }
        }

        public static WoWContainer GetBagByIndex(int slot)
        {
            if (GetBagAtIndex == null)
                GetBagAtIndex =
                    Manager.Memory.RegisterDelegate<GetBagAtIndexDelegate>((IntPtr)Pointers.Container.GetBagAtIndex);
            return Manager.ObjectManager.GetObjectByGuid(GetBagAtIndex(slot)) as WoWContainer;
        }

        #region Nested type: GetBagAtIndexDelegate

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ulong GetBagAtIndexDelegate(int index);

        #endregion

        public ulong GetItemGuid(int index)
        {
            if (index > 35 || index > Slots || index < 0)
                return 0;

            return GetAbsoluteDescriptor<ulong>((int)WoWContainerFields.CONTAINER_FIELD_SLOT_1 * 0x4 + (index * 8));
        }

        public WoWItem GetItem(int index)
        {
            return Manager.ObjectManager.GetObjectByGuid(GetItemGuid(index)) as WoWItem;
        }

        public List<WoWItem> Items
        {
            get { return Enumerable.Range(0, Slots).Select(i => GetItem(i)).Where(i => i != null && i.IsValid).ToList(); }
        }
    }
}