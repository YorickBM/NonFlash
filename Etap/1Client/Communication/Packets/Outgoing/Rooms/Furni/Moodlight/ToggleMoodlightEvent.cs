using Retro.Hotel.Rooms;
using Retro.Hotel.Items;

namespace Retro.Communication.Packets.Incoming.Rooms.Furni.Moodlight
{
    class ToggleMoodlightEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            Room Room;

            if (!RetroEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;
            
            if (!Room.CheckRights(Session, true) || Room.MoodlightData == null)
                return;

            Item Item = Room.GetRoomItemHandler().GetItem(Room.MoodlightData.ItemId);
            if (Item == null || Item.GetBaseItem().InteractionType != InteractionType.MOODLIGHT)
                return;

            if (Room.MoodlightData.Enabled)
                Room.MoodlightData.Disable();
            else
                Room.MoodlightData.Enable();

            Item.ExtraData = Room.MoodlightData.GenerateExtraData();
            Item.UpdateState();
        }
    }
}