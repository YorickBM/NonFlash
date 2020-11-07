using System;
using Etap.Utilities;
using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;

namespace Etap.Communication.Packets.Outgoing.Moderation
{
    class ModeratorSupportTicketComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Packet.PopInt();
        }
    }
}