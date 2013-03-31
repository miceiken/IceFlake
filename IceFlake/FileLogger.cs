using System;
using System.IO;

namespace IceFlake
{
    public class FileLogger : ILog
    {
        private readonly StreamWriter sw;

        public FileLogger()
        {
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs"));
            string datestamp = DateTime.Now.ToString("dd.MM HH.mm.ss");
            sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", datestamp + ".txt"));
            sw.AutoFlush = true;

            Log.AddReader(this);
        }

        public static FileLogger Instance { get; set; }

        #region ILog Members

        public void WriteLine(LogEntry entry)
        {
            sw.WriteLine("[" + entry.Type.ToString().ToUpper() + "] " + entry.FormattedMessage);
        }

        #endregion
    }
}
