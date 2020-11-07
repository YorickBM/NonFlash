using System.Collections.Generic;

using Retro.Hotel.Rooms;
using Retro.Communication.Packets.Outgoing.Groups;

namespace Retro.Communication.Packets.Incoming.Groups
{
    class GetGroupCreationWindowEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null)
                return;

            List<RoomData> ValidRooms = new List<RoomData>();
            foreach (RoomData Data in Session.GetHabbo().UsersRooms)
            {
                if (Data.Group == null)
                    ValidRooms.Add(Data);
            }

            Session.SendMessage(new GroupCreationWindowComposer(ValidRooms));
        }
    }
}
