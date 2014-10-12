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
        private WoWConsole _console;

        public override void OnStart()
        {
            if (_console == null)
            {
                _console = new WoWConsole();
                _console.SetConsoleKey(_consoleKey);
                Print("Console is bound to key '{0}'", _consoleKey);
            }
            _console.RegisterCommand("test", TestCommand, CommandCategory.Debug, "Test help string");
            _console.RegisterCommand("dance", DanceCommand, CommandCategory.Debug, "Dance");
            _console.Write("Hello from ConsoleScript!", WoWConsoleColor.Echo);
        }

        public override void OnTick()
        {
        }

        public override void OnTerminate()
        {
            _console.UnregisterCommand("test");
            _console.UnregisterCommand("dance");
            _console.Write("Good-bye!", WoWConsoleColor.Echo);
        }

        private bool TestCommand(string cmd, string args)
        {
            _console.Write("Hello from TestCommand: cmd '%s', args '%s'", WoWConsoleColor.Input, cmd, args);

            return true;
        }

        private bool DanceCommand(string cmd, string args)
        {
            _console.Write("Let's dance!", WoWConsoleColor.Highlight);
            WoWScript.ExecuteNoResults("DoEmote(\"dance\")");

            return true;
        }
    }
}
