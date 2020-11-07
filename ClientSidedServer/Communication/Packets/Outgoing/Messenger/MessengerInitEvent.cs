using System.Linq;
using System.Collections.Generic;

using Retro.Hotel.Users.Messenger;
using Retro.Communication.Packets.Outgoing.Messenger;

namespace Retro.Communication.Packets.Incoming.Messenger
{
    class MessengerInitEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || Session.GetHabbo().GetMessenger() == null)
                return;

            Session.GetHabbo().GetMessenger().OnStatusChanged(false);

            ICollection<MessengerBuddy> Friends = new List<MessengerBuddy>();
            foreach (MessengerBuddy Buddy in Session.GetHabbo().GetMessenger().GetFriends().ToList())
            {
                if (Buddy == null || Buddy.IsOnline) continue;
                Friends.Add(Buddy);
            }

            Session.SendMessage(new MessengerInitComposer(Session));
            Session.SendMessage(new BuddyListComposer(Friends, Session.GetHabbo(), 1, 0));

            Session.GetHabbo().GetMessenger().ProcessOfflineMessages();
        }
    }
}