using System;
using System.Runtime.InteropServices;
using System.Threading;
using IceFlake.Runtime;
using GreyMagic.Internals;
using SlimDX.Direct3D9;

namespace IceFlake.DirectX
{
    public delegate void EndSceneCallback();

    public static class Direct3D
    {
        private const int VMT_ENDSCENE = 42;
        public static CallbackManager<EndSceneCallback> CallbackManager = new CallbackManager<EndSceneCallback>();

        private static Direct3D9EndScene _endSceneDelegate;
        private static Detour _endSceneHook;
        public static Device Device { get; private set; }

        public static int FrameCount { get; private set; }

        public static event EventHandler OnFirstFrame;
        public static event EventHandler OnLastFrame;

        private static int EndSceneHook(IntPtr device)
        {
            try
            {
                if (FrameCount == -1)
                {
                    if (OnLastFrame != null)
                        OnLastFrame(null, new EventArgs());
                    Device = null;
                }
                else
                {
                    if (Device == null)
                        Device = Device.FromPointer(device);

                    if (FrameCount == 0)
                        if (OnFirstFrame != null)
                            OnFirstFrame(null, new EventArgs());

                    PrepareRenderState();
                    CallbackManager.Invoke();
                    // TEMP SOLUTION
                    // TODO: FIX THIS
                    IceFlake.Client.Scripts.ScriptManager.Pulse(); 
                }
            }
            catch (Exception e)
            {
                Log.WriteLine("Error: " + e.ToLongString());
            }

            if (FrameCount != -1)
                FrameCount += 1;

            return (int) _endSceneHook.CallOriginal(device);
        }

        public static void Initialize()
        {
            IntPtr endScenePointer = IntPtr.Zero;
            using (var d3d = new SlimDX.Direct3D9.Direct3D())
            {
                using (
                    var tmpDevice = new Device(d3d, 0, DeviceType.Hardware, IntPtr.Zero,
                                               CreateFlags.HardwareVertexProcessing,
                                               new PresentParameters {BackBufferWidth = 1, BackBufferHeight = 1}))
                {
                    endScenePointer = Manager.Memory.GetObjectVtableFunction(tmpDevice.ComPointer, VMT_ENDSCENE);
                }
            }

            _endSceneDelegate = Manager.Memory.RegisterDelegate<Direct3D9EndScene>(endScenePointer);
            _endSceneHook = Manager.Memory.Detours.CreateAndApply(_endSceneDelegate, new Direct3D9EndScene(EndSceneHook),
                                                                  "D9EndScene");

            Log.WriteLine("EndScene detoured at 0x{0:X}", endScenePointer);
        }

        public static void Shutdown()
        {
            if (Device == null)
                return;

            if (FrameCount > 0)
            {
                FrameCount = -1;
                while (Device != null)
                    Thread.Sleep(0);
            }
        }

        private static void PrepareRenderState()
        {
            if (Device == null)
                return;

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
            Device.SetRenderState(RenderState.ZFunc, 4); //
            Device.SetRenderState(RenderState.AlphaBlendEnable, true);
            Device.SetRenderState(RenderState.DestinationBlend, Blend.InverseSourceAlpha);
            Device.SetRenderState(RenderState.Lighting, false);
            Device.SetTexture(0, null);
            Device.SetRenderState(RenderState.CullMode, Cull.None);
        }

        #region Nested type: Direct3D9EndScene

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int Direct3D9EndScene(IntPtr device);

        #endregion
    }
}