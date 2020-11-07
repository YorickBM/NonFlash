using Retro.Hotel.Rooms;
using Retro.Hotel.Items;
using Retro.Communication.Packets.Outgoing.Rooms.Furni.Stickys;

namespace Retro.Communication.Packets.Incoming.Rooms.Furni.Stickys
{
	class GetStickyNoteEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            Room Room;

            if (!RetroEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;

            Item Item = Room.GetRoomItemHandler().GetItem(Packet.PopInt());
            if (Item == null || Item.GetBaseItem().InteractionType != InteractionType.POSTIT)
                return;

            Session.SendMessage(new StickyNoteComposer(Item.Id.ToString(), Item.ExtraData));
        }
    }
}