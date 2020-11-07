using Retro.Communication.Packets.Outgoing.Inventory.Purse;
using Retro.Communication.Packets.Outgoing.Rooms.Notifications;


namespace Retro.Communication.Packets.Incoming.LandingView
{
    class GiveConcurrentUsersReward : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session.GetHabbo().GetStats().PurchaseUsersConcurrent)
            {
                Session.SendMessage(new RoomAlertComposer("U heeft deze prijs ontvangen."));
            }

            string badge = RetroEnvironment.GetGame().GetSettingsManager().TryGetValue("usersconcurrent_badge");
            int pixeles = int.Parse(RetroEnvironment.GetGame().GetSettingsManager().TryGetValue("usersconcurrent_pixeles"));

            Session.GetHabbo().GOTWPoints = Session.GetHabbo().GOTWPoints + pixeles;
            Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().GOTWPoints, pixeles, 103));
            Session.GetHabbo().GetBadgeComponent().GiveBadge(badge, true, Session);
            Session.SendMessage(new RoomAlertComposer("U heeft een badge ontvangen en " + pixeles + " duckets."));
            Session.GetHabbo().GetStats().PurchaseUsersConcurrent = true;
        }
    }
}
