using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retro.Communication.Packets.Outgoing.LandingView
{
    class CommunityGoalComposer : ServerPacket
    {
        public CommunityGoalComposer()
            : base(ServerPacketHeader.CommunityGoalComposer)
        {
            base.WriteBoolean(true); //Achieved?
            base.WriteInteger(0); //User Amount
            base.WriteInteger(1); //User Rank
            base.WriteInteger(2); //Total Amount
            base.WriteInteger(3); //Community Highest Achieved
            base.WriteInteger(4); //Community Score Untill Next Level
            base.WriteInteger(5); //Percent Completed Till Next Level
            base.WriteString("WORLDCUP01");
            base.WriteInteger(6); //Timer
            base.WriteInteger(1); //Rank Count
            base.WriteInteger(1); //Rank level
        }
    }
}
