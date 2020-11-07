using Etap.Communication.Packets.Outgoing;
using System;

namespace Etap.Communication.Packets.Outgoing.Handshake
{
    class PingEvent : ServerPacket
    {
        public PingEvent() : base(ServerPacketHeader.PingMessageEvent)
        {

        }
    }
}
