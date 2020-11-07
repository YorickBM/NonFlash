using Etap.Communication.Packets;
using Etap.Hotel.GameClients;
using Etap.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etap.Communication.Packets.Incoming.Handshake
{
    class GetUserDataComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int id = Packet.PopInt();
            string look = Packet.PopString();
            string motto = Packet.PopString();
            string gender = Packet.PopString();
            int relationshipType = Packet.PopInt(); //-1
        }
    }
}
