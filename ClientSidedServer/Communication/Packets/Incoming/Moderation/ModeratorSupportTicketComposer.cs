using System;
using Retro.Utilities;
using Retro.Communication.Packets.Incoming;
using Retro.Hotel.GameClients;

namespace Retro.Communication.Packets.Outgoing.Moderation
{
    class ModeratorSupportTicketComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Packet.PopInt();
        }
    }
}