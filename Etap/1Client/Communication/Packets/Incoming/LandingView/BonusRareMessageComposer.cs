using Retro.Communication.Packets.Outgoing.Inventory.Furni;
using Retro.Communication.Packets.Outgoing.Inventory.Purse;
using Retro.Communication.Packets.Outgoing.Rooms.Notifications;
using Retro.Hotel.GameClients;
using Retro.Hotel.Items;
using Retro.Hotel.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Retro.Communication.Packets.Outgoing.LandingView
{
    class BonusRareMessageComposer : ServerPacket
    {
        public BonusRareMessageComposer(GameClient Session)
            : base(ServerPacketHeader.BonusRareMessageComposer)
        {
            string product = RetroEnvironment.GetGame().GetSettingsManager().TryGetValue("bonus_rare_productdata_name");
            int baseid = int.Parse(RetroEnvironment.GetGame().GetSettingsManager().TryGetValue("bonus_rare_item_baseid"));
            int score = int.Parse(RetroEnvironment.GetGame().GetSettingsManager().TryGetValue("bonus_rare_total_score"));

            base.WriteString(product);
            base.WriteInteger(baseid);
            base.WriteInteger(score);
            base.WriteInteger(Session.GetHabbo().BonusPoints >= score ? score : score - Session.GetHabbo().BonusPoints); //Total To Gain
            if (Session.GetHabbo().BonusPoints >= score)
            {
                Session.GetHabbo().BonusPoints -= score;
                Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().BonusPoints, score, 101));
                Session.SendMessage(new RoomAlertComposer("You have completed your Rare Bonus, you already have your prize in inventory! You will receive another when you pick up new bonuses."));
                ItemData Item = null;
                if (!RetroEnvironment.GetGame().GetItemManager().GetItem((baseid), out Item))
                {
                    return;
                }

                Item GiveItem = ItemFactory.CreateSingleItemNullable(Item, Session.GetHabbo(), "", "");
                if (GiveItem != null)
                {
                    Session.GetHabbo().GetInventoryComponent().TryAddItem(GiveItem);

                    Session.SendMessage(new FurniListNotificationComposer(GiveItem.Id, 1));
                    Session.SendMessage(new FurniListUpdateComposer());
                }

                Session.GetHabbo().GetInventoryComponent().UpdateItems(false);
            }
        }
    }
}
