using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using IceFlake.Client;
using IceFlake.Client.Scripts;

namespace IceFlake.Scripts
{
    public class ConsoleScript : Script
    {
        public ConsoleScript()
            : base("Console", "Test")
        {
        }

        private readonly string _consoleKey = "z";

        public override void OnStart()
        {
            Manager.Console.SetConsoleKey(_consoleKey);
            Print("Console is bound to key '{0}'", _consoleKey);

            Manager.Console.RegisterCommand("test", TestCommand, CommandCategory.Debug, "Test help string");
            Manager.Console.RegisterCommand("dance", DanceCommand, CommandCategory.Debug, "Dance");
            Manager.Console.Write("Hello from ConsoleScript!", WoWConsoleColor.Echo);
        }

        public override void OnTick()
        {
        }

        public override void OnTerminate()
        {
            Manager.Console.UnregisterCommand("test");
            Manager.Console.UnregisterCommand("dance");
            Manager.Console.Write("Good-bye!", WoWConsoleColor.Echo);
        }

        private int TestCommand(string cmd, string args)
        {
            // This crashes, probably because of params string[] args
            //Manager.Console.Write("Hello from TestCommand: cmd '%s', args '%s'", WoWConsoleColor.Input, cmd, args);
            Manager.Console.Write(string.Format("Hello from TestCommand: cmd '{0}', args '{1}'", cmd, args), WoWConsoleColor.Input);
            return 1;
        }

        private int DanceCommand(string cmd, string args)
        {
            Manager.Console.Write("Let's dance!", WoWConsoleColor.Highlight);
            WoWScript.ExecuteNoResults("DoEmote(\"dance\")");

            return 1;
        }
    }
}
