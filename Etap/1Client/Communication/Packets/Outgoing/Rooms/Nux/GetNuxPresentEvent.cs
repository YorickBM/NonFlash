using System;
using Retro.Hotel.GameClients;
using Retro.Communication.Packets.Outgoing.Inventory.Purse;
using Retro.Utilities;
using Retro.Hotel.Items;
using Retro.Communication.Packets.Outgoing.Inventory.Furni;
using Retro.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Retro.Communication.Packets.Incoming.Rooms.Nux
{
    class GetNuxPresentEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int Data1 = Packet.PopInt(); // ELEMENTO 1
            int Data2 = Packet.PopInt(); // ELEMENTO 2
            int Data3 = Packet.PopInt(); // ELEMENTO 3
            int Data4 = Packet.PopInt(); // SELECTOR
            var RewardName = "";

            switch (Data4)
            {
                case 0:
                    int RewardDiamonds = RandomNumber.GenerateRandom(0, 5);
                    Session.GetHabbo().Diamonds += RewardDiamonds;
                    Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, 0, 5));
                    Session.SendMessage(RoomNotificationComposer.SendBubble("diamonds", "U won, " + RewardDiamonds + " diamanten.", ""));
                    break;
                case 1:
                    int num = 31 * 6;
                    Session.GetHabbo().GetClubManager().AddOrExtendSubscription("habbo_vip", num * 24 * 3600, Session);
                    Session.SendMessage(RoomNotificationComposer.SendBubble("hc", "U heeft 6 maanden gratis HC, maak er goed gebruik van!", ""));
                    //int RewardGotw = RandomNumber.GenerateRandom(25, 50);
                    //Session.GetHabbo().GOTWPoints += RewardGotw;
                    //Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().GOTWPoints, RewardGotw, 103));
                    break;
                case 2:
                    int RewardItem = RandomNumber.GenerateRandom(1, 10);
                    var RewardItemId = 0;

                    switch (RewardItem)
                    {
                        case 1:
                            RewardItemId = 155; // Rubber Duck
                            RewardName = "Rubber Duck";
                            break;
                        case 2:
                            RewardItemId = 2607; // CD Antigo
                            RewardName = "Oude CD";
                            break;
                        case 3:
                            RewardItemId = 155; // Rubber Duck
                            RewardName = "Rubber Duck";
                            break;
                        case 4:
                            RewardItemId = 3226; // Gnome
                            RewardName = "Gnome";
                            break;
                        case 5:
                            RewardItemId = 155; // Rubber Duck
                            RewardName = "Rubber Duck";
                            break;
                        case 6:
                            RewardItemId = 3291; // Hand
                            RewardName = "Hand";
                            break;
                        case 7:
                            RewardItemId = 206; // Pumpkin
                            RewardName = "Pumpkin";
                            break;
                        case 8:
                            RewardItemId = 9159; // Spider web
                            RewardName = "Spider web";
                            break;
                        case 9:
                            RewardItemId = 2064; // Credit
                            RewardName = "Credit";
                            break;
                        case 10:
                            RewardItemId = 2064; // Credit
                            RewardName = "Credit";
                            break;
                    }
                    ItemData Item = null;
                    if (!RetroEnvironment.GetGame().GetItemManager().GetItem(RewardItemId, out Item))
                    { return; }

                    Item GiveItem = ItemFactory.CreateSingleItemNullable(Item, Session.GetHabbo(), "", "");
                    if (GiveItem != null)
                    {
                        Session.GetHabbo().GetInventoryComponent().TryAddItem(GiveItem);

                        Session.SendMessage(new FurniListNotificationComposer(GiveItem.Id, 1));
                        Session.SendMessage(new FurniListUpdateComposer());
                        Session.SendMessage(RoomNotificationComposer.SendBubble("voucher", "U heeft net " + RewardName + " gewonnen.\n\n " + Session.GetHabbo().Username + ", Controleer uw inventaris, er blijkt iets nieuws te zijn!", ""));
                    }

                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    break;
            }
            RetroEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_AnimationRanking", 1);
        }
    }
}