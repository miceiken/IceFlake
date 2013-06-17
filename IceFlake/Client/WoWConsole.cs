using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using IceFlake.Client.Patchables;

namespace IceFlake.Client
{
    //public enum WoWConsoleColor : int
    //{
    //    DEFAULT_COLOR = 0x0,
    //    INPUT_COLOR = 0x1,
    //    ECHO_COLOR = 0x2,
    //    ERROR_COLOR = 0x3,
    //    WARNING_COLOR = 0x4,
    //    GLOBAL_COLOR = 0x5,
    //    ADMIN_COLOR = 0x6,
    //    HIGHLIGHT_COLOR = 0x7,
    //    BACKGROUND_COLOR = 0x8,
    //}

    //public enum CommandCategory : int
    //{
    //    CATEGORY_DEBUG = 0x0,
    //    CATEGORY_GRAPHICS = 0x1,
    //    CATEGORY_CONSOLE = 0x2,
    //    CATEGORY_COMBAT = 0x3,
    //    CATEGORY_GAME = 0x4,
    //    CATEGORY_DEFAULT = 0x5,
    //    CATEGORY_NET = 0x6,
    //    CATEGORY_SOUND = 0x7,
    //    CATEGORY_GM = 0x8,
    //}

    //public class WoWConsole
    //{
    //    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    //    private delegate void ConsoleWriteADelegate(string text, WoWConsoleColor color, params string[] args);
    //    private ConsoleWriteADelegate _write;

    //    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    //    private delegate bool ConsoleRegisterCommandDelegate(string command, IntPtr handler, CommandCategory category, string help);
    //    private ConsoleRegisterCommandDelegate _registerCommand;

    //    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    //    private delegate bool ConsoleUnregisterCommandDelegate(string command);
    //    private ConsoleUnregisterCommandDelegate _unregisterCommand;

    //    public WoWConsole()
    //    {
    //        Toggle(true);

    //        _write = Manager.Memory.RegisterDelegate<ConsoleWriteADelegate>((IntPtr)Pointers.Console.WriteA);
    //        _registerCommand = Manager.Memory.RegisterDelegate<ConsoleRegisterCommandDelegate>((IntPtr)Pointers.Console.RegisterCommand);
    //        _unregisterCommand = Manager.Memory.RegisterDelegate<ConsoleUnregisterCommandDelegate>((IntPtr)Pointers.Console.UnregisterCommand);
    //    }

    //    public void Toggle(bool enable)
    //    {
    //        Manager.Memory.Write<int>((IntPtr)Pointers.Console.Enable, enable ? 1 : 0);
    //    }

    //    public void Write(string text, WoWConsoleColor color, params string[] args)
    //    {
    //        _write(text, color, args);
    //    }
    //}

    // Credits go to [TOM_RUS] for this one
    // http://www.ownedcore.com/forums/world-of-warcraft/world-of-warcraft-bots-programs/wow-memory-editing/406212-3-3-5a-implementing-new-console-commands.html

    //class Console
    //{
    //    public enum CommandCategory : int
    //    {
    //        Debug = 0,
    //        Graphics = 1,
    //        Console = 2,
    //        Combat = 3,
    //        Game = 4,
    //        Default = 5,
    //        Net = 6,
    //        Sound = 7,
    //        Gm = 8
    //    }
    //    /// <summary>
    //    /// Delegate that represents console command
    //    /// </summary>
    //    /// <param name="command">Command name</param>
    //    /// <param name="args">Command arguments</param>
    //    /// <returns>true if succeed or false if failed</returns>
    //    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    //    public delegate bool ConsoleCommandDelegate(string command, string args);
    //    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    //    delegate bool ConsoleCommandRegister(IntPtr commandName, IntPtr handler, CommandCategory category, IntPtr help, IntPtr unk);
    //    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    //    delegate void ConsoleCommandUnregister(string commandName);
    //    public class ConsoleCommand
    //    {
    //        public readonly string Name;
    //        public readonly IntPtr NamePtr;
    //        public readonly string Help;
    //        public readonly IntPtr HelpPtr;
    //        public readonly CommandCategory Category;
    //        private readonly ConsoleCommandDelegate Handler;
    //        public readonly IntPtr HandlerPtr;
    //        private Detour<ConsoleCommandDelegate> HandlerDetour;
    //        public ConsoleCommand(string name, string help, CommandCategory category)
    //        {
    //            Name = name;
    //            NamePtr = Memory.Alloc(Name.Length + 1);
    //            Memory.Write<string>(NamePtr, Name);
    //            Help = help;
    //            HelpPtr = Memory.Alloc(Help.Length + 1);
    //            Memory.Write<string>(HelpPtr, Help);
    //            Category = category;
    //            Handler = new ConsoleCommandDelegate(CommandHandler);
    //            HandlerPtr = Offsets.GetRandomCodeCave;
    //            HandlerDetour = new Detour<ConsoleCommandDelegate>(HandlerPtr, Handler);
    //        }
    //        public void Free()
    //        {
    //            Memory.Free(NamePtr);
    //            Memory.Free(HelpPtr);
    //            HandlerDetour.Remove();
    //        }
    //        private bool CommandHandler(string command, string args)
    //        {
    //            Lua.DoString("message(\"Console: command {0}, args {1}\")", command, args);
    //            return true;
    //        }
    //    }
    //}
}
