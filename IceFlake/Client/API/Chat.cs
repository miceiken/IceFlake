using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace IceFlake.Client
{
    public class Chat
    {
        private static OnMessageEventHandler Event_OnNewMessageDelegate;
        internal static List<ChatMessageStruct> WoWChat = new List<ChatMessageStruct>();

        public static event OnMessageEventHandler Event_OnNewMessage
        {
            add
            {
                OnMessageEventHandler handler2;
                OnMessageEventHandler handler = Event_OnNewMessageDelegate;
                do
                {
                    handler2 = handler;
                    OnMessageEventHandler handler3 = (OnMessageEventHandler)Delegate.Combine(handler2, value);
                    handler = Interlocked.CompareExchange<OnMessageEventHandler>(ref Event_OnNewMessageDelegate, handler3, handler2);
                }
                while (handler != handler2);
            }
            remove
            {
                OnMessageEventHandler handler2;
                OnMessageEventHandler handler = Event_OnNewMessageDelegate;
                do
                {
                    handler2 = handler;
                    OnMessageEventHandler handler3 = (OnMessageEventHandler)Delegate.Remove(handler2, value);
                    handler = Interlocked.CompareExchange<OnMessageEventHandler>(ref Event_OnNewMessageDelegate, handler3, handler2);
                }
                while (handler != handler2);
            }
        }

        private static ChatMessageStruct ParseMsg(uint BaseMsg)
        {
            ChatMessageStruct struct2 = new ChatMessageStruct
            {
                FormattedMsg = Manager.Memory.ReadString(new IntPtr(BaseMsg + 60)),
                SenderGUID = Manager.Memory.Read<ulong>(new IntPtr(BaseMsg)),
                Type = (ChatType)Manager.Memory.Read<byte>(new IntPtr(BaseMsg + 0x17ac)),
                Timestamp = Manager.Memory.Read<int>(new IntPtr(BaseMsg + 0x17b8))
            };
            try
            {
                struct2.Channel = struct2.FormattedMsg.Split('[')[2].Split(']')[0];
                struct2.Player = struct2.FormattedMsg.Split('[')[3].Split(']')[0];
                struct2.Message = struct2.FormattedMsg.Split('[')[6].Split(']')[0];
            }
            catch { }

            return struct2;
        }


        internal static Thread ChatThread = new Thread(new ThreadStart(Refresh));
        private static void Refresh()
        {
            bool flag = true;
            while (true)
            {
                try
                {
                    for (int i = 0; i < 0x3b; i++)
                    {
                        uint baseMsg = (uint)(0xb75a60 + (i * 0x17c0));

                        if (Manager.Memory.Read<ulong>(new IntPtr(baseMsg)) != 0)
                        {
                            ChatMessageStruct item = ParseMsg(baseMsg);
                            if (!WoWChat.Contains(item))
                            {
                                OnMessageEventArgs e = new OnMessageEventArgs(item);
                                if (!flag && (Event_OnNewMessageDelegate != null))
                                {
                                    Event_OnNewMessageDelegate(null, e);
                                }
                                WoWChat.Add(item);
                            }
                        }
                        Thread.Sleep(10);
                    }
                    flag = false;
                }
                catch { }

                Thread.Sleep(100);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ChatMessageStruct
        {
            public string FormattedMsg;
            public ulong SenderGUID;
            public Chat.ChatType Type;
            public string Channel;
            public string Player;
            public string Message;
            public int Timestamp;
        }

        //private enum ChatStruct
        //{
        //    CHANNEL_NUMBER = 0x17b0,
        //    FORMATTED_MESSAGE = 0x44,
        //    MESSAGE_TYPE = 0x17b4,
        //    PURE_TEXT = 0xbfc,
        //    SENDER_GUID = 0,
        //    SEQUENCE = 0x17b4,
        //    TIME = 0x17c0
        //}

        public enum ChatType : byte
        {
            ADDON = 0,
            AFK = 0x17,
            BATTLEGROUND = 0x2c,
            BATTLEGROUND_LEADER = 0x2d,
            BG_EVENT_ALLIANCE = 0x24,
            BG_EVENT_HORDE = 0x25,
            BG_EVENT_NEUTRAL = 0x23,
            CHANNEL = 0x11,
            CHANNEL_JOIN = 0x12,
            CHANNEL_LEAVE = 0x13,
            CHANNEL_LIST = 20,
            CHANNEL_NOTICE = 0x15,
            CHANNEL_NOTICE_USER = 0x16,
            COMBAT_FACTION_CHANGE = 0x26,
            DND = 0x18,
            EMOTE = 10,
            FILTERED = 0x2b,
            GUILD = 4,
            IGNORED = 0x19,
            LOOT = 0x1b,
            MONSTER_EMOTE = 0x10,
            MONSTER_PARTY = 13,
            MONSTER_SAY = 12,
            MONSTER_WHISPER = 15,
            MONSTER_YELL = 14,
            OFFICER = 5,
            PARTY = 2,
            PARTY_LEADER = 0x33,
            RAID = 3,
            RAID_LEADER = 0x27,
            RAID_WARNING = 40,
            RAID_WARNING_WIDESCREEN = 0x29,
            RESTRICTED = 0x2e,
            SAY = 1,
            SKILL = 0x1a,
            SYSTEM = 0x1c,
            TEXT_EMOTE = 11,
            WHISPER_FROM = 7,
            WHISPER_MOB = 8,
            WHISPER_TO = 9,
            YELL = 6
        }

        public class OnMessageEventArgs : EventArgs
        {
            private Chat.ChatMessageStruct Msg = new Chat.ChatMessageStruct();

            public OnMessageEventArgs(Chat.ChatMessageStruct theMsg)
            {
                this.Msg = theMsg;
            }

            public Chat.ChatMessageStruct EventMessage
            {
                get
                {
                    return this.Msg;
                }
            }
        }

        public delegate void OnMessageEventHandler(object sender, Chat.OnMessageEventArgs e);

        // LUA
        public static void SendGeneral(string Text)
        {
            WoWScript.ExecuteNoResults("SendChatMessage(\"" + Text + "\", \"SAY\", GetDefaultLanguage(\"player\"), \"\");");
        }

        public static void SendGuild(string Text)
        {
            WoWScript.ExecuteNoResults("SendChatMessage(\"" + Text + "\", \"GUILD\", GetDefaultLanguage(\"player\"), \"\");");
        }

        public static void SendParty(string Text)
        {
            WoWScript.ExecuteNoResults("SendChatMessage(\"" + Text + "\", \"PARTY\", GetDefaultLanguage(\"player\"), \"\");");
        }

        public static void SendRaid(string Text)
        {
            WoWScript.ExecuteNoResults("SendChatMessage(\"" + Text + "\", \"RAID\", GetDefaultLanguage(\"player\"), \"\");");
        }

        public static void SendWhisp(string PlayerName, string Text)
        {
            WoWScript.ExecuteNoResults("SendChatMessage(\"" + Text + "\", \"WHISPER\", GetDefaultLanguage(\"player\"), \"" + PlayerName + "\");");
        }
    }
}