using System;
using IceFlake.Client.Patchables;

namespace IceFlake.Client.Objects
{
    public class WoWCorpse : WoWObject
    {
        public WoWCorpse(IntPtr pointer)
            : base(pointer)
        {
        }

        public ulong Owner
        {
            get { return GetDescriptor<ulong>(WoWCorpseFields.CORPSE_FIELD_OWNER); }
        }

        public ulong PartyGUID
        {
            get { return GetDescriptor<ulong>(WoWCorpseFields.CORPSE_FIELD_PARTY); }
        }

        public uint DisplayID
        {
            get { return GetDescriptor<uint>(WoWCorpseFields.CORPSE_FIELD_DISPLAY_ID); }
        }

        // WoWCorpseFields.Items

        public uint Flags
        {
            get { return GetDescriptor<uint>(WoWCorpseFields.CORPSE_FIELD_FLAGS); }
        }

        public uint DynamicFlags
        {
            get { return GetDescriptor<uint>(WoWCorpseFields.CORPSE_FIELD_DYNAMIC_FLAGS); }
        }
    }
}