
using Retro.Communication.Packets.Outgoing.Catalog;
using Retro.Communication.Packets.Outgoing.Inventory.Furni;
using Retro.Communication.Packets.Outgoing.Inventory.Purse;
using Retro.Communication.Packets.Outgoing.Rooms.Notifications;
using Retro.Hotel.Items;
using Retro.Hotel.Catalog;
using Retro.Database.Interfaces;
using System.Data;
using System;

namespace Retro.Communication.Packets.Incoming.Catalog
{
    class BuyTargettedOfferMessage : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)

        {
            #region RETURN VALUES
            var offer = RetroEnvironment.GetGame().GetTargetedOffersManager().TargetedOffer;
            var habbo = Session.GetHabbo();
            if (offer == null || habbo == null)
            {
                Session.SendMessage(new PurchaseErrorComposer(1));
                return;
            }
            #endregion

            #region FIELDS
            Packet.PopInt();
            var amount = Packet.PopInt();
            if (amount > offer.Limit)
            {
                Session.SendMessage(new PurchaseErrorComposer(1));
                return;
            }
            var creditsCost = int.Parse(offer.Price[0]) * amount;
            var extraMoneyCost = int.Parse(offer.Price[1]) * amount;
            #endregion

            #region CREDITS COST
            if (creditsCost > 0)
            {
                if (habbo.Credits < creditsCost)
                {
                    Session.SendMessage(new PurchaseErrorComposer(1));
                    return;
                }

                habbo.Credits -= creditsCost;
                Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits - creditsCost));
            }
            #endregion

            #region EXTRA MONEY COST
            if (extraMoneyCost > 0)
            {
                #region GET MONEY TYPE AND DISCOUNT
                switch (offer.MoneyType)
                {
                    #region DUCKETS COST
                    case "duckets":
                        {
                            if (habbo.Duckets < extraMoneyCost)
                            {
                                Session.SendMessage(new PurchaseErrorComposer(1));
                                return;
                            }

                            Session.GetHabbo().Duckets -= extraMoneyCost;
                            Session.SendPacket(new HabboActivityPointNotificationComposer(Session.GetHabbo().Duckets, Session.GetHabbo().Duckets));
                            break;
                        }
                    #endregion

                    #region DIAMONDS COST
                    case "diamonds":
                    case "diamanten":
                        {
                            if (habbo.Diamonds < extraMoneyCost)
                            {
                                Session.SendMessage(new PurchaseErrorComposer(1));
                                return;
                            }

                            Session.GetHabbo().Diamonds -= extraMoneyCost;
                            Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, 0, 5));
                            break;
                        }
                    #endregion

                    #region GOTW COST
                    case "gotw":
                    case "gotwpoints":
                    case "eventpunten":
                        {
                            if (habbo.GOTWPoints < extraMoneyCost)
                            {
                                Session.SendMessage(new PurchaseErrorComposer(1));
                                return;
                            }

                            Session.GetHabbo().GOTWPoints -= extraMoneyCost;
                            Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, 0, 103));
                            break;
                        }
                        #endregion
                }
                #endregion
            }
            #endregion

            #region BUY AND CREATE ITEMS PROGRESS
            TargetedOffers TargetedOffer = RetroEnvironment.GetGame().GetTargetedOffersManager().TargetedOffer;
            using (IQueryAdapter dbQuery = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbQuery.SetQuery("SELECT targeted_buy FROM users where id = " + habbo.Id + " LIMIT 1");
                DataTable count = dbQuery.getTable();
                foreach (DataRow Row in count.Rows)
                {
                    int offer2 = Convert.ToInt32(Row["targeted_buy"]);

                    if (TargetedOffer.Limit == offer2)
                    {
                        Session.SendMessage(new RoomAlertComposer("Você já passo do limite de compra para esta Oferta."));
                    }

                    else

                    {
                        using (var dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
                            dbClient.runFastQuery("UPDATE users SET targeted_buy = targeted_buy +1 WHERE id = " + Session.GetHabbo().Id + ";");
                        foreach (var product in offer.Products)
                        {
                            #region CHECK PRODUCT TYPE
                            switch (product.ItemType)
                            {
                                #region NORMAL ITEMS CASE
                                case "item":
                                    {
                                        ItemData item = null;
                                        if (!RetroEnvironment.GetGame().GetItemManager().GetItem(int.Parse(product.Item), out item)) return;
                                        if (item == null) return;
                                        var cItem = ItemFactory.CreateSingleItemNullable(item, Session.GetHabbo(), string.Empty, string.Empty);
                                        if (cItem != null)
                                        {
                                            Session.GetHabbo().GetInventoryComponent().TryAddItem(cItem);

                                            Session.SendMessage(new FurniListAddComposer(cItem));
                                            Session.SendMessage(new FurniListUpdateComposer());

                                        }

                                        Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                                        break;
                                    }
                                #endregion

                                #region BADGE CASE
                                case "badge":
                                    {
                                        if (habbo.GetBadgeComponent().HasBadge(product.Item))
                                        {
                                        }

                                        habbo.GetBadgeComponent().GiveBadge(product.Item, true, Session);
                                        break;
                                    }
                                    #endregion
                            }
                            #endregion
                        }
                    }
                }
            }
            #endregion

            #region RE-OPEN TARGETED BOX
            TargetedOffers TargetedOffer2 = RetroEnvironment.GetGame().GetTargetedOffersManager().TargetedOffer;
            using (IQueryAdapter dbQuery2 = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbQuery2.SetQuery("SELECT targeted_buy FROM users where id = " + habbo.Id + " LIMIT 1");
                DataTable count2 = dbQuery2.getTable();
                foreach (DataRow Row2 in count2.Rows)
                {
                    int offer22 = Convert.ToInt32(Row2["targeted_buy"]);


                    if (TargetedOffer2.Limit != offer22)
                    {
                        Session.SendMessage(RetroEnvironment.GetGame().GetTargetedOffersManager().TargetedOffer.Serialize());
                    }
                }
            }
            #endregion
        }
    }
}