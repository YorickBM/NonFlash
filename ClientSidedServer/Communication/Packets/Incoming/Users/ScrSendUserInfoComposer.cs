using Retro.Communication.Packets.Incoming;
using Retro.Hotel.GameClients;

namespace Retro.Communication.Packets.Outgoing.Users
{
    class ScrSendUserInfoComposer : IPacketEvent
    {
        /*private static readonly ILog log = LogManager.GetLogger("Habbie.Communication.Packets.Incoming.Handshake");
        public ScrSendUserInfoComposer(Habbo habbo)
            : base(ServerPacketHeader.ScrSendUserInfoMessageComposer)
        {
            WriteString("habbo_club");

            if (habbo.GetClubManager().HasSubscription("habbo_vip"))
            {

                Double Expire = habbo.GetClubManager().GetSubscription("habbo_vip").ExpireTime;
                Double TimeLeft = Expire - RetroEnvironment.GetUnixTimestamp();
                int TotalDaysLeft = (int)Math.Ceiling(TimeLeft / 86400);
                int MonthsLeft = TotalDaysLeft / 31;

                if (MonthsLeft >= 1)
                {
                    MonthsLeft--;
                }
                base.WriteInteger(TotalDaysLeft - (MonthsLeft * 31));
                base.WriteInteger(2); // ??
                base.WriteInteger(MonthsLeft);
                base.WriteInteger(1); // type
                base.WriteBoolean(true);
                base.WriteBoolean(true);
                base.WriteInteger(0);
                base.WriteInteger(Convert.ToInt32(TimeLeft)); // days i have on hc
                base.WriteInteger(Convert.ToInt32(TimeLeft)); // days i have on vip
            }
            else
            {
                base.WriteInteger(0);
                base.WriteInteger(0); // ??
                base.WriteInteger(0);
                base.WriteInteger(0); // type
                base.WriteBoolean(false);
                base.WriteBoolean(true);
                base.WriteInteger(0);
                base.WriteInteger(100); // days i have on hc
                base.WriteInteger(100); // days i have on vip
            }
        }*/
        public void Parse(GameClient Session, ClientPacket Packet)
        {
        }
    }
}
