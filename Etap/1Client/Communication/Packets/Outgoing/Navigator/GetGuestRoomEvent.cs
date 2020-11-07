

namespace Etap.Communication.Packets.Outgoing.Navigator
{
    class GetGuestRoomEvent : ServerPacket
    {
        /*
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int roomID = Packet.PopInt();

            RoomData roomData = RetroEnvironment.GetGame().GetRoomManager().GenerateRoomData(roomID);
            if (roomData == null)
                return;

            bool isLoading = Packet.PopInt() == 1;
            bool checkEntry = Packet.PopInt() == 1;

            Session.SendMessage(new GetGuestRoomResultComposer(Session, roomData, isLoading, checkEntry));
        }*/

        public GetGuestRoomEvent(int roomID, int isLoading, int checkEntry) : base(ServerPacketHeader.GetGuestRoomMessageEvent)
        {
            base.WriteInteger(roomID);
            base.WriteInteger(isLoading);
            base.WriteInteger(checkEntry);
        }
    }
}
