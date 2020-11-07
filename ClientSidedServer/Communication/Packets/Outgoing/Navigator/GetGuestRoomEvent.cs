using Retro.Communication.Packets.Outgoing.Navigator;
using Retro.Hotel.Rooms;

namespace Retro.Communication.Packets.Incoming.Navigator
{
    class GetGuestRoomEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int roomID = Packet.PopInt();

            RoomData roomData = RetroEnvironment.GetGame().GetRoomManager().GenerateRoomData(roomID);
            if (roomData == null)
                return;

            bool isLoading = Packet.PopInt() == 1;
            bool checkEntry = Packet.PopInt() == 1;

            Session.SendMessage(new GetGuestRoomResultComposer(Session, roomData, isLoading, checkEntry));
        }
    }
}
