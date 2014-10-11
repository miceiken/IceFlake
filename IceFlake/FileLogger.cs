using System;
using System.IO;

namespace IceFlake
{
    public sealed class FileLogger : ILog, IDisposable
    {
        private readonly StreamWriter sw;

        public FileLogger()
        {
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs"));
            sw =
                new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs",
                    string.Format("log-{0:yyyy-MM-dd_hh-mm-ss}.txt", DateTime.Now)));
            sw.AutoFlush = true;

            Log.AddReader(this);
        }

        public static FileLogger Instance { get; set; }

        #region ILog Members

        public void WriteLine(LogEntry entry)
        {
            sw.WriteLine("[" + entry.Type.ToString().ToUpper()[0] + "] " + entry.FormattedMessage);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            sw.Flush();
            sw.Close();
        }

        #endregion
    }
}