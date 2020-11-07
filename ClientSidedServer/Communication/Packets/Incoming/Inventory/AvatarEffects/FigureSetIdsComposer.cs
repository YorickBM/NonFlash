using System.Linq;
using System.Collections.Generic;
using Retro.Communication.Packets.Incoming;
using Retro.Hotel.GameClients;

namespace Retro.Communication.Packets.Outgoing.Inventory.AvatarEffects
{
    class FigureSetIdsComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int ClothingParts0 = Packet.PopInt();
            for(int i = 0; i < ClothingParts0; i++)
            {
                string part = Packet.PopString();
            }

            int ClothingParts1 = Packet.PopInt();
            for (int i = 0; i < ClothingParts0; i++)
            {
                string part = Packet.PopString();
            }
        }
    }
}
