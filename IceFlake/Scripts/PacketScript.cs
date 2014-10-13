using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Configuration;
using System.Runtime.InteropServices;
using System.Text;
using IceFlake.Client;
using IceFlake.Client.Patchables;
using IceFlake.Client.Scripts;

namespace IceFlake.Scripts
{
    public class PacketScript : Script
    {
        public PacketScript()
            : base("Packet", "Test")
        {
        }

        public override void OnStart()
        {
            SendCastSpell();
            //RegisterChatHandler();
            //RegisterLookupHandler();
        }

        public override void OnTick()
        {
        }

        public override void OnTerminate()
        {
            ResetChatHandler();
            ResetLookupHandler();
        }

        #region CastSpell Packet

        // https://github.com/cmangos/mangos-wotlk/blob/master/src/game/SpellHandler.cpp#L336
        private void SendCastSpell()
        {
            var csp = new CastSpellPacket()
            {
                CastCount = 1,
                SpellID = 49276, // Lesser Healing Wave
                unkFlags = 0,
                TargetFlags = SpellCastTargetFlags.TARGET_FLAG_SELF,
                TargetGUID = (long)Manager.LocalPlayer.Guid,
            };

            var data = new CDataStore(WoWClientServices.NetMessage.CMSG_CAST_SPELL);

            data.Write<byte>(csp.CastCount);
            data.Write<int>(csp.SpellID);
            data.Write<byte>(csp.unkFlags);
            data.Write<int>((int)csp.TargetFlags);
            data.Write<long>(csp.TargetGUID);

            Manager.ClientServices.SendPacket(data);
        }

        public struct CastSpellPacket
        {
            public byte CastCount;
            public int SpellID;
            public byte unkFlags;
            public SpellCastTargetFlags TargetFlags;
            public long TargetGUID;
        }

        public enum SpellCastTargetFlags : int
        {
            TARGET_FLAG_SELF = 0x00000000,
            TARGET_FLAG_UNUSED1 = 0x00000001, // not used in any spells as of 3.0.3 (can be set dynamically)
            TARGET_FLAG_UNIT = 0x00000002, // pguid
            TARGET_FLAG_UNUSED2 = 0x00000004, // not used in any spells as of 3.0.3 (can be set dynamically)
            TARGET_FLAG_UNUSED3 = 0x00000008, // not used in any spells as of 3.0.3 (can be set dynamically)
            TARGET_FLAG_ITEM = 0x00000010, // pguid
            TARGET_FLAG_SOURCE_LOCATION = 0x00000020, // pguid + 3 float
            TARGET_FLAG_DEST_LOCATION = 0x00000040, // pguid + 3 float
            TARGET_FLAG_OBJECT_UNK = 0x00000080, // used in 7 spells only
            TARGET_FLAG_UNIT_UNK = 0x00000100, // looks like self target (480 spells)
            TARGET_FLAG_PVP_CORPSE = 0x00000200, // pguid
            TARGET_FLAG_UNIT_CORPSE = 0x00000400, // 10 spells (gathering professions)
            TARGET_FLAG_OBJECT = 0x00000800, // pguid, 2 spells
            TARGET_FLAG_TRADE_ITEM = 0x00001000, // pguid, 0 spells
            TARGET_FLAG_STRING = 0x00002000, // string, 0 spells
            TARGET_FLAG_UNK1 = 0x00004000, // 199 spells, opening object/lock
            TARGET_FLAG_CORPSE = 0x00008000, // pguid, resurrection spells
            TARGET_FLAG_UNK2 = 0x00010000, // pguid, not used in any spells as of 3.0.3 (can be set dynamically)
            TARGET_FLAG_GLYPH = 0x00020000, // used in glyph spells
            TARGET_FLAG_UNK3 = 0x00040000, //
            TARGET_FLAG_VISUAL_CHAIN = 0x00080000 // uint32, loop { vec3, guid -> if guid == 0 break }
        };

        #endregion

        #region Lookup Packets

        private void RegisterLookupHandler()
        {
            Manager.ClientServices.SetMessageHandler(WoWClientServices.NetMessage.SMSG_DBLOOKUP, LookupResultHandler, IntPtr.Zero);
        }

        private void ResetLookupHandler()
        {
            
        }

        private void SendLookupPacket()
        {
            var data = new CDataStore(WoWClientServices.NetMessage.CMSG_DBLOOKUP);
            data.PutString("Test 0");
            Manager.ClientServices.SendPacket(data);
        }

        private static int LookupResultHandler(IntPtr param, WoWClientServices.NetMessage msgId, uint time, IntPtr pData)
        {
            var data = new CDataStore(pData);
            var received = data.GetString(256);
            Log.WriteLine("LookupResultHandler: param {0:8X}, time {1}, received {2}", param, time, received);

            return 1;
        }

        #endregion

        #region Chat Packets

        private static PacketHandler _chatMessageHandler;
        private static readonly IntPtr ChatMessageHandlerPtr = (IntPtr)0x0050EBA0;

        private void RegisterChatHandler()
        {
            _chatMessageHandler = Manager.Memory.RegisterDelegate<PacketHandler>(ChatMessageHandlerPtr);
            Manager.ClientServices.SetMessageHandler(WoWClientServices.NetMessage.SMSG_MESSAGECHAT, OnChatMessage, IntPtr.Zero);
        }

        private void ResetChatHandler()
        {
            // Set the message handler back to default.
            Manager.ClientServices.SetMessageHandler(WoWClientServices.NetMessage.SMSG_MESSAGECHAT, ChatMessageHandlerPtr, IntPtr.Zero);
        }

        // https://github.com/cmangos/mangos-wotlk/blob/master/src/game/Chat.cpp#L3512
        private static int OnChatMessage(IntPtr param, WoWClientServices.NetMessage msgId, uint time, IntPtr pData)
        {
            var data = new CDataStore(pData);

            var sb = new StringBuilder();
            var type = (ChatMsgType)data.Read<byte>();
            sb.AppendFormat("[T:{0}] ", type);
            sb.AppendFormat("[L:{0}] ", data.Read<int>());
            sb.AppendFormat("[SG:{0}] ", data.Read<long>());
            data.Read<int>();

            switch (type)
            {
                case ChatMsgType.CHAT_MSG_MONSTER_SAY:
                case ChatMsgType.CHAT_MSG_MONSTER_PARTY:
                case ChatMsgType.CHAT_MSG_MONSTER_YELL:
                case ChatMsgType.CHAT_MSG_MONSTER_WHISPER:
                case ChatMsgType.CHAT_MSG_MONSTER_EMOTE:
                case ChatMsgType.CHAT_MSG_RAID_BOSS_WHISPER:
                case ChatMsgType.CHAT_MSG_RAID_BOSS_EMOTE:
                case ChatMsgType.CHAT_MSG_BATTLENET:
                case ChatMsgType.CHAT_MSG_WHISPER_FOREIGN:
                    // Sender length, name and target GUID
                    sb.AppendFormat("[SN:{0}] ", data.GetString(data.Read<int>()));
                    sb.AppendFormat("[TG:{0}] ", data.Read<long>());
                    /*
                    if (targetGuid && !targetGuid.IsPlayer() && !targetGuid.IsPet() && (msgtype != CHAT_MSG_WHISPER_FOREIGN))
                    {
                        data << uint32(strlen(targetName) + 1); // target name length
                        data << targetName; // target name
                    }
                    */
                    break;

                case ChatMsgType.CHAT_MSG_BG_SYSTEM_NEUTRAL:
                case ChatMsgType.CHAT_MSG_BG_SYSTEM_ALLIANCE:
                case ChatMsgType.CHAT_MSG_BG_SYSTEM_HORDE:
                    // Target guid, sender length and name
                    sb.AppendFormat("[TG:{0}] ", data.Read<long>());
                    /*
                    if (targetGuid && !targetGuid.IsPlayer())
                    {
                        MANGOS_ASSERT(targetName);
                        data << uint32(strlen(targetName) + 1); // target name length
                        data << targetName; // target name
                    }
                    */ 
                    //sb.AppendFormat("[SN:{0}] ", data.GetString(data.Read<int>()));
                    break;

                case ChatMsgType.CHAT_MSG_ACHIEVEMENT:
                case ChatMsgType.CHAT_MSG_GUILD_ACHIEVEMENT:
                    // Target GUID
                    sb.AppendFormat("[TG:{0}] ", data.Read<long>());
                    break;

                case ChatMsgType.CHAT_MSG_CHANNEL:
                    // Channel name, and target GUID
                    sb.AppendFormat("[CN:{0}] ", data.GetString(64)); // verify length of channel name
                    sb.AppendFormat("[TG:{0}] ", data.Read<long>());
                    break;

                default:
                    // Target GUID
                    sb.AppendFormat("[TG:{0}] ", data.Read<long>());
                    break;
            }

            // Message length and content, tag
            sb.AppendFormat("[M:{0}] ", data.GetString(data.Read<int>()));
            sb.AppendFormat("[T:{0}] ", (ChatTag)data.Read<byte>());

            Log.WriteLine(sb.ToString());

            // Call the WoW's internal chat message handler.
            data.Prepare();
            return _chatMessageHandler(param, msgId, time, pData);
        }

        public enum ChatTag : byte
        {
            CHAT_TAG_NONE = 0x00,
            CHAT_TAG_AFK = 0x01,
            CHAT_TAG_DND = 0x02,
            CHAT_TAG_GM = 0x04,
            CHAT_TAG_COM = 0x08, // Commentator
            CHAT_TAG_DEV = 0x10, // Developer
        }

        public enum ChatMsgType : byte
        {
            CHAT_MSG_ADDON = 0xFF,
            CHAT_MSG_SYSTEM = 0x00,
            CHAT_MSG_SAY = 0x01,
            CHAT_MSG_PARTY = 0x02,
            CHAT_MSG_RAID = 0x03,
            CHAT_MSG_GUILD = 0x04,
            CHAT_MSG_OFFICER = 0x05,
            CHAT_MSG_YELL = 0x06,
            CHAT_MSG_WHISPER = 0x07,
            CHAT_MSG_WHISPER_FOREIGN = 0x08,
            CHAT_MSG_WHISPER_INFORM = 0x09,
            CHAT_MSG_EMOTE = 0x0A,
            CHAT_MSG_TEXT_EMOTE = 0x0B,
            CHAT_MSG_MONSTER_SAY = 0x0C,
            CHAT_MSG_MONSTER_PARTY = 0x0D,
            CHAT_MSG_MONSTER_YELL = 0x0E,
            CHAT_MSG_MONSTER_WHISPER = 0x0F,
            CHAT_MSG_MONSTER_EMOTE = 0x10,
            CHAT_MSG_CHANNEL = 0x11,
            CHAT_MSG_CHANNEL_JOIN = 0x12,
            CHAT_MSG_CHANNEL_LEAVE = 0x13,
            CHAT_MSG_CHANNEL_LIST = 0x14,
            CHAT_MSG_CHANNEL_NOTICE = 0x15,
            CHAT_MSG_CHANNEL_NOTICE_USER = 0x16,
            CHAT_MSG_AFK = 0x17,
            CHAT_MSG_DND = 0x18,
            CHAT_MSG_IGNORED = 0x19,
            CHAT_MSG_SKILL = 0x1A,
            CHAT_MSG_LOOT = 0x1B,
            CHAT_MSG_MONEY = 0x1C,
            CHAT_MSG_OPENING = 0x1D,
            CHAT_MSG_TRADESKILLS = 0x1E,
            CHAT_MSG_PET_INFO = 0x1F,
            CHAT_MSG_COMBAT_MISC_INFO = 0x20,
            CHAT_MSG_COMBAT_XP_GAIN = 0x21,
            CHAT_MSG_COMBAT_HONOR_GAIN = 0x22,
            CHAT_MSG_COMBAT_FACTION_CHANGE = 0x23,
            CHAT_MSG_BG_SYSTEM_NEUTRAL = 0x24,
            CHAT_MSG_BG_SYSTEM_ALLIANCE = 0x25,
            CHAT_MSG_BG_SYSTEM_HORDE = 0x26,
            CHAT_MSG_RAID_LEADER = 0x27,
            CHAT_MSG_RAID_WARNING = 0x28,
            CHAT_MSG_RAID_BOSS_EMOTE = 0x29,
            CHAT_MSG_RAID_BOSS_WHISPER = 0x2A,
            CHAT_MSG_FILTERED = 0x2B,
            CHAT_MSG_BATTLEGROUND = 0x2C,
            CHAT_MSG_BATTLEGROUND_LEADER = 0x2D,
            CHAT_MSG_RESTRICTED = 0x2E,
            CHAT_MSG_BATTLENET = 0x2F,
            CHAT_MSG_ACHIEVEMENT = 0x30,
            CHAT_MSG_GUILD_ACHIEVEMENT = 0x31,
            CHAT_MSG_ARENA_POINTS = 0x32,
            CHAT_MSG_PARTY_LEADER = 0x33
        }

        #endregion
    }
}
