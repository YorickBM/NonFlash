using Retro.Communication.Packets.Outgoing.Users;

namespace Retro.Communication.Packets.Incoming.Inventory.Purse
{
    class GetHabboClubCenterInfoMessageEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new GetHabboClubCenterInfoMessageComposer(Session));
        }
    }
}