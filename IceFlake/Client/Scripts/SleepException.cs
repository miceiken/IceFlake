using System;

namespace IceFlake.Client.Scripts
{
    public class SleepException : Exception
    {
        public SleepException(int ms)
            : base()
        {
            Time = TimeSpan.FromMilliseconds(ms);
        }

        public SleepException(TimeSpan time)
            : base()
        {
            Time = time;
        }

        public TimeSpan Time
        {
            get;
            private set;
        }
    }
}
