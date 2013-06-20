using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.Runtime;

namespace IceFlake.Client.Routines
{
    public abstract class RoutineAction
    {
        public RoutineAction(RoutineBrain brain, int priority)
        {
            this.Brain = brain;
            this.Priority = priority;
        }

        public RoutineBrain Brain
        {
            get;
            private set;
        }

        public virtual int Priority
        {
            get;
            private set;
        }

        public virtual bool IsWanted
        {
            get { return true; }
        }

        public virtual bool IsReady
        {
            get { return true; }
        }

        public abstract void Execute();

        public void Sleep(int ms)
        {
            throw new SleepException(ms);
        }

        public void Print(string text, params object[] args)
        {
            Log.WriteLine(text, args);
        }
    }
}
