using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using IceFlake.Client.Patchables;
using IceFlake.Runtime;

namespace IceFlake.Client
{
    public enum WoWConsoleColor
    {
        Default = 0x0,
        Input = 0x1,
        Echo = 0x2,
        Error = 0x3,
        Warning = 0x4,
        Global = 0x5,
        Admin = 0x6,
        Highlight = 0x7,
        Background = 0x8,
    }

    public enum CommandCategory
    {
        Debug = 0x0,
        Graphics = 0x1,
        Console = 0x2,
        Combat = 0x3,
        Game = 0x4,
        Default = 0x5,
        Net = 0x6,
        Sound = 0x7,
        GM = 0x8,
    }

    // https://github.com/tomrus88/WowAddin/blob/master/WowAddin/Console.h
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int CommandHandler(string cmd, string args);

    public class WoWConsole
    {
        public WoWConsole()
        {
            Toggle(true);
        }

        ~WoWConsole()
        {
            foreach (var strPtr in _stringPointers.Values)
                Release(strPtr);
            _stringPointers.Clear();
        }

        private readonly Dictionary<string, KeyValuePair<IntPtr, IntPtr>> _stringPointers = new Dictionary<string, KeyValuePair<IntPtr, IntPtr>>();

        public void Toggle(bool enable)
        {
            Manager.Memory.Write((IntPtr)Pointers.Console.Enable, enable ? 1 : 0);
        }

        public void SetConsoleKey(string key)
        {
            WoWScript.ExecuteNoResults("SetConsoleKey(\"" + key + "\")");
        }

        public void Write(string text, WoWConsoleColor color, params string[] args)
        {
            if (_write == null)
                _write = Manager.Memory.RegisterDelegate<ConsoleWriteADelegate>((IntPtr)Pointers.Console.WriteA);
            _write(text, color, args);
        }

        public bool RegisterCommand(string command, CommandHandler handler, CommandCategory category, string help)
        {
            if (_registerCommand == null)
                _registerCommand =
                Manager.Memory.RegisterDelegate<ConsoleRegisterCommandDelegate>(
                    (IntPtr)Pointers.Console.RegisterCommand);

            if (_stringPointers.ContainsKey(command)) // Commmand by that name already registered
                return false;

            var cmdPtr = Marshal.AllocHGlobal(command.Length + 1);
            Manager.Memory.WriteString(cmdPtr, command, Encoding.UTF8);

            var helpPtr = Marshal.AllocHGlobal(help.Length + 1);
            Manager.Memory.WriteString(helpPtr, help, Encoding.UTF8);

            _stringPointers.Add(command, new KeyValuePair<IntPtr, IntPtr>(cmdPtr, helpPtr));

            return _registerCommand(cmdPtr, Marshal.GetFunctionPointerForDelegate(handler), category, helpPtr);
        }

        public void UnregisterCommand(string command)
        {
            if (_unregisterCommand == null)
                _unregisterCommand =
                Manager.Memory.RegisterDelegate<ConsoleUnregisterCommandDelegate>(
                    (IntPtr)Pointers.Console.UnregisterCommand);

            if (!_stringPointers.ContainsKey(command)) // Commmand by that name is not registered
                return;

            var strPtr = _stringPointers[command];
            _unregisterCommand(strPtr.Key);
            _stringPointers.Remove(command);
            Release(strPtr);
        }

        private void Release(KeyValuePair<IntPtr, IntPtr> ptr)
        {
            Marshal.FreeHGlobal(ptr.Key);
            Marshal.FreeHGlobal(ptr.Value);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool ConsoleRegisterCommandDelegate(IntPtr commandPtr, IntPtr handler, CommandCategory category, IntPtr helpPtr);
        private ConsoleRegisterCommandDelegate _registerCommand;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ConsoleUnregisterCommandDelegate(IntPtr commandPtr);
        private ConsoleUnregisterCommandDelegate _unregisterCommand;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ConsoleWriteADelegate(string text, WoWConsoleColor color, params string[] args);
        private ConsoleWriteADelegate _write;
    }
}