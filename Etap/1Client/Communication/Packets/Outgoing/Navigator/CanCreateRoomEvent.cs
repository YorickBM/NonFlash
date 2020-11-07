using Retro.Communication.Packets.Outgoing.Navigator;

namespace Retro.Communication.Packets.Incoming.Navigator
{
    class CanCreateRoomEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new CanCreateRoomComposer(false, 150));
        }
    }
}
