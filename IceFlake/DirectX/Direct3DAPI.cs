using System;
using System.Runtime.InteropServices;

namespace IceFlake.DirectX
{
    public static class Direct3DAPI
    {
        public const uint SDKVersion = 32;

        public const int EndSceneOffset = 42;
        public const int ResetOffset = 16;

        [DllImport("d3d9.dll")]
        public static extern IntPtr Direct3DCreate9(uint sdkVersion);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void D3DRelease(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int Direct3D9CreateDevice(IntPtr instance, uint adapter, uint deviceType,
                                                     IntPtr focusWindow,
                                                     uint behaviorFlags,
                                                     [In] ref PresentParameters presentationParameters,
                                                     [Out] out IntPtr returnedDeviceInterface);

        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        public delegate int Direct3D9EndScene(IntPtr instance);

        [StructLayout(LayoutKind.Sequential)]
        public struct PresentParameters
        {
            public readonly uint BackBufferWidth;
            public readonly uint BackBufferHeight;
            public uint BackBufferFormat;
            public readonly uint BackBufferCount;
            public readonly uint MultiSampleType;
            public readonly uint MultiSampleQuality;
            public uint SwapEffect;
            public readonly IntPtr hDeviceWindow;
            [MarshalAs(UnmanagedType.Bool)]
            public bool Windowed;
            [MarshalAs(UnmanagedType.Bool)]
            public readonly bool EnableAutoDepthStencil;
            public readonly uint AutoDepthStencilFormat;
            public readonly uint Flags;
            public readonly uint FullScreen_RefreshRateInHz;
            public readonly uint PresentationInterval;
        }
    }
}
