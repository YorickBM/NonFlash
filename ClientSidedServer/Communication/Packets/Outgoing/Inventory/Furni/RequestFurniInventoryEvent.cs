﻿using System.Linq;
using System.Collections.Generic;

using MoreLinq;
using Retro.Hotel.Items;
using Retro.Communication.Packets.Outgoing.Inventory.Furni;

namespace Retro.Communication.Packets.Incoming.Inventory.Furni
{
    class RequestFurniInventoryEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            IEnumerable<Item> Items = Session.GetHabbo().GetInventoryComponent().GetWallAndFloor;

            int page = 0;
            int pages = ((Items.Count() - 1) / 700) + 1;

            if (!Items.Any())
            {
                Session.SendMessage(new FurniListComposer(Items.ToList(), 1, 0));
            }
            else
            {
                foreach (ICollection<Item> batch in Items.Batch(700))
                {
                    Session.SendMessage(new FurniListComposer(batch.ToList(), pages, page));

                    page++;
                }
            }
        }
    }
}
