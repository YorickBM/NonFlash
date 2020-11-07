using System.Linq;
using System.Collections.Generic;
using Etap.Communication.Packets.Outgoing;

namespace Retro.Communication.Packets.Outgoing.Inventory.Furni
{
    class RequestFurniInventoryEvent : ServerPacket
    {
        public RequestFurniInventoryEvent() : base(ServerPacketHeader.RequestFurniInventoryMessageEvent)
        {
        }
    }
}
