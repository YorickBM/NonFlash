using Retro.Communication.Packets.Outgoing;
using Retro.Communication.Packets.Outgoing.LandingView;
using Retro.Hotel.GameClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retro.Communication.Packets.Incoming.LandingView
{
    class ConcurrentUsersCompetition : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int goal = int.Parse(RetroEnvironment.GetGame().GetSettingsManager().TryGetValue("usersconcurrent_goal"));
            int UsersOnline = RetroEnvironment.GetGame().GetClientManager().Count;
            foreach (GameClient Target in RetroEnvironment.GetGame().GetClientManager().GetClients.ToList())
            {
                if (UsersOnline < goal)
                {
                    int type = 1;
                    Target.SendMessage(new ConcurrentUsersGoalProgressComposer(UsersOnline, type, goal));
                }
                else if (!Target.GetHabbo().GetStats().PurchaseUsersConcurrent && UsersOnline >= goal)
                {
                    int type = 2;
                    Target.SendMessage(new ConcurrentUsersGoalProgressComposer(UsersOnline, type, goal));
                }
                else if (Target.GetHabbo().GetStats().PurchaseUsersConcurrent && UsersOnline >= goal)
                {
                    int type = 3;
                    Target.SendMessage(new ConcurrentUsersGoalProgressComposer(UsersOnline, type, goal));
                }
                else
                {
                    int type = 0;
                    Target.SendMessage(new ConcurrentUsersGoalProgressComposer(UsersOnline, type, goal));
                }
            }
        }
    }
}
