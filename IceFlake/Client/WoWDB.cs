using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using IceFlake.Client.Patchables;

namespace IceFlake.Client
{
    public class WoWDB
    {
        private static readonly Dictionary<ClientDB, DbTable> Tables = new Dictionary<ClientDB, DbTable>();

        public WoWDB()
        {
            for (var tableBase = (IntPtr)Pointers.DBC.RegisterBase;
                 Manager.Memory.Read<byte>(tableBase) != 0xC3;
                 tableBase += 0x11)
            {
                var index = Manager.Memory.Read<uint>(tableBase + 1);
                var tablePtr = new IntPtr(Manager.Memory.Read<int>(tableBase + 0xB) + 0x18);
                Tables.Add((ClientDB)index, new DbTable(tablePtr));
            }
        }

        public DbTable this[ClientDB db] { get { return Tables[db]; } }

        #region Nested type: DbTable

        public class DbTable
        {
            internal readonly IntPtr Address;
            private ClientDb_GetLocalizedRow _getLocalizedRow;
            private ClientDb_GetRow _getRow;

            public DbTable(IntPtr address)
            {
                Address = address;
                var h = Manager.Memory.Read<DbHeader>(Address);
                MaxIndex = h.MaxIndex;
                MinIndex = h.MinIndex;
            }

            public uint MaxIndex { get; private set; }
            public uint MinIndex { get; private set; }

            public Row GetRow(int index)
            {
                return GetRowFromDelegate(index);
            }

            public Row GetLocalizedRow(int index)
            {
                if (_getLocalizedRow == null)
                {
                    _getLocalizedRow = Manager.Memory.RegisterDelegate<ClientDb_GetLocalizedRow>((IntPtr)Pointers.DBC.GetLocalizedRow);
                }
                IntPtr rowPtr = Marshal.AllocHGlobal(4 * 4 * 256);
                int tmp = _getLocalizedRow(new IntPtr(Address.ToInt32() - 0x18), index, rowPtr);
                if (tmp != 0)
                {
                    return new Row(rowPtr, true);
                }
                Marshal.FreeHGlobal(rowPtr);
                return null;
            }

            private Row GetRowFromDelegate(int index)
            {
                if (_getRow == null)
                {
                    _getRow = Manager.Memory.RegisterDelegate<ClientDb_GetRow>((IntPtr)Pointers.DBC.GetRow);
                }
                var ret = new IntPtr(_getRow(new IntPtr(Address.ToInt32()), index));
                return ret == IntPtr.Zero ? null : new Row(ret, false);
            }

            #region Nested type: ClientDb_GetLocalizedRow

            [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
            private delegate int ClientDb_GetLocalizedRow(IntPtr instance, int index, IntPtr rowPtr);

            #endregion

            #region Nested type: ClientDb_GetRow

            [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
            private delegate int ClientDb_GetRow(IntPtr instance, int idx);

            #endregion

            #region Nested type: DbHeader

            [StructLayout(LayoutKind.Sequential)]
            private struct DbHeader
            {
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
                public uint[] Junk;

                public uint MaxIndex;
                public uint MinIndex;
            }

            #endregion

            #region Nested type: Row

            public class Row : IDisposable
            {
                private readonly bool _isManagedMem;
                private IntPtr _rowPtr;

                private Row(IntPtr rowPtr)
                {
                    _rowPtr = rowPtr;
                }

                internal Row(IntPtr rowPtr, bool isManagedMem)
                    : this(rowPtr)
                {
                    _isManagedMem = isManagedMem;
                }

                #region IDisposable Members

                public void Dispose()
                {
                    if (_isManagedMem)
                    {
                        Marshal.FreeHGlobal(_rowPtr);
                    }

                    _rowPtr = IntPtr.Zero;
                    GC.SuppressFinalize(this);
                }

                #endregion

                public T GetField<T>(uint index) where T : struct
                {
                    try
                    {
                        if (typeof(T) == typeof(string))
                        {
                            // Sometimes.... generics ****ing suck
                            object s = Marshal.PtrToStringAnsi(Manager.Memory.Read<IntPtr>(new IntPtr((uint)_rowPtr + (index * 4))));
                            return (T)s;
                        }

                        return Manager.Memory.Read<T>(new IntPtr((uint)_rowPtr + (index * 4)));
                    }
                    catch
                    {
                        return default(T);
                    }
                }

                //public void SetField(uint index, int value)
                //{
                //    byte[] bs = BitConverter.GetBytes(value);
                //    Win32.WriteBytes((IntPtr)(_rowPtr.ToUInt32() + (index * 4)), bs);
                //}

                private object structedObject = null;
                public T GetStruct<T>() where T : struct
                {
                    try
                    {
                        if (structedObject == null)
                        {
                            var addr = (IntPtr)_rowPtr;
                            structedObject = (T)Marshal.PtrToStructure(addr, typeof(T));
                            Marshal.FreeHGlobal(addr);
                        }
                        return (T)structedObject;
                    }
                    catch
                    {
                        return default(T);
                    }
                }
            }

            #endregion
        }

        #endregion
    }
}
