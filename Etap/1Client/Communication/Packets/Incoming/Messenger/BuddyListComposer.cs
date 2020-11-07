using System;
using System.Linq;
using System.Collections.Generic;
using Etap.Communication.Packets;
using Etap.Hotel.GameClients;
using Etap.Utilities;
using Etap.Communication.Packets.Outgoing.Handshake;

namespace Etap.Communication.Packets.Incoming.Messenger
{
    class BuddyListComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int friends = Packet.PopInt();

            for (int i = 0; i < friends; i++)
            {
                int friendId = Packet.PopInt();
                string username = Packet.PopString();
                bool isOnline = Packet.PopBoolean();
                bool isInRoom = Packet.PopBoolean();
                int relationshipType = Packet.PopInt();

                Session.SendPacket(new GetUserDataEvent(friendId));
            }
            //Make it so friends thingy gets all the appropriate stuff
        }
    }
}
