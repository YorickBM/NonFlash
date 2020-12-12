using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Etap.Hotel.GameClients;

namespace Etap.Communication.Packets.Incoming.Rooms.Engine
{
    class UsersComposer : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            int users = packet.PopInt();
            for(int i = 0; i < users; i++)
            {

            }
        }
    }
}