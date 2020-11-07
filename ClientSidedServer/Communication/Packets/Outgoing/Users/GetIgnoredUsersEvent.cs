﻿using System.Collections.Generic;
using Retro.Hotel.Users;
using Retro.Communication.Packets.Outgoing.Users;

namespace Retro.Communication.Packets.Incoming.Users
{
    class GetIgnoredUsersEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient session, ClientPacket packet)
        {
            List<string> ignoredUsers = new List<string>();

            foreach (int userId in new List<int>(session.GetHabbo().GetIgnores().IgnoredUserIds()))
            {
                Habbo player = RetroEnvironment.GetHabboById(userId);
                if (player != null)
                {
                    if (!ignoredUsers.Contains(player.Username))
                        ignoredUsers.Add(player.Username);
                }
            }

            session.SendMessage(new IgnoredUsersComposer(ignoredUsers));
        }
    }
}