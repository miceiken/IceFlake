using System;
using System.Runtime.InteropServices;

namespace IceFlake.DirectX
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3
    {
        public unsafe Vector3(IntPtr pVector)
        {
            x = *(float*) pVector;
            y = *((float*) pVector + 1);
            z = *((float*) pVector + 2);
        }

        public unsafe Vector3(float* pVector)
        {
            x = *pVector;
            y = *(pVector + 1);
            z = *(pVector + 2);
        }

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        private float x;
        private float y;
        private float z;

        public float GetDistance(Vector3 location)
        {
            return GetDistance(location.x, location.y, location.z);
        }

        public float GetDistance(float x, float y, float z)
        {
            float dx = this.x - x;
            float dy = this.y - y;
            float dz = this.z - z;

            return (float) Math.Sqrt((dx*dx) + (dy*dy) + (dz*dz));
        }

        public float GetXRotation(Vector3 location)
        {
            return GetXRotation(location.y, location.z);
        }

        public float GetXRotation(float y, float z)
        {
            float dy = y - this.y;
            float dz = z - this.z;

            return (float) Math.Atan2(dz, dy);
        }

        public float GetYRotation(Vector3 location)
        {
            return GetYRotation(location.x, location.z);
        }

        public float GetYRotation(float x, float z)
        {
            float dx = x - this.x;
            float dz = z - this.z;

            return (float) Math.Atan2(dz, dx);
        }

        public float GetZRotation(Vector3 location)
        {
            return GetZRotation(location.x, location.y);
        }

        public float GetZRotation(float x, float y)
        {
            float dx = x - this.x;
            float dy = y - this.y;

            return (float) Math.Atan2(dy, dx);
        }

        public float GetHorizontalAngle(Vector3 location)
        {
            return GetZRotation(location.x, location.y);
        }

        public float GetHorizontalAngle(float x, float y)
        {
            return GetZRotation(x, y);
        }

        public float GetVerticalAngle(Vector3 location)
        {
            return GetVerticalAngle(location.x, location.y, location.z);
        }

        public float GetVerticalAngle(float x, float y, float z)
        {
            x = this.x - x;
            y = this.y - y;
            z = this.z - z;

            double a = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            double b = z;

            double result = -Math.Atan(b/a);

            if (result < -Math.PI/2)
                result += (Math.PI*2);

            return (float) result;
        }

        public Vector3 Cross(Vector3 vector)
        {
            var result = new Vector3();

            result.x = y*vector.Z - z*vector.Y;
            result.y = z*vector.X - x*vector.Z;
            result.z = x*vector.Y - y*vector.X;

            return result;
        }

        public float Dot(Vector3 vector)
        {
            if (Length <= 0 || vector.Length <= 0)
                return 0.0f;

            return (x)*(vector.X) + (y)*(vector.Y) + (z)*(vector.Z);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is Vector3 && (Vector3) obj == this;
        }

        public override string ToString()
        {
            return string.Concat(x.ToString("0.##"), ", ", y.ToString("0.##"), ", ", z.ToString("0.##"));
        }

        #region Properties

        public float Length
        {
            get { return (float) Math.Sqrt(x*x + y*y + z*z); }
        }

        public Vector3 Normal
        {
            get
            {
                float length = Length;
                if (length == 0)
                    return Zero;

                return new Vector3(x/length, y/length, z/length);
            }
        }

        public float X
        {
            get { return x; }
        }

        public float Y
        {
            get { return y; }
        }

        public float Z
        {
            get { return z; }
        }

        #endregion

        #region Static members

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static Vector3 operator *(Vector3 v1, float value)
        {
            return new Vector3(v1.x*value, v1.y*value, v1.z*value);
        }

        public static Vector3 operator /(Vector3 v1, float value)
        {
            return new Vector3(v1.x/value, v1.y/value, v1.z/value);
        }

        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
        }

        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return !(v1 == v2);
        }

        public static readonly Vector3 Zero = new Vector3(0, 0, 0);
        public static readonly Vector3 Up = new Vector3(0, 0, 1);

        #endregion
    }
}