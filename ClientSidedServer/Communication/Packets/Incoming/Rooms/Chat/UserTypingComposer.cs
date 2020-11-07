using Retro.Communication.Packets.Incoming;
using Retro.Hotel.GameClients;
using System;

namespace Retro.Communication.Packets.Outgoing.Rooms.Chat
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