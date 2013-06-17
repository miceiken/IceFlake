using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using IceFlake.Client.Patchables;
using SlimDX;

namespace IceFlake.Client
{
    public unsafe class WoWCamera
    {
        public WoWCamera()
        {
            this.Pointer = Manager.Memory.Read<IntPtr>(new IntPtr(Manager.Memory.Read<uint>((IntPtr)Pointers.Drawing.WorldFrame) + Pointers.Drawing.ActiveCamera));
        }

        public IntPtr Pointer
        {
            get;
            private set;
        }

        public bool IsValid { get { return Pointer != IntPtr.Zero; } }

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate Vector3* ForwardDelegate(IntPtr ptr, Vector3* vecOut);
        private ForwardDelegate _Forward;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate Vector3* RightDelegate(IntPtr ptr, Vector3* vecOut);
        private RightDelegate _Right;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate Vector3* UpDelegate(IntPtr ptr, Vector3* vecOut);
        private UpDelegate _Up;

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
                var cam = GetCamera();
                return Matrix.PerspectiveFovRH(cam.FieldOfView * 0.6f, cam.Aspect, cam.NearPlane, cam.FarPlane);
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
}
