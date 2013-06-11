using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.DirectX;

namespace IceFlake.Client
{
    public class EndSceneExecute
    {
        private Queue<Action> ExecutionQueue;

        public EndSceneExecute()
        {
            ExecutionQueue = new Queue<Action>();
        }

        public void AddExececution(Action action)
        {
            ExecutionQueue.Enqueue(action);
        }

        [EndSceneHandler]
        private void Direct3D_EndScene()
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
