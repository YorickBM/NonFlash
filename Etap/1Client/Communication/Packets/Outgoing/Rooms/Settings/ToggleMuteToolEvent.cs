using System.Linq;
using System.Collections.Generic;
using Retro.Hotel.Rooms;
using Retro.Communication.Packets.Outgoing.Rooms.Settings;

namespace Retro.Communication.Packets.Incoming.Rooms.Settings
{
    class ToggleMuteToolEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null || !Room.CheckRights(Session, true))
                return;

            Room.RoomMuted = !Room.RoomMuted;

            List<RoomUser> roomUsers = Room.GetRoomUserManager().GetRoomUsers();
            foreach (RoomUser roomUser in roomUsers.ToList())
            {
                if (roomUser == null || roomUser.GetClient() == null)
                    continue;

                if (Room.RoomMuted)
                    roomUser.GetClient().SendWhisper("This room has been muted.");
                else
                    roomUser.GetClient().SendWhisper("This room has been unmuted.");
            }

            Room.SendMessage(new RoomMuteSettingsComposer(Room.RoomMuted));
        }
    }
}
