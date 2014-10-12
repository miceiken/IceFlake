using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using GreyMagic.Native;
using IceFlake.Client.Patchables;
using IceFlake.Runtime;
using Microsoft.Xna.Framework;

namespace IceFlake.Client
{
    // https://github.com/tomrus88/WowAddin/blob/master/WowAddin/CDataStore.h
    public class DataStore
    {
        public DataStore()
        {
            Initialize();
        }

        public DataStore(IntPtr ptr)
        {
            Pointer = ptr;
        }

        public DataStore(ClientServices.NetMessage msg)
        {
            Initialize();
            PutInt32((int)msg);
        }

        public IntPtr Pointer { get; private set; }

        #region Properties

        public IntPtr Buffer
        {
            get { return Manager.Memory.Read<IntPtr>(Pointer + 0x4); }
        }

        // Base Offset
        public uint Base
        {
            get { return Manager.Memory.Read<uint>(Pointer + 0x8); }
        }

        // Amount of space allocated
        public uint Alloc
        {
            get { return Manager.Memory.Read<uint>(Pointer + 0xC); }
        }

        // Total written data (write position)
        public uint Size
        {
            get { return Manager.Memory.Read<uint>(Pointer + 0x10); }
        }

        // Read position, -1 when not finalized
        public uint Read
        {
            get { return Manager.Memory.Read<uint>(Pointer + 0x14); }
        }

        public bool IsFinal { get { return Read != 0; } }

        #endregion

        #region Methods

        private void Initialize()
        {
            if (_initialize == null)
                _initialize = Manager.Memory.RegisterDelegate<InitializeDelegate>(Pointers.Packets.Initialize.ToPointer());

            var dataPtr = Marshal.AllocHGlobal(0x18);
            Pointer = _initialize(dataPtr);
        }

        public void PutInt8(byte value)
        {
            if (_putInt8 == null)
                _putInt8 = Manager.Memory.RegisterDelegate<PutInt8Delegate>(Pointers.Packets.PutInt8.ToPointer());
            _putInt8(Pointer, value);
        }

        public void PutInt16(short value)
        {
            if (_putInt16 == null)
                _putInt16 = Manager.Memory.RegisterDelegate<PutInt16Delegate>(Pointers.Packets.PutInt16.ToPointer());
            _putInt16(Pointer, value);
        }

        public void PutInt32(int value)
        {
            if (_putInt32 == null)
                _putInt32 = Manager.Memory.RegisterDelegate<PutInt32Delegate>(Pointers.Packets.PutInt32.ToPointer());
            _putInt32(Pointer, value);
        }

        public void PutInt64(long value)
        {
            if (_putInt64 == null)
                _putInt64 = Manager.Memory.RegisterDelegate<PutInt64Delegate>(Pointers.Packets.PutInt64.ToPointer());
            _putInt64(Pointer, value);
        }

        public void PutFloat(float value)
        {
            if (_putFloat == null)
                _putFloat = Manager.Memory.RegisterDelegate<PutFloatDelegate>(Pointers.Packets.PutFloat.ToPointer());
            _putFloat(Pointer, value);
        }

        public void PutString(string value)
        {
            if (_putString == null)
                _putString = Manager.Memory.RegisterDelegate<PutStringDelegate>(Pointers.Packets.PutFloat.ToPointer());
            _putString(Pointer, value);
        }

        public void PutBytes(byte[] value, uint size)
        {
            if (_putBytes == null)
                _putBytes = Manager.Memory.RegisterDelegate<PutBytesDelegate>(Pointers.Packets.PutFloat.ToPointer());
            _putBytes(Pointer, value, size);
        }

        public void PutBytes(byte[] value)
        {
            PutBytes(value, (uint)value.Length);
        }

        public byte GetInt8()
        {
            if (_getInt8 == null)
                _getInt8 = Manager.Memory.RegisterDelegate<GetInt8Delegate>(Pointers.Packets.GetInt8.ToPointer());

            byte value = 0;
            _getInt8(Pointer, ref value);
            return value;
        }

        public short GetInt16()
        {
            if (_getInt16 == null)
                _getInt16 = Manager.Memory.RegisterDelegate<GetInt16Delegate>(Pointers.Packets.GetInt16.ToPointer());

            short value = 0;
            _getInt16(Pointer, ref value);
            return value;
        }

        public int GetInt32()
        {
            if (_getInt32 == null)
                _getInt32 = Manager.Memory.RegisterDelegate<GetInt32Delegate>(Pointers.Packets.GetInt32.ToPointer());

            int value = 0;
            _getInt32(Pointer, ref value);
            return value;
        }

        public long GetInt64()
        {
            if (_getInt64 == null)
                _getInt64 = Manager.Memory.RegisterDelegate<GetInt64Delegate>(Pointers.Packets.GetInt64.ToPointer());

            long value = 0;
            _getInt64(Pointer, ref value);
            return value;
        }

        public float GetFloat()
        {
            if (_getFloat == null)
                _getFloat = Manager.Memory.RegisterDelegate<GetFloatDelegate>(Pointers.Packets.GetFloat.ToPointer());

            float value = 0;
            _getFloat(Pointer, ref value);
            return value;
        }

        public string GetString(uint maxChars)
        {
            if (_getString == null)
                _getString = Manager.Memory.RegisterDelegate<GetStringDelegate>(Pointers.Packets.GetFloat.ToPointer());

            var strPtr = IntPtr.Zero;
            _getString(Pointer, ref strPtr, maxChars);
            return Manager.Memory.ReadString(strPtr);
        }

        public byte[] GetBytes(uint numBytes)
        {
            if (_getBytes == null)
                _getBytes = Manager.Memory.RegisterDelegate<GetBytesDelegate>(Pointers.Packets.GetFloat.ToPointer());

            var bufferPtr = IntPtr.Zero;
            _getBytes(Pointer, ref bufferPtr, numBytes);
            return Manager.Memory.ReadBytes(bufferPtr, (int)numBytes);
        }

        public void Finalize()
        {
            if (_finalize == null)
                _finalize = Manager.Memory.RegisterDelegate<FinalizeDelegate>(Pointers.Packets.Initialize.ToPointer());
            _finalize(Pointer);
        }

        private void Destroy()
        {
            if (_destroy == null)
                _destroy = Manager.Memory.RegisterDelegate<DestroyDelegate>(Pointers.Packets.Initialize.ToPointer());
            _destroy(Pointer);
        }

        #endregion

        #region Delegate

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr InitializeDelegate(IntPtr dataStorePtr);
        private static InitializeDelegate _initialize;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr PutInt8Delegate(IntPtr dataStorePtr, byte val);
        private static PutInt8Delegate _putInt8;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr PutInt16Delegate(IntPtr dataStorePtr, short val);
        private static PutInt16Delegate _putInt16;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr PutInt32Delegate(IntPtr dataStorePtr, int val);
        private static PutInt32Delegate _putInt32;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr PutInt64Delegate(IntPtr dataStorePtr, long val);
        private static PutInt64Delegate _putInt64;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr PutFloatDelegate(IntPtr dataStorePtr, float val);
        private static PutFloatDelegate _putFloat;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr PutStringDelegate(IntPtr dataStorePtr, string val);
        private static PutStringDelegate _putString;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr PutBytesDelegate(IntPtr dataStorePtr, byte[] buffer, uint size);
        private static PutBytesDelegate _putBytes;



        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr GetInt8Delegate(IntPtr dataStorePtr, ref byte val);
        private static GetInt8Delegate _getInt8;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr GetInt16Delegate(IntPtr dataStorePtr, ref short val);
        private static GetInt16Delegate _getInt16;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr GetInt32Delegate(IntPtr dataStorePtr, ref int val);
        private static GetInt32Delegate _getInt32;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr GetInt64Delegate(IntPtr dataStorePtr, ref long val);
        private static GetInt64Delegate _getInt64;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr GetFloatDelegate(IntPtr dataStorePtr, ref float val);
        private static GetFloatDelegate _getFloat;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr GetStringDelegate(IntPtr dataStorePtr, ref IntPtr stringPtr, uint maxChars);
        private static GetStringDelegate _getString;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr GetBytesDelegate(IntPtr dataStorePtr, ref IntPtr bufferPtr, uint numBytes);
        private static GetBytesDelegate _getBytes;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void FinalizeDelegate(IntPtr dataStorePtr);
        private static FinalizeDelegate _finalize;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void DestroyDelegate(IntPtr dataStorePtr);
        private static DestroyDelegate _destroy;

        #endregion
    }
}
