using Etap.Communication.Packets.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etap._1Client.Communication.Packets.Outgoing.Messenger
{
    class GetBuddyRequestEvent : ServerPacket
    {
        public GetBuddyRequestEvent() : base(ServerPacketHeader.GetBuddyRequestsMessageEvent)
        {

        }
    }
}
