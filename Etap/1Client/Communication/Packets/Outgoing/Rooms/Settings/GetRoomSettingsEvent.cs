using Retro.Hotel.Rooms;
using Retro.Communication.Packets.Outgoing.Rooms.Settings;

namespace Retro.Communication.Packets.Incoming.Rooms.Settings
{
    class GetRoomSettingsEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Room Room = RetroEnvironment.GetGame().GetRoomManager().LoadRoom(Packet.PopInt());
            if (Room == null )
                return;

            if (!Room.CheckRights(Session, true))
                return;

            Session.SendMessage(new RoomSettingsDataComposer(Room));
        }
    }
}
