using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IceFlake.Runtime
{
    public class CallbackManager<T>
    {
        private readonly List<T> _callbacks;

        public CallbackManager()
        {
            _callbacks = new List<T>();
        }

        public void Register(T callback)
        {
            _callbacks.Add(callback);
        }

        public void Remove(T callback)
        {
            _callbacks.Remove(callback);
        }

        public void Invoke(params object[] args)
        {
            foreach (T callback in _callbacks)
                ((Delegate)(object)callback).DynamicInvoke(args);
        }
    }
}
