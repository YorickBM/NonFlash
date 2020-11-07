using Retro.Hotel.Rooms;
using Retro.Hotel.Items;
using Retro.Hotel.Quests;
using Retro.Communication.Packets.Outgoing.Rooms.Engine;



namespace Retro.Communication.Packets.Incoming.Rooms.Engine
{
    class MoveObjectEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            int ItemId = Packet.PopInt();
            if (ItemId == 0)
                return;

            Room Room;

            if (!RetroEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;

            Item Item;
            if (Room.Group != null)
            {
                if (!Room.CheckRights(Session, false, true))
                {
                    Item = Room.GetRoomItemHandler().GetItem(ItemId);
                    if (Item == null)
                        return;

                    Session.SendMessage(new ObjectUpdateComposer(Item, Room.OwnerId));
                    return;
                }
            }
            else
            {
                if (!Room.CheckRights(Session))
                {
                    return;
                }
            }

            Item = Room.GetRoomItemHandler().GetItem(ItemId);

            if (Item == null)
                return;

            int x = Packet.PopInt();
            int y = Packet.PopInt();
            int Rotation = Packet.PopInt();

            if (x != Item.GetX || y != Item.GetY)
                RetroEnvironment.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.FURNI_MOVE);

            if (Rotation != Item.Rotation)
                RetroEnvironment.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.FURNI_ROTATE);

            if (!Room.GetRoomItemHandler().SetFloorItem(Session, Item, x, y, Rotation, false, false, true))
            {
                Room.SendMessage(new ObjectUpdateComposer(Item, Room.OwnerId));
                return;
            }

            if (Item.GetZ >= 0.1)
                RetroEnvironment.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.FURNI_STACK);
        }
    }
}