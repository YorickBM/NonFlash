using Retro.Hotel.Rooms;
using Retro.Communication.Packets.Outgoing.Moderation;

namespace Retro.Communication.Packets.Incoming.Moderation
{
    class GetModeratorRoomInfoEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
                return;

            int RoomId = Packet.PopInt();

            RoomData Data = RetroEnvironment.GetGame().GetRoomManager().GenerateRoomData(RoomId);
            if (Data == null)
                return;

            Room Room;

            if (!RetroEnvironment.GetGame().GetRoomManager().TryGetRoom(RoomId, out Room))
                return;

            Session.SendMessage(new ModeratorRoomInfoComposer(Data, (Room.GetRoomUserManager().GetRoomUserByHabbo(Data.OwnerName) != null)));
        }
    }
}
