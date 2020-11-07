using Retro.Communication.Packets.Outgoing.Rooms.Furni.RentableSpaces;

namespace Retro.Communication.Packets.Incoming.Rooms.Furni.RentableSpaces
{
	class GetRentableSpaceEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int Something = Packet.PopInt();
            Session.SendMessage(new RentableSpaceComposer());
        }
    }
}
