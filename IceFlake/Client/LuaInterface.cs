using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Core.Client.Patchables;

namespace Core.Client
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
            get { return WoWCore.Memory.Read<IntPtr>((IntPtr) Pointers.LuaInterface.LuaState, true); }
        }

        public static void Pop(IntPtr state, int n)
        {
            SetTop(state, -(n) - 1);
        }

        public static void Initialize()
        {
            GetTop = WoWCore.Memory.RegisterDelegate<LuaGetTopDelegate>((IntPtr) Pointers.LuaInterface.LuaGetTop, true);
            SetTop = WoWCore.Memory.RegisterDelegate<LuaSetTopDelegate>((IntPtr) Pointers.LuaInterface.LuaSetTop, true);
            Type = WoWCore.Memory.RegisterDelegate<LuaTypeDelegate>((IntPtr) Pointers.LuaInterface.LuaType, true);
            ToLString =
                WoWCore.Memory.RegisterDelegate<LuaToLStringDelegate>((IntPtr) Pointers.LuaInterface.LuaToLString, true);
            ToBoolean =
                WoWCore.Memory.RegisterDelegate<LuaToBooleanDelegate>((IntPtr) Pointers.LuaInterface.LuaToBoolean, true);
            ToNumber = WoWCore.Memory.RegisterDelegate<LuaToNumberDelegate>((IntPtr) Pointers.LuaInterface.LuaToNumber,
                                                                            true);
            PCall = WoWCore.Memory.RegisterDelegate<LuaPCallDelegate>((IntPtr) Pointers.LuaInterface.LuaPCall, true);
            LoadBuffer =
                WoWCore.Memory.RegisterDelegate<LuaLoadBufferDelegate>((IntPtr) Pointers.LuaInterface.LuaLoadBuffer,
                                                                       true);
        }

        public static string StackObjectToString(IntPtr state, int index)
        {
            var ltype = (LuaConstant) Type(state, index);

            switch (ltype)
            {
                case LuaConstant.TypeNil:
                    return "nil";

                case LuaConstant.TypeBoolean:
                    return ToBoolean(state, index) > 0 ? "true" : "false";

                case LuaConstant.TypeNumber:
                    return ToNumber(state, index).ToString(CultureInfo.InvariantCulture);

                case LuaConstant.TypeString:
                    return WoWCore.Memory.ReadString(ToLString(state, index, 0));

                default:
                    return "<unknown lua type>";
            }
        }
    }
}