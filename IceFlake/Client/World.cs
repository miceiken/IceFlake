using System;
using System.Runtime.InteropServices;
using Core.Client.Patchables;

namespace Core.Client
{
    public static class World
    {
        private static TracelineDelegate _traceline;

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //private delegate bool HandleTerrainClickDelegate(TerrainClickEvent terrainClickArgs);
        //private static HandleTerrainClickDelegate _handleTerrainClick;

        //public static bool HandleTerrainClick(Location loc)
        //{
        //    if (_handleTerrainClick == null)
        //        _handleTerrainClick = Core.Memory.RegisterDelegate<HandleTerrainClickDelegate>((IntPtr)Pointers.World.HandleTerrainClick, true);

        //    return _handleTerrainClick(new TerrainClickEvent { Position = loc });
        //}        

        public static uint CurrentMapId
        {
            get { return WoWCore.Memory.Read<uint>((IntPtr) Pointers.World.CurrentMapId, true); }
        }

        public static string CurrentMap
        {
            get { return new DBC<MapRec>((IntPtr) ClientDB.Map)[CurrentMapId].m_MapName_lang; }
        }

        public static uint CurrentZoneId
        {
            get { return WoWCore.Memory.Read<uint>((IntPtr) Pointers.World.ZoneID, true); }
        }

        public static TracelineResult Traceline(Location start, Location end, uint flags)
        {
            if (_traceline == null)
                _traceline = WoWCore.Memory.RegisterDelegate<TracelineDelegate>((IntPtr) Pointers.World.Traceline, true);

            float dist = 1.0f;
            Location result;
            return _traceline(ref start, ref end, out result, ref dist, flags, 0)
                       ? TracelineResult.Collided
                       : TracelineResult.NoCollision;
        }

        public static TracelineResult Traceline(Location start, Location end)
        {
            return Traceline(start, end, 0x120171);
        }

        public static TracelineResult LineOfSightTest(Location start, Location end)
        {
            start.Z += 1.3f;
            end.Z += 1.3f;
            return Traceline(start, end, 0x100121);
        }

        #region Nested type: TracelineDelegate

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool TracelineDelegate(
            ref Location start, ref Location end, out Location result, ref float distanceTravelled, uint flags,
            uint zero);

        #endregion
    }
}