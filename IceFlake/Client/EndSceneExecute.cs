using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using IceFlake.DirectX;

namespace IceFlake.Client
{
    public class EndSceneExecute : IPulsable
    {
        private Queue<Action> ExecutionQueue;

        public EndSceneExecute()
        {
            ExecutionQueue = new Queue<Action>();
        }

        public void AddExececution(Action action, bool postpone = false)
        {
            // If we're already in the main thread we're also in the EndScene hook which means we can run the command without any problems
            // sometimes this is however not desired (maybe this needs to happen the next frame) so we check if we want it postponed
            if (postpone && Thread.CurrentThread.ManagedThreadId == Process.GetCurrentProcess().Threads[0].Id)
                action.Invoke();
            else
                ExecutionQueue.Enqueue(action);
        }

        public void Direct3D_EndScene()
        {
            if (ExecutionQueue == null)
                return;

            if (ExecutionQueue.Count == 0)
                return;

            var action = ExecutionQueue.Dequeue();
            action.Invoke();
        }
    }
}
