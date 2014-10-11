using System;

namespace IceFlake.Runtime
{
    public class SleepException : Exception
    {
        public SleepException(int ms)
        {
            Time = TimeSpan.FromMilliseconds(ms);
        }

        public SleepException(TimeSpan time)
        {
            Time = time;
        }

        public TimeSpan Time { get; private set; }
    }
}