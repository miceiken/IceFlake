using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.Client;
using IceFlake.Client.Scripts;

namespace IceFlake.Scripts
{
    public class PacketScript : Script
    {
        public PacketScript()
            : base("Packet", "Test")
        {
        }

        private ClientServices _cs;

        public override void OnStart()
        {
            if (_cs == null)
                _cs = new ClientServices();

            _cs.SetMessageHandler(ClientServices.NetMessage.SMSG_MESSAGECHAT, OnChatMessage, IntPtr.Zero);

            var data = new DataStore(ClientServices.NetMessage.CMSG_MESSAGECHAT);
            data.PutInt8(1); // CHAT_MSG_SAY
            data.PutInt32(0x01); // LANG_ORCISH (does this work?)
            data.PutString("Hello World!");
            _cs.SendPacket(data);
        }

        private bool OnChatMessage(IntPtr param, ClientServices.NetMessage msgId, uint time, IntPtr pData)
        {
            var data = new DataStore(pData);
            var type = data.GetInt8();
            var lang = data.GetInt32();
            var guid = data.GetInt64();
            if (type == 17) // Channel
                data.GetString(64);
            var guid2 = data.GetInt64();
            var length = data.GetInt32();
            var message = data.GetString(256);
            var afk = data.GetInt8();

            return true;
        }
    }
}
