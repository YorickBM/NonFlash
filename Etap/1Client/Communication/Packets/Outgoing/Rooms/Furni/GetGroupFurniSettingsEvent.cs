﻿using Retro.Hotel.Items;
using Retro.Hotel.Groups;

using Retro.Communication.Packets.Outgoing.Groups;

namespace Retro.Communication.Packets.Incoming.Rooms.Furni
{
	class GetGroupFurniSettingsEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().InRoom)
                return;

            int ItemId = Packet.PopInt();
            int GroupId = Packet.PopInt();

            Item Item = Session.GetHabbo().CurrentRoom.GetRoomItemHandler().GetItem(ItemId);
            if (Item == null)
                return;

            if (Item.Data.InteractionType != InteractionType.GUILD_GATE)
                return;

            Group Group = null;
            if (!RetroEnvironment.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group))
                return;

            Session.SendMessage(new GroupFurniSettingsComposer(Group, ItemId, Session.GetHabbo().Id));
            Session.SendMessage(new GroupInfoComposer(Group, Session, false));
        }
    }
}