using System.Linq;
using System.Collections.Generic;

using Retro.Hotel.Users.Messenger;
using Retro.Communication.Packets.Outgoing.Messenger;

namespace Retro.Communication.Packets.Incoming.Messenger
{
    class GetBuddyRequestsEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            ICollection<MessengerRequest> Requests = Session.GetHabbo().GetMessenger().GetRequests().ToList();

            Session.SendMessage(new BuddyRequestsComposer(Requests));
        }
    }
}
