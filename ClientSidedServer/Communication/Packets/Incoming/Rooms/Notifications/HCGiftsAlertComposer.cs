using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Retro.Hotel.GameClients;

namespace Retro.Communication.Packets.Incoming.Rooms.Notifications
{
    public class HCGiftsAlertComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int x0 = Packet.PopInt();
        }
    }
}