using System.Collections.Generic;
using Retro.Hotel.Users.Messenger;
using Retro.Hotel.Cache.Type;

namespace Retro.Communication.Packets.Outgoing.Messenger
{
	class BuddyRequestsComposer : ServerPacket
    {
        public BuddyRequestsComposer(ICollection<MessengerRequest> Requests)
            : base(ServerPacketHeader.BuddyRequestsMessageComposer)
        {
			WriteInteger(Requests.Count);
			WriteInteger(Requests.Count);

            foreach (MessengerRequest Request in Requests)
            {
				WriteInteger(Request.From);
				WriteString(Request.Username);

                UserCache User = RetroEnvironment.GetGame().GetCacheManager().GenerateUser(Request.From);
				WriteString(User != null ? User.Look : "");
            }
        }
    }
}
