using System;
using System.Runtime.InteropServices;
using IceFlake.Client.Patchables;

namespace IceFlake.Client.Objects
{
    public class WoWObject
    {
        private static SelectObjectDelegate _selectObject;
        private readonly GetObjectFacingDelegate _getObjectFacing;
        private readonly GetObjectLocationDelegate _getObjectLocation;
        private readonly GetObjectNameDelegate _getObjectName;
        private readonly InteractDelegate _interact;

        public WoWObject(IntPtr pointer)
        {
            Pointer = pointer;

            if (IsValid)
            {
                _getObjectName = RegisterVirtualFunction<GetObjectNameDelegate>(Pointers.Object.GetObjectName * 4);
                _getObjectLocation =
                    RegisterVirtualFunction<GetObjectLocationDelegate>(Pointers.Object.GetObjectLocation * 4);
                _getObjectFacing = RegisterVirtualFunction<GetObjectFacingDelegate>(Pointers.Object.GetObjectFacing * 4);
                _interact = RegisterVirtualFunction<InteractDelegate>(Pointers.Object.Interact * 4);
            }
        }

        public IntPtr Pointer { get; set; }

        public bool IsValid
        {
            get { return Pointer != IntPtr.Zero; }
        }

        public string Name
        {
            get
            {
                IntPtr pointer = _getObjectName(Pointer);
                if (pointer == IntPtr.Zero)
                    return "UNKNOWN";
                return Core.Memory.ReadString(pointer);
            }
        }

        public Location Location
        {
            get
            {
                Location ret;
                _getObjectLocation(Pointer, out ret);
                return ret;
            }
        }

        public float Facing
        {
            get
            {
                float facing = _getObjectFacing(Pointer);
                return facing;
            }
        }

        //public bool InLoS
        //{
        //    get
        //    {
        //        return World.LineOfSightTest(Location, Core.ObjectManager.LocalPlayer.Location) ==
        //               TracelineResult.NoCollision;
        //    }
        //}

        public WoWObjectType Type
        {
            get { return (WoWObjectType)GetDescriptor<uint>(WoWObjectFields.OBJECT_FIELD_TYPE); }
        }

        public ulong Guid
        {
            get { return GetDescriptor<ulong>(WoWObjectFields.OBJECT_FIELD_GUID); }
        }

        public uint Entry
        {
            get { return GetDescriptor<uint>(WoWObjectFields.OBJECT_FIELD_ENTRY); }
        }

        public float Distance
        {
            get
            {
                WoWLocalPlayer local = Core.ObjectManager.LocalPlayer;
                if (local == null || !local.IsValid)
                    return float.NaN;
                return (float)local.Location.DistanceTo(Location);
            }
        }

        public bool IsContainer
        {
            get { return Type.HasFlag(WoWObjectType.Container); }
        }

        public bool IsCorpse
        {
            get { return Type.HasFlag(WoWObjectType.Corpse); }
        }

        public bool IsGameObject
        {
            get { return Type.HasFlag(WoWObjectType.GameObject); }
        }

        public bool IsDynamicObject
        {
            get { return Type.HasFlag(WoWObjectType.DynamicObject); }
        }

        public bool IsUnit
        {
            get { return Type.HasFlag(WoWObjectType.Unit); }
        }

        public bool IsPlayer
        {
            get { return Type.HasFlag(WoWObjectType.Player); }
        }

        public bool IsItem
        {
            get { return Type.HasFlag(WoWObjectType.Item); }
        }

        protected T RegisterVirtualFunction<T>(uint offset) where T : class
        {
            IntPtr pointer = Core.Memory.GetObjectVtableFunction(Pointer, offset / 4);
            if (pointer == IntPtr.Zero)
                return null;
            return Core.Memory.RegisterDelegate<T>(pointer);
        }

        public void Select()
        {
            if (_selectObject == null)
                _selectObject =
                    Core.Memory.RegisterDelegate<SelectObjectDelegate>((IntPtr)Pointers.Object.SelectObject);

            _selectObject(Guid);
        }

        public void Interact()
        {
            _interact(Pointer);
            Helper.ResetHardwareAction();
        }

        public void Face()
        {
            Core.LocalPlayer.LookAt(Location);
        }

        //protected unsafe T GetDescriptor<T>(int offset)
        //{
        //    uint descriptorArray = *(uint*)(Pointer + Offsets.DescriptorOffset);
        //    int size = Marshal.SizeOf(typeof(T));
        //    object ret = null;
        //    switch (size)
        //    {
        //        case 1:
        //            ret = *(byte*)(descriptorArray + offset);
        //            break;

        //        case 2:
        //            ret = *(short*)(descriptorArray + offset);
        //            break;

        //        case 4:
        //            ret = *(uint*)(descriptorArray + offset);
        //            break;

        //        case 8:
        //            ret = *(ulong*)(descriptorArray + offset);
        //            break;
        //    }
        //    return (T)ret;
        //}

        protected T GetDescriptor<T>(Enum idx) where T : struct
        {
            return GetDescriptor<T>(Convert.ToInt32(idx));
        }

        protected T GetDescriptor<T>(int idx) where T : struct
        {
            var descriptorArray = Core.Memory.Read<uint>(new IntPtr(Pointer.ToInt64() + 0x8));
            //return Core.Memory.Read<T>(new IntPtr(descriptorArray + idx));
            return Core.Memory.Read<T>(new IntPtr(descriptorArray + (idx * 4)));
        }

        protected bool HasFlag(Enum idx, Enum flag)
        {
            var flags = GetDescriptor<uint>(idx);
            return (flags & Convert.ToInt32(flag)) != 0;
        }

        public override string ToString()
        {
            return "[\"" + Name + "\", Distance = " + (int)Distance + ", Type = " + Type + "]";
        }

        #region Nested type: GetObjectFacingDelegate

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate float GetObjectFacingDelegate(IntPtr thisPointer);

        #endregion

        #region Nested type: GetObjectLocationDelegate

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void GetObjectLocationDelegate(IntPtr thisPointer, out Location loc);

        #endregion

        #region Nested type: GetObjectNameDelegate

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr GetObjectNameDelegate(IntPtr thisPointer);

        #endregion

        #region Nested type: InteractDelegate

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void InteractDelegate(IntPtr thisPointer);

        #endregion

        #region Nested type: SelectObjectDelegate

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SelectObjectDelegate(ulong guid);

        #endregion
    }
}