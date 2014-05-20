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
            Manager.Events.Register("CHAT_MSG_*", HandleChatEvents);
        }

        public override void OnTick()
        {
        }

        public override void OnTerminate()
        {
            Manager.Events.Remove("CHAT_MSG_*", HandleChatEvents);
        }

        private void HandleChatEvents(string ev, List<string> args)
        {
            Print("[{0}] {1}: {2}", ev.Substring(9), args[1], args[0]);
        }
    }
}
