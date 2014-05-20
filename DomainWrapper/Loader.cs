using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using System.Linq;
using DllExporter;

namespace DomainWrapper
{
    public static class Loader
    {
        const string APP_NAME = "IceFlake";

        //public static unsafe int Start(IntPtr* pPath)
        [DllExport]
        [STAThread]
        public static void Host([MarshalAs(UnmanagedType.LPWStr)]string loadDir)
        {
#if LAUNCH_MDA
            System.Diagnostics.Debugger.Launch();
#endif
            loadDir = Path.GetDirectoryName(loadDir);
            Trace.Assert(Directory.Exists(loadDir));

            Trace.Listeners.Add(new TextWriterTraceListener(Path.Combine(loadDir, @"Logs\", APP_NAME + ".Loader.log")));

            try
            {
                using (var host = new PathedDomainHost(APP_NAME, loadDir))
                {
                    host.Execute();
                }
                //var asm_bytes = File.ReadAllBytes(_dllPath);

                //var args = new Object[] {
                //    new String[] {
                //        path
                //    }
                //};

                //var asm = Assembly.Load(asm_bytes);
                //return Convert.ToInt32(asm.EntryPoint.Invoke(null, args));
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
            }
        }
    }
}