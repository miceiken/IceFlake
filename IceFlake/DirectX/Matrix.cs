using System;
using System.Runtime.InteropServices;

namespace IceFlake.DirectX
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix
    {
        private float _11, _12, _13, _14;
        private float _21, _22, _23, _24;
        private float _31, _32, _33, _34;
        private float _41, _42, _43, _44;

        public float this[int row, int column]
        {
            get
            {
                switch (row * 4 + column)
                {
                    case 0:
                        return _11;

                    case 1:
                        return _12;

                    case 2:
                        return _13;

                    case 3:
                        return _14;

                    case 4:
                        return _21;

                    case 5:
                        return _22;

                    case 6:
                        return _23;

                    case 7:
                        return _24;

                    case 8:
                        return _31;

                    case 9:
                        return _32;

                    case 10:
                        return _33;

                    case 11:
                        return _34;

                    case 12:
                        return _41;

                    case 13:
                        return _42;

                    case 14:
                        return _43;

                    case 15:
                        return _44;

                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (row * 4 + column)
                {
                    case 0:
                        _11 = value;
                        break;

                    case 1:
                        _12 = value;
                        break;

                    case 2:
                        _13 = value;
                        break;

                    case 3:
                        _14 = value;
                        break;

                    case 4:
                        _21 = value;
                        break;

                    case 5:
                        _22 = value;
                        break;

                    case 6:
                        _23 = value;
                        break;

                    case 7:
                        _24 = value;
                        break;

                    case 8:
                        _31 = value;
                        break;

                    case 9:
                        _32 = value;
                        break;

                    case 10:
                        _33 = value;
                        break;

                    case 11:
                        _34 = value;
                        break;

                    case 12:
                        _41 = value;
                        break;

                    case 13:
                        _42 = value;
                        break;

                    case 14:
                        _43 = value;
                        break;

                    case 15:
                        _44 = value;
                        break;

                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        #region Static members

        public static Matrix operator *(Matrix matrix1, Matrix matrix2)
        {
            Matrix result = new Matrix();

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    result[i, j] = matrix2[i, 0] * matrix1[0, j] + matrix2[i, 1] * matrix1[1, j] + matrix2[i, 2] * matrix1[2, j] + matrix2[i, 3] * matrix1[3, j];

            return result;
        }

        public static Matrix Identity()
        {
            Matrix result = new Matrix();

            result[0, 0] = 1f;
            result[1, 1] = 1f;
            result[2, 2] = 1f;
            result[3, 3] = 1f;

            return result;
        }

        public static Matrix Translation(Vector3 vector)
        {
            return Translation(vector.X, vector.Y, vector.Z);
        }
        public static Matrix Translation(float x, float y, float z)
        {
            Matrix result = Matrix.Identity();

            result[3, 0] = x;
            result[3, 1] = y;
            result[3, 2] = z;

            return result;
        }

        public static Matrix LookAtLH(Vector3 eye, Vector3 at, Vector3 up)
        {
            Matrix result = new Matrix();

            Vector3 vectorNormal = (at - eye).Normal;
            Vector3 right = up.Cross(vectorNormal);
            Vector3 rightNormal = right.Normal;
            Vector3 upNormal = vectorNormal.Cross(right).Normal;

            result[0, 0] = rightNormal.X;
            result[1, 0] = rightNormal.Y;
            result[2, 0] = rightNormal.Z;
            result[3, 0] = -rightNormal.Dot(eye);
            result[0, 1] = upNormal.X;
            result[1, 1] = upNormal.Y;
            result[2, 1] = upNormal.Z;
            result[3, 1] = -upNormal.Dot(eye);
            result[0, 2] = vectorNormal.X;
            result[1, 2] = vectorNormal.Y;
            result[2, 2] = vectorNormal.Z;
            result[3, 2] = -vectorNormal.Dot(eye);
            result[0, 3] = 0.0f;
            result[1, 3] = 0.0f;
            result[2, 3] = 0.0f;
            result[3, 3] = 1.0f;

            return result;
        }

        public static Matrix LookAtRH(Vector3 eye, Vector3 at, Vector3 up)
        {
            Matrix result = new Matrix();

            Vector3 vectorNormal = (at - eye).Normal;
            Vector3 right = up.Cross(vectorNormal);
            Vector3 rightNormal = right.Normal;
            Vector3 upNormal = vectorNormal.Cross(right).Normal;

            result[0, 0] = -rightNormal.X;
            result[1, 0] = -rightNormal.Y;
            result[2, 0] = -rightNormal.Z;
            result[3, 0] = rightNormal.Dot(eye);
            result[0, 1] = upNormal.X;
            result[1, 1] = upNormal.Y;
            result[2, 1] = upNormal.Z;
            result[3, 1] = -upNormal.Dot(eye);
            result[0, 2] = -vectorNormal.X;
            result[1, 2] = -vectorNormal.Y;
            result[2, 2] = -vectorNormal.Z;
            result[3, 2] = vectorNormal.Dot(eye);
            result[0, 3] = 0.0f;
            result[1, 3] = 0.0f;
            result[2, 3] = 0.0f;
            result[3, 3] = 1.0f;

            return result;
        }

        public static Matrix PerspectiveFovLH(float fieldOfViewY, float aspectRatio, float zNearPlane, float zFarPlane)
        {
            Matrix result = Matrix.Identity();

            result[0, 0] = 1.0f / (aspectRatio * (float)Math.Tan(fieldOfViewY / 2.0f));
            result[1, 1] = 1.0f / (float)Math.Tan(fieldOfViewY / 2.0f);
            result[2, 2] = zFarPlane / (zFarPlane - zNearPlane);
            result[2, 3] = 1.0f;
            result[3, 2] = (zFarPlane * zNearPlane) / (zNearPlane - zFarPlane);
            result[3, 3] = 0.0f;

            return result;
        }

        public static Matrix PerspectiveFovRH(float fovy, float aspect, float zn, float zf)
        {
            Matrix result = Matrix.Identity();

            result[0, 0] = 1.0f / (aspect * (float)Math.Tan(fovy / 2.0f));
            result[1, 1] = 1.0f / (float)Math.Tan(fovy / 2.0f);
            result[2, 2] = zf / (zn - zf);
            result[2, 3] = -1.0f;
            result[3, 2] = (zf * zn) / (zn - zf);
            result[3, 3] = 0.0f;

            return result;
        }

        public static Matrix RotationX(float angle)
        {
            Matrix result = Matrix.Identity();

            result[1, 1] = (float)Math.Cos(angle);
            result[2, 2] = (float)Math.Cos(angle);
            result[1, 2] = (float)Math.Sin(angle);
            result[2, 1] = -(float)Math.Sin(angle);

            return result;
        }

        public static Matrix RotationY(float angle)
        {
            Matrix result = Matrix.Identity();

            result[0, 0] = (float)Math.Cos(angle);
            result[2, 2] = (float)Math.Cos(angle);
            result[0, 2] = -(float)Math.Sin(angle);
            result[2, 0] = (float)Math.Sin(angle);

            return result;
        }

        public static Matrix RotationZ(float angle)
        {
            Matrix result = Matrix.Identity();

            result[0, 0] = (float)Math.Cos(angle);
            result[1, 1] = (float)Math.Cos(angle);
            result[0, 1] = (float)Math.Sin(angle);
            result[1, 0] = -(float)Math.Sin(angle);

            return result;
        }

        public static Matrix RotationYawPitchRoll(float yaw, float pitch, float roll)
        {
            Matrix result = Identity();
            Matrix temp;

            temp = RotationZ(yaw);
            result = result * temp;

            temp = RotationY(pitch);
            result = result * temp;

            temp = RotationX(roll);
            result = result * temp;

            return result;
        }

        #endregion
    }
}
