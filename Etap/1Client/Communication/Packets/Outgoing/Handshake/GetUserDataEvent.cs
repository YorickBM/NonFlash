using Etap.Communication.Packets.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etap.Communication.Packets.Outgoing.Handshake
{
    class GetUserDataEvent : ServerPacket
    {
        public GetUserDataEvent(int id) : base(ServerPacketHeader.GetUserDataEvent)
        {
            WriteInteger(id);
        }
    }
}
