using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Etap.Core;
using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;
using Etap.Communication.Packets.Outgoing.Handshake;
using Etap.Communication.Packets.Outgoing;
using Etap.Utilities;
using Etap.Communication.Packets.Outgoing.BuildersClub;
using Etap.Communication.Packets.Outgoing.Inventory.Achievements;
using Etap.Communication.Packets.Outgoing.Inventory.AvatarEffects;
using Etap.Communication.Packets.Outgoing.Misc;
using Etap.Communication.Packets.Outgoing.Moderation;
using Etap.Communication.Packets.Outgoing.Navigator;
using Etap.Communication.Packets.Outgoing.Notifications;
using Etap.Communication.Packets.Incoming.Navigator;
using Retro.Communication.Packets.Incoming.Inventory.Achievements;
using Retro.Communication.Packets.Incoming.Sound;
using Etap.Communication.Packets.Outgoing.Users;
using Retro.Communication.Packets.Incoming.Inventory.AvatarEffects;
using Etap.Communication.Packets.Incoming.Rooms.Chat;
using Etap.Communication.Packets.Incoming.Rooms.Notifications;
using Retro.Communication.Packets.Incoming.Inventory.Furni;
using Etap.Communication.Packets.Incoming.Messenger;
using Etap.Communication.Packets.Incoming.Handshake;
using Retro.Communication.Packets.Outgoing.Catalog;
using Etap.Communication.Packets.Incoming.Catalog;
using Retro.Communication.Packets.Incoming.Inventory.Badges;
using Retro.Communication.Packets.Incoming.Inventory.Purse;

namespace Etap.Communication.Packets
{
    public sealed class PacketManager
    {

        /// <summary>
        ///     Testing the Task code
        /// </summary>
        private readonly bool IgnoreTasks = true;

        /// <summary>
        ///     The maximum time a task can run for before it is considered dead
        ///     (can be used for debugging any locking issues with certain areas of code)
        /// </summary>
        private readonly int MaximumRunTimeInSec = 300; // 5 minutes

        /// <summary>
        ///     Should the handler throw errors or log and continue.
        /// </summary>
        private readonly bool ThrowUserErrors = false;

        /// <summary>
        ///     The task factory which is used for running Asynchronous tasks, in this case we use it to execute packets.
        /// </summary>
        private readonly TaskFactory _eventDispatcher;

        private readonly Dictionary<int, IPacketEvent> _incomingPackets;
        private readonly Dictionary<int, string> _packetNames;

        /// <summary>
        ///     Currently running tasks to keep track of what the current load is
        /// </summary>
        private readonly ConcurrentDictionary<int, Task> _runningTasks;

        public PacketManager()
        {
            _incomingPackets = new Dictionary<int, IPacketEvent>();

            _eventDispatcher = new TaskFactory(TaskCreationOptions.PreferFairness, TaskContinuationOptions.None);
            _runningTasks = new ConcurrentDictionary<int, Task>();
            _packetNames = new Dictionary<int, string>();

            RegisterNames();

            RegisterBuildersClub();
            RegisterCatalog();
            RegisterHandshake();
            RegisterInventory();
            RegisterMessenger();
            RegisterMisc();
            RegisterModeration();
            RegisterNavigator();
            RegisterNotifications();
            RegisterRooms();
            RegisterSound();
            RegisterUsers();
        }

        public void TryExecutePacket(GameClient Session, ClientPacket Packet)
        {
            if (!_incomingPackets.TryGetValue(Packet.Id, out IPacketEvent Pak))
            {
                if (System.Diagnostics.Debugger.IsAttached || ExtraSettings.DEBUG_ENABLED == true)
                    Logger.Debug("Unknown Packet: " + Packet.ToString());
                return;
            }

            if (System.Diagnostics.Debugger.IsAttached || ExtraSettings.DEBUG_ENABLED == true)
            {
                if (_packetNames.ContainsKey(Packet.Id))
                    Logger.Debug("Received Packet: [" + Packet.Id + "] " + _packetNames[Packet.Id]);
                else
                    Logger.Debug("Received Packet: [" + Packet.Id + "] UnnamedPacketEvent");
            }

            if (!IgnoreTasks)
                ExecutePacketAsync(Session, Packet, Pak);
            else
                Pak.Parse(Session, Packet);
        }

        private void ExecutePacketAsync(GameClient Session, ClientPacket Packet, IPacketEvent Pak)
        {
            DateTime Start = DateTime.Now;

            var CancelSource = new CancellationTokenSource();
            CancellationToken Token = CancelSource.Token;

            Task t = _eventDispatcher.StartNew(() =>
            {
                Pak.Parse(Session, Packet);
                Token.ThrowIfCancellationRequested();
            }, Token);

            _runningTasks.TryAdd(t.Id, t);

            try
            {
                if (!t.Wait(MaximumRunTimeInSec * 1000, Token))
                {
                    CancelSource.Cancel();
                }
            }
            catch (AggregateException ex)
            {
                foreach (Exception e in ex.Flatten().InnerExceptions)
                {
                    if (ThrowUserErrors)
                    {
                        throw e;
                    }
                    else
                    {
                        //log.Fatal("Unhandled Error: " + e.Message + " - " + e.StackTrace);
                        Session.Disconnect();
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Session.Disconnect();
            }
            finally
            {
                _runningTasks.TryRemove(t.Id, out Task RemovedTask);

                CancelSource.Dispose();

                //log.Debug("Event took " + (DateTime.Now - Start).Milliseconds + "ms to complete.");
            }
        }

        public void WaitForAllToComplete()
        {
            foreach (Task t in _runningTasks.Values.ToList())
            {
                t.Wait();
            }
        }

        public void UnregisterAll()
        {
            _incomingPackets.Clear();
        }

        #region Init Packet Classes
        private void RegisterBuildersClub()
        {
            _incomingPackets.Add(ClientPacketHeader.BCBorrowedItemsMessageComposer, new BCBorrowedItemsComposer());
            _incomingPackets.Add(ClientPacketHeader.BuildersClubMembershipMessageComposer, new BuildersClubMembershipComposer());
        }
        private void RegisterHandshake()
        {
            _incomingPackets.Add(ClientPacketHeader.GetUserDataComposer, new GetUserDataComposer());
            _incomingPackets.Add(ClientPacketHeader.AuthenticationOKMessageComposer, new AuthenticationOKComposer());
            _incomingPackets.Add(ClientPacketHeader.AvailabilityStatusMessageComposer, new AvailabilityStatusComposer());
            _incomingPackets.Add(ClientPacketHeader.GenericErrorMessageComposer, new GenericErrorComposer());
            _incomingPackets.Add(ClientPacketHeader.InitCryptoMessageComposer, new InitCryptoComposer());
            _incomingPackets.Add(ClientPacketHeader.PongMessageComposer, new PongComposer());
            _incomingPackets.Add(ClientPacketHeader.SecretKeyMessageComposer, new SecretKeyComposer());
            _incomingPackets.Add(ClientPacketHeader.SetUniqueIdMessageComposer, new SetUniqueIdComposer());
            _incomingPackets.Add(ClientPacketHeader.UserObjectMessageComposer, new UserObjectComposer());
            _incomingPackets.Add(ClientPacketHeader.UserPerksMessageComposer, new UserPerksComposer());
            _incomingPackets.Add(ClientPacketHeader.UserRightsMessageComposer, new UserRightsComposer());
        }
        private void RegisterCatalog()
        {
            _incomingPackets.Add(ClientPacketHeader.CatalogPreferencesMessageComposer, new CatalogPreferencesComposer());
            _incomingPackets.Add(ClientPacketHeader.CatalogItemDiscountMessageComposer, new CatalogItemDiscountComposer());
            _incomingPackets.Add(ClientPacketHeader.CatalogIndexMessageComposer, new CatalogIndexComposer());
        }
        private void RegisterInventory()
        {
            //Achievements
            _incomingPackets.Add(ClientPacketHeader.AchievementProgressedMessageComposer, new AchievementProgressedComposer());
            _incomingPackets.Add(ClientPacketHeader.AchievementScoreMessageComposer, new AchievementScoreComposer());
            _incomingPackets.Add(ClientPacketHeader.BadgeDefinitionsMessageComposer, new BadgeDefinitionsComposer());
            _incomingPackets.Add(ClientPacketHeader.AchievementUnlockedMessageComposer, new AchievementUnlockedComposer());

            //AvatarEffects
            _incomingPackets.Add(ClientPacketHeader.AvatarEffectsMessageComposer, new AvatarEffectsComposer());
            _incomingPackets.Add(ClientPacketHeader.FigureSetIdsMessageComposer, new FigureSetIdsComposer());

            //Badges
            _incomingPackets.Add(ClientPacketHeader.BadgesMessageComposer, new BadgesComposer());

            //Furni
            _incomingPackets.Add(ClientPacketHeader.FurniListMessageComposer, new FurniListComposer());
            _incomingPackets.Add(ClientPacketHeader.FurniListNotificationMessageComposer, new FurniListNotificationComposer());

            //Purse
            _incomingPackets.Add(ClientPacketHeader.HabboActivityPointNotificationMessageComposer, new HabboActivityPointNotificationComposer());
        }
        private void RegisterMessenger()
        {
            //Friends
            _incomingPackets.Add(ClientPacketHeader.BuddyRequestsMessageComposer, new BuddyRequestsComposer());
            _incomingPackets.Add(ClientPacketHeader.BuddyListMessageComposer, new BuddyListComposer());

            //Messenger
            _incomingPackets.Add(ClientPacketHeader.MessengerInitMessageComposer, new MessengerInitComposer());
        }
        private void RegisterMisc()
        {
            _incomingPackets.Add(ClientPacketHeader.LatencyResponseMessageComposer, new LatencyTestComposer());
        }
        private void RegisterModeration()
        {
            _incomingPackets.Add(ClientPacketHeader.CfhTopicsInitMessageComposer, new CfhTopicsInitComposer());
            _incomingPackets.Add(ClientPacketHeader.ModeratorInitMessageComposer, new ModeratorInitComposer());
            _incomingPackets.Add(ClientPacketHeader.ModeratorSupportTicketMessageComposer, new ModeratorSupportTicketComposer());
        }
        private void RegisterNavigator()
        {
            _incomingPackets.Add(ClientPacketHeader.FavouritesMessageComposer, new FavouritesComposer());
            _incomingPackets.Add(ClientPacketHeader.NavigatorCollapsedCategoriesMessageComposer, new NavigatorCollapsedCategoriesComposer());
            _incomingPackets.Add(ClientPacketHeader.NavigatorLiftedRoomsMessageComposer, new NavigatorLiftedRoomsComposer());
            _incomingPackets.Add(ClientPacketHeader.NavigatorMetaDataParserMessageComposer, new NavigatorMetaDataParserComposer());
            _incomingPackets.Add(ClientPacketHeader.NavigatorPreferencesMessageComposer, new NavigatorPreferencesComposer());
            _incomingPackets.Add(ClientPacketHeader.NavigatorSettingsMessageComposer, new NavigatorSettingsComposer());
            _incomingPackets.Add(ClientPacketHeader.UserFlatCatsMessageComposer, new UserFlatCatsComposer());
            _incomingPackets.Add(ClientPacketHeader.NavigatorFlatCatsMessageComposer, new NavigatorFlatCatsComposer());
            _incomingPackets.Add(ClientPacketHeader.NavigatorSearchResultSetMessageComposer, new NavigatorSearchResultSetComposer());
        }
        private void RegisterNotifications()
        {
            _incomingPackets.Add(ClientPacketHeader.MOTDNotificationMessageComposer, new MOTDNotificationComposer());
        }
        private void RegisterRooms()
        {
            //Chat
            _incomingPackets.Add(ClientPacketHeader.ChatMessageComposer, new ChatComposer());
            _incomingPackets.Add(ClientPacketHeader.FloodControlMessageComposer, new FloodControlComposer());
            _incomingPackets.Add(ClientPacketHeader.ShoutMessageComposer, new ShoutComposer());
            _incomingPackets.Add(ClientPacketHeader.UserTypingMessageComposer, new UserTypingComposer());
            _incomingPackets.Add(ClientPacketHeader.WhisperMessageComposer, new WhisperComposer());

            //Notifications
            _incomingPackets.Add(ClientPacketHeader.AlertNotificationHCMessageComposer, new AlertNotificationHCMessageComposer());
            _incomingPackets.Add(ClientPacketHeader.HCGiftsAlertComposer, new HCGiftsAlertComposer());
            _incomingPackets.Add(ClientPacketHeader.MassEventComposer, new MassEventComposer());
            _incomingPackets.Add(ClientPacketHeader.RoomCustomizedAlertComposer, new RoomCustomizedAlertComposer());
            _incomingPackets.Add(ClientPacketHeader.RoomErrorNotifMessageComposer, new RoomErrorNotifComposer());
            _incomingPackets.Add(ClientPacketHeader.RoomNotificationMessageComposer, new RoomNotificationComposer());

        }
        private void RegisterSound()
        {
            _incomingPackets.Add(ClientPacketHeader.SoundSettingsMessageComposer, new SoundSettingsComposer());
        }
        private void RegisterUsers()
        {
            _incomingPackets.Add(ClientPacketHeader.ScrSendUserInfoMessageComposer, new ScrSendUserInfoComposer());
        }
        #endregion

        #region Init Packet Names
        public void RegisterNames()
        {
            #region Availability
            _packetNames.Add(ClientPacketHeader.MaintenanceStatusMessageComposer, "MaintenanceStatusMessageComposer");
            #endregion
            #region Avatar
            ///_packetNames.Add(ClientPacketHeader.WardrobeMessageComposer, "WardrobeMessageComposer"); ID num already exsist
            #endregion
            #region Builders Club
            _packetNames.Add(ClientPacketHeader.BCBorrowedItemsMessageComposer, "BCBorrowedItemsMessageComposer");
            _packetNames.Add(ClientPacketHeader.BuildersClubMembershipMessageComposer, "BuildersClubMembershipMessageComposer");
            #endregion
            #region Campaigns
            _packetNames.Add(ClientPacketHeader.CampaignCalendarGiftMessageComposer, "CampaignCalendarGiftMessageComposer");
            #endregion
            #region Catalog
            _packetNames.Add(ClientPacketHeader.CatalogPreferencesMessageComposer, "CatalogPreferencesMessageComposer");
            _packetNames.Add(ClientPacketHeader.CatalogIndexMessageComposer, "CatalogIndexMessageComposer");
            _packetNames.Add(ClientPacketHeader.CatalogItemDiscountMessageComposer, "CatalogItemDiscountMessageComposer");
            _packetNames.Add(ClientPacketHeader.CatalogOfferMessageComposer, "CatalogOfferMessageComposer");
            _packetNames.Add(ClientPacketHeader.CatalogPageMessageComposer, "CatalogPageMessageComposer");
            _packetNames.Add(ClientPacketHeader.CatalogUpdatedMessageComposer, "CatalogUpdatedMessageComposer");
            _packetNames.Add(ClientPacketHeader.CheckGnomeNameMessageComposer, "CheckGnomeNameMessageComposer");
            _packetNames.Add(ClientPacketHeader.CheckPetNameMessageComposer, "CheckPetNameMessageComposer");
            _packetNames.Add(ClientPacketHeader.ClubGiftsMessageComposer, "ClubGiftsMessageComposer");
            ///_packetNames.Add(ClientPacketHeader.GetCatalogRoomPromotionComposer, "GetCatalogRoomPromotionComposer");
            _packetNames.Add(ClientPacketHeader.GiftWrappingConfigurationMessageComposer, "GetCatalogRoomPromotionComposer");
            _packetNames.Add(ClientPacketHeader.GiftWrappingErrorMessageComposer, "GiftWrappingErrorMessageComposer");
            _packetNames.Add(ClientPacketHeader.GroupFurniConfigMessageComposer, "GroupFurniConfigMessageComposer");
            _packetNames.Add(ClientPacketHeader.MarketplaceConfigurationMessageComposer, "MarketplaceConfigurationMessageComposer");
            _packetNames.Add(ClientPacketHeader.PresentDeliverErrorMessageComposer, "PresentDeliverErrorMessageComposer");
            _packetNames.Add(ClientPacketHeader.PromotableRoomsMessageComposer, "PromotableRoomsMessageComposer");
            _packetNames.Add(ClientPacketHeader.PurchaseErrorMessageComposer, "PurchaseErrorMessageComposer");
            _packetNames.Add(ClientPacketHeader.PurchaseOKMessageComposer, "PurchaseOKMessageComposer");
            _packetNames.Add(ClientPacketHeader.RecyclerRewardsMessageComposer, "RecyclerRewardsMessageComposer");
            _packetNames.Add(ClientPacketHeader.RecyclerStateComposer, "RecyclerStateComposer");
            _packetNames.Add(ClientPacketHeader.ReloadRecyclerComposer, "ReloadRecyclerComposer");
            _packetNames.Add(ClientPacketHeader.SellablePetBreedsMessageComposer, "SellablePetBreedsMessageComposer");
            _packetNames.Add(ClientPacketHeader.VoucherRedeemErrorMessageComposer, "VoucherRedeemErrorMessageComposer");
            _packetNames.Add(ClientPacketHeader.VoucherRedeemOkMessageComposer, "VoucherRedeemOkMessageComposer");
            #endregion
            #region GameCenter
            _packetNames.Add(ClientPacketHeader.Game1WeeklyLeaderboardMessageComposer, "GameWeeklyLeaderboardMessageComposer");
            _packetNames.Add(ClientPacketHeader.GameAccountStatusMessageComposer, "GameAccountStatusMessageComposer");
            _packetNames.Add(ClientPacketHeader.GameAchievementListMessageComposer, "GameAchievementListMessageComposer");
            _packetNames.Add(ClientPacketHeader.GameListMessageComposer, "GameListMessageComposer");
            _packetNames.Add(ClientPacketHeader.JoinQueueMessageComposer, "JoinQueueMessageComposer");
            _packetNames.Add(ClientPacketHeader.LoadGameMessageComposer, "LoadGameMessageComposer");
            _packetNames.Add(ClientPacketHeader.PlayableGamesMessageComposer, "PlayableGamesMessageComposer");
            #endregion
            #region Groups
            #region Forms
            _packetNames.Add(ClientPacketHeader.ForumDataMessageComposer, "ForumDataMessageComposer");
            _packetNames.Add(ClientPacketHeader.ForumsListDataMessageComposer, "ForumsListDataMessageComposer");
            _packetNames.Add(ClientPacketHeader.PostUpdatedMessageComposer, "PostUpdatedMessageComposer");
            _packetNames.Add(ClientPacketHeader.ThreadCreatedMessageComposer, "ThreadCreatedMessageComposer");
            _packetNames.Add(ClientPacketHeader.ThreadDataMessageComposer, "ThreadDataMessageComposer");
            _packetNames.Add(ClientPacketHeader.ThreadsListDataMessageComposer, "ThreadsListDataMessageComposer");
            _packetNames.Add(ClientPacketHeader.ThreadReplyMessageComposer, "ThreadReplyMessageComposer");
            _packetNames.Add(ClientPacketHeader.ThreadUpdatedMessageComposer, "ThreadUpdatedMessageComposer");
            #endregion

            _packetNames.Add(ClientPacketHeader.BadgeEditorPartsMessageComposer, "BadgeEditorPartsMessageComposer");
            _packetNames.Add(ClientPacketHeader.GroupCreationWindowMessageComposer, "GroupCreationWindowMessageComposer");
            _packetNames.Add(ClientPacketHeader.GroupFurniSettingsMessageComposer, "GroupFurniSettingsMessageComposer");
            _packetNames.Add(ClientPacketHeader.GroupInfoMessageComposer, "GroupInfoMessageComposer");
            _packetNames.Add(ClientPacketHeader.GroupMembersMessageComposer, "GroupMembersMessageComposer");
            _packetNames.Add(ClientPacketHeader.GroupMembershipRequestedMessageComposer, "GroupMembershipRequestedMessageComposer");
            _packetNames.Add(ClientPacketHeader.GroupMemberUpdatedMessageComposer, "GroupMemberUpdatedMessageComposer");
            _packetNames.Add(ClientPacketHeader.ManageGroupMessageComposer, "ManageGroupMessageComposer");
            _packetNames.Add(ClientPacketHeader.NewGroupInfoMessageComposer, "NewGroupInfoMessageComposer");
            _packetNames.Add(ClientPacketHeader.RefreshFavouriteGroupMessageComposer, "RefreshFavouriteGroupMessageComposer");
            _packetNames.Add(ClientPacketHeader.SetGroupIdMessageComposer, "SetGroupIdMessageComposer");
            _packetNames.Add(ClientPacketHeader.UnknownGroupMessageComposer, "UnknownGroupMessageComposer");
            _packetNames.Add(ClientPacketHeader.UpdateFavouriteGroupMessageComposer, "UpdateFavouriteGroupMessageComposer");
            #endregion
            #region HotelCamera
            _packetNames.Add(ClientPacketHeader.CameraFinishParticipateCompetitionMessageComposer, "PlayableGamesMessageComposer");
            _packetNames.Add(ClientPacketHeader.CameraFinishPublishMessageComposer, "PlayableGamesMessageComposer");
            _packetNames.Add(ClientPacketHeader.CameraSendImageUrlMessageComposer, "PlayableGamesMessageComposer");
            ///_packetNames.Add(ClientPacketHeader.CameraFinishPurchaseComposer, "PlayableGamesMessageComposer");
            _packetNames.Add(ClientPacketHeader.SendRoomThumbnailAlertMessageComposer, "PlayableGamesMessageComposer");
            _packetNames.Add(ClientPacketHeader.SetCameraPicturePriceMessageComposer, "PlayableGamesMessageComposer");
            #endregion
            #region Handshake
            _packetNames.Add(ClientPacketHeader.GetUserDataComposer, "GetUserDataComposer");
            _packetNames.Add(ClientPacketHeader.AuthenticationOKMessageComposer, "AuthenticationOKMessageComposer");
            _packetNames.Add(ClientPacketHeader.AvailabilityStatusMessageComposer, "AvailabilityStatusMessageComposer");
            _packetNames.Add(ClientPacketHeader.GenericErrorMessageComposer, "GenericErrorMessageComposer");
            _packetNames.Add(ClientPacketHeader.InitCryptoMessageComposer, "InitCryptoMessageComposer");
            _packetNames.Add(ClientPacketHeader.PongMessageComposer, "PongMessageComposer");
            _packetNames.Add(ClientPacketHeader.SecretKeyMessageComposer, "SecretKeyMessageComposer");
            _packetNames.Add(ClientPacketHeader.SetUniqueIdMessageComposer, "SetUniqueIdMessageComposer");
            _packetNames.Add(ClientPacketHeader.UserObjectMessageComposer, "UserObjectMessageComposer");
            _packetNames.Add(ClientPacketHeader.UserPerksMessageComposer, "UserPerksMessageComposer");
            _packetNames.Add(ClientPacketHeader.UserRightsMessageComposer, "UserRightsMessageComposer");
            #endregion
            #region Help
            #region Helpers
            _packetNames.Add(ClientPacketHeader.CallForHelperErrorMessageComposer, "UserRightsMessageComposer");
            _packetNames.Add(ClientPacketHeader.CallForHelperWindowMessageComposer, "UserRightsMessageComposer");
            _packetNames.Add(ClientPacketHeader.CloseHelperSessionMessageComposer, "UserRightsMessageComposer");
            _packetNames.Add(ClientPacketHeader.EndHelperSessionMessageComposer, "UserRightsMessageComposer");
            _packetNames.Add(ClientPacketHeader.HandleHelperToolMessageComposer, "UserRightsMessageComposer");
            _packetNames.Add(ClientPacketHeader.HelperSessionChatIsTypingMessageComposer, "UserRightsMessageComposer");
            _packetNames.Add(ClientPacketHeader.HelperSessionInvinteRoomMessageComposer, "UserRightsMessageComposer");
            _packetNames.Add(ClientPacketHeader.HelperSessionSendChatMessageComposer, "UserRightsMessageComposer");
            _packetNames.Add(ClientPacketHeader.HelperSessionVisiteRoomMessageComposer, "UserRightsMessageComposer");
            _packetNames.Add(ClientPacketHeader.InitHelperSessionChatMessageComposer, "UserRightsMessageComposer");
            #endregion

            _packetNames.Add(ClientPacketHeader.SanctionStatusMessageComposer, "SanctionStatusMessageComposer");
            _packetNames.Add(ClientPacketHeader.SendBullyReportMessageComposer, "SendBullyReportMessageComposer");
            _packetNames.Add(ClientPacketHeader.SubmitBullyReportMessageComposer, "SubmitBullyReportMessageComposer");
            #endregion
            #region Inventory
            #region Achievements
            _packetNames.Add(ClientPacketHeader.AchievementProgressedMessageComposer, "AchievementProgressedMessageComposer");
            _packetNames.Add(ClientPacketHeader.AchievementsMessageComposer, "AchievementsMessageComposer");
            _packetNames.Add(ClientPacketHeader.AchievementScoreMessageComposer, "AchievementScoreMessageComposer");
            _packetNames.Add(ClientPacketHeader.AchievementUnlockedMessageComposer, "AchievementUnlockedMessageComposer");
            _packetNames.Add(ClientPacketHeader.BadgeDefinitionsMessageComposer, "BadgeDefinitionsMessageComposer");
            #endregion
            #region AvatarEffects
            _packetNames.Add(ClientPacketHeader.AvatarEffectActivatedMessageComposer, "AvatarEffectActivatedMessageComposer");
            _packetNames.Add(ClientPacketHeader.AvatarEffectsMessageComposer, "AvatarEffectsComposer");
            _packetNames.Add(ClientPacketHeader.AvatarEffectAddedMessageComposer, "AvatarEffectAddedMessageComposer");
            _packetNames.Add(ClientPacketHeader.AvatarEffectExpiredMessageComposer, "AvatarEffectExpiredMessageComposer");
            _packetNames.Add(ClientPacketHeader.FigureSetIdsMessageComposer, "FigureSetIdsMessageComposer");
            #endregion
            #region Badges
            _packetNames.Add(ClientPacketHeader.BadgesMessageComposer, "BadgesMessageComposer");
            #endregion
            #region Bots
            _packetNames.Add(ClientPacketHeader.BotInventoryMessageComposer, "BotInventoryMessageComposer");
            #endregion
            #region Furni
            _packetNames.Add(ClientPacketHeader.FurniListAddMessageComposer, "FurniListAddMessageComposer");
            _packetNames.Add(ClientPacketHeader.FurniListMessageComposer, "FurniListMessageComposer");
            _packetNames.Add(ClientPacketHeader.FurniListNotificationMessageComposer, "FurniListNotificationMessageComposer");
            _packetNames.Add(ClientPacketHeader.FurniListRemoveMessageComposer, "FurniListRemoveMessageComposer");
            _packetNames.Add(ClientPacketHeader.FurniListUpdateMessageComposer, "FurniListUpdateMessageComposer");
            #endregion
            #region Pets
            _packetNames.Add(ClientPacketHeader.PetInventoryMessageComposer, "PetInventoryMessageComposer");
            #endregion
            #region Purse
            _packetNames.Add(ClientPacketHeader.ActivityPointsMessageComposer, "ActivityPointsMessageComposer");
            _packetNames.Add(ClientPacketHeader.CreditBalanceMessageComposer, "CreditBalanceMessageComposer");
            _packetNames.Add(ClientPacketHeader.HabboActivityPointNotificationMessageComposer, "HabboActivityPointNotificationMessageComposer");
            #endregion
            #region Trading
            _packetNames.Add(ClientPacketHeader.TradingAcceptMessageComposer, "TradingAcceptMessageComposer");
            _packetNames.Add(ClientPacketHeader.TradingClosedMessageComposer, "TradingClosedMessageComposer");
            _packetNames.Add(ClientPacketHeader.TradingCompleteMessageComposer, "TradingCompleteMessageComposer");
            ///_packetNames.Add(ClientPacketHeader.TradingConfirmedMessageComposer, "TradingConfirmedMessageComposer"); same as TradingAcceptMessageComposer
            _packetNames.Add(ClientPacketHeader.TradingErrorMessageComposer, "TradingErrorMessageComposer");
            _packetNames.Add(ClientPacketHeader.TradingFinishMessageComposer, "TradingFinishMessageComposer");
            _packetNames.Add(ClientPacketHeader.TradingStartMessageComposer, "TradingStartMessageComposer");
            _packetNames.Add(ClientPacketHeader.TradingUpdateMessageComposer, "TradingUpdateMessageComposer");
            #endregion
            #endregion
            #region LandingView
            _packetNames.Add(ClientPacketHeader.BonusRareMessageComposer, "BonusRareMessageComposer");
            _packetNames.Add(ClientPacketHeader.CampaignMessageComposer, "CampaignMessageComposer");
            _packetNames.Add(ClientPacketHeader.CommunityGoalComposer, "CommunityGoalComposer");
            _packetNames.Add(ClientPacketHeader.ConcurrentUsersGoalProgressMessageComposer, "ConcurrentUsersGoalProgressMessageComposer");
            ///_packetNames.Add(ClientPacketHeader.HallOfFameMessageComposer, "HallOfFameMessageComposer");
            _packetNames.Add(ClientPacketHeader.PromoArticlesMessageComposer, "PromoArticlesMessageComposer");
            #endregion
            #region Marketplace
            _packetNames.Add(ClientPacketHeader.MarketplaceCancelOfferResultMessageComposer, "MarketplaceCancelOfferResultMessageComposer");
            _packetNames.Add(ClientPacketHeader.MarketplaceCanMakeOfferResultMessageComposer, "MarketplaceCanMakeOfferResultMessageComposer");
            _packetNames.Add(ClientPacketHeader.MarketplaceItemStatsMessageComposer, "MarketplaceItemStatsMessageComposer");
            _packetNames.Add(ClientPacketHeader.MarketplaceMakeOfferResultMessageComposer, "MarketplaceMakeOfferResultMessageComposer");
            _packetNames.Add(ClientPacketHeader.MarketPlaceOffersMessageComposer, "MarketPlaceOffersMessageComposer");
            _packetNames.Add(ClientPacketHeader.MarketPlaceOwnOffersMessageComposer, "MarketPlaceOwnOffersMessageComposer");
            #endregion
            #region Messenger
            _packetNames.Add(ClientPacketHeader.BuddyListMessageComposer, "BuddyListMessageComposer");
            _packetNames.Add(ClientPacketHeader.BuddyRequestsMessageComposer, "BuddyRequestsMessageComposer");
            _packetNames.Add(ClientPacketHeader.FindFriendsProcessResultMessageComposer, "FindFriendsProcessResultMessageComposer");
            _packetNames.Add(ClientPacketHeader.FollowFriendFailedMessageComposer, "FollowFriendFailedMessageComposer");
            _packetNames.Add(ClientPacketHeader.FriendListUpdateMessageComposer, "FriendListUpdateMessageComposer");
            _packetNames.Add(ClientPacketHeader.FriendNotificationMessageComposer, "FriendNotificationMessageComposer");
            _packetNames.Add(ClientPacketHeader.HabboSearchResultMessageComposer, "HabboSearchResultMessageComposer");
            _packetNames.Add(ClientPacketHeader.InstantMessageErrorMessageComposer, "InstantMessageErrorMessageComposer");
            _packetNames.Add(ClientPacketHeader.MessengerErrorMessageComposer, "MessengerErrorMessageComposer");
            _packetNames.Add(ClientPacketHeader.MessengerInitMessageComposer, "MessengerInitMessageComposer");
            _packetNames.Add(ClientPacketHeader.NewBuddyRequestMessageComposer, "NewBuddyRequestMessageComposer");
            _packetNames.Add(ClientPacketHeader.NewConsoleMessageMessageComposer, "NewConsoleMessageMessageComposer");
            _packetNames.Add(ClientPacketHeader.RoomInviteMessageComposer, "RoomInviteMessageComposer");
            #endregion
            #region Misc
            _packetNames.Add(ClientPacketHeader.LatencyResponseMessageComposer, "LatencyResponseMessageComposer");
            _packetNames.Add(ClientPacketHeader.VideoOffersRewardsMessageComposer, "VideoOffersRewardsMessageComposer");
            #endregion
            #region Moderation
            _packetNames.Add(ClientPacketHeader.BroadcastMessageAlertMessageComposer, "BroadcastMessageAlertMessageComposer");
            _packetNames.Add(ClientPacketHeader.CallForHelpPendingCallsMessageComposer, "CallForHelpPendingCallsMessageComposer");
            _packetNames.Add(ClientPacketHeader.CfhTopicsInitMessageComposer, "CfhTopicsInitMessageComposer");
            _packetNames.Add(ClientPacketHeader.ModeratorInitMessageComposer, "ModeratorInitMessageComposer");
            _packetNames.Add(ClientPacketHeader.ModeratorRoomChatlogMessageComposer, "ModeratorRoomChatlogMessageComposer");
            _packetNames.Add(ClientPacketHeader.ModeratorRoomInfoMessageComposer, "ModeratorRoomInfoMessageComposer");
            _packetNames.Add(ClientPacketHeader.ModeratorSupportTicketMessageComposer, "ModeratorSupportTicketMessageComposer");
            ///_packetNames.Add(ClientPacketHeader.ModeratorSupportTicketResponseMessageComposer, "ModeratorSupportTicketResponseMessageComposer"); ID = -1
            ///_packetNames.Add(ClientPacketHeader.ModeratorTicketChatlogMessageComposer, "ModeratorTicketChatlogMessageComposer"); ID = -1
            _packetNames.Add(ClientPacketHeader.ModeratorUserChatlogMessageComposer, "ModeratorUserChatlogMessageComposer");
            _packetNames.Add(ClientPacketHeader.ModeratorUserInfoMessageComposer, "ModeratorUserInfoMessageComposer");
            _packetNames.Add(ClientPacketHeader.ModeratorUserRoomVisitsMessageComposer, "ModeratorUserRoomVisitsMessageComposer");
            _packetNames.Add(ClientPacketHeader.MutedMessageComposer, "MutedMessageComposer");
            ///_packetNames.Add(ClientPacketHeader.OpenHelpToolMessageComposer, "OpenHelpToolMessageComposer"); same as CallForHelpPendingCallsMessageComposer
            #endregion
            #region Navigator
            _packetNames.Add(ClientPacketHeader.CanCreateRoomMessageComposer, "CanCreateRoomMessageComposer");
            _packetNames.Add(ClientPacketHeader.DoorbellMessageComposer, "DoorbellMessageComposer");
            _packetNames.Add(ClientPacketHeader.FavouritesMessageComposer, "FavouritesMessageComposer");
            _packetNames.Add(ClientPacketHeader.FlatAccessDeniedMessageComposer, "FlatAccessDeniedMessageComposer");
            _packetNames.Add(ClientPacketHeader.FlatCreatedMessageComposer, "FlatCreatedMessageComposer");
            _packetNames.Add(ClientPacketHeader.GetGuestRoomResultMessageComposer, "GetGuestRoomResultMessageComposer");
            _packetNames.Add(ClientPacketHeader.GuestRoomSearchResultMessageComposer, "GuestRoomSearchResultMessageComposer");
            _packetNames.Add(ClientPacketHeader.NavigatorCollapsedCategoriesMessageComposer, "NavigatorCollapsedCategoriesMessageComposer");
            _packetNames.Add(ClientPacketHeader.NavigatorFlatCatsMessageComposer, "NavigatorFlatCatsMessageComposer");
            _packetNames.Add(ClientPacketHeader.NavigatorLiftedRoomsMessageComposer, "NavigatorLiftedRoomsMessageComposer");
            _packetNames.Add(ClientPacketHeader.NavigatorMetaDataParserMessageComposer, "NavigatorMetaDataParserMessageComposer");
            _packetNames.Add(ClientPacketHeader.NavigatorPreferencesMessageComposer, "NavigatorPreferencesMessageComposer");
            _packetNames.Add(ClientPacketHeader.NavigatorSearchResultSetMessageComposer, "NavigatorSearchResultSetMessageComposer");
            _packetNames.Add(ClientPacketHeader.NavigatorSettingsMessageComposer, "NavigatorSettingsMessageComposer");
            _packetNames.Add(ClientPacketHeader.PopularRoomTagsResultMessageComposer, "PopularRoomTagsResultMessageComposer");
            _packetNames.Add(ClientPacketHeader.RoomInfoUpdatedMessageComposer, "RoomInfoUpdatedMessageComposer");
            _packetNames.Add(ClientPacketHeader.RoomRatingMessageComposer, "RoomRatingMessageComposer");
            _packetNames.Add(ClientPacketHeader.UpdateFavouriteRoomMessageComposer, "UpdateFavouriteRoomMessageComposer");
            _packetNames.Add(ClientPacketHeader.UserFlatCatsMessageComposer, "UserFlatCatsMessageComposer");
            #endregion
            #region Notifications
            _packetNames.Add(ClientPacketHeader.MOTDNotificationMessageComposer, "MOTDNotificationMessageComposer");
            _packetNames.Add(ClientPacketHeader.RoomNotificationMessageComposer, "RoomNotificationMessageComposer");
            ///_packetNames.Add(ClientPacketHeader.SuperNotificationMessageComposer, "SuperNotificationMessageComposer"); same as RoomNotificationMessageComposer
            #endregion
            #region Pets
            _packetNames.Add(ClientPacketHeader.PetBreedingMessageComposer, "PetBreedingMessageComposer");
            _packetNames.Add(ClientPacketHeader.RespectPetNotificationMessageComposer, "RespectPetNotificationMessageComposer");
            #endregion
            #region Quests
            _packetNames.Add(ClientPacketHeader.QuestAbortedMessageComposer, "QuestAbortedMessageComposer");
            _packetNames.Add(ClientPacketHeader.QuestCompletedMessageComposer, "QuestCompletedMessageComposer");
            _packetNames.Add(ClientPacketHeader.QuestListMessageComposer, "QuestListMessageComposer");
            _packetNames.Add(ClientPacketHeader.QuestStartedMessageComposer, "QuestStartedMessageComposer");
            #endregion
            #region QuickPolls
            _packetNames.Add(ClientPacketHeader.QuickPollMessageComposer, "QuickPollMessageComposer");
            _packetNames.Add(ClientPacketHeader.QuickPollResultMessageComposer, "QuickPollResultMessageComposer");
            _packetNames.Add(ClientPacketHeader.QuickPollResultsMessageComposer, "QuickPollResultsMessageComposer");
            #endregion
            #region Quiz
            _packetNames.Add(ClientPacketHeader.PostQuizAnswersMessageComposer, "PostQuizAnswersMessageComposer");
            _packetNames.Add(ClientPacketHeader.QuizDataMessageComposer, "QuizDataMessageComposer");
            ///_packetNames.Add(ClientPacketHeader.QuizResultsMessageComposer, "QuizResultsMessageComposer"); same as PostQuizAnswersMessageComposer
            #endregion
            #region Rooms
            #region Action
            _packetNames.Add(ClientPacketHeader.IgnoreStatusMessageComposer, "IgnoreStatusMessageComposer");
            #endregion
            #region AI
            #region Bots
            _packetNames.Add(ClientPacketHeader.OpenBotActionMessageComposer, "OpenBotActionMessageComposer");
            #endregion
            #region Pets
            _packetNames.Add(ClientPacketHeader.AddExperiencePointsMessageComposer, "AddExperiencePointsMessageComposer");
            _packetNames.Add(ClientPacketHeader.PetHorseFigureInformationMessageComposer, "PetHorseFigureInformationMessageComposer");
            _packetNames.Add(ClientPacketHeader.PetInformationMessageComposer, "PetInformationMessageComposer");
            _packetNames.Add(ClientPacketHeader.PetTrainingPanelMessageComposer, "PetTrainingPanelMessageComposer");
            #endregion
            #endregion
            #region Avatar
            _packetNames.Add(ClientPacketHeader.ActionMessageComposer, "ActionMessageComposer");
            _packetNames.Add(ClientPacketHeader.AvatarAspectUpdateMessageComposer, "AvatarAspectUpdateMessageComposer");
            _packetNames.Add(ClientPacketHeader.AvatarEffectMessageComposer, "AvatarEffectMessageComposer");
            _packetNames.Add(ClientPacketHeader.CarryObjectMessageComposer, "CarryObjectMessageComposer");
            _packetNames.Add(ClientPacketHeader.DanceMessageComposer, "DanceMessageComposer");
            _packetNames.Add(ClientPacketHeader.SleepMessageComposer, "SleepMessageComposer");
            #endregion
            #region Chat
            _packetNames.Add(ClientPacketHeader.ChatMessageComposer, "ChatMessageComposer");
            _packetNames.Add(ClientPacketHeader.FloodControlMessageComposer, "FloodControlMessageComposer");
            _packetNames.Add(ClientPacketHeader.ShoutMessageComposer, "ShoutMessageComposer");
            _packetNames.Add(ClientPacketHeader.UserTypingMessageComposer, "UserTypingMessageComposer");
            _packetNames.Add(ClientPacketHeader.WhisperMessageComposer, "WhisperMessageComposer");
            #endregion
            #region Engine
            ///_packetNames.Add(ClientPacketHeader.AvatarAspectUpdateMessageComposer, "AvatarAspectUpdateMessageComposer"); same as FigureUpdateMessageComposer
            _packetNames.Add(ClientPacketHeader.FloorHeightMapMessageComposer, "FloorHeightMapMessageComposer");
            _packetNames.Add(ClientPacketHeader.FurnitureAliasesMessageComposer, "FurnitureAliasesMessageComposer");
            _packetNames.Add(ClientPacketHeader.HeightMapMessageComposer, "HeightMapMessageComposer");
            _packetNames.Add(ClientPacketHeader.ItemAddMessageComposer, "ItemAddMessageComposer");
            _packetNames.Add(ClientPacketHeader.ItemRemoveMessageComposer, "ItemRemoveMessageComposer");
            _packetNames.Add(ClientPacketHeader.ItemsMessageComposer, "ItemsMessageComposer");
            _packetNames.Add(ClientPacketHeader.ItemUpdateMessageComposer, "ItemUpdateMessageComposer");
            _packetNames.Add(ClientPacketHeader.ObjectAddMessageComposer, "ObjectAddMessageComposer");
            _packetNames.Add(ClientPacketHeader.ObjectRemoveMessageComposer, "ObjectRemoveMessageComposer");
            _packetNames.Add(ClientPacketHeader.ObjectsMessageComposer, "ObjectsMessageComposer");
            _packetNames.Add(ClientPacketHeader.ObjectUpdateMessageComposer, "ObjectUpdateMessageComposer");
            _packetNames.Add(ClientPacketHeader.RoomEntryInfoMessageComposer, "RoomEntryInfoMessageComposer");
            _packetNames.Add(ClientPacketHeader.RoomEventMessageComposer, "RoomEventMessageComposer");
            _packetNames.Add(ClientPacketHeader.RoomPropertyMessageComposer, "RoomPropertyMessageComposer");
            _packetNames.Add(ClientPacketHeader.RoomVisualizationSettingsMessageComposer, "RoomVisualizationSettingsMessageComposer");
            _packetNames.Add(ClientPacketHeader.SlideObjectBundleMessageComposer, "SlideObjectBundleMessageComposer");
            _packetNames.Add(ClientPacketHeader.UserChangeMessageComposer, "UserChangeMessageComposer");
            _packetNames.Add(ClientPacketHeader.UserNameChangeMessageComposer, "UserNameChangeMessageComposer");
            _packetNames.Add(ClientPacketHeader.UserRemoveMessageComposer, "UserRemoveMessageComposer");
            _packetNames.Add(ClientPacketHeader.UsersMessageComposer, "UsersMessageComposer");
            _packetNames.Add(ClientPacketHeader.UserUpdateMessageComposer, "UserUpdateMessageComposer");
            #endregion
            #region FloorPlan
            _packetNames.Add(ClientPacketHeader.FloorPlanFloorMapMessageComposer, "FloorPlanFloorMapMessageComposer");
            _packetNames.Add(ClientPacketHeader.FloorPlanSendDoorMessageComposer, "FloorPlanSendDoorMessageComposer");
            #endregion
            #region Freeze
            _packetNames.Add(ClientPacketHeader.UpdateFreezeLivesMessageComposer, "UpdateFreezeLivesMessageComposer");
            #endregion
            #region Furni
            #region LoveLocks
            _packetNames.Add(ClientPacketHeader.LoveLockDialogueCloseMessageComposer, "LoveLockDialogueCloseMessageComposer");
            _packetNames.Add(ClientPacketHeader.LoveLockDialogueMessageComposer, "LoveLockDialogueMessageComposer");
            ///_packetNames.Add(ClientPacketHeader.LoveLockDialogueSetLockedMessageComposer, "LoveLockDialogueSetLockedMessageComposer"); same as LoveLockDialogueCloseMessageComposer
            #endregion
            #region Moodlight
            _packetNames.Add(ClientPacketHeader.MoodlightConfigMessageComposer, "MoodlightConfigMessageComposer");
            #endregion
            #region RentableSpaces
            _packetNames.Add(ClientPacketHeader.RentableSpaceMessageComposer, "RentableSpaceMessageComposer");
            _packetNames.Add(ClientPacketHeader.RentableSpacesErrorMessageComposer, "RentableSpacesErrorMessageComposer");
            #endregion
            #region Stickys
            _packetNames.Add(ClientPacketHeader.StickyNoteMessageComposer, "StickyNoteMessageComposer");
            #endregion
            #region Wired
            _packetNames.Add(ClientPacketHeader.HideWiredConfigMessageComposer, "HideWiredConfigMessageComposer");
            _packetNames.Add(ClientPacketHeader.WiredConditionConfigMessageComposer, "WiredConditionConfigMessageComposer");
            _packetNames.Add(ClientPacketHeader.WiredEffectConfigMessageComposer, "WiredEffectConfigMessageComposer");
            _packetNames.Add(ClientPacketHeader.WiredTriggerConfigMessageComposer, "WiredTriggerConfigMessageComposer");
            _packetNames.Add(ClientPacketHeader.WiredSmartAlertComposer, "WiredSmartAlertComposer");
            #endregion
            #region YoutubeTelevisions
            _packetNames.Add(ClientPacketHeader.YoutubeDisplayPlaylistsMessageComposer, "YoutubeDisplayPlaylistsMessageComposer");
            _packetNames.Add(ClientPacketHeader.GetYouTubeVideoMessageComposer, "GetYouTubeVideoMessageComposer");
            #endregion
            #region Crafting
            _packetNames.Add(ClientPacketHeader.CraftableProductsMessageComposer, "CraftableProductsMessageComposer");
            _packetNames.Add(ClientPacketHeader.CraftingFoundMessageComposer, "CraftingFoundMessageComposer");
            _packetNames.Add(ClientPacketHeader.CraftingRecipeMessageComposer, "CraftingRecipeMessageComposer");
            _packetNames.Add(ClientPacketHeader.CraftingResultMessageComposer, "CraftingResultMessageComposer");
            #endregion
            _packetNames.Add(ClientPacketHeader.GnomeBoxMessageComposer, "GnomeBoxMessageComposer");
            _packetNames.Add(ClientPacketHeader.OpenGiftMessageComposer, "OpenGiftMessageComposer");
            _packetNames.Add(ClientPacketHeader.UpdateMagicTileMessageComposer, "UpdateMagicTileMessageComposer");
            #endregion
            #region Notifications
            _packetNames.Add(ClientPacketHeader.AlertNotificationHCMessageComposer, "AlertNotificationHCMessageComposer");
            _packetNames.Add(ClientPacketHeader.HCGiftsAlertComposer, "HCGiftsAlertComposer");
            _packetNames.Add(ClientPacketHeader.MassEventComposer, "MassEventComposer");
            _packetNames.Add(ClientPacketHeader.RoomCustomizedAlertComposer, "RoomCustomizedAlertComposer");
            _packetNames.Add(ClientPacketHeader.RoomErrorNotifMessageComposer, "RoomErrorNotifMessageComposer");
            #endregion
            #region Nux
            ///_packetNames.Add(ClientPacketHeader.NuxAlertMessageComposer, "NuxAlertMessageComposer"); same as MassEventComposer
            _packetNames.Add(ClientPacketHeader.NuxItemListComposer, "NuxItemListComposer");
            #endregion
            #region Permissions
            _packetNames.Add(ClientPacketHeader.YouAreControllerMessageComposer, "YouAreControllerMessageComposer");
            _packetNames.Add(ClientPacketHeader.YouAreNotControllerMessageComposer, "YouAreNotControllerMessageComposer");
            _packetNames.Add(ClientPacketHeader.YouAreOwnerMessageComposer, "YouAreOwnerMessageComposer");
            #endregion
            #region Polls
            ///_packetNames.Add(ClientPacketHeader.QuestionParserMessageComposer, "QuestionParserMessageComposer"); same as QuickPollMessageComposer
            _packetNames.Add(ClientPacketHeader.PollContentsMessageComposer, "PollContentsMessageComposer");
            _packetNames.Add(ClientPacketHeader.PollOfferMessageComposer, "PollOfferMessageComposer");
            #endregion
            #region Session
            _packetNames.Add(ClientPacketHeader.CantConnectMessageComposer, "CantConnectMessageComposer");
            _packetNames.Add(ClientPacketHeader.CloseConnectionMessageComposer, "CloseConnectionMessageComposer");
            _packetNames.Add(ClientPacketHeader.FlatAccessibleMessageComposer, "FlatAccessibleMessageComposer");
            _packetNames.Add(ClientPacketHeader.OpenFlatConnectionMessageComposer, "OpenFlatConnectionMessageComposer");
            _packetNames.Add(ClientPacketHeader.RoomForwardMessageComposer, "RoomForwardMessageComposer");
            _packetNames.Add(ClientPacketHeader.RoomReadyMessageComposer, "RoomReadyMessageComposer");
            #endregion
            #region Settings
            _packetNames.Add(ClientPacketHeader.FlatControllerAddedMessageComposer, "FlatControllerAddedMessageComposer");
            _packetNames.Add(ClientPacketHeader.FlatControllerRemovedMessageComposer, "FlatControllerRemovedMessageComposer");
            _packetNames.Add(ClientPacketHeader.GetRoomBannedUsersMessageComposer, "GetRoomBannedUsersMessageComposer");
            _packetNames.Add(ClientPacketHeader.GetRoomFilterListMessageComposer, "GetRoomFilterListMessageComposer");
            _packetNames.Add(ClientPacketHeader.RoomMuteSettingsMessageComposer, "RoomMuteSettingsMessageComposer");
            _packetNames.Add(ClientPacketHeader.RoomRightsListMessageComposer, "RoomRightsListMessageComposer");
            _packetNames.Add(ClientPacketHeader.RoomSettingsDataMessageComposer, "RoomSettingsDataMessageComposer");
            _packetNames.Add(ClientPacketHeader.RoomSettingsSavedMessageComposer, "RoomSettingsSavedMessageComposer");
            _packetNames.Add(ClientPacketHeader.UnbanUserFromRoomMessageComposer, "UnbanUserFromRoomMessageComposer");
            #endregion
            #endregion
            #region Sound
            _packetNames.Add(ClientPacketHeader.LoadJukeboxUserMusicItemsMessageComposer, "LoadJukeboxUserMusicItemsMessageComposer");
            _packetNames.Add(ClientPacketHeader.SetJukeboxNowPlayingMessageComposer, "SetJukeboxNowPlayingMessageComposer");
            _packetNames.Add(ClientPacketHeader.SetJukeboxPlayListMessageComposer, "SetJukeboxPlayListMessageComposer");
            ///_packetNames.Add(ClientPacketHeader.SetJukeboxSongMusicDataMessageComposer, "AchievementProgressedMessageComposer"); same as TraxSongInfoMessageComposer
            _packetNames.Add(ClientPacketHeader.SoundSettingsMessageComposer, "SoundSettingsMessageComposer");
            _packetNames.Add(ClientPacketHeader.TraxSongInfoMessageComposer, "TraxSongInfoMessageComposer");
            #endregion
            #region Talents
            _packetNames.Add(ClientPacketHeader.TalentLevelUpMessageComposer, "TalentLevelUpMessageComposer");
            _packetNames.Add(ClientPacketHeader.TalentTrackMessageComposer, "TalentTrackMessageComposer");
            _packetNames.Add(ClientPacketHeader.TalentTrackLevelMessageComposer, "TalentTrackLevelMessageComposer");
            #endregion
            #region Users
            ///_packetNames.Add(ClientPacketHeader.GetHabboClubCenterInfoMessageComposer, "GetHabboClubCenterInfoMessageComposer");
            _packetNames.Add(ClientPacketHeader.GetRelationshipsMessageComposer, "GetRelationshipsMessageComposer");
            _packetNames.Add(ClientPacketHeader.HabboGroupBadgesMessageComposer, "HabboGroupBadgesMessageComposer");
            _packetNames.Add(ClientPacketHeader.HabboUserBadgesMessageComposer, "HabboUserBadgesMessageComposer");
            _packetNames.Add(ClientPacketHeader.IgnoredUsersMessageComposer, "IgnoredUsersMessageComposer");
            _packetNames.Add(ClientPacketHeader.NameChangeUpdateMessageComposer, "NameChangeUpdateMessageComposer");
            _packetNames.Add(ClientPacketHeader.ProfileInformationMessageComposer, "ProfileInformationMessageComposer");
            _packetNames.Add(ClientPacketHeader.RespectNotificationMessageComposer, "RespectNotificationMessageComposer");
            _packetNames.Add(ClientPacketHeader.ScrSendUserInfoMessageComposer, "ScrSendUserInfoMessageComposer");
            _packetNames.Add(ClientPacketHeader.UpdateUsernameMessageComposer, "UpdateUsernameMessageComposer");
            ///_packetNames.Add(ClientPacketHeader.UserClubMessageComposer, "UserClubMessageComposer");
            _packetNames.Add(ClientPacketHeader.UserTagsMessageComposer, "UserTagsMessageComposer");
            #endregion
        }
        #endregion
    }
}