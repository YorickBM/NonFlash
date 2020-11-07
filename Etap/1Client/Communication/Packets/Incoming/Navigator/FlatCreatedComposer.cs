using Etap.Hotel.GameClients;

namespace Etap.Communication.Packets.Incoming.Navigator
{
	class FlatCreatedComposer : IPacketEvent
    {
        /*public FlatCreatedComposer(int roomID, string roomName)
            : base(ServerPacketHeader.FlatCreatedMessageComposer)
        {
			WriteInteger(roomID);
			WriteString(roomName);
        }*/

        public void Parse(GameClient Session, ClientPacket packet)
        {
            int roomId = packet.PopInt();
            string roomName = packet.PopString();
        }
    }
}
