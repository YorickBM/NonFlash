using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Retro.Communication.Packets.Outgoing.Rooms.Nux;
using Retro.Communication.Packets.Outgoing;
using Retro.Hotel.GameClients;

namespace Retro.Communication.Packets.Incoming.Rooms.Nux
{
    class RoomNuxAlert : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            var habbo = Session.GetHabbo();
            if (!habbo.PassedNuxNavigator && !habbo.PassedNuxCatalog && !habbo.PassedNuxItems && !habbo.PassedNuxMMenu && !habbo.PassedNuxChat && !habbo.PassedNuxCredits && !habbo.PassedNuxDuckets)
            {
                Session.SendMessage(new NuxAlertComposer("helpBubble/add/BOTTOM_BAR_NAVIGATOR/nux.bot.info.navigator.1"));
                habbo.PassedNuxNavigator = true;
            }

            else if (habbo.PassedNuxNavigator && !habbo.PassedNuxCatalog && !habbo.PassedNuxItems && !habbo.PassedNuxMMenu && !habbo.PassedNuxChat && !habbo.PassedNuxCredits && !habbo.PassedNuxDuckets)
            {
                Session.SendMessage(new NuxAlertComposer("helpBubble/add/BOTTOM_BAR_CATALOGUE/nux.bot.info.shop.1"));
                habbo.PassedNuxCatalog = true;
            }
            else if (habbo.PassedNuxNavigator && habbo.PassedNuxCatalog && !habbo.PassedNuxItems && !habbo.PassedNuxMMenu && !habbo.PassedNuxChat && !habbo.PassedNuxCredits && !habbo.PassedNuxDuckets)
            {
                Session.SendMessage(new NuxAlertComposer("helpBubble/add/BOTTOM_BAR_INVENTORY/nux.bot.info.inventory.1"));
                habbo.PassedNuxItems = true;
            }
            else if (habbo.PassedNuxNavigator && habbo.PassedNuxCatalog && habbo.PassedNuxItems && !habbo.PassedNuxMMenu && !habbo.PassedNuxChat && !habbo.PassedNuxCredits && !habbo.PassedNuxDuckets)
            {
                Session.SendMessage(new NuxAlertComposer("helpBubble/add/MEMENU_CLOTHES/nux.bot.info.memenu.1"));
                habbo.PassedNuxMMenu = true;
            }
            else if (habbo.PassedNuxNavigator && habbo.PassedNuxCatalog && habbo.PassedNuxItems && habbo.PassedNuxMMenu && !habbo.PassedNuxChat && !habbo.PassedNuxCredits && !habbo.PassedNuxDuckets)
            {
                Session.SendMessage(new NuxAlertComposer("helpBubble/add/CHAT_INPUT/nux.bot.info.chat.1"));
                habbo.PassedNuxChat = true;
            }
            else if (habbo.PassedNuxNavigator && habbo.PassedNuxCatalog && habbo.PassedNuxItems && habbo.PassedNuxMMenu && habbo.PassedNuxChat && !habbo.PassedNuxCredits && !habbo.PassedNuxDuckets)
            {
                Session.SendMessage(new NuxAlertComposer("helpBubble/add/CREDITS_BUTTON/Hier zie je hoeveel credits je hebt."));
                habbo.PassedNuxCredits = true;
            }
            else if (habbo.PassedNuxNavigator && habbo.PassedNuxCatalog && habbo.PassedNuxItems && habbo.PassedNuxMMenu && habbo.PassedNuxChat && habbo.PassedNuxCredits && !habbo.PassedNuxDuckets)
            {
                Session.SendMessage(new NuxAlertComposer("helpBubble/add/DUCKETS_BUTTON/nux.bot.info.duckets.1"));
                habbo.PassedNuxDuckets = true;
            }

            if (habbo.PassedNuxNavigator && habbo.PassedNuxCatalog && habbo.PassedNuxItems && habbo.PassedNuxMMenu && habbo.PassedNuxChat && habbo.PassedNuxCredits && habbo.PassedNuxDuckets)
            {
                Session.SendMessage(new NuxAlertComposer("nux/lobbyoffer/show"));
                habbo._NUX = false;
                Session.SendMessage(new NuxItemListComposer());
                using (var dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
                    dbClient.runFastQuery("UPDATE users SET nux_user = 'false' WHERE id = " + Session.GetHabbo().Id + ";");
                var nuxStatus = new ServerPacket(ServerPacketHeader.NuxUserStatus);
                nuxStatus.WriteInteger(0);
                Session.SendMessage(nuxStatus);
            }
        }
    }
}
