using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using IceFlake.Client.Objects;
using IceFlake.Client.Patchables;
using IceFlake.DirectX;

namespace IceFlake.Client
{
    public class ObjectManager
    {
        private static EnumVisibleObjectsDelegate _enumVisibleObjects;
        private static IntPtr _ourCallback;
        private static GetObjectByGuidDelegate _getObjectByGuid;
        private static GetLocalPlayerDelegate _getLocalPlayer;

        #region Fields

        private readonly EnumVisibleObjectsCallback _callback;
        private readonly Dictionary<ulong, WoWObject> _objects = new Dictionary<ulong, WoWObject>();

        #endregion

        #region Properties

        public WoWLocalPlayer LocalPlayer { get; private set; }
        public List<WoWObject> Objects { get; private set; }

        #endregion

        public ObjectManager()
        {
            _callback = Callback;

            _enumVisibleObjects =
                Manager.Memory.RegisterDelegate<EnumVisibleObjectsDelegate>(
                    (IntPtr) Pointers.ObjectManager.EnumVisibleObjects);
            _getObjectByGuid =
                Manager.Memory.RegisterDelegate<GetObjectByGuidDelegate>(
                    (IntPtr) Pointers.ObjectManager.GetObjectByGuid);
            _getLocalPlayer =
                Manager.Memory.RegisterDelegate<GetLocalPlayerDelegate>(
                    (IntPtr) Pointers.ObjectManager.GetLocalPlayerGuid);
            _ourCallback = Marshal.GetFunctionPointerForDelegate(_callback);
        }

        public bool IsInGame
        {
            get { return LocalPlayer != null; }
        }

        [EndSceneHandler]
        public void Direct3D_EndScene()
        {
            ulong localPlayerGuid = _getLocalPlayer();
            if (localPlayerGuid == 0)
                return;

            IntPtr localPlayerPointer = _getObjectByGuid(localPlayerGuid, -1);
            if (localPlayerPointer == IntPtr.Zero)
                return;

            LocalPlayer = new WoWLocalPlayer(localPlayerPointer);

            foreach (WoWObject obj in _objects.Values)
                obj.Pointer = IntPtr.Zero;

            _enumVisibleObjects(_ourCallback, 0);

            foreach (var pair in _objects.Where(p => p.Value.Pointer == IntPtr.Zero).ToList())
                _objects.Remove(pair.Key);

            if (Direct3D.FrameCount == 0)
            {
                Log.WriteLine("ObjectManager: {0} objects", _objects.Count);
            }

            Objects = _objects.Values.ToList();
        }

        public WoWObject GetObjectByGuid(ulong guid)
        {
            if (_objects.ContainsKey(guid))
                return _objects[guid];
            return null;
        }

        private int Callback(ulong guid, uint filter)
        {
            IntPtr pointer = _getObjectByGuid(guid, -1);
            if (pointer == IntPtr.Zero)
                return 1;

            if (_objects.ContainsKey(guid))
                _objects[guid].Pointer = pointer;
            else
            {
                var obj = new WoWObject(pointer);
                WoWObjectType type = obj.Type;

                if (type.HasFlag(WoWObjectType.Player))
                    _objects.Add(guid, new WoWPlayer(pointer));
                else if (type.HasFlag(WoWObjectType.Unit))
                    _objects.Add(guid, new WoWUnit(pointer));
                else if (type.HasFlag(WoWObjectType.Container))
                    _objects.Add(guid, new WoWContainer(pointer));
                else if (type.HasFlag(WoWObjectType.Item))
                    _objects.Add(guid, new WoWItem(pointer));
                else if (type.HasFlag(WoWObjectType.Corpse))
                    _objects.Add(guid, new WoWCorpse(pointer));
                else if (type.HasFlag(WoWObjectType.GameObject))
                    _objects.Add(guid, new WoWGameObject(pointer));
                else if (type.HasFlag(WoWObjectType.DynamicObject))
                    _objects.Add(guid, new WoWDynamicObject(pointer));
                else
                    _objects.Add(guid, obj);
            }
            return 1;
        }

        #region Nested type: EnumVisibleObjectsCallback

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int EnumVisibleObjectsCallback(ulong guid, uint filter);

        #endregion

        #region Nested type: EnumVisibleObjectsDelegate

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint EnumVisibleObjectsDelegate(IntPtr callback, int filter);

        #endregion

        #region Nested type: GetLocalPlayerDelegate

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ulong GetLocalPlayerDelegate();

        #endregion

        #region Nested type: GetObjectByGuidDelegate

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr GetObjectByGuidDelegate(ulong guid, int filter);

        #endregion
    }
}