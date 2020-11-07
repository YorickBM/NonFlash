using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Etap.Communication.Packets.Outgoing;
using Etap.Hotel.GameClients;
using Etap.Communication.Packets.Outgoing.Handshake;

namespace Etap.Communication.Packets.Incoming.Misc
{
    class PongMessageEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendPacket(new PingEvent());
        }
    }
}
