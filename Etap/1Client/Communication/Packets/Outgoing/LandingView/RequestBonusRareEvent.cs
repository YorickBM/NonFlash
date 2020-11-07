using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Retro.Communication.Packets.Outgoing.LandingView;

namespace Retro.Communication.Packets.Incoming.LandingView
{
    class RequestBonusRareEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new BonusRareMessageComposer(Session));
        }
    }
}
