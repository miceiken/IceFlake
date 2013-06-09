using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using IceFlake.DirectX;

namespace IceFlake
{
    public static class EntryPoint
    {
        internal static IceForm AppForm
        {
            get;
            set;
        }

        [STAThread]
        public static void Main(string args)
        {
            FileLogger.Instance = new FileLogger();
            Log.WriteLine("Initializing IceFlake");
            Application.ApplicationExit += OnApplicationExit;
            AppDomain.CurrentDomain.UnhandledException += LogUnhandledException;
            Manager.Initialize();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(AppForm = new IceForm());
        }

        private static void OnApplicationExit(object sender, EventArgs e)
        {
            if (AppForm != null)
                Log.RemoveReader(AppForm);
            Log.WriteLine("Shutting down IceFlake");
            Direct3D.Shutdown();
        }

        private static void LogUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.WriteLine(LogType.Error, "Unhandled exception:");
            var ex = e.ExceptionObject as Exception;
            do
            {
                Log.WriteLine(LogType.Error, "\t{0}", ex.Message);
                ex = ex.InnerException;
            } while (ex.InnerException != null);
        }
    }
}
