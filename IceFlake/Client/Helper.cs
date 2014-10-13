using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using IceFlake.Client.Patchables;
using IceFlake.Runtime;

namespace IceFlake.Client
{
    public static class Helper
    {
        private static PerformanceCounterDelegate _performanceCounter;

        public static uint PerformanceCount
        {
            get
            {
                if (_performanceCounter == null)
                    _performanceCounter =
                        Manager.Memory.RegisterDelegate<PerformanceCounterDelegate>(
                            (IntPtr)Pointers.Other.PerformanceCounter);

                return _performanceCounter();
            }
        }

        public static void Initialize()
        {
            Manager.Events.Register("PLAYER_REGEN_DISABLED", SetInCombat);
            Manager.Events.Register("PLAYER_REGEN_ENABLED", UnsetInCombat);
        }

        public static void ResetHardwareAction()
        {
            Manager.Memory.Write(Pointers.Other.LastHardwareAction.ToPointer(), PerformanceCount);
        }

        private static void SetInCombat(string ev, List<string> args)
        {
            InCombat = true;
        }

        private static void UnsetInCombat(string ev, List<string> args)
        {
            InCombat = false;
        }

        public static string ToWowCurrency(this ulong amount)
        {
            return String.Format("{0}g {1}s {2}c", amount / 10000, amount / 100 % 100, amount % 100);
        }

        // https://github.com/tomrus88/WowAddin/blob/master/WowAddin/dllmain.cpp#L34
        // Fix InvalidPtrCheck for callbacks outside of .text section
        public static void FixInvalidPtrCheck()
        {
            Manager.Memory.Write<uint>(Pointers.Console.InvalidPtrCheck.ToPointer(), 0x00000001);
            Manager.Memory.Write<uint>(Pointers.Console.InvalidPtrCheck.ToPointer() + 0x4, 0x7FFFFFFF);
        }

        #region Properties

        public static bool InCombat { get; private set; }

        #endregion

        #region Typedefs & Delegates

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint PerformanceCounterDelegate();

        #endregion
    }
}