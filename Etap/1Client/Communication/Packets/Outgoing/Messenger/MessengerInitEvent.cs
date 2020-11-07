using System.Linq;
using System.Collections.Generic;

using Etap.Communication.Packets.Outgoing;

namespace Retro.Communication.Packets.Incoming.Messenger
{
    class MessengerInitEvent : ServerPacket
    {
        public MessengerInitEvent() : base(ServerPacketHeader.MessengerInitMessageEvent)
        {

        }
    }
}