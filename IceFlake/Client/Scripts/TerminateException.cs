using System;

namespace IceFlake.Client.Scripts
{
    public class TerminateException : Exception
    {
        public TerminateException()
        {
        }

        public TerminateException(string reason)
        {
        }

        public string Reason { get; private set; }
    }
}