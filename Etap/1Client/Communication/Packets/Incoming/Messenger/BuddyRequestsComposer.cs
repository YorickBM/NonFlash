using System.Collections.Generic;
using Etap.Hotel.GameClients;
using Etap.Communication.Packets.Incoming;
using Etap.Communication.Packets;

namespace Etap.Communication.Packets.Incoming.Messenger
{
    class BuddyRequestsComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int RequestCount = Packet.PopInt();
            int RequestCount2 = Packet.PopInt();

            for (int i = 0; i < RequestCount2; i++)
            {
                int from = Packet.PopInt();
                string fromUsername = Packet.PopString();

                string look = Packet.PopString();
            }
        }
    }
}
