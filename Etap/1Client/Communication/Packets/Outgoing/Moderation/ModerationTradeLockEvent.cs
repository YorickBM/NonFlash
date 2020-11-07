using Retro.Database.Interfaces;
using Retro.Hotel.Users;


namespace Retro.Communication.Packets.Incoming.Moderation
{
    class ModerationTradeLockEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().GetPermissions().HasRight("mod_trade_lock"))
                return;

            int UserId = Packet.PopInt();
            string Message = Packet.PopString();
            double Days = (Packet.PopInt() / 1440);
            string Unknown1 = Packet.PopString();
            string Unknown2 = Packet.PopString();
            

            double Length = (RetroEnvironment.GetUnixTimestamp() + (Days * 86400));

            Habbo Habbo = RetroEnvironment.GetHabboById(UserId);
            if (Habbo == null)
            {
                Session.SendWhisper("There was an error finding this user in the database.");
                return;
            }

            if (Habbo.GetPermissions().HasRight("mod_trade_lock") && !Session.GetHabbo().GetPermissions().HasRight("mod_trade_lock_any"))
            {
                Session.SendWhisper("Oops, you can not block another user rated at 5 or higher.");
                return;
            }

            if (Days < 1)
                Days = 1;

            if (Days > 365)
                Days = 365;

            using (IQueryAdapter dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("UPDATE `user_info` SET `trading_locked` = '" + Length + "', `trading_locks_count` = `trading_locks_count` + '1' WHERE `user_id` = '" + Habbo.Id + "' LIMIT 1");
            }

            if (Habbo.GetClient() != null)
            {
                Habbo.TradingLockExpiry = Length;
                Habbo.GetClient().SendNotification("Your exchanges have been banned for " + Days + " Day(s)!\r\rReason:\r\r" + Message);
            }
        }
    }
}
