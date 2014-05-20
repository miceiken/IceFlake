using GreyMagic.Internals;
using IceFlake.Runtime;
#if SLIMDX
using SlimDX;
using SlimDX.Direct3D9;
#endif
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace IceFlake.DirectX
{
    public static class Direct3D
    {
        private static Direct3DAPI.Direct3D9EndScene _endSceneDelegate;
        private static Detour _endSceneHook;
        private static Detour _resetHook;
        private static ManualResetEventSlim FrameQueueFinalized;

        public static int FrameCount { get; private set; }

        public static event EventHandler OnFirstFrame;
        public static event EventHandler OnLastFrame = (sender, e) => FrameQueueFinalized.Set();

#if SLIMDX
        public static Device Device { get; private set; }
#endif

        private static int EndSceneHook(IntPtr device)
        {
            try
            {
                if (FrameCount == -1)
                {
                    Log.WriteLine("[D] OnLastFrame");
                    if (OnLastFrame != null)
                        OnLastFrame(null, new EventArgs());
#if SLIMDX
                    Device = null;
#endif
                }
                else
                {
#if SLIMDX
                    if (Device == null)
                        Device = Device.FromPointer(device);
#endif

                    if (FrameCount == 0)
                        if (OnFirstFrame != null)
                            OnFirstFrame(null, new EventArgs());

                    PrepareRenderState();

                    foreach (var pulsable in _pulsables)
                        pulsable.Direct3D_EndScene();
                }
            }
            catch (Exception e)
            {
                Log.LogException(e);
            }

            if (FrameCount != -1)
                FrameCount += 1;

            return (int)_endSceneHook.CallOriginal(device);
        }

        private static int ResetHook(IntPtr device, Direct3DAPI.PresentParameters pp)
        {
#if SLIMDX
            Device = null;
#endif
            return (int)_resetHook.CallOriginal(device, pp);
        }

        public static void Initialize()
        {
            FrameQueueFinalized = new ManualResetEventSlim(false);

            var endScenePointer = GetEndScenePointer();
            _endSceneDelegate = Manager.Memory.RegisterDelegate<Direct3DAPI.Direct3D9EndScene>(endScenePointer);            
            _endSceneHook = Manager.Memory.Detours.CreateAndApply(_endSceneDelegate, new Direct3DAPI.Direct3D9EndScene(EndSceneHook), "EndScene");

            Log.WriteLine("Direct3D9x:");
            Log.WriteLine("\tEndScene: 0x{0:X}", endScenePointer);
        }

        public static void Shutdown()
        {
            Log.WriteLine("[D] D3DShutdown");
            _pulsables.Clear();

            FrameCount = -1;
            FrameQueueFinalized.Wait();

            Manager.Memory.Detours.RemoveAll();
            Manager.Memory.Patches.RemoveAll();
        }

        private static void PrepareRenderState()
        {
#if SLIMDX
            if (Device == null)
                return;

            if (Manager.Camera == null)
                return;

            //var preRenderState = new StateBlock(Device, StateBlockType.All);
            //preRenderState.Capture();

            var viewport = Device.Viewport;
            viewport.MinZ = 0.0f;
            viewport.MaxZ = 0.94f;
            Device.Viewport = viewport;

            Device.SetTransform(TransformState.View, Manager.Camera.View);
            Device.SetTransform(TransformState.Projection, Manager.Camera.Projection);

            Device.VertexShader = null;
            Device.PixelShader = null;
            Device.SetRenderState(RenderState.ZEnable, true);
            Device.SetRenderState(RenderState.ZWriteEnable, true);
            Device.SetRenderState(RenderState.ZFunc, Compare.LessEqual);
            Device.SetRenderState(RenderState.AlphaBlendEnable, true);
            Device.SetRenderState(RenderState.DestinationBlend, Blend.InverseSourceAlpha);
            Device.SetRenderState(RenderState.Lighting, false);
            Device.SetTexture(0, null);
            Device.SetRenderState(RenderState.CullMode, Cull.None);

            //preRenderState.Apply();
#endif
        }

        private static LinkedList<IPulsable> _pulsables = new LinkedList<IPulsable>();
        public static void RegisterCallback(IPulsable pulsable)
        {
            _pulsables.AddLast(pulsable);
        }

        public static void RegisterCallbacks(params IPulsable[] pulsables)
        {
            foreach (var pulsable in pulsables)
                RegisterCallback(pulsable);
        }

        public static void RemoveCallback(IPulsable pulsable)
        {
            if (_pulsables.Contains(pulsable))
                _pulsables.Remove(pulsable);
        }

        private unsafe static IntPtr GetEndScenePointer()
        {
            // Device
            IntPtr ptr = Manager.Memory.Read<IntPtr>((IntPtr)0xC5DF88);
            ptr = Manager.Memory.Read<IntPtr>(IntPtr.Add(ptr, 0x397C));

            // Scene
            ptr = Manager.Memory.Read<IntPtr>(ptr);

            // EndScene
            ptr = Manager.Memory.Read<IntPtr>(IntPtr.Add(ptr, 0xA8));
            return ptr;
        }
    }
}