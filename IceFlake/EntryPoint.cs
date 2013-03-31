using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace IceFlake
{
    static class EntryPoint
    {
        [STAThread]
        public static void Main(string args)
        {
            FileLogger.Instance = new FileLogger();
            Log.WriteLine("Initializing IceFlake");
            Core.Initialize();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new IceForm());
        }
    }
}
