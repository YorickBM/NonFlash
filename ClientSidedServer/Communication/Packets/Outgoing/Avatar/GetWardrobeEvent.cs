using Retro.Communication.Packets.Outgoing.Avatar;

namespace Retro.Communication.Packets.Incoming.Avatar
{
    class GetWardrobeEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendPacket(new WardrobeComposer(Session));
        }
    }
}
