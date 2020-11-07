using System.Data;

using Retro.Hotel.GameClients;
using Retro.Hotel.Catalog.Vouchers;



using Retro.Communication.Packets.Outgoing.Catalog;
using Retro.Communication.Packets.Outgoing.Inventory.Purse;
using Retro.Core;
using Retro.Database.Interfaces;
using Retro.Hotel.Items;
using Retro.Communication.Packets.Outgoing.Inventory.Furni;
using Retro.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Retro.Communication.Packets.Incoming.Catalog
{
    public class RedeemVoucherEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string VoucherCode = Packet.PopString().Replace("\r", "");

            Voucher Voucher = null;
            if (!RetroEnvironment.GetGame().GetCatalog().GetVoucherManager().TryGetVoucher(VoucherCode, out Voucher))
            {
                Session.SendMessage(new VoucherRedeemErrorComposer(0));
                return;
            }

            if (Voucher.CurrentUses >= Voucher.MaxUses)
            {
                Session.SendNotification("Oeps, deze tegoedbon heeft de maximale gebruikslimiet bereikt!");
                return;
            }

            DataRow GetRow = null;
            using (IQueryAdapter dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `user_vouchers` WHERE `user_id` = '" + Session.GetHabbo().Id + "' AND `voucher` = @Voucher LIMIT 1");
                dbClient.AddParameter("Voucher", VoucherCode);
                GetRow = dbClient.getRow();
            }

            if (GetRow != null)
            {
                Session.SendNotification("U hebt deze vouchercode al gebruikt, één voor elke gebruiker, sorry!");
                return;
            }
            else
            {
                using (IQueryAdapter dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("INSERT INTO `user_vouchers` (`user_id`,`voucher`) VALUES ('" + Session.GetHabbo().Id + "', @Voucher)");
                    dbClient.AddParameter("Voucher", VoucherCode);
                    dbClient.RunQuery();
                }
            }

            Voucher.UpdateUses();

            if (Voucher.Type == VoucherType.CREDIT)
            {
                Session.GetHabbo().Credits += Voucher.Value;
                Session.SendPacket(new CreditBalanceComposer(Session.GetHabbo().Credits));
                //Session.SendMessage(RoomNotificationComposer.SendBubble("voucher", "Você acaba de receber um premio! " + Voucher.Value + " créditos. Use com sabedoria " + Session.GetHabbo().Username + ".", ""));
            }
            else if (Voucher.Type == VoucherType.DUCKET)
            {
                Session.GetHabbo().Duckets += Voucher.Value;
                Session.SendPacket(new HabboActivityPointNotificationComposer(Session.GetHabbo().Duckets, Voucher.Value));
                //Session.SendMessage(RoomNotificationComposer.SendBubble("voucher", "Você acaba de receber um premio! " + Voucher.Value + " duckets. Use com sabedoria " + Session.GetHabbo().Username + ".", ""));
            }
            else if (Voucher.Type == VoucherType.DIAMOND)
            {
                Session.GetHabbo().Diamonds += Voucher.Value;
                Session.SendPacket(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, Voucher.Value, 5));
                //Session.SendMessage(RoomNotificationComposer.SendBubble("voucher", "Você acaba de receber um premio! " + Voucher.Value + " diamantes. Use com sabedoria " + Session.GetHabbo().Username + ".", ""));
            }
            else if (Voucher.Type == VoucherType.GOTW)
            {
                Session.GetHabbo().GOTWPoints += Voucher.Value;
                Session.SendPacket(new HabboActivityPointNotificationComposer(Session.GetHabbo().GOTWPoints, Voucher.Value, 103));
                //Session.SendMessage(RoomNotificationComposer.SendBubble("voucher", "Você acaba de receber um premio! " + Voucher.Value + " "+ExtraSettings.PTOS_COINS+ ". Use com sabedoria " + Session.GetHabbo().Username + ".", ""));
            }
            else if (Voucher.Type == VoucherType.ITEM)
            {

                ItemData Item = null;
                if (!RetroEnvironment.GetGame().GetItemManager().GetItem((Voucher.Value), out Item))
                {
                    // No existe este ItemId.
                    return;
                }

                Item GiveItem = ItemFactory.CreateSingleItemNullable(Item, Session.GetHabbo(), "", "");
                if (GiveItem != null)
                {
                    Session.GetHabbo().GetInventoryComponent().TryAddItem(GiveItem);

                    Session.SendPacket(new FurniListNotificationComposer(GiveItem.Id, 1));
                    Session.SendPacket(new FurniListUpdateComposer());
                    //Session.SendMessage(RoomNotificationComposer.SendBubble("voucher", "Você acabou de receber o objeto raro, corre " + Session.GetHabbo().Username + ", confira seu invetário algo novo está ai!", ""));
                }

                Session.GetHabbo().GetInventoryComponent().UpdateItems(false);
            }

            Session.SendMessage(new VoucherRedeemOkComposer());
        }
    }
}