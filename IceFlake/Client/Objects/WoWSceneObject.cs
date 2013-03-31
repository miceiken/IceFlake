using System;
using IceFlake.Client.Patchables;

namespace IceFlake.Client.Objects
{
    public class WoWSceneObject : WoWObject
    {
        public WoWSceneObject(IntPtr pointer)
            : base(pointer)
        {
        }

        public uint ScriptPackageID
        {
            get { return GetDescriptor<uint>(WoWSceneObjectFields.ScriptPackageID); }
        }

        public uint RndSeedVal
        {
            get { return GetDescriptor<uint>(WoWSceneObjectFields.RndSeedVal); }
        }

        public ulong CreatedBy
        {
            get { return GetDescriptor<uint>(WoWSceneObjectFields.CreatedBy); }
        }
    }
}