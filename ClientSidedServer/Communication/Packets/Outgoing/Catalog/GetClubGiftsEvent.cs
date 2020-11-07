using Retro.Hotel.GameClients;
using Retro.Communication.Packets.Outgoing.Catalog;

namespace Retro.Communication.Packets.Incoming.Catalog
{
    class GetClubGiftsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {

            Session.SendMessage(new ClubGiftsComposer());
        }
    }
}
