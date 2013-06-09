using System;

namespace IceFlake.Client.Scripts
{
    public class ScriptThread
    {
        public ScriptThread(Action action)
        {
            IsAlive = true;
            Action = action;
        }

        public bool IsAlive
        {
            get;
            private set;
        }

        public string ExitReason
        {
            get;
            private set;
        }

        private Action Action
        {
            get;
            set;
        }

        private DateTime SleepTime
        {
            get;
            set;
        }

        internal void Tick()
        {
            if (SleepTime >= DateTime.Now)
                return;

            if (IsAlive)
            {
                try
                { Action(); }
                catch (SleepException ex)
                { SleepTime = DateTime.Now + ex.Time; }
                catch (TerminateException ex)
                {
                    IsAlive = false;
                    ExitReason = ex.Reason;
                }
            }
        }
    }
}
