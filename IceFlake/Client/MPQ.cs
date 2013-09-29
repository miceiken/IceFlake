using System;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace IceFlake.Client
{
    static class MPQ
    {
        const int MPQ_OPEN_READ_ONLY = 0x0100;
        static IntPtr handle;

        static MPQ()
        {
            string path = @"C:\WoW 335\Data\";
            SFileOpenArchive(path + "common.MPQ", 0, MPQ_OPEN_READ_ONLY, out handle);
            string[] patches = { "common-2.MPQ", "expansion.MPQ", "patch.MPQ", "patch-2.MPQ", "patch-3.MPQ", "texture.mpq" };

            foreach (string s in patches)
                SFileOpenPatchArchive(handle, path + s, "", 0);
        }

        public static IntPtr OpenFile(string filename)
        {
            IntPtr file;
            SFileOpenFileEx(handle, filename, 0, out file);
            return file;
        }

        public static int Seek(IntPtr file, int pos)
        {
            int pos_h;
            return SFileSetFilePointer(file, pos, out pos_h, 0);
        }

        public static byte[] ReadFile(IntPtr file, int count)
        {
            byte[] buf = new byte[count];
            int read;
            SFileReadFile(file, buf, count, out read, IntPtr.Zero);
            return buf;
        }

        public static T Read<T>(IntPtr h) where T : new()
        {
            object ret;

            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Int32:
                    ret = BitConverter.ToInt32(MPQ.ReadFile(h, 4), 0);
                    break;
                case TypeCode.Int64:
                    ret = BitConverter.ToInt64(MPQ.ReadFile(h, 8), 0);
                    break;
                case TypeCode.Single:
                    ret = BitConverter.ToSingle(MPQ.ReadFile(h, 4), 0);
                    break;
                case TypeCode.Byte:
                    ret = MPQ.ReadFile(h, 1)[0];
                    break;
                default:
                    throw new NotSupportedException(typeof(T).FullName + " is not currently supported by Read<T>");
            }
            return (T)ret;
        }

        public static T ReadStruct<T>(IntPtr h) where T : new()
        {
            T ret = new T();
            int size = Marshal.SizeOf(ret);
            Byte[] b = MPQ.ReadFile(h, size);
            GCHandle pinnedArray = GCHandle.Alloc(b, GCHandleType.Pinned);
            IntPtr addr = pinnedArray.AddrOfPinnedObject();
            ret = (T)Marshal.PtrToStructure(addr, typeof(T));
            pinnedArray.Free();

            return ret;
        }

        public static T ReadChunk<T>(IntPtr h) where T : new()
        {
            byte[] b;
            int size;
            GCHandle pinnedArray;
            IntPtr addr;
            T type = new T();
            string id;

            b = MPQ.ReadFile(h, 4);

            Array.Reverse(b);
            id = Encoding.Default.GetString(b);

            b = MPQ.ReadFile(h, 4);
            size = BitConverter.ToInt32(b, 0);

            b = MPQ.ReadFile(h, size);

            pinnedArray = GCHandle.Alloc(b, GCHandleType.Pinned);
            addr = pinnedArray.AddrOfPinnedObject();
            type = (T)Marshal.PtrToStructure(addr, typeof(T));
            pinnedArray.Free();

            return type;
        }

        public static void SkipChunk(IntPtr h)
        {
            byte[] b;
            int size;
            GCHandle pinnedArray;
            IntPtr addr;
            string id;

            b = MPQ.ReadFile(h, 4);

            Array.Reverse(b);
            id = Encoding.Default.GetString(b);

            b = MPQ.ReadFile(h, 4);
            size = BitConverter.ToInt32(b, 0);

            b = MPQ.ReadFile(h, size);
        }

        public static bool HasFile(string file)
        {
            string[] s = file.Split('\\');
            file = s[s.Length - 1];
            return SFileHasFile(handle, file);
        }

        static public int Position(IntPtr file)
        {
            int pos_h;
            return SFileSetFilePointer(file, 0, out pos_h, 1);
        }

        [DllImport("StormLib.dll")]
        static extern bool SFileOpenArchive([MarshalAs(UnmanagedType.LPStr)] string name, int p, uint flags, out IntPtr handle);

        [DllImport("StormLib.dll")]
        static extern bool SFileOpenPatchArchive(IntPtr handle, string name, string prefix, uint flags);

        [DllImport("StormLib.dll")]
        static extern bool SFileCloseArchive(IntPtr handle);

        [DllImport("StormLib.dll")]
        static extern bool SFileOpenFileEx(IntPtr handle, string name, uint searchScope, out IntPtr file);

        [DllImport("StormLib.dll")]
        static extern bool SFileReadFile(IntPtr handle, [MarshalAs(UnmanagedType.LPArray)] byte[] buffer, int count, out int read, IntPtr io);

        [DllImport("StormLib.dll")]
        static extern int SFileSetFilePointer(IntPtr handle, int pos, out int pos_h, int move_method);

        [DllImport("StormLib.dll")]
        static extern bool SFileHasFile(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)] string name);
    }

    public class MPQStream
    {
        public MPQStream(IntPtr fileHandle)
        {
            this.Handle = fileHandle;
            this.Position = 0;
            this.Length = int.MaxValue; // :D
        }

        private IntPtr Handle { get; set; }
        public int Position { get; set; }
        public int Length { get; private set; }

        public T Read<T>() where T : new()
        {
            object ret = null;

            var size = Marshal.SizeOf(typeof(T));
            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Int32:
                    ret = BitConverter.ToInt32(ReadBytes(4), 0);
                    break;
                case TypeCode.Int64:
                    ret = BitConverter.ToInt64(ReadBytes(8), 0);
                    break;
                case TypeCode.UInt32:
                    ret = BitConverter.ToUInt32(ReadBytes(4), 0);
                    break;
                case TypeCode.UInt64:
                    ret = BitConverter.ToUInt64(ReadBytes(8), 0);
                    break;
                case TypeCode.Single:
                    ret = BitConverter.ToSingle(ReadBytes(4), 0);
                    break;
                case TypeCode.Double:
                    ret = BitConverter.ToDouble(ReadBytes(4), 0);
                    break;
                case TypeCode.Byte:
                    ret = ReadBytes(1)[0];
                    break;
                default:
                    throw new NotSupportedException(typeof(T).FullName + " is not currently supported by Read<T>");
            }
            return (T)ret;
        }

        public byte[] ReadBytes(int count)
        {
            return ReadBytes(Position, count);
        }

        public byte[] ReadBytes(int position, int count)
        {
            byte[] buf = new byte[count];
            int read;
            Seek(position, SeekOrigin.Begin);
            SFileReadFile(Handle, buf, count, out read, IntPtr.Zero);
            Seek(position + count, SeekOrigin.Begin);
            return buf;
        }

        public int Seek(int position, SeekOrigin seekOrigin)
        {
            int pos_h = 0;
            switch (seekOrigin)
            {
                case SeekOrigin.Begin:
                    Position = SFileSetFilePointer(Handle, position, out pos_h, 0);
                    break;
                case SeekOrigin.Current:
                    Position = SFileSetFilePointer(Handle, position, out pos_h, 1);
                    break;
                case SeekOrigin.End:
                    Position = SFileSetFilePointer(Handle, position, out pos_h, 2);
                    break;
            }      

            if (Position < 0) Position = 0;
            //if (Position > Length) Position = Length;

            return Position;
        }

        [DllImport("StormLib.dll")]
        static extern bool SFileReadFile(IntPtr handle, [MarshalAs(UnmanagedType.LPArray)] byte[] buffer, int count, out int read, IntPtr io);

        [DllImport("StormLib.dll")]
        static extern int SFileSetFilePointer(IntPtr handle, int pos, out int pos_h, int move_method);
    }
}
