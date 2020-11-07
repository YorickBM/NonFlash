using Retro.Hotel.Rooms;
using Retro.Communication.Packets.Outgoing.Rooms.Session;
using Retro.Communication.Packets.Outgoing.Messenger;

namespace Retro.Communication.Packets.Incoming.Messenger
{
    class FindNewFriendsEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Room Instance = RetroEnvironment.GetGame().GetRoomManager().TryGetRandomLoadedRoom();

            if (Instance != null)
            {
                Session.SendMessage(new FindFriendsProcessResultComposer(true));
                Session.SendMessage(new RoomForwardComposer(Instance.Id));
            }
            else
            {
                Session.SendMessage(new FindFriendsProcessResultComposer(false));
            }
        }
    }
}
