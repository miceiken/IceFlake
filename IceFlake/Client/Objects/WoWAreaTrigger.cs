using System;
using IceFlake.Client.Patchables;

namespace IceFlake.Client.Objects
{
    public class WoWAreaTrigger : WoWObject
    {
        public WoWAreaTrigger(IntPtr pointer)
            : base(pointer)
        {
        }

        public ulong CasterGuid
        {
            get { return GetDescriptor<uint>(WoWAreaTriggerFields.Caster); }
        }

        public uint Duration
        {
            get { return GetDescriptor<uint>(WoWAreaTriggerFields.Duration); }
        }

        public uint SpellID
        {
            get { return GetDescriptor<uint>(WoWAreaTriggerFields.SpellID); }
        }

        public uint SpellVisualID
        {
            get { return GetDescriptor<uint>(WoWAreaTriggerFields.SpellVisualID); }
        }
    }
}