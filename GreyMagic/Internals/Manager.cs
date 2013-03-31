using System;
using System.Collections.Generic;

namespace GreyMagic.Internals
{
    /// <summary>
    /// Represents an operation in memory, be it a patch, detour, or anything else.
    /// </summary>
    public interface IMemoryOperation : IDisposable
    {
        /// <summary>
        /// Returns true if this IMemoryOperation is currently applied.
        /// </summary>
        bool IsApplied { get; }

        /// <summary>
        /// Returns the name for this IMemoryOperation.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Removes this IMemoryOperation from memory. (Reverts the bytes back to their originals.)
        /// </summary>
        /// <returns></returns>
        bool Remove();

        /// <summary>
        /// Applies this IMemoryOperation to memory. (Writes new bytes to memory)
        /// </summary>
        /// <returns></returns>
        bool Apply();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Manager<T> where T : IMemoryOperation
    {
        private readonly MemoryBase _memory;
        protected internal Dictionary<string, T> Applications = new Dictionary<string, T>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Manager&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="memory">The memory.</param>
        /// <remarks>
        /// Created 2012-01-16 17:43 by Nesox.
        /// </remarks>
        internal Manager(MemoryBase memory)
        {
            _memory = memory;
        }

        /// <summary>
        /// Gets the win32.
        /// </summary>
        /// <remarks>Created 2012-01-16 17:43 by Nesox.</remarks>
        internal MemoryBase Memory
        {
            get { return _memory; }
        }

        /// <summary>
        /// Retrieves an IMemoryOperation by name.
        /// </summary>
        /// <param name="name">The name given to the IMemoryOperation</param>
        /// <returns></returns>
        public virtual T this[string name]
        {
            get { return Applications[name]; }
        }

        /// <summary>
        /// Applies all the IMemoryOperations contained in this manager via their Apply() method.
        /// </summary>
        public virtual void ApplyAll()
        {
            foreach (var dictionary in Applications)
            {
                dictionary.Value.Apply();
            }
        }

        /// <summary>
        /// Removes all the IMemoryOperations contained in this manager via their Remove() method.
        /// </summary>
        public virtual void RemoveAll()
        {
            foreach (var dictionary in Applications)
            {
                dictionary.Value.Remove();
            }
        }

        /// <summary>
        /// Deletes all the IMemoryOperations contained in this manager.
        /// </summary>
        public virtual void DeleteAll()
        {
            foreach (var dictionary in Applications)
            {
                dictionary.Value.Dispose();
            }
            Applications.Clear();
        }

        /// <summary>
        /// Deletes a specific IMemoryOperation contained in this manager, by name.
        /// </summary>
        /// <param name="name"></param>
        public virtual void Delete(string name)
        {
            if (Applications.ContainsKey(name))
            {
                Applications[name].Dispose();
                Applications.Remove(name);
            }
        }
    }
}