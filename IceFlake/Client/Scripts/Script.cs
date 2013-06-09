using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IceFlake.Client.Scripts
{
    public abstract class Script
    {
        public Script(string name, string category)
        {
            Name = name;
            Category = category;
            IsRunning = false;
            ThreadPool = new List<ScriptThread>();            
            MainThread = null;
        }

        public string Name
        {
            get;
            private set;
        }

        public string Category
        {
            get;
            private set;
        }

        public bool IsRunning
        {
            get;
            private set;
        }

        public List<ScriptThread> ThreadPool
        {
            get;
            private set;
        }

        private ScriptThread MainThread
        {
            get;
            set;
        }

        protected ScriptThread StartThread(Action action)
        {
            var thread = new ScriptThread(action);
            ThreadPool.Add(thread);
            return thread;
        }

        protected void StopThread(string reason = null)
        {
            throw new TerminateException(reason);
        }

        public void Start()
        {
            IsRunning = true;
        }

        private void StartInternal()
        {
            Print("Script starting");
            OnStart();            
            MainThread = new ScriptThread(new Action(OnTick));          
            OnStarted();
        }

        public void Stop()
        {
            IsRunning = false;
        }

        private void TerminateInternal(string message = "Script terminated")
        {
            OnTerminate();
            IsRunning = false;
            MainThread = null;
            ThreadPool.Clear();
            Print(message);
            OnTerminated();            
        }

        internal void Tick()
        {
            try
            {
                if (IsRunning && MainThread == null)
                    StartInternal();

                if (IsRunning)
                {
                    MainThread.Tick();

                    foreach (var thread in ThreadPool)
                        thread.Tick();

                    ThreadPool.Where(t => !t.IsAlive).ToList().ForEach(t => ThreadPool.Remove(t));
                }

                if (!IsRunning && MainThread != null)
                    TerminateInternal();

                if (MainThread != null && !MainThread.IsAlive)
                    TerminateInternal(MainThread.ExitReason ?? "Script finished");
            }
            catch (Exception ex)
            {
                TerminateInternal("Error: " + ex.ToString());
            }
        }

        public void Print(string message, params object[] args)
        {
            Log.WriteLine("[{0}] {1}", Name, string.Format(message, args));
        }

        protected void Sleep(int ms)
        {
            throw new SleepException(ms);
        }

        public virtual void OnStart() { }
        public virtual void OnTick() { Stop(); }
        public virtual void OnTerminate() { }

        public event EventHandler OnStartedEvent;
        private void OnStarted()
        {
            if (OnStartedEvent != null)
                OnStartedEvent(this, new EventArgs());
        }

        public event EventHandler OnStoppedEvent;
        private void OnTerminated()
        {
            if (OnStoppedEvent != null)
                OnStoppedEvent(this, new EventArgs());
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1}", Category, Name);
        }
    }
}
