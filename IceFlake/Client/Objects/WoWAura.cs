using System;
using IceFlake.Client.Patchables;

namespace IceFlake.Client.Objects
{
    public class WoWAura
    {
        public WoWAura(IntPtr pointer)
        {
            if (pointer == IntPtr.Zero)
                Invalidate();
            else
                Validate(pointer);
        }

        private IntPtr Pointer { get; set; }

        private AuraRec Entry { get; set; }

        public uint ID { get; private set; }

        public string Name { get; private set; }

        public bool IsValid
        {
            get { return ID != 0 && Remaining >= 0; }
        }

        public WoWUnit Caster
        {
            get { return Core.ObjectManager.GetObjectByGuid(CasterGuid) as WoWUnit; }
        }

        public ulong CasterGuid
        {
            get { return Entry.CreatorGuid; }
        }

        public bool IsMine
        {
            get { return CasterGuid == Core.LocalPlayer.Guid; }
        }

        public AuraFlags Flags
        {
            get { return Entry.Flags; }
        }

        public int Level
        {
            get { return Entry.Level; }
        }

        public ushort StackCount
        {
            get { return Entry.StackCount; }
        }

        public int Duration
        {
            get { return (int) (Entry.Duration/1000); }
        }

        public int Remaining
        {
            get
            {
                uint endTime = Entry.EndTime;
                return (int) ((endTime == 0 ? 0 : endTime - Helper.PerformanceCount)/1000);
            }
        }

        internal void Validate(IntPtr pointer)
        {
            Pointer = pointer;
            Entry = Core.Memory.Read<AuraRec>(Pointer);
            ID = Entry.AuraId;
            var SpellRow = new DBC<SpellRec>((IntPtr) ClientDB.Spell);
            if (SpellRow.HasRow(Entry.AuraId))
                Name = SpellRow[Entry.AuraId].Name;
            else
                Name = "unknown";
        }

        internal void Invalidate()
        {
            Pointer = IntPtr.Zero;
            ID = 0;
            Name = "unknown";
        }
    }
}