using Retro.Hotel.Rooms;
using Retro.Communication.Packets.Outgoing.Rooms.Settings;

namespace Retro.Communication.Packets.Incoming.Rooms.Settings
{
	class GetRoomBannedUsersEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            Room Instance = Session.GetHabbo().CurrentRoom;
            if (Instance == null || !Instance.CheckRights(Session, true))
                return;

            if (Instance.GetBans().BannedUsers().Count > 0)
                Session.SendMessage(new GetRoomBannedUsersComposer(Instance));
        }
    }
}
