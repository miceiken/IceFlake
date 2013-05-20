using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace PatherPath
{
    public class Logger
    {
        private static StreamWriter Handle;
        public static void Log(string message)
        {
            Graph.PathGraph.LogDelegate.Invoke(message);
        }
        public static void Debug(string message)
        {
            if (Handle == null) Handle = File.AppendText("pathlog.txt");
            Handle.WriteLine(message);
        }
    }
}
