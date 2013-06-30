using System;
using System.Runtime.InteropServices;
using IceFlake.Client.Patchables;

namespace IceFlake.Client
{
    public static class WoWWorld
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int TracelineDelegate(
            ref Location start, ref Location end, out Location result, ref float distanceTravelled, uint flags,
            uint zero);
        private static TracelineDelegate _traceline;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool HandleTerrainClickDelegate(TerrainClickEvent terrainClickArgs);
        private static HandleTerrainClickDelegate _handleTerrainClick;

        public static int CurrentMapId
        {
            get { return Manager.Memory.Read<int>((IntPtr)Pointers.World.CurrentMapId); }
        }

        public static string CurrentMap
        {
            //get { return Manager.DBC[ClientDB.Map].GetLocalizedRow((int)CurrentMapId).GetStruct<MapRec>().m_MapName_lang; }
            get { return Manager.Memory.ReadString((IntPtr)Pointers.World.InternalMapName); }
        }

        public static string CurrentZone
        {
            get { return Manager.Memory.ReadString(Manager.Memory.Read<IntPtr>((IntPtr)Pointers.World.ZoneText)); }
        }

        public static string CurrentSubZone
        {
            get { return Manager.Memory.ReadString(Manager.Memory.Read<IntPtr>((IntPtr)Pointers.World.SubZoneText)); }
        }

        public static uint CurrentZoneId
        {
            get { return Manager.Memory.Read<uint>((IntPtr)Pointers.World.ZoneID); }
        }

        public static bool HandleTerrainClick(Location loc)
        {
            if (_handleTerrainClick == null)
                _handleTerrainClick = Manager.Memory.RegisterDelegate<HandleTerrainClickDelegate>((IntPtr)Pointers.World.HandleTerrainClick, true);

            return _handleTerrainClick(new TerrainClickEvent { GUID = 0ul, Position = loc, Button = MouseButton.None | MouseButton.Left });
        }

        public static int Traceline(Location start, Location end, out Location result, uint flags)
        {
            if (_traceline == null)
                _traceline = Manager.Memory.RegisterDelegate<TracelineDelegate>((IntPtr)Pointers.World.Traceline);

            float dist = 1.0f;
            return _traceline(ref start, ref end, out result, ref dist, flags, 0);
        }

        public static int Traceline(Location start, Location end)
        {
            Location result;
            return Traceline(start, end, out result, 0x120171);
        }

        public static int LineOfSightTest(Location start, Location end)
        {
            start.Z += 1.3f;
            end.Z += 1.3f;
            Location result;
            return Traceline(start, end, out result, 0x120171); // 0x100121
        }
    }
}