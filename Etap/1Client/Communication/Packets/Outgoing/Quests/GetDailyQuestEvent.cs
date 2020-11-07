/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Retro.Hotel.GameClients;
using Retro.Communication.Packets.Outgoing.LandingView;

namespace Retro.Communication.Packets.Incoming.Quests
{
    class GetDailyQuestEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int UsersOnline = RetroEnvironment.GetGame().GetClientManager().Count;

            Session.SendMessage(new ConcurrentUsersGoalProgressComposer(UsersOnline));
        }
    }
}*/
