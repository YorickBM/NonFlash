using Retro.Hotel.Rooms;
using System;

namespace Retro.Communication.Packets.Incoming.Rooms.Avatar
{
    class ApplySignEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int SignId = Packet.PopInt();
            Room Room;

            if (!RetroEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;


            User.UnIdle();

            User.SetStatus("sign", Convert.ToString(SignId));
            User.UpdateNeeded = true;
            User.SignTime = RetroEnvironment.GetUnixTimestamp() + 5;
        }
    }
}