using Retro.Database.Interfaces;
using Retro.Hotel.GameClients;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retro.Communication.Packets.Outgoing.Help
{
	class SanctionStatusComposer : ServerPacket
	{
        private static readonly ILog log = LogManager.GetLogger("Habbie.Communication.Packets.Outgoing.Help");
        public SanctionStatusComposer(GameClient Session)
			: base(ServerPacketHeader.SanctionStatusMessageComposer)
		{
            DataRow StatRow = null;
            using (IQueryAdapter dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `user_sancties` WHERE `userid` = '" + Session.GetHabbo().Id + "' LIMIT 1;");
                StatRow = dbClient.getRow();

                if(StatRow == null)
                {
                    dbClient.runFastQuery("INSERT INTO `user_sancties` (`userid`) VALUES ('" + Session.GetHabbo().Id + "')");

                    dbClient.SetQuery("SELECT * FROM `user_sancties` WHERE `userid` = '" + Session.GetHabbo().Id + "' LIMIT 1;");
                    StatRow = dbClient.getRow();
                }
            }

            WriteBoolean(true); //Tekst rood > Meest recenta Sanctie
            WriteBoolean(false); //false = Wel Rpoefperiode, true = geen proefperiode
            WriteString(Convert.ToString(StatRow["prev_sanction"])); //Type Sacntie?
            WriteInteger(Convert.ToInt32(StatRow["prev_sanction_time"])); //Geen idee
            WriteInteger(Convert.ToInt32(StatRow["int-2"])); //Geen idee
            WriteString(Convert.ToString(StatRow["reason"])); //reden
			WriteString(StatRow["timestamp"] + ""); //Start Datum Proefperiode
            WriteInteger(Convert.ToInt32(StatRow["probation_time"])); //Proefperiode. uren over
			WriteString(Convert.ToString(StatRow["next_sanction"])); //Type Sacntie?
			WriteInteger(Convert.ToInt32(StatRow["next_sanction_time"])); //Geen idee
            WriteInteger(Convert.ToInt32(StatRow["int-5"])); //Geen idee
            WriteString(""); //Geen idee
            WriteBoolean(false); //Geen idee
        }
	}
}