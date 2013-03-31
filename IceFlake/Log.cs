using System;
using System.Collections.Generic;

namespace IceFlake
{
    public static class Log
    {
        private static readonly LinkedList<ILog> LogReaders = new LinkedList<ILog>();
        private static readonly LinkedList<LogEntry> LogContent = new LinkedList<LogEntry>();

        static Log()
        {
        }

        public static void WriteLine(string text, params object[] args)
        {
            WriteLine(LogType.Normal, text, args);
        }

        public static void WriteLine(LogType type, string text, params object[] args)
        {
            var entry = new LogEntry(type, DateTime.Now, string.Format(text, args));
            LogContent.AddLast(entry);

            foreach (ILog LogReader in LogReaders)
                LogReader.WriteLine(entry);
        }

        public static void AddReader(ILog LogReader)
        {
            LogReaders.AddLast(LogReader);
            foreach (LogEntry LogLines in LogContent)
                LogReader.WriteLine(LogLines);
        }

        public static void RemoveReader(ILog LogReader)
        {
            LogReaders.Remove(LogReader);
        }
    }

    public interface ILog
    {
        void WriteLine(LogEntry entry);
    }

    public class LogEntry
    {
        public string Message = string.Empty;
        public DateTime Time = DateTime.Now;
        public LogType Type = LogType.Normal;

        public LogEntry(LogType type, DateTime time, string text)
        {
            Type = type;
            Time = time;
            Message = text;
        }

        public LogEntry(LogType type, string text, params object[] args)
        {
            Type = type;
            Time = DateTime.Now;
            Message = string.Format(text, args);
        }

        public string FormattedMessage
        {
            get { return string.Format("[{0}] {1}", Time.ToString("HH:mm:ss"), Message); }
        }
    }

    public enum LogType
    {
        Error,
        Warning,
        Normal,
        Information,
        Good,
    }
}
