using Retro.Hotel.Rooms;
using Retro.Hotel.Items;
using Retro.Communication.Packets.Outgoing.Rooms.Engine;

namespace Retro.Communication.Packets.Incoming.Rooms.Engine
{
    class MoveWallItemEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Room Room = null;
            if (!RetroEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;

            if (!Room.CheckRights(Session))
                return;

            int itemID = Packet.PopInt();
            string wallPositionData = Packet.PopString();

            Item Item = Room.GetRoomItemHandler().GetItem(itemID);

            if (Item == null)
                return;

            try
            {
                string WallPos = Room.GetRoomItemHandler().WallPositionCheck(":" + wallPositionData.Split(':')[1]);
                Item.wallCoord = WallPos;
            }
            catch { return; }

            Room.GetRoomItemHandler().UpdateItem(Item);
            Room.SendMessage(new ItemUpdateComposer(Item, Room.OwnerId));
        }
    }
}
