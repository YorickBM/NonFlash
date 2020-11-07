using Retro.Hotel.Rooms;
using Retro.Communication.Packets.Outgoing.Navigator;

namespace Retro.Communication.Packets.Incoming.Navigator
{
    class UpdateNavigatorSettingsEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int roomID = Packet.PopInt();
            if (roomID == 0)
                return;

            RoomData Data = RetroEnvironment.GetGame().GetRoomManager().GenerateRoomData(roomID);
            if (Data == null)
                return;

            Session.GetHabbo().HomeRoom = roomID;
            Session.SendMessage(new NavigatorSettingsComposer(roomID));
        }
    }
}
