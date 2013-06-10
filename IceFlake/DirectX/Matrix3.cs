using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace IceFlake.DirectX
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix3
    {
        private float _11, _12, _13;
        private float _21, _22, _23;
        private float _31, _32, _33;

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

                    case 4:
                        return _21;

                    case 5:
                        return _22;

                    case 6:
                        return _23;

                    case 8:
                        return _31;

                    case 9:
                        return _32;

                    case 10:
                        return _33;

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

                    case 4:
                        _21 = value;
                        break;

                    case 5:
                        _22 = value;
                        break;

                    case 6:
                        _23 = value;
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

                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }
    }
}
