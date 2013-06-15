using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace IceFlake.Client
{
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
