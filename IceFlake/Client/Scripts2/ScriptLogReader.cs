using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanLayer.Library;
using cleanLayer.Library.Scripts;

namespace cleanLayer.GUI
{
    public class ScriptLogReader : LogReader
    {
        public ScriptLogReader(Script script, int maxLength = 100000)
            : base(script.Name, maxLength)
        {

            this.script = script;

            script.OnStartedEvent += new EventHandler(script_StateChanged);
            script.OnStoppedEvent += new EventHandler(script_StateChanged);
        }

        private Script script;

        #region Event Handlers

        private void script_StateChanged(object sender, EventArgs e)
        {
            OnPropertyChanged("IsRunning");
        }

        #endregion

        #region Properties

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

        #endregion
    }
}
