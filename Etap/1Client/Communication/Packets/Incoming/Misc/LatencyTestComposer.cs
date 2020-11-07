using System;
using Etap.Communication.Packets.Incoming;
using Etap.Communication.Packets.Incoming.Misc;
using Etap.Hotel.GameClients;

namespace Etap.Communication.Packets.Outgoing.Misc
{
    class LatencyTestComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendPacket(new LatencyTestEvent(Session, Packet.PopInt()));
        }
    }
}
