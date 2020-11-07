using Retro.Communication.Packets.Outgoing;
using System;

namespace Retro.Communication.Packets.Incoming.Handshake
{
    class PingEvent : ServerPacket
    {
        public PingEvent() : base(ServerPacketHeader.PingMessageEvent)
        {

        }
    }
}
