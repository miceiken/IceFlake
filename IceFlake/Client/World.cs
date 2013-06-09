using System;
using System.Runtime.InteropServices;
using IceFlake.Client.Patchables;

namespace IceFlake.Client
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

        public static int CurrentMapId
        {
            get { return Manager.Memory.Read<int>((IntPtr)Pointers.World.CurrentMapId); }
        }

        public static string CurrentMap
        {
            //get { return Manager.DBC[ClientDB.Map].GetLocalizedRow((int)CurrentMapId).GetStruct<MapRec>().m_MapName_lang; }
            get { return Manager.Memory.ReadString((IntPtr)Pointers.World.InternalMapName); }
        }

        public static uint CurrentZoneId
        {
            get { return Manager.Memory.Read<uint>((IntPtr)Pointers.World.ZoneID); }
        }

        public static TracelineResult Traceline(Location start, Location end, uint flags)
        {
            if (_traceline == null)
                _traceline = Manager.Memory.RegisterDelegate<TracelineDelegate>((IntPtr)Pointers.World.Traceline);

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