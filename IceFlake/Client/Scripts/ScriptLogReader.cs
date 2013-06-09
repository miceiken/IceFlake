using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.Runtime;
using IceFlake.Client;
using System.ComponentModel;

namespace IceFlake.Client.Scripts
{
    public class ScriptLogReader
    {
        public ScriptLogReader(Script script)
        {

            this.script = script;
        }

        private Script script;

        public Script Script
        {
            get { return script; }
        }

        public bool IsRunning
        {
            get { return script.IsRunning; }
        }

        public string Category
        {
            get { return script.Category; }
        }
    }
}
