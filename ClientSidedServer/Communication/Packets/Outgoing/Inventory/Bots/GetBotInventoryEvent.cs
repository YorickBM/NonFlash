using System.Collections.Generic;

using Retro.Hotel.Users.Inventory.Bots;
using Retro.Communication.Packets.Outgoing.Inventory.Bots;

namespace Retro.Communication.Packets.Incoming.Inventory.Bots
{
    class GetBotInventoryEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session.GetHabbo().GetInventoryComponent() == null)
                return;

            ICollection<Bot> Bots = Session.GetHabbo().GetInventoryComponent().GetBots();
            Session.SendMessage(new BotInventoryComposer(Bots));
        }
    }
}
