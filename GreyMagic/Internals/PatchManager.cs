using System;
using System.Linq;

namespace GreyMagic.Internals
{
    /// <summary>
    /// A manager class to handle memory patches.
    /// </summary>
    public class PatchManager : Manager<Patch>
    {
        internal PatchManager(MemoryBase memory) : base(memory)
        {
        }

        /// <summary>
        /// Applies all enabled patches in this manager via their Apply() method.
        /// </summary>
        public override void ApplyAll()
        {
            foreach (Patch patch in Applications.Values)
            {
                if (patch.Enabled && !patch.IsApplied)
                    patch.Apply();
            }
        }

        /// <summary>
        /// Removes all the IMemoryOperations contained in this manager via their Remove() method.
        /// </summary>
        public override void RemoveAll()
        {
            foreach (Patch patch in Applications.Values)
            {
                if (patch.IsApplied)
                    patch.Remove();
            }
        }

        /// <summary>
        /// Creates a new <see cref="Patch"/> at the specified address.
        /// </summary>
        /// <param name="address">The address to begin the patch.</param>
        /// <param name="patchWith">The bytes to be written as the patch.</param>
        /// <param name="name">The name of the patch.</param>
        /// <returns>A patch object that exposes the required methods to apply and remove the patch.</returns>
        public Patch Create(IntPtr address, byte[] patchWith, string name)
        {
            if (!Applications.ContainsKey(name))
            {
                var p = new Patch(address, patchWith, name, Memory);
                Applications.Add(name, p);
                return p;
            }
            return Applications[name];
        }

        /// <summary>
        /// Creates a new <see cref="Patch"/> at the specified address, and applies it.
        /// </summary>
        /// <param name="address">The address to begin the patch.</param>
        /// <param name="patchWith">The bytes to be written as the patch.</param>
        /// <param name="name">The name of the patch.</param>
        /// <returns>A patch object that exposes the required methods to apply and remove the patch.</returns>
        public Patch CreateAndApply(IntPtr address, byte[] patchWith, string name)
        {
            Patch p = Create(address, patchWith, name);
            if (p != null)
                p.Apply();

            return p;
        }
    }

    /// <summary>
    /// Contains methods, and information for a memory patch.
    /// </summary>
    public class Patch : IMemoryOperation
    {
        private readonly IntPtr _address;
        private readonly MemoryBase _memory;
        private readonly byte[] _originalBytes;
        private readonly byte[] _patchBytes;

        internal Patch(IntPtr address, byte[] patchWith, string name, MemoryBase memory)
        {
            Name = name;
            _memory = memory;
            _address = address;
            _patchBytes = patchWith;
            _originalBytes = _memory.ReadBytes(address, patchWith.Length);
        }

        public bool Enabled { get; set; }

        #region IMemoryOperation Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (IsApplied)
                Remove();

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Removes this Patch from memory. (Reverts the bytes back to their originals.)
        /// </summary>
        /// <returns></returns>
        public bool Remove()
        {
            try
            {
                _memory.WriteBytes(_address, _originalBytes);
            }
            catch
            {
            }

            return false;
        }

        /// <summary>
        /// Applies this Patch to memory. (Writes new bytes to memory)
        /// </summary>
        /// <returns></returns>
        public bool Apply()
        {
            try
            {
                _memory.WriteBytes(_address, _patchBytes);
                return true;
            }
            catch
            {
            }

            return false;
        }

        /// <summary>
        /// Returns true if this Patch is currently applied.
        /// </summary>
        public bool IsApplied
        {
            get { return _memory.ReadBytes(_address, _patchBytes.Length).SequenceEqual(_patchBytes); }
        }

        /// <summary>
        /// Returns the name for this Patch.
        /// </summary>
        public string Name { get; private set; }

        #endregion

        /// <summary>
        /// Allows an <see cref="T:System.Object"/> to attempt to free resources and perform other cleanup operations before the <see cref="T:System.Object"/> is reclaimed by garbage collection.
        /// </summary>
        ~Patch()
        {
            Dispose();
        }
    }
}