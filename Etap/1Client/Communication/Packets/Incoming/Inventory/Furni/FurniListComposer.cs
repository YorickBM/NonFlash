using Etap.Communication.Packets;
using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;
using System.Collections.Generic;

namespace Retro.Communication.Packets.Incoming.Inventory.Furni
{
    class FurniListComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int pages = Packet.PopInt();
            int page = Packet.PopInt();

            int items = Packet.PopInt();
            for(int i = 0; i < items; i++)
            {
                int itemId = Packet.PopInt();
                string baseItemType = Packet.PopString();
                int itemId2 = Packet.PopInt(); //Duplicate i guess
                int spriteId = Packet.PopInt();

                int x0 = Packet.PopInt();
                int x1 = Packet.PopInt();
                string ExtraData = Packet.PopString();
                int limitedNo = Packet.PopInt(); // Is Item Limited
                int limitedTotal = Packet.PopInt(); //Max Limited Items

                bool allowEcotronRecycle = Packet.PopBoolean();
                bool allowTrade = Packet.PopBoolean();
                bool allowInventoryStack = Packet.PopBoolean();
                bool isRare = Packet.PopBoolean();
                int secondsToExpiration = Packet.PopInt(); //Max time you can have the item
                bool x2 = Packet.PopBoolean();
                int x3 = Packet.PopInt(); //Item RoomId

                //if Item Is Not Wall Item
                string x4 = Packet.PopString();
                int x5 = Packet.PopInt();
            }
        }
    }
}