using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using IceFlake.Client.Patchables;
using IceFlake.DirectX;
using SlimDX;

namespace IceFlake.Client
{
#pragma warning disable 0169
    public struct CameraInfo
    {
        uint unk0;
        uint unk1;
        public Vector3 Position;
        public Matrix3 Matrix;
        public float FieldOfView;
        float unk2;
        int unk3;
        public float NearZ;
        public float FarZ;
        public float Aspect;
    }
#pragma warning restore 0169

    public unsafe class Camera
    {
        public IntPtr Pointer
        {
            get
            {
                return Manager.Memory.Read<IntPtr>(new IntPtr(Manager.Memory.Read<uint>((IntPtr)Pointers.Drawing.WorldFrame) + Pointers.Drawing.ActiveCamera));
            }
        }

        public bool IsValid { get { return Pointer != IntPtr.Zero; } }

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate float GetFovDelegate(IntPtr ptr);
        private GetFovDelegate _GetFov;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate Vector3* ForwardDelegate(IntPtr ptr, Vector3* vecOut);
        private ForwardDelegate _Forward;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate Vector3* RightDelegate(IntPtr ptr, Vector3* vecOut);
        private RightDelegate _Right;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate Vector3* UpDelegate(IntPtr ptr, Vector3* vecOut);
        private UpDelegate _Up;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate double GetFarClipDelegate();
        private GetFarClipDelegate GetFarClip;

        public float FieldOfView
        {
            get
            {
                if (_GetFov == null)
                    _GetFov = Manager.Memory.RegisterDelegate<GetFovDelegate>(Manager.Memory.GetObjectVtableFunction(Pointer, 0));

                return _GetFov(Pointer);
            }
        }

        public Vector3 Forward
        {
            get
            {
                if (_Forward == null)
                    _Forward = Manager.Memory.RegisterDelegate<ForwardDelegate>(Manager.Memory.GetObjectVtableFunction(Pointer, 1));

                var res = new Vector3();
                _Forward(Pointer, &res);
                return res;
            }
        }

        public Vector3 Right
        {
            get
            {
                if (_Right == null)
                    _Right = Manager.Memory.RegisterDelegate<RightDelegate>(Manager.Memory.GetObjectVtableFunction(Pointer, 2));

                var res = new Vector3();
                _Right(Pointer, &res);
                return res;
            }
        }

        public Vector3 Up
        {
            get
            {
                if (_Up == null)
                    _Up = Manager.Memory.RegisterDelegate<UpDelegate>(Manager.Memory.GetObjectVtableFunction(Pointer, 3));

                var res = new Vector3();
                _Up(Pointer, &res);
                return res;
            }
        }

        public Matrix Projection
        {
            get
            {
                if (GetFarClip == null)
                    GetFarClip = Manager.Memory.RegisterDelegate<GetFarClipDelegate>((IntPtr)Pointers.Drawing.GetFarClip);

                var cam = GetCamera();
                //return Matrix.PerspectiveFovRH(FieldOfView * 0.6f, Aspect, cam.NearZ, cam.FarZ);
                //return Matrix.PerspectiveFovRH(FieldOfView * 0.6f, Aspect, 0.2f, (float)GetFarClip());
                return Matrix.PerspectiveFovRH(FieldOfView * 0.6f, cam.Aspect, 0.2f, (float)GetFarClip());
            }
        }

        public Matrix View
        {
            get
            {
                var cam = GetCamera();
                var eye = cam.Position;
                var at = eye + Forward;
                return Matrix.LookAtRH(eye, at, new Vector3(0, 0, 1));
            }
        }

        public CameraInfo GetCamera()
        {
            return Manager.Memory.Read<CameraInfo>(Pointer);
        }
    }

    //public Vector3 Position
    //{
    //    get
    //    {
    //        return Memory.Read<Vector3>((uint)(pClass + 8));
    //    }
    //}

    //public Matrix3 Facing
    //{
    //    get
    //    {
    //        return Memory.Read<Matrix3>((uint)(pClass + 20));
    //    }
    //}

    //public float NearZ
    //{
    //    get
    //    {
    //        return Memory.Read<float>((uint)(pClass + 56));
    //    }
    //}

    //public float FarZ
    //{
    //    get
    //    {
    //        return Memory.Read<float>((uint)(pClass + 60));
    //    }
    //}

    //public float Fov
    //{
    //    get
    //    {
    //        return Memory.Read<float>((uint)(pClass + 64));
    //    }
    //}

    //public float Aspect
    //{
    //    get
    //    {
    //        return Memory.Read<float>((uint)(pClass + 68));
    //    }
    //}
}
