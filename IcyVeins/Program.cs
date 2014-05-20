using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.ComponentModel;

namespace IcyVeins
{
    class Program
    {
        const string DefaultTarget = "Wow", DefaultPayload = "DomainWrapper.dll", DefaultExport = "Host";

        static Dictionary<string, string> ParseArgs(string[] args)
        {
            Func<string, bool> isOption = x => x.Length >= 3 && x.Substring(0, 2).Equals("--");

            var opts = args.Select(a => a.ToUpperInvariant());
            var optionKeys = (from opt in opts where isOption(opt) select opt.Substring(2).ToUpperInvariant());
            var optionPairs = (from key in optionKeys select key).
                Select(k => new KeyValuePair<string, string>(k, new Func<string>(() =>
                {
                    var next = opts.SkipWhile(o => !o.Equals("--" + k)).Skip(1).FirstOrDefault();
                    return isOption(next) ? null : next;
                })()));

            return optionPairs.ToDictionary(p => p.Key, p => p.Value);
        }

        static void Main(string[] args)
        {
            try
            {
                Process.EnterDebugMode();

                var options = ParseArgs(args);

                string target, payload;
                if (!options.TryGetValue("TARGET", out target)) target = DefaultTarget;

                if (!options.TryGetValue("PAYLOAD", out payload)) payload = DefaultPayload;
                payload = Path.GetFullPath(payload);

                var targetProcs = Process.GetProcessesByName(target);
                if (targetProcs.Length == 0)
                    throw new ArgumentException("No processes with name " + target + " were found.");

                Process targetProc = null;

                string pidInput;
                if (options.TryGetValue("PID", out pidInput))
                {
                    switch (pidInput)
                    {
                        case "FIRST":
                            targetProc = targetProcs.FirstOrDefault();
                            break;
                        case "LAST":
                            targetProc = targetProcs.LastOrDefault();
                            break;
                        default:
                            int PID = Convert.ToInt32(pidInput);
                            targetProc = targetProcs.Where(p => p.Id == PID).FirstOrDefault();
                            break;
                    }
                }

                if (targetProc == null)
                    throw new ArgumentException("A process with name " + target + " and PID " + pidInput + " was not found.");

                var handle = Imports.OpenProcess(
                    ProcessAccessFlags.QueryInformation | ProcessAccessFlags.CreateThread |
                    ProcessAccessFlags.VMOperation | ProcessAccessFlags.VMWrite |
                    ProcessAccessFlags.VMRead, false, targetProc.Id);
                if (handle == IntPtr.Zero)
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                IntPtr pLibBase = IntPtr.Zero;
                try
                {
                    pLibBase = Inject(handle, targetProc, payload);
                }
                finally
                {
                    if (pLibBase != IntPtr.Zero)
                        Eject(handle, pLibBase);
                }
            }
            finally
            {
                Process.LeaveDebugMode();
            }
        }

        static IntPtr Inject(IntPtr hTarget, Process targetProc, string payload, bool final = false)
        {
            if (!File.Exists(payload))
                throw new FileNotFoundException("Can't find " + payload + " to inject");

            var payloadName = Path.GetFileName(payload);
            var libPathSize = (uint)Encoding.Unicode.GetByteCount(payload);

            IntPtr
                pLibPath = Marshal.StringToHGlobalUni(payload),
                pExternLibPath = IntPtr.Zero;
            try
            {
                var hKernel = Imports.GetModuleHandle("kernel32");
                var pLoadLib = Imports.GetProcAddress(hKernel, "LoadLibraryW");

                pExternLibPath = Imports.VirtualAllocEx(hTarget, IntPtr.Zero, libPathSize, AllocationType.Commit, MemoryProtection.ReadWrite);

                int bytesWritten;
                Imports.WriteProcessMemory(hTarget, pExternLibPath, pLibPath, libPathSize, out bytesWritten);

                IntPtr
                    pLibBase,
                    hMod = CRTWithWait(hTarget, pLoadLib, pExternLibPath);
                if (hMod == IntPtr.Zero)
                    pLibBase = (from ProcessModule module in targetProc.Modules
                                where module.ModuleName.Equals(payloadName)
                                select module)
                    .Single().BaseAddress;
                else
                    pLibBase = hMod;

                var oHost = FindExportRVA(payload, DefaultExport).ToInt32();
                CRTWithWait(hTarget, pLibBase + oHost, pExternLibPath);

                return pLibBase;
            }
            finally
            {
                Marshal.FreeHGlobal(pLibPath);
                Imports.VirtualFreeEx(hTarget, pExternLibPath, 0, AllocationType.Release);
            }
        }

        static void Eject(IntPtr hTarget, IntPtr pLibBase)
        {
            var hKernel = Imports.GetModuleHandle("kernel32");
            var pFreeLib = Imports.GetProcAddress(hKernel, "FreeLibrary");
            CRTWithWait(hTarget, pFreeLib, pLibBase);
        }

        static IntPtr CRTWithWait(IntPtr handle, IntPtr pTarget, IntPtr pParam)
        {
            var hThread = IntPtr.Zero;
            try
            {
                hThread = Imports.CreateRemoteThread(handle, IntPtr.Zero, 0, pTarget, pParam, 0, IntPtr.Zero);
                if (Imports.WaitForSingleObject(hThread, (uint)ThreadWaitValue.Infinite) != (uint)ThreadWaitValue.Object0)
                    return IntPtr.Zero;
                //throw new Win32Exception(Marshal.GetLastWin32Error());

                IntPtr hLibModule;
                if (!Imports.GetExitCodeThread(hThread, out hLibModule))
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                return hLibModule;
            }
            finally
            {
                Imports.CloseHandle(hThread);
            }
        }

        static IntPtr FindExportRVA(string payload, string export)
        {
            var hModule = IntPtr.Zero;
            try
            {
                hModule = Imports.LoadLibraryEx(payload, IntPtr.Zero, LoadLibraryExFlags.DontResolveDllReferences);
                if (hModule == IntPtr.Zero)
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                var pFunc = Imports.GetProcAddress(hModule, export);
                if (pFunc == IntPtr.Zero)
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                return pFunc - hModule.ToInt32();
            }
            finally
            {
                try
                {
                    Imports.CloseHandle(hModule);
                }
                catch (SEHException)
                {
                    //expected. http://stackoverflow.com/questions/9867334/why-is-the-handling-of-exceptions-from-closehandle-different-between-net-4-and
                }
            }
        }
    }
}
