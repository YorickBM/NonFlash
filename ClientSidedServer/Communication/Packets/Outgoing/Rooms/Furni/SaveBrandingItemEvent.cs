using System;
using Retro.Hotel.Rooms;
using Retro.Hotel.Items;

namespace Retro.Communication.Packets.Incoming.Rooms.Furni
{
    class SaveBrandingItemEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().InRoom)
                return;

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            if (!Session.GetHabbo().GetPermissions().HasRight("room_item_save_branding_items"))
            {
                Session.SendWhisper("You are not allowed to edit this background!");
                return;
            }

            int ItemId = Packet.PopInt();
            Item Item = Room.GetRoomItemHandler().GetItem(ItemId);
            if (Item == null)
                return;

            if (Item.Data.InteractionType == InteractionType.BACKGROUND || Item.Data.InteractionType == InteractionType.TERMINAL)
            {
                int Data = Packet.PopInt();
                string BrandData = "state" + Convert.ToChar(9) + "0";

                for (int i = 1; i <= Data; i++)
                {
                    BrandData = BrandData + Convert.ToChar(9) + Packet.PopString();
                }

                Item.ExtraData = BrandData;
            }

            else if (Item.Data.InteractionType == InteractionType.FX_PROVIDER)
            {
                Packet.PopInt();
                Packet.PopString();
                Item.ExtraData = Packet.PopString();
            }

            else if (Item.Data.InteractionType == InteractionType.INFO_TERMINAL)
            {
                Packet.PopInt();
                Packet.PopString();
                Item.ExtraData = Packet.PopString();
            }

            //else if (Item.Data.ItemName.Contains("info_terminal"))
            //{
            //    Packet.PopInt();
            //    Packet.PopString();
            //    Item.ExtraData = Packet.PopString();
            //}

            Room.GetRoomItemHandler().SetFloorItem(Session, Item, Item.GetX, Item.GetY, Item.Rotation, false, false, true);
        }
    }
}