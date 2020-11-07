using Retro.Hotel.Rooms;
using Retro.Communication.Packets.Outgoing.Rooms.Settings;

namespace Retro.Communication.Packets.Incoming.Rooms.Settings
{
    class UnbanUserFromRoomEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            Room Instance = Session.GetHabbo().CurrentRoom;
            if (Instance == null || !Instance.CheckRights(Session, true))
                return;

            int UserId = Packet.PopInt();
            int RoomId = Packet.PopInt();

            if (Instance.GetBans().IsBanned(UserId))
            {
                Instance.GetBans().Unban(UserId);
                Session.SendMessage(new UnbanUserFromRoomComposer(RoomId, UserId));
            }
        }
    }
}