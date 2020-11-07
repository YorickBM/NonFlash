using System;
using System.Linq;
using System.Collections.Generic;
using Retro.Communication.Packets.Outgoing.Rooms.Notifications;
using Retro.Core;
using Retro.Hotel.Catalog;
using Retro.Hotel.GameClients;
using Retro.Hotel.Items;
using Retro.Hotel.Users.Effects;
using Retro.Hotel.Items.Utilities;
using Retro.Hotel.Users.Inventory.Bots;
using Retro.Hotel.Rooms.AI;
using Retro.Communication.Packets.Outgoing.Catalog;
using Retro.Communication.Packets.Outgoing.Inventory.Bots;
using Retro.Communication.Packets.Outgoing.Inventory.Pets;
using Retro.Communication.Packets.Outgoing.Inventory.Purse;
using Retro.Communication.Packets.Outgoing.Inventory.Furni;
using Retro.Communication.Packets.Outgoing.Inventory.AvatarEffects;
using Retro.Database.Interfaces;
using Retro.Hotel.Groups;
using Retro.Communication.Packets.Outgoing.Navigator;
using Retro.Utilities;
using Retro.Hotel.Groups.Forums;
using Retro.Communication.Packets.Outgoing.Moderation;
using Retro.Communication.Packets.Outgoing.Users;
using Retro.Communication.Packets.Outgoing.Messenger;
using System.Data;

namespace Retro.Communication.Packets.Incoming.Catalog
{
    public class PurchaseFromCatalogEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            ICollection<Item> FloorItems = Session.GetHabbo().GetInventoryComponent().GetFloorItems();
            ICollection<Item> WallItems = Session.GetHabbo().GetInventoryComponent().GetWallItems();

            if (RetroEnvironment.GetGame().GetSettingsManager().TryGetValue("catalog.enabled") != "1")
            {
                Session.SendNotification("Hotelmanagers hebben de catalogus uitgeschakeld!");
                return;
            }

            int PageId = Packet.PopInt();
            int ItemId = Packet.PopInt();
            string ExtraData = Packet.PopString();
            int Amount = Packet.PopInt();


			if (!RetroEnvironment.GetGame().GetCatalog().TryGetPage(PageId, out CatalogPage Page))
				return;

			if (!Page.Enabled || !Page.Visible || Page.MinimumRank  >  Session.GetHabbo().Rank || (Page.MinimumVIP > Session.GetHabbo().VIPRank && Session.GetHabbo().Rank == 1))
                return;

			if (!Page.Items.TryGetValue(ItemId, out CatalogItem Item))
			{
				if (Page.ItemOffers.ContainsKey(ItemId))
				{
					Item = Page.ItemOffers[ItemId];
					if (Item == null)
						return;
				}
				else
					return;
			}

            if (Session.GetHabbo().Rank > 0)
            {
                DataRow presothiago = null;
                using (var dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT Presidio FROM users WHERE id = '" + Session.GetHabbo().Id + "'");
                    presothiago = dbClient.getRow();
                }

                if (Convert.ToBoolean(presothiago["Presidio"]) == true)
                {
                    if (Session.GetHabbo().Rank > 0)
                    {
                        string thiago = Session.GetHabbo().Look;
                        Session.SendMessage(new RoomNotificationComposer("police_announcement", "message", "Je zit vast en kunt de catalogus niet openen."));
                        return;
                    }
                }
            }

            ItemData baseItem = Item.GetBaseItem(Item.ItemId);
            if (baseItem != null)
            {
                if (baseItem.InteractionType == InteractionType.club_1_month || baseItem.InteractionType == InteractionType.club_3_month || baseItem.InteractionType == InteractionType.club_6_month)
                {
                    int Months = 0;

                    switch (baseItem.InteractionType)
                    {
                        case InteractionType.club_1_month:
                            Months = 1;
                            break;

                        case InteractionType.club_3_month:
                            Months = 3;
                            break;

                        case InteractionType.club_6_month:
                            Months = 6;
                            break;
                    }

                    int num = num = 31 * Months;

                    if (Item.CostCredits > Session.GetHabbo().Credits || Item.CostPixels > Session.GetHabbo().Duckets || Item.CostDiamonds > Session.GetHabbo().Diamonds || Item.CostGotw > Session.GetHabbo().GOTWPoints)
                        return;

                    if (Item.CostCredits > 0)
                    {
                        Session.GetHabbo().Credits -= Item.CostCredits;
                        Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits));
                    }

                    if (Item.CostPixels > 0)
                    {
                        Session.GetHabbo().Duckets -= Item.CostPixels;
                        Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Duckets, Session.GetHabbo().Duckets));//Love you, ME.
                    }

                    if (Item.CostDiamonds > 0)
                    {
                        Session.GetHabbo().Diamonds -= Item.CostDiamonds;
                        Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, 0, 5));
                    }

                    if (Item.CostGotw > 0)
                    {
                        Session.GetHabbo().GOTWPoints -= Item.CostGotw;
                        Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().GOTWPoints, 0, 103));
                    }

                    Session.GetHabbo().GetClubManager().AddOrExtendSubscription("habbo_vip", num * 24 * 3600, Session);
                    Session.GetHabbo().GetBadgeComponent().GiveBadge("HC1", true, Session);

                    Session.SendMessage(new ScrSendUserInfoComposer(Session.GetHabbo()));
                    Session.SendMessage(new PurchaseOKComposer(Item, Item.Data, Item.Items));
                    Session.SendMessage(new FurniListUpdateComposer());
                    return;
                }
            }

            if (baseItem.InteractionType == InteractionType.namecolor)
            {
                if (Item.CostCredits > Session.GetHabbo().Credits || Item.CostPixels > Session.GetHabbo().Duckets || Item.CostDiamonds > Session.GetHabbo().Diamonds || Item.CostGotw > Session.GetHabbo().GOTWPoints)
                    return;

                if (Item.CostCredits > 0)
                {
                    Session.GetHabbo().Credits -= Item.CostCredits;
                    Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits));
                    Session.GetHabbo().CreditsSpend = Item.CostCredits + Session.GetHabbo().CreditsSpend;
                }

                if (Item.CostPixels > 0)
                {
                    Session.GetHabbo().Duckets -= Item.CostPixels;
                    Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Duckets, Session.GetHabbo().Duckets));//Love you, ME.
                }

                if (Item.CostDiamonds > 0)
                {
                    Session.GetHabbo().Diamonds -= Item.CostDiamonds;
                    Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, 0, 5));
                }

                if (Item.CostGotw > 0)
                {
                    Session.GetHabbo().GOTWPoints -= Item.CostGotw;
                    Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().GOTWPoints, 0, 103));
                }

                using (IQueryAdapter dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.runFastQuery("UPDATE `users` SET `name_color` = '#" + Item.Name + "' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                }

                Session.GetHabbo().chatHTMLColour = "#" + Item.Name;
                Session.SendMessage(new ScrSendUserInfoComposer(Session.GetHabbo()));
                Session.SendMessage(new PurchaseOKComposer(Item, Item.Data, Item.Items));
                Session.SendMessage(new FurniListUpdateComposer());
                return;
            }

            if (baseItem.InteractionType == InteractionType.prefixname)
            { 
                if (ExtraData.Length > 6 || ExtraData.Length == 0 || ExtraData.Length < 0)
                {
                    Session.SendMessage(new PurchaseOKComposer(Item, Item.Data, Item.Items));
                    Session.SendWhisper("U moet een code van 1 tot 6 tekens invoeren om te kopen.", 34);
                    return;
                }

                    if (!Session.GetHabbo().GetPermissions().HasRight("word_filter_override") &&
                    RetroEnvironment.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(ExtraData, out string word))
                    {
                    // Todos os direitos resevados ao criador do comando: Thiago Araujo (Plus Emulador) Servidores de SAO
                    // Comando editaveu abaixo mais cuidado pra não faze merda
                    Session.GetHabbo().BannedPhraseCount++;
                    if (Session.GetHabbo().BannedPhraseCount >= 1)
                    {

                        Session.GetHabbo().TimeMuted = 10;
                        Session.SendNotification("U bent het zwijgen opgelegd, een moderator zal je zaak beoordelen, blijkbaar heb je een hotel genomineerd! Blijf niet bekendmaken dat u een hotel hebt.<font size =\"11\" color=\"#fc0a3a\">  <b>" + Session.GetHabbo().BannedPhraseCount + "/5</b></font> Als u nummer 5/5 bereikt, wordt u automatisch geblokkeerd");
                        RetroEnvironment.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("Alerta publicitário:",
                            "Let op medewerkers, de gebruiker <b>" + Session.GetHabbo().Username + "</b> een link geplaatst van een website of hotel bij het kopen van een tag in de winkel, zou u dit kunnen onderzoeken? klik gewoon op de onderstaande knop * Ga naar kamer *. <i> het woord is:<font size =\"11\" color=\"#f40909\">  <b>  " + ExtraData +
                            "</b></font></i>   in een kamer\r\n" + "- Gebruikersnaam <font size =\"11\" color=\"#0b82c6\">  <b>" +
                            Session.GetHabbo().Username + "</b>", "", "Ga naar de kamer", "event:navigator/goto/" +
                            Session.GetHabbo().CurrentRoomId));
                    }

                    if (Session.GetHabbo().BannedPhraseCount >= 5)
                    {
                        RetroEnvironment.GetGame().GetModerationManager().BanUser("Frank", Hotel.Moderation.ModerationBanType.USERNAME, Session.GetHabbo().Username, "Verbannen voor de zin (" + ExtraData + ")", (RetroEnvironment.GetUnixTimestamp() + 78892200));
                        Session.Disconnect();
                        return;
                    }
                    Session.SendMessage(new RoomNotificationComposer("furni_placement_error", "message", "Ongepast bericht in " + RetroEnvironment.HotelName + " . We onderzoeken wat u zei." + " " + Session.GetHabbo().Username + " " + "in de kamer!"));
                    return;
                }

                    if (ExtraData.ToUpper().Contains("ADM") || ExtraData.ToUpper().Contains("ADMIN") || ExtraData.ToUpper().Contains("GER") || ExtraData.ToUpper().Contains("DONO") || ExtraData.ToUpper().Contains("RANK") || ExtraData.ToUpper().Contains("MNG") || ExtraData.ToUpper().Contains("MOD") || ExtraData.ToUpper().Contains("STAFF") || ExtraData.ToUpper().Contains("ALFA") || ExtraData.ToUpper().Contains("ALPHA") || ExtraData.ToUpper().Contains("HELPER") || ExtraData.ToUpper().Contains("GM") || ExtraData.ToUpper().Contains("CEO") || ExtraData.ToUpper().Contains("ROOKIE") || ExtraData.ToUpper().Contains("M0D") || ExtraData.ToUpper().Contains("DEV") || ExtraData.ToUpper().Contains("OWNR") || ExtraData.ToUpper().Contains("FUNDADOR") || ExtraData.ToUpper().Contains("<") || ExtraData.ToUpper().Contains(">") || ExtraData.ToUpper().Contains("POLICIAL") || ExtraData.ToUpper().Contains("policial") || ExtraData.ToUpper().Contains("ajudante") || ExtraData.ToUpper().Contains("embaixador") || ExtraData.ToUpper().Contains("AJUDANTE") || ExtraData.ToUpper().Contains("EMBAIXADOR") || ExtraData.ToUpper().Contains("VIP") || ExtraData.ToUpper().Contains("vip") || ExtraData.ToUpper().Contains("PROG") || ExtraData.ToUpper().Contains("prog"))
                    {
                        Session.SendMessage(new PurchaseOKComposer(Item, Item.Data, Item.Items));
                        Session.SendWhisper("Wat probeer je? Plaats geen administratieve tag er zijn hier gevolgen voor!", 34);
                        return;
                    }

                if (ExtraData == "off" || ExtraData == "")
                {
                    Session.GetHabbo()._NamePrefix = "";
                    Session.SendNotification("Wij hebben efficiënte naamvoorvoegsels voor uw" + RetroEnvironment.HotelName + "'s");
                }

				ExtraData = RetroEnvironment.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(ExtraData, out string character) ? "" : ExtraData;

				if (string.IsNullOrEmpty(ExtraData))
                {
                    Session.SendMessage(new PurchaseOKComposer(Item, Item.Data, Item.Items));
                    Session.SendWhisper(character.ToUpper() + " Geen passend woord!", 34);
                    return;
                }

                if (Item.CostCredits > Session.GetHabbo().Credits || Item.CostPixels > Session.GetHabbo().Duckets || Item.CostDiamonds > Session.GetHabbo().Diamonds || Item.CostGotw > Session.GetHabbo().GOTWPoints)
                    return;

                if (Item.CostCredits > 0)
                {
                    Session.GetHabbo().Credits -= Item.CostCredits;
                    Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits));
                    Session.GetHabbo().CreditsSpend = Item.CostCredits + Session.GetHabbo().CreditsSpend;
                }

                if (Item.CostPixels > 0)
                {
                    Session.GetHabbo().Duckets -= Item.CostPixels;
                    Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Duckets, Session.GetHabbo().Duckets));//Love you, ME.
                }

                if (Item.CostDiamonds > 0)
                {
                    Session.GetHabbo().Diamonds -= Item.CostDiamonds;
                    Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, 0, 5));
                }

                if (Item.CostGotw > 0)
                {
                    Session.GetHabbo().GOTWPoints -= Item.CostGotw;
                    Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().GOTWPoints, 0, 103));
                }

                using (IQueryAdapter dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.runFastQuery("UPDATE `users` SET `prefix_name` = '" + ExtraData + "' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                }

                Session.GetHabbo()._NamePrefix = ExtraData;
                Session.SendMessage(new ScrSendUserInfoComposer(Session.GetHabbo()));
                Session.SendMessage(new PurchaseOKComposer(Item, Item.Data, Item.Items));
                Session.SendMessage(new FurniListUpdateComposer());
                return;
            }

            if (baseItem.InteractionType == InteractionType.prefixcolor)
            {
                if (Item.CostCredits > Session.GetHabbo().Credits || Item.CostPixels > Session.GetHabbo().Duckets || Item.CostDiamonds > Session.GetHabbo().Diamonds || Item.CostGotw > Session.GetHabbo().GOTWPoints)
                    return;

                if (Item.CostCredits > 0)
                {
                    Session.GetHabbo().Credits -= Item.CostCredits;
                    Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits));
                    Session.GetHabbo().CreditsSpend = Item.CostCredits + Session.GetHabbo().CreditsSpend;
                }

                if (Item.CostPixels > 0)
                {
                    Session.GetHabbo().Duckets -= Item.CostPixels;
                    Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Duckets, Session.GetHabbo().Duckets));//Love you, ME.
                }

                if (Item.CostDiamonds > 0)
                {
                    Session.GetHabbo().Diamonds -= Item.CostDiamonds;
                    Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, 0, 5));
                }

                if (Item.CostGotw > 0)
                {
                    Session.GetHabbo().GOTWPoints -= Item.CostGotw;
                    Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().GOTWPoints, 0, 103));
                }

                using (IQueryAdapter dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("UPDATE `users` SET `prefix_name_color` = @prefixn WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                    dbClient.AddParameter("prefixn", "#" + Item.Name);
                    dbClient.RunQuery();
                }

                Session.GetHabbo()._NamePrefixColor = "#" + Item.Name;
                Session.SendMessage(new ScrSendUserInfoComposer(Session.GetHabbo()));
                Session.SendMessage(new PurchaseOKComposer(Item, Item.Data, Item.Items));
                Session.SendMessage(new FurniListUpdateComposer());
                return;
            }

            if (baseItem.InteractionType == InteractionType.CLUB_VIP || baseItem.InteractionType == InteractionType.CLUB_VIP2)
          {
               // int Months = 0;

                switch (baseItem.InteractionType)
                {
                    case InteractionType.CLUB_VIP:
                       /// Months = 1;
                           break;

                   case InteractionType.CLUB_VIP2:
                    //    Months = 3;
                       break;
               }

                if (Item.CostCredits > Session.GetHabbo().Credits || Item.CostPixels > Session.GetHabbo().Duckets || Item.CostDiamonds > Session.GetHabbo().Diamonds || Item.CostGotw > Session.GetHabbo().GOTWPoints)
                    return;

                if (Item.CostCredits > 0)
                {
                    Session.GetHabbo().Credits -= Item.CostCredits;
                    Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits));
                    Session.GetHabbo().CreditsSpend = Item.CostCredits + Session.GetHabbo().CreditsSpend;
                }

                if (Item.CostPixels > 0)
                {
                    Session.GetHabbo().Duckets -= Item.CostPixels;
                    Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Duckets, Session.GetHabbo().Duckets));//Love you, ME.
                }

                if (Item.CostDiamonds > 0)
                {
                    Session.GetHabbo().Diamonds -= Item.CostDiamonds;
                    Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, 0, 5));
                }

                if (Item.CostGotw > 0)
                {
                    Session.GetHabbo().GOTWPoints -= Item.CostGotw;
                    Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().GOTWPoints, 0, 103));
                }
               

                Session.GetHabbo().GetClubManager().AddOrExtendSubscription("club_vip", 1 * 24 * 3600, Session);
                Session.GetHabbo().GetBadgeComponent().GiveBadge("DVIP", true, Session);

                RetroEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_VipClub", 1);
                Session.SendMessage(new ScrSendUserInfoComposer(Session.GetHabbo()));
                Session.SendMessage(new PurchaseOKComposer(Item, Item.Data, Item.Items));
                Session.SendMessage(new FurniListUpdateComposer());

                if (Session.GetHabbo().Rank > 2)
                {
                    Session.SendWhisper("Oops! Het ging even fout!");
                    return;
                }

                else if (Session.GetHabbo().Rank < 2)
                {
                    using (IQueryAdapter dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.runFastQuery("UPDATE `users` SET `rank` = '2' WHERE `id` = '" + Session.GetHabbo().Id + "'");
                        dbClient.runFastQuery("UPDATE `users` SET `rank_vip` = '1' WHERE `id` = '" + Session.GetHabbo().Id + "'");
                        Session.GetHabbo().Rank = 2;
                        Session.GetHabbo().VIPRank = 1;
                    }
                }

                return;
            }

            if (Amount < 1 || Amount > 100 || !Item.HaveOffer)
                Amount = 1;

            int AmountPurchase = Item.Amount > 1 ? Item.Amount : Amount;

            int TotalCreditsCost = Amount > 1 ? ((Item.CostCredits * Amount) - ((int)Math.Floor((double)Amount / 6) * Item.CostCredits)) : Item.CostCredits;
            int TotalPixelCost = Amount > 1 ? ((Item.CostPixels * Amount) - ((int)Math.Floor((double)Amount / 6) * Item.CostPixels)) : Item.CostPixels;
            int TotalDiamondCost = Amount > 1 ? ((Item.CostDiamonds * Amount) - ((int)Math.Floor((double)Amount / 6) * Item.CostDiamonds)) : Item.CostDiamonds;
            int TotalGotwCost = Amount > 1 ? ((Item.CostGotw * Amount) - ((int)Math.Floor((double)Amount / 6) * Item.CostGotw)) : Item.CostGotw;
			
            if (Session.GetHabbo().Credits < TotalCreditsCost || Session.GetHabbo().Duckets < TotalPixelCost || Session.GetHabbo().Diamonds < TotalDiamondCost || Session.GetHabbo().GOTWPoints < TotalGotwCost)
                return;

            int LimitedEditionSells = 0;
            int LimitedEditionStack = 0;

            #region PREDESIGNED_ROOM
            if (Item.PredesignedId > 0 && RetroEnvironment.GetGame().GetCatalog().GetPredesignedRooms().predesignedRoom.ContainsKey((uint)Item.PredesignedId))
            {
                if (Item.CostCredits > Session.GetHabbo().Credits)
                    return;

                if (Item.CostCredits > 0)
                {
                    Session.GetHabbo().Credits -= Item.CostCredits;
                    Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits));
                    Session.GetHabbo().CreditsSpend = Item.CostCredits + Session.GetHabbo().CreditsSpend;
                }

                if (Item.CostPixels > 0)
                {
                    Session.GetHabbo().Duckets -= Item.CostPixels;
                    Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Duckets, Session.GetHabbo().Duckets));//Love you, Tom.
                }

                if (Item.CostDiamonds > 0)
                {
                    Session.GetHabbo().Diamonds -= Item.CostDiamonds;
                    Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, 0, 5));
                }

                if (Item.CostGotw > 0)
                {
                    Session.GetHabbo().GOTWPoints -= Item.CostGotw;
                    Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().GOTWPoints, 0, 103));
                }
                #region SELECT ROOM AND CREATE NEW
                var predesigned = RetroEnvironment.GetGame().GetCatalog().GetPredesignedRooms().predesignedRoom[(uint)Item.PredesignedId];
                var decoration = predesigned.RoomDecoration;

                var createRoom = RetroEnvironment.GetGame().GetRoomManager().CreateRoom(Session, "Bundel: " + Item.Name + " !", "Dit is een kamerpakket Gekocht in de winkel van Habbie!", predesigned.RoomModel, 1, 25, 1);

                createRoom.FloorThickness = int.Parse(decoration[0]);
                createRoom.WallThickness = int.Parse(decoration[1]);
                createRoom.Model.WallHeight = int.Parse(decoration[2]);
                createRoom.Hidewall = ((decoration[3] == "True") ? 1 : 0);
                createRoom.Wallpaper = decoration[4];
                createRoom.Landscape = decoration[5];
                createRoom.Floor = decoration[6];
                var newRoom = RetroEnvironment.GetGame().GetRoomManager().LoadRoom(createRoom.Id);
                #endregion

                #region CREATE FLOOR ITEMS
                if (predesigned.FloorItems != null)
                    foreach (var floorItems in predesigned.FloorItemData)
                        using (var dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
                            dbClient.RunQuery("INSERT INTO items VALUES (null, " + Session.GetHabbo().Id + ", " + newRoom.RoomId + ", " + floorItems.BaseItem + ", '" + floorItems.ExtraData + "', " +
                                floorItems.X + ", " + floorItems.Y + ", " + TextHandling.GetString(floorItems.Z) + ", " + floorItems.Rot + ", '', 0, 0);");
                #endregion

                #region CREATE WALL ITEMS
                if (predesigned.WallItems != null)
                    foreach (var wallItems in predesigned.WallItemData)
                        using (var dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
                            dbClient.RunQuery("INSERT INTO items VALUES (null, " + Session.GetHabbo().Id + ", " + newRoom.RoomId + ", " + wallItems.BaseItem + ", '" + wallItems.ExtraData +
                                "', 0, 0, 0, 0, '" + wallItems.WallCoord + "', 0, 0);");
                #endregion

                #region VERIFY IF CONTAINS BADGE AND GIVE
                if (Item.Badge != string.Empty) Session.GetHabbo().GetBadgeComponent().GiveBadge(Item.Badge, true, Session);
                #endregion

                #region GENERATE ROOM AND SEND PACKET
                Session.SendMessage(new PurchaseOKComposer());
                Session.GetHabbo().GetInventoryComponent().UpdateItems(false);
                RetroEnvironment.GetGame().GetRoomManager().LoadRoom(newRoom.Id).GetRoomItemHandler().LoadFurniture();
                var newFloorItems = newRoom.GetRoomItemHandler().GetFloor;
                foreach (var roomItem in newFloorItems) newRoom.GetRoomItemHandler().SetFloorItem(roomItem, roomItem.GetX, roomItem.GetY, roomItem.GetZ);
                var newWallItems = newRoom.GetRoomItemHandler().GetWall;
                foreach (var roomItem in newWallItems) newRoom.GetRoomItemHandler().SetWallItem(Session, roomItem);
                Session.SendMessage(new FlatCreatedComposer(newRoom.Id, newRoom.Name));
                #endregion
                return;
            }
            #endregion

            #region Create the extradata
            switch (Item.Data.InteractionType)
            {
                case InteractionType.NONE:
                    ExtraData = "";
                    break;

                case InteractionType.GUILD_FORUM:
                    Group Gp;
                    GroupForum Gf;
                    int GpId;
                    if (!int.TryParse(ExtraData, out GpId))
                    {
                        Session.SendNotification("Oops, er is een fout opgetreden bij het ophalen van de groeps-ID");
                        Session.SendMessage(new PurchaseOKComposer());
                        return;
                    }
                    if (!RetroEnvironment.GetGame().GetGroupManager().TryGetGroup(GpId, out Gp))
                    {
                        Session.SendNotification("Oops, dit ID bestaat niet!");
                        Session.SendMessage(new PurchaseOKComposer());
                        return;
                    }

                    if (Gp.CreatorId != Session.GetHabbo().Id)
                    {
                        Session.SendNotification("Oops! U bent niet de eigenaar van de groep. \n\n Een forum moet door de groepseigenaar worden gemaakt ...");
                        Session.SendMessage(new PurchaseOKComposer());
                        return;
                    }

                    Gf = RetroEnvironment.GetGame().GetGroupForumManager().CreateGroupForum(Gp);
                    Session.SendMessage(new RoomNotificationComposer("forums.delivered", new Dictionary<string, string>
                            { { "groupId", Gp.Id.ToString() },  { "groupName", Gp.Name } }));
                    break;

                case InteractionType.GUILD_FORUM_CHAT:
                    Group thegroup;
                    Group Group = null;
                    if (!RetroEnvironment.GetGame().GetGroupManager().TryGetGroup(Convert.ToInt32(ExtraData), out thegroup))
                        return;
                    if (!(RetroEnvironment.GetGame().GetGroupManager().GetGroupsForUser(Session.GetHabbo().Id).Contains(thegroup)))
                    {
                        return;
                    }

                    int groupID = Convert.ToInt32(ExtraData);
                    if (thegroup.CreatorId != Session.GetHabbo().Id)
                    {
                        Session.SendNotification("Oops! Je bent niet de eigenaar van de groep, dus je kunt niet een groepschat kopen voor deze groep!");
                        Session.SendMessage(new PurchaseOKComposer());
                        return;
                    }

                    using (IQueryAdapter dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("UPDATE groups SET has_chat = '1' WHERE id = @id");
                        dbClient.AddParameter("id", groupID);
                        dbClient.RunQuery();
                    }

                    RetroEnvironment.GetGame().GetClientManager().GetClientByUserID(Session.GetHabbo().Id).SendMessage(new FriendListUpdateComposer(Group, 1));
                    Session.SendNotification("Succesvol uw groupschat aangemaakt.");
                    Session.SendMessage(new PurchaseOKComposer());

                    break;

                case InteractionType.GUILD_ITEM:
                case InteractionType.GUILD_GATE:
                case InteractionType.HCGATE:
                case InteractionType.VIPGATE:
                    break;

                case InteractionType.PINATA:
                case InteractionType.PINATATRIGGERED:
                case InteractionType.MAGICEGG:
                case InteractionType.MAGICCHEST:
                    ExtraData = "0";
                    break;

                #region Pet handling

                case InteractionType.PET:
                    try
                    {
                        string[] Bits = ExtraData.Split('\n');
                        string PetName = Bits[0];
                        string Race = Bits[1];
                        string Color = Bits[2];

                        int.Parse(Race); // to trigger any possible errors

                        if (!PetUtility.CheckPetName(PetName))
                            return;

                        if (Race.Length > 2)
                            return;

                        if (Color.Length != 6)
                            return;

                        RetroEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_PetLover", 1);
                    }
                    catch (Exception e)
                    {
                        ExceptionLogger.LogException(e);
                        return;
                    }

                    break;

                #endregion

                case InteractionType.FLOOR:
                case InteractionType.WALLPAPER:
                case InteractionType.LANDSCAPE:

                    Double Number = 0;

                    try
                    {
                        if (string.IsNullOrEmpty(ExtraData))
                            Number = 0;
                        else
                            Number = Double.Parse(ExtraData, RetroEnvironment.CultureInfo);
                    }
                    catch (Exception e)
                    {
                        ExceptionLogger.LogException(e);
                    }

                    ExtraData = Number.ToString().Replace(',', '.');
                    break; // maintain extra data // todo: validate

                case InteractionType.POSTIT:
                    ExtraData = "FFFF33";
                    break;

                case InteractionType.MOODLIGHT:
                    ExtraData = "1,1,1,#000000,255";
                    break;

                case InteractionType.TROPHY:
                    ExtraData = Session.GetHabbo().Username + Convert.ToChar(9) + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + Convert.ToChar(9) + ExtraData;
                    break;

                case InteractionType.MANNEQUIN:
                    ExtraData = "m" + Convert.ToChar(5) + ".ch-210-1321.lg-285-92" + Convert.ToChar(5) + "Default Mannequin";
                    break;

                case InteractionType.FOOTBALL_GATE:
                    ExtraData = "hd-99999-99999.lg-270-62;hd-99999-99999.ch-630-62.lg-695-62";
                    break;

                case InteractionType.vikingtent:
                    ExtraData = "0";
                    break;

                case InteractionType.BADGE_DISPLAY:
                    if (!Session.GetHabbo().GetBadgeComponent().HasBadge(ExtraData))
                    {
                        Session.SendMessage(new BroadcastMessageAlertComposer("Het lijkt erop dat u deze badge niet bezit!"));
                        return;
                    }

                    ExtraData = ExtraData + Convert.ToChar(9) + Session.GetHabbo().Username + Convert.ToChar(9) + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;
                    break;

                case InteractionType.BADGE:
                    {
                        if (Session.GetHabbo().GetBadgeComponent().HasBadge(Item.Data.ItemName))
                        {
                            Session.SendMessage(new PurchaseErrorComposer(1));
                            return;
                        }
                        break;
                    }
                default:
                    ExtraData = "";
                    break;
            }
            #endregion


            if (Item.IsLimited)
            {
                if (Item.LimitedEditionStack <= Item.LimitedEditionSells)
                {
                    Session.SendNotification("Dit item is uitverkocht!\n\n" + "Houd er rekening mee dat u geen ander artikel heeft ontvangen (Er zijn hiervoor geen kosten in rekening gebracht!)");
                    Session.SendMessage(new CatalogUpdatedComposer());
                    Session.SendMessage(new PurchaseOKComposer());
                    return;
                }

                Item.LimitedEditionSells++;
                using (IQueryAdapter dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("UPDATE `catalog_items` SET `limited_sells` = @limitSells WHERE `id` = @itemId LIMIT 1");
                    dbClient.AddParameter("limitSells", Item.LimitedEditionSells);
                    dbClient.AddParameter("itemId", Item.Id);
                    dbClient.RunQuery();

                    LimitedEditionSells = Item.LimitedEditionSells;
                    LimitedEditionStack = Item.LimitedEditionStack;
                }

                if (Session.GetHabbo().Rank == 1)
                {
                    //RetroEnvironment.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + Item.Data.ItemName + "_icon", 3, "De gebruiker " + Session.GetHabbo().Username + " kocht de zeldzame LTD: " + Item.Name + "  Slot: " + Item.LimitedEditionSells + "/" + Item.LimitedEditionStack, "!"));
                }
            }

            if (Item.CostCredits > 0)
            {
                Session.GetHabbo().Credits -= TotalCreditsCost;
                Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits));
                Session.GetHabbo().CreditsSpend = Item.CostCredits + Session.GetHabbo().CreditsSpend;
            }

            if (Item.CostPixels > 0)
            {
                Session.GetHabbo().Duckets -= TotalPixelCost;
                Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Duckets, Session.GetHabbo().Duckets));//Love you, ME.
            }

            if (Item.CostDiamonds > 0)
            {
                Session.GetHabbo().Diamonds -= TotalDiamondCost;
                Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, 0, 5));
            }

            if (Item.CostGotw > 0)
            {
                Session.GetHabbo().GOTWPoints -= TotalGotwCost;
                Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().GOTWPoints, 0, 103));
            }

            Item NewItem = null;
            switch (Item.Data.Type.ToString().ToLower())
            {
                default:
                    List<Item> GeneratedGenericItems = new List<Item>();

                    switch (Item.Data.InteractionType)
                    {
                        default:
                            if (AmountPurchase > 1)
                            {
                                List<Item> Items = ItemFactory.CreateMultipleItems(Item.Data, Session.GetHabbo(), ExtraData, AmountPurchase);

                                if (Items != null)
                                {
                                    GeneratedGenericItems.AddRange(Items);
                                }
                            }
                            else
                            {
                                NewItem = ItemFactory.CreateSingleItemNullable(Item.Data, Session.GetHabbo(), ExtraData, ExtraData, 0, LimitedEditionSells, LimitedEditionStack);

                                if (NewItem != null)
                                {
                                    GeneratedGenericItems.Add(NewItem);
                                }
                            }
                            break;

                        case InteractionType.GUILD_GATE:
                        case InteractionType.GUILD_ITEM:
                        case InteractionType.GUILD_FORUM:
                            int groupId = 0;
                            int.TryParse(ExtraData, out groupId);
                            if (AmountPurchase > 1)
                            {
                                List<Item> Items = ItemFactory.CreateMultipleItems(Item.Data, Session.GetHabbo(), ExtraData, AmountPurchase, groupId);

								if (Items != null)
								{
									GeneratedGenericItems.AddRange(Items);
								}
							}
							else
							{
								NewItem = ItemFactory.CreateSingleItemNullable(Item.Data, Session.GetHabbo(), ExtraData, ExtraData, groupId);

								if (NewItem != null)
								{
									GeneratedGenericItems.Add(NewItem);
								}
							}
							break;

						case InteractionType.MUSIC_DISC:
                            string flags = Convert.ToString(Item.ExtradataInt);
                            if (AmountPurchase > 1)
                            {
                                List<Item> Items = ItemFactory.CreateMultipleItems(Item.Data, Session.GetHabbo(), flags, AmountPurchase);

                                if (Items != null)
                                {
                                    GeneratedGenericItems.AddRange(Items);
                                    RetroEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_MusicCollector", 1);
                                }
                            }
                            else
                            {
                                NewItem = ItemFactory.CreateSingleItemNullable(Item.Data, Session.GetHabbo(), flags, flags);

                                if (NewItem != null)
                                {
                                    GeneratedGenericItems.Add(NewItem);
                                    RetroEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_MusicCollector", 1);
                                }
                            }
                            break;

                        case InteractionType.ARROW:
                        case InteractionType.TELEPORT:
                            for (int i = 0; i < AmountPurchase; i++)
                            {
                                List<Item> TeleItems = ItemFactory.CreateTeleporterItems(Item.Data, Session.GetHabbo());

                                if (TeleItems != null)
                                {
                                    GeneratedGenericItems.AddRange(TeleItems);
                                }
                            }
                            break;

                        case InteractionType.MOODLIGHT:
                            {
                                if (AmountPurchase > 1)
                                {
                                    List<Item> Items = ItemFactory.CreateMultipleItems(Item.Data, Session.GetHabbo(), ExtraData, AmountPurchase);

                                    if (Items != null)
                                    {
                                        GeneratedGenericItems.AddRange(Items);
                                        foreach (Item I in Items)
                                        {
                                            ItemFactory.CreateMoodlightData(I);
                                        }
                                    }
                                }
                                else
                                {
                                    NewItem = ItemFactory.CreateSingleItemNullable(Item.Data, Session.GetHabbo(), ExtraData, ExtraData);

                                    if (NewItem != null)
                                    {
                                        GeneratedGenericItems.Add(NewItem);
                                        ItemFactory.CreateMoodlightData(NewItem);
                                    }
                                }
                            }
                            break;
                

                        case InteractionType.TONER:
                            {
                                if (AmountPurchase > 1)
                                {
                                    List<Item> Items = ItemFactory.CreateMultipleItems(Item.Data, Session.GetHabbo(), ExtraData, AmountPurchase);

                                    if (Items != null)
                                    {
                                        GeneratedGenericItems.AddRange(Items);
                                        foreach (Item I in Items)
                                        {
                                            ItemFactory.CreateTonerData(I);
                                        }
                                    }
                                }
                                else
                                {
                                    NewItem = ItemFactory.CreateSingleItemNullable(Item.Data, Session.GetHabbo(), ExtraData, ExtraData);

                                    if (NewItem != null)
                                    {
                                        GeneratedGenericItems.Add(NewItem);
                                        ItemFactory.CreateTonerData(NewItem);
                                    }
                                }
                            }
                            break;

                        case InteractionType.DEAL:
                            {
                                //Fetch the deal where the ID is this items ID.
                                var DealItems = (from d in Page.Deals.Values.ToList() where d.Id == Item.Id select d);

                                //This bit, iterating ONE item? How can I make this simpler
                                foreach (CatalogDeal DealItem in DealItems)
                                {
                                    //Here I loop the DealItems ItemDataList.
                                    foreach (CatalogItem CatalogItem in DealItem.ItemDataList.ToList())
                                    {
                                        List<Item> Items = ItemFactory.CreateMultipleItems(CatalogItem.Data, Session.GetHabbo(), "", AmountPurchase);

                                        if (Items != null)
                                        {
                                            GeneratedGenericItems.AddRange(Items);
                                        }
                                    }
                                }
                                break;
                            }

                    }

                    foreach (Item PurchasedItem in GeneratedGenericItems)
                    {
                        if (Session.GetHabbo().GetInventoryComponent().TryAddItem(PurchasedItem))
                        {
                            //Session.SendMessage(new FurniListAddComposer(PurchasedItem));
                            Session.SendMessage(new FurniListNotificationComposer(PurchasedItem.Id, 1));
                        }
                    }
                    break;

                case "e":
                    AvatarEffect Effect = null;

                    if (Session.GetHabbo().Effects().HasEffect(Item.Data.SpriteId))
                    {
                        Effect = Session.GetHabbo().Effects().GetEffectNullable(Item.Data.SpriteId);

                        if (Effect != null)
                        {
                            Effect.AddToQuantity();
                        }
                    }
                    else
                        Effect = AvatarEffectFactory.CreateNullable(Session.GetHabbo(), Item.Data.SpriteId, 3600);

                    if (Effect != null)// && Session.GetHabbo().Effects().TryAdd(Effect))
                    {
                        Session.SendMessage(new AvatarEffectAddedComposer(Item.Data.SpriteId, 3600));
                        Session.GetHabbo().Effects().ApplyEffect(Item.Data.SpriteId);
                        Session.SendMessage(new RoomNotificationComposer("furni_placement_error", "message", "U koopt het effect " + Item.Data.SpriteId + " in de winkel, onthoud dat je het effect op je avatar hebt opgedaan, maar ga pas naar je inventaris nadat U heeft gerelogd"));

                    }
                    break;

                case "r":
                    Bot Bot = BotUtility.CreateBot(Item.Data, Session.GetHabbo().Id);
                    if (Bot != null)
                    {
                        Session.GetHabbo().GetInventoryComponent().TryAddBot(Bot);
                        Session.SendMessage(new BotInventoryComposer(Session.GetHabbo().GetInventoryComponent().GetBots()));
                        Session.SendMessage(new FurniListNotificationComposer(Bot.Id, 5));
                    }
                    else
                        Session.SendNotification("Er is een fout opgetreden bij het kopen van dit item!");
                    break;

                case "b":
                    {
                        Session.GetHabbo().GetBadgeComponent().GiveBadge(Item.Data.ItemName, true, Session);
                        Session.SendMessage(new FurniListNotificationComposer(0, 4));
                        break;
                    }

                case "p":
                    {
                        string[] PetData = ExtraData.Split('\n');
                        
                        Pet GeneratedPet = PetUtility.CreatePet(Session.GetHabbo().Id, PetData[0], Item.Data.BehaviourData, PetData[1], PetData[2]);
                        if (GeneratedPet != null)
                        {
                            Session.GetHabbo().GetInventoryComponent().TryAddPet(GeneratedPet);

                            Session.SendMessage(new FurniListNotificationComposer(GeneratedPet.PetId, 3));
                            Session.SendMessage(new PetInventoryComposer(Session.GetHabbo().GetInventoryComponent().GetPets()));

							if (RetroEnvironment.GetGame().GetItemManager().GetItem(320, out ItemData PetFood))
							{
								Item Food = ItemFactory.CreateSingleItemNullable(PetFood, Session.GetHabbo(), "", "");
								if (Food != null)
								{
									Session.GetHabbo().GetInventoryComponent().TryAddItem(Food);
									Session.SendMessage(new FurniListNotificationComposer(Food.Id, 1));

                                    Session.SendNotification("Zo te zien heb je zonet een huisdier gekocht. Wij als Habbie Management willen u graag vermelden dat deze nog bugs bevatten.");
                                }
							}
						}
                        break;
                    }
            }

            if (Item.Badge != string.Empty) Session.GetHabbo().GetBadgeComponent().GiveBadge(Item.Badge, true, Session);
            Session.SendMessage(new PurchaseOKComposer(Item, Item.Data, Item.Items));
            Session.SendMessage(new FurniListUpdateComposer());
           
            }
        }
    }