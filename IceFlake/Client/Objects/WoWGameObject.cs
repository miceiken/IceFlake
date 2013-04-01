using System;
using IceFlake.Client.Patchables;

namespace IceFlake.Client.Objects
{
    public class WoWGameObject : WoWObject
    {
        public WoWGameObject(IntPtr pointer)
            : base(pointer)
        {
        }

        public ulong CreatedBy
        {
            get { return GetDescriptor<ulong>(WoWGameObjectFields.OBJECT_FIELD_CREATED_BY); }
        }

        public uint DisplayId
        {
            get { return GetDescriptor<uint>(WoWGameObjectFields.GAMEOBJECT_DISPLAYID); }
        }

        public uint Flags
        {
            get { return GetDescriptor<uint>(WoWGameObjectFields.GAMEOBJECT_FLAGS); }
        }

        // 4x4 Matrix?
        //public uint ParentRotation
        //{
        //    get { return GetDescriptor<uint>(WoWGameObjectFields.ParentRotation); }
        //}

        public uint Faction
        {
            get { return GetDescriptor<uint>(WoWGameObjectFields.GAMEOBJECT_FACTION); }
        }

        public uint Level
        {
            get { return GetDescriptor<uint>(WoWGameObjectFields.GAMEOBJECT_LEVEL); }
        }

        public bool Locked
        {
            get { return (Flags & (uint) GameObjectFlags.Locked) > 0; }
        }

        public bool InUse
        {
            get { return (Flags & (uint) GameObjectFlags.InUse) > 0; }
        }

        public bool IsTransport
        {
            get { return (Flags & (uint) GameObjectFlags.Transport) > 0; }
        }

        public bool CreatedByMe
        {
            get { return CreatedBy == Manager.ObjectManager.LocalPlayer.Guid; }
        }
    }
}