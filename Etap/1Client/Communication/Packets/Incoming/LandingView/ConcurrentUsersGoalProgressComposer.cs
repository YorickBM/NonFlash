using Retro.Core;
using Retro.Hotel.GameClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retro.Communication.Packets.Outgoing.LandingView
{
    class ConcurrentUsersGoalProgressComposer : ServerPacket
    {
        public ConcurrentUsersGoalProgressComposer(int UsersNow, int type, int goal)
            : base(ServerPacketHeader.ConcurrentUsersGoalProgressMessageComposer)
        {
            base.WriteInteger(type);
            base.WriteInteger(UsersNow);
            base.WriteInteger(goal);
        }
    }
}
