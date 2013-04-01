using System;
using System.Globalization;
using System.Runtime.InteropServices;
using IceFlake.Client.Patchables;

namespace IceFlake.Client
{
    internal static class LuaInterface
    {
        #region Delegates

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int LuaGetTopDelegate(IntPtr luaState);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate int LuaLoadBufferDelegate(
            IntPtr luaState, IntPtr buffer, int bufferLength,
            [MarshalAs(UnmanagedType.LPStr)] string chunkName);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int LuaPCallDelegate(IntPtr luaState, int nargs, int nresults, int errfunc);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void LuaSetTopDelegate(IntPtr luaState, int index);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int LuaToBooleanDelegate(IntPtr luaState, int index);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr LuaToLStringDelegate(IntPtr luaState, int index, int zero);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate double LuaToNumberDelegate(IntPtr luaState, int index);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int LuaTypeDelegate(IntPtr luaState, int index);

        #endregion

        #region LuaConstant enum

        public enum LuaConstant
        {
            MultRet = -1,
            TypeNil = 0,
            TypeBoolean = 1,
            TypeNumber = 3,
            TypeString = 4
        }

        #endregion

        public static LuaGetTopDelegate GetTop;
        public static LuaSetTopDelegate SetTop;

        public static LuaTypeDelegate Type;

        public static LuaToLStringDelegate ToLString;

        public static LuaToBooleanDelegate ToBoolean;

        public static LuaToNumberDelegate ToNumber;

        public static LuaPCallDelegate PCall;

        public static LuaLoadBufferDelegate LoadBuffer;

        public static IntPtr LuaState
        {
            get { return Manager.Memory.Read<IntPtr>((IntPtr)Pointers.LuaInterface.LuaState); }
        }

        public static void Pop(IntPtr state, int n)
        {
            SetTop(state, -(n) - 1);
        }

        public static void Initialize()
        {
            GetTop = Manager.Memory.RegisterDelegate<LuaGetTopDelegate>((IntPtr)Pointers.LuaInterface.LuaGetTop);
            SetTop = Manager.Memory.RegisterDelegate<LuaSetTopDelegate>((IntPtr)Pointers.LuaInterface.LuaSetTop);
            Type = Manager.Memory.RegisterDelegate<LuaTypeDelegate>((IntPtr)Pointers.LuaInterface.LuaType);
            ToLString =
                Manager.Memory.RegisterDelegate<LuaToLStringDelegate>((IntPtr)Pointers.LuaInterface.LuaToLString);
            ToBoolean =
                Manager.Memory.RegisterDelegate<LuaToBooleanDelegate>((IntPtr)Pointers.LuaInterface.LuaToBoolean);
            ToNumber = Manager.Memory.RegisterDelegate<LuaToNumberDelegate>((IntPtr)Pointers.LuaInterface.LuaToNumber);
            PCall = Manager.Memory.RegisterDelegate<LuaPCallDelegate>((IntPtr)Pointers.LuaInterface.LuaPCall);
            LoadBuffer =
                Manager.Memory.RegisterDelegate<LuaLoadBufferDelegate>((IntPtr)Pointers.LuaInterface.LuaLoadBuffer);
        }

        public static string StackObjectToString(IntPtr state, int index)
        {
            var ltype = (LuaConstant)Type(state, index);

            switch (ltype)
            {
                case LuaConstant.TypeNil:
                    return "nil";

                case LuaConstant.TypeBoolean:
                    return ToBoolean(state, index) > 0 ? "true" : "false";

                case LuaConstant.TypeNumber:
                    return ToNumber(state, index).ToString(CultureInfo.InvariantCulture);

                case LuaConstant.TypeString:
                    return Manager.Memory.ReadString(ToLString(state, index, 0));

                default:
                    return "<unknown lua type>";
            }
        }
    }
}