
using Retro.Hotel.Rooms;
using Retro.Communication.Packets.Outgoing.Rooms.Settings;

namespace Retro.Communication.Packets.Incoming.Rooms.Settings
{
	class GetRoomRightsEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            Room Instance = Session.GetHabbo().CurrentRoom;
            if (Instance == null)
                return;

            if (!Instance.CheckRights(Session))
                return;


            Session.SendMessage(new RoomRightsListComposer(Instance));
        }
    }
}
