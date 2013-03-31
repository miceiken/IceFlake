using System;
using IceFlake.Client.Patchables;

namespace IceFlake.Client.Objects
{
    public class WoWDynamicObject : WoWObject
    {
        public WoWDynamicObject(IntPtr pointer)
            : base(pointer)
        {
        }

        public ulong CasterGuid
        {
            get { return GetDescriptor<ulong>(WoWDynamicObjectFields.DYNAMICOBJECT_CASTER); }
        }

        public uint SpellId
        {
            get { return GetDescriptor<uint>(WoWDynamicObjectFields.DYNAMICOBJECT_SPELLID); }
        }

        public float Radius
        {
            get { return GetDescriptor<float>(WoWDynamicObjectFields.DYNAMICOBJECT_RADIUS); }
        }

        public uint CastTime
        {
            get { return GetDescriptor<uint>(WoWDynamicObjectFields.DYNAMICOBJECT_CASTTIME); }
        }
    }
}