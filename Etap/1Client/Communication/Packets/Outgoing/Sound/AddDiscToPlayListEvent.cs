﻿using Retro.Hotel.GameClients;
using Retro.Communication.Packets.Outgoing.Rooms.Notifications;
namespace Retro.Communication.Packets.Incoming.Sound
{
    class AddDiscToPlayListEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var room = Session.GetHabbo().CurrentRoom;
            if (!room.CheckRights(Session))
                return;
            //Console.WriteLine(Packet.ToString());

            var itemid = Packet.PopInt();//item id
            var songid = Packet.PopInt();//Song id

            var item = room.GetRoomItemHandler().GetItem(itemid);
            if (item == null)
                return;
            if (!room.GetTraxManager().AddDisc(item))
                Session.SendMessage(new RoomNotificationComposer("", "Oops! This disk was bad", "error", "", ""));

        }
    }
}
