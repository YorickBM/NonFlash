using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;
using System;

namespace Etap.Communication.Packets.Incoming.Rooms.Chat
{
    public class UserTypingComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int virtualId = Packet.PopInt();
            int isTrue = Packet.PopInt();
            bool typing = Convert.ToBoolean(isTrue);
        }
    }
}