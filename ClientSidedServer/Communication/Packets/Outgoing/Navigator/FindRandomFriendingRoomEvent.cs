using Retro.Communication.Packets.Outgoing.Rooms.Session;
using Retro.Communication.Packets.Outgoing.Rooms.Nux;
using Retro.Hotel.Rooms;

namespace Retro.Communication.Packets.Incoming.Navigator
{
    class FindRandomFriendingRoomEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            var type = Packet.PopString();
            if (type == "predefined_noob_lobby")
            {
                Session.SendMessage(new NuxAlertComposer("nux/lobbyoffer/hide"));
                Session.SendMessage(new RoomForwardComposer(4));
                return;
            }

            Room Instance = RetroEnvironment.GetGame().GetRoomManager().TryGetRandomLoadedRoom();
            if (Instance != null)
                Session.SendMessage(new RoomForwardComposer(Instance.Id));
        }
    }
}
