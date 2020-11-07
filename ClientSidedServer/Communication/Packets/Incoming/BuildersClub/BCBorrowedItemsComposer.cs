using Retro.Communication.Packets.Incoming;
using Retro.Hotel.GameClients;

namespace Retro.Communication.Packets.Outgoing.BuildersClub
{
    class BCBorrowedItemsComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int i0 = Packet.PopInt();
        }
    }
}
