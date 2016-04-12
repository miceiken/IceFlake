using System;
using System.Runtime.InteropServices;
using IceFlake.Client.Patchables;

namespace IceFlake.Client.Objects
{
    public class WoWObject
    {
        #region Typedefs & Delegates

        private static SelectObjectDelegate _selectObject;
        private readonly GetObjectFacingDelegate _getObjectFacing;
        private readonly GetObjectLocationDelegate _getObjectLocation;
        private readonly GetObjectNameDelegate _getObjectName;
        private readonly InteractDelegate _interact;

        #endregion

        public WoWObject(IntPtr pointer)
        {
            Pointer = pointer;

            if (IsValid)
            {
                _getObjectName = RegisterVirtualFunction<GetObjectNameDelegate>(Pointers.Object.GetObjectName);
                _getObjectLocation =
                    RegisterVirtualFunction<GetObjectLocationDelegate>(Pointers.Object.GetObjectLocation);
                _getObjectFacing = RegisterVirtualFunction<GetObjectFacingDelegate>(Pointers.Object.GetObjectFacing);
                _interact = RegisterVirtualFunction<InteractDelegate>(Pointers.Object.Interact);
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
                return Manager.Memory.ReadString(pointer);
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

        public virtual float Facing
        {
            get
            {
                float facing = _getObjectFacing(Pointer);
                return facing;
            }
        }

        public bool InLoS
        {
            get
            {
                Location result;
                return (WoWWorld.LineOfSightTest(Location, Manager.ObjectManager.LocalPlayer.Location) & 0xFF) == 0;
            }
        }

        public WoWObjectType Type
        {
            get { return (WoWObjectType) GetDescriptor<uint>(WoWObjectFields.OBJECT_FIELD_TYPE); }
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
                WoWLocalPlayer local = Manager.ObjectManager.LocalPlayer;
                if (local == null || !local.IsValid)
                    return float.NaN;
                return (float) local.Location.DistanceTo(Location);
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
            IntPtr pointer = Manager.Memory.GetObjectVtableFunction(Pointer, offset);
            if (pointer == IntPtr.Zero)
                return null;
            return Manager.Memory.RegisterDelegate<T>(pointer);
        }

        public void Select()
        {
            if (_selectObject == null)
                _selectObject =
                    Manager.Memory.RegisterDelegate<SelectObjectDelegate>((IntPtr) Pointers.Object.SelectObject);

            _selectObject(Guid);
        }

        public void Interact()
        {
            _interact(Pointer);
            Helper.ResetHardwareAction();
        }

        public void Face()
        {
            Manager.LocalPlayer.LookAt(Location);
        }

        internal T GetDescriptor<T>(Enum idx) where T : struct
        {
            return GetDescriptor<T>(Convert.ToInt32(idx));
        }

        internal T GetDescriptor<T>(int idx) where T : struct
        {
            return GetAbsoluteDescriptor<T>(idx*0x4);
        }

        internal T GetAbsoluteDescriptor<T>(int offset) where T : struct
        {
            var descriptorArray = Manager.Memory.Read<uint>(new IntPtr(Pointer.ToInt64() + 0x8));
            return Manager.Memory.Read<T>(new IntPtr(descriptorArray + offset));
        }

        internal void SetDescriptor<T>(Enum idx, T value) where T : struct
        {
            SetDescriptor(Convert.ToInt32(idx), value);
        }

        internal void SetDescriptor<T>(int idx, T value) where T : struct
        {
            SetAbsoluteDescriptor(idx*0x4, value);
        }

        internal void SetAbsoluteDescriptor<T>(int offset, T value) where T : struct
        {
            var descriptorArray = Manager.Memory.Read<uint>(new IntPtr(Pointer.ToInt64() + 0x8));
            Manager.Memory.Write(new IntPtr(descriptorArray + offset), value);
        }

        protected bool HasFlag(Enum idx, Enum flag)
        {
            var flags = GetDescriptor<uint>(idx);
            return (flags & Convert.ToInt32(flag)) != 0;
        }

        public override string ToString()
        {
            return "[\"" + Name + "\", Distance = " + (int) Distance + ", Type = " + Type + "]";
        }

        public static implicit operator IntPtr(WoWObject obj)
        {
            return obj.Pointer;
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