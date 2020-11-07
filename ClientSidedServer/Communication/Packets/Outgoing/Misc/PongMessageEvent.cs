using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Retro.Communication.Packets.Outgoing;

namespace Retro.Communication.Packets.Incoming.Misc
{
    class PongMessageEvent : ClientPacket
    {
        public PongMessageEvent() : base(ClientPacketHeader.PongMessageComposer)
        {
        }
    }
}
