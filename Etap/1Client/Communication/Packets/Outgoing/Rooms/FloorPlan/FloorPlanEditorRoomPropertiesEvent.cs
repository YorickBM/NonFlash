
using System.Collections.Generic;

using Retro.Hotel.Rooms;
using Retro.Hotel.Items;
using Retro.Communication.Packets.Outgoing.Rooms.FloorPlan;
using Retro.Communication.Packets.Outgoing.Rooms.Engine;

namespace Retro.Communication.Packets.Incoming.Rooms.FloorPlan
{
    class FloorPlanEditorRoomPropertiesEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            DynamicRoomModel Model = Room.GetGameMap().Model;
            if (Model == null)
                return;

            ICollection<Item> FloorItems = Room.GetRoomItemHandler().GetFloor;

            Session.SendMessage(new FloorPlanFloorMapComposer(FloorItems));
            Session.SendMessage(new FloorPlanSendDoorComposer(Model.DoorX, Model.DoorY, Model.DoorOrientation));
            Session.SendMessage(new RoomVisualizationSettingsComposer(Room.WallThickness, Room.FloorThickness, RetroEnvironment.EnumToBool(Room.Hidewall.ToString())));
        }
    }
}
