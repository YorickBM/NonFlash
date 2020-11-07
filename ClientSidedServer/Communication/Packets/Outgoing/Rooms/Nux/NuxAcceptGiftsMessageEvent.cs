using System;
using Retro.Hotel.GameClients;
using Retro.Communication.Packets.Outgoing.Inventory.Purse;
using Retro.Utilities;
using Retro.Hotel.Items;
using Retro.Communication.Packets.Outgoing.Inventory.Furni;
using Retro.Communication.Packets.Outgoing.Rooms.Notifications;
using Retro.Communication.Packets.Outgoing.Rooms.Nux;

namespace Retro.Communication.Packets.Incoming.Rooms.Nux
{
    class NuxAcceptGiftsMessageEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new NuxItemListComposer());
        }
    }
}