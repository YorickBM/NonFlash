using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;

namespace Etap.Communication.Packets.Outgoing.BuildersClub
{
    class BCBorrowedItemsComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int i0 = Packet.PopInt();
        }
    }
}
