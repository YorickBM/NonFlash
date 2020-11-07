using Retro.Communication.Packets.Outgoing.Inventory.Badges;

namespace Retro.Communication.Packets.Incoming.Inventory.Badges
{
    class GetBadgesEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new BadgesComposer(Session));
        }
    }
}
