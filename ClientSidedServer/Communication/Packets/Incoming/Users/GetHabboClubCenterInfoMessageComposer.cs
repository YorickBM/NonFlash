using System;
using Retro.Hotel.GameClients;
using log4net;

namespace Retro.Communication.Packets.Outgoing.Users
{
    class GetHabboClubCenterInfoMessageComposer : ServerPacket
    {
        private static readonly ILog log = LogManager.GetLogger("Habbie.Communication.Packets.Incoming.Handshake");
        public GetHabboClubCenterInfoMessageComposer(GameClient Session)
			: base(ServerPacketHeader.HabboClubCenterInfoMessageComposer)
        {
            DateTime localDate = DateTime.Now;
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Session.GetHabbo().GetClubManager().GetSubscription("habbo_vip").ActivateTime);
            double days = localDate.Subtract(origin).TotalDays;

            WriteInteger(Convert.ToInt32(days));//streakduration in days  (2005)
            if (Session.GetHabbo().GetClubManager().HasSubscription("habbo_vip"))
				WriteString(origin.ToString("dd/MM/yyyy tt"));//joindate hh:mm:ss
            else
		        WriteString("U heeft nog geen HC!");

            int bonusCredits = bonus(Convert.ToInt32(days));
            int CreditsSpend = Session.GetHabbo().CreditsSpend;

            WriteInteger(0);
			WriteInteger(0);//this should be a double 
			WriteInteger(0);//unused 
			WriteInteger(0);//unused 
			WriteInteger(CreditsSpend / 10);//CreditsSpend 
            WriteInteger(bonusCredits);//streakbonus  
            WriteInteger(CreditsSpend / 10);//CreditsSpend 
            WriteInteger(10080);//next pay in minutes
        }

         private int bonus(int days)
        {
            int bonus = 1;

            if(days > 6 && days < 30)
            {
                bonus = 5;
            } else if (days > 29 && days < 60)
            {
                bonus = 10;
            } else if (days > 59 && days < 90)
            {
                bonus = 15;
            } else if (days > 89 && days < 180)
            {
                bonus = 20;
            } else if (days > 179 && days < 365)
            {
                bonus = 25;
            } else if (days > 365)
            {
                bonus = 30;
            }

            return bonus;
        }
    }
}