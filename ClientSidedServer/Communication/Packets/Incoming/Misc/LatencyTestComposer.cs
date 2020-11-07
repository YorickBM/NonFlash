using System;
using Retro.Communication.Packets.Incoming;
using Retro.Communication.Packets.Incoming.Misc;
using Retro.Hotel.GameClients;

namespace Retro.Communication.Packets.Outgoing.Misc
{
    class LatencyTestComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendPacket(new LatencyTestEvent(Session, Packet.PopInt()));
        }
    }
}
