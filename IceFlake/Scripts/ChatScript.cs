using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.Client;
using IceFlake.Client.API;
using IceFlake.Client.Patchables;
using IceFlake.Client.Objects;
using IceFlake.Client.Scripts;

namespace IceFlake.Scripts
{
    public class ChatScript : Script
    {
        public ChatScript()
            : base("Chat", "Monitoring")
        { }

        public override void OnStart()
        {
            base.OnStart();
        }

        public override void OnTick()
        {
            base.OnTick();
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
        }

        private void HandleChatEvents(string ev, List<string> args)
        {
        }
    }
}
