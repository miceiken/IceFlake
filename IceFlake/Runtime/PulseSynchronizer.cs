using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace IceFlake.Runtime
{
    public class PulseSynchronizer
    {
        public PulseSynchronizer()
        {
            m_awaiters = new LinkedList<PulseAwaiter>();
            m_context = new SingleThreadSynchronizationContext();
        }

        private LinkedList<PulseAwaiter> m_awaiters;
        private SingleThreadSynchronizationContext m_context;

        public void Register(PulseAwaiter awaiter)
        {
            m_awaiters.AddLast(awaiter);
        }

        public void Execute(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("Action");

            var synchronizer = PulseSynchronizer.Current;
            try
            {
                s_current = this;

                var context = SynchronizationContext.Current;
                try
                {
                    SynchronizationContext.SetSynchronizationContext(m_context);

                    action();
                }
                finally
                {
                    SynchronizationContext.SetSynchronizationContext(context);
                }
            }
            finally
            {
                s_current = synchronizer;
            }
        }

        public void Run()
        {
            Execute(() =>
            {
                // check all pending awaiters
                var node = m_awaiters.Last;
                while (node != null)
                {
                    var nextNode = node.Previous;

                    var completed = node.Value.CheckCompletition();
                    if (completed)
                        node.List.Remove(node);

                    node = nextNode;
                }

                // run continuations
                m_context.Run();
            });
        }

        #region Static members

        [ThreadStatic]
        private static PulseSynchronizer s_current = null;

        public static PulseSynchronizer Current { get { return s_current; } }

        #endregion

        #region Nested type: SingleThreadSynchronizationContext

        public class SingleThreadSynchronizationContext : SynchronizationContext
        {
            public SingleThreadSynchronizationContext()
            {
                m_queue = new Queue<Tuple<SendOrPostCallback, object>>();
                m_nextQueue = new Queue<Tuple<SendOrPostCallback, object>>();
            }

            private Queue<Tuple<SendOrPostCallback, object>> m_queue;
            private Queue<Tuple<SendOrPostCallback, object>> m_nextQueue;

            public override void Post(SendOrPostCallback callback, object state)
            {
                if (callback == null)
                    throw new ArgumentNullException("callback");

                m_queue.Enqueue(new Tuple<SendOrPostCallback, object>(callback, state));
            }

            public override void Send(SendOrPostCallback callback, object state)
            {
                throw new NotSupportedException("Synchronous operations are not supported.");
            }

            public void Run()
            {
                var tmp = m_nextQueue;
                m_nextQueue = m_queue;
                m_queue = tmp;

                while (m_nextQueue.Count > 0)
                {
                    var item = m_nextQueue.Dequeue();

                    item.Item1(item.Item2);
                }
            }
        }

        #endregion
    }

    public class PulseAwaiter : INotifyCompletion
    {
        public PulseAwaiter(Func<bool> hasCompleted)
        {
            if (hasCompleted == null)
                throw new ArgumentNullException("hasCompleted");

            m_hasCompleted = hasCompleted;

            PulseSynchronizer.Current.Register(this);
        }

        private Func<bool> m_hasCompleted;
        private Task m_continuation;

        public bool CheckCompletition()
        {
            if (IsCompleted)
                return true;

            IsCompleted = m_hasCompleted();

            if (IsCompleted && m_continuation != null)
                m_continuation.Start(TaskScheduler.FromCurrentSynchronizationContext());

            return IsCompleted;
        }

        public bool IsCompleted
        {
            get;
            private set;
        }

        public void GetResult()
        {
        }

        public void OnCompleted(Action continuation)
        {
            m_continuation = new Task(continuation);
        }
    }

    public class Tasklet
    {
        public Tasklet(string name)
        {
            Name = name;

            m_lives = s_random.Next(2, 10);
        }

        private int m_lives;

        public string Name { get; private set; }

        public bool IsAlive
        {
            get
            {
                Log.WriteLine("{0} - remaining lives {1}", Name, m_lives);

                // simulate lifetime
                return m_lives-- > 0;
            }
        }

        public PulseAwaiter GetAwaiter()
        {
            return new PulseAwaiter(() => !IsAlive);
        }

        #region Static members

        private static Random s_random = new Random();

        #endregion
    }
}