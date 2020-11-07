using System.Collections.Generic;
using Retro.Hotel.Users;
using Retro.Hotel.Groups;
using Retro.Communication.Packets.Outgoing.Users;
using Retro.Database.Interfaces;


namespace Retro.Communication.Packets.Incoming.Users
{
    class OpenPlayerProfileEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int userID = Packet.PopInt();

            Habbo targetData = RetroEnvironment.GetHabboById(userID);
            if (targetData == null)
            {
                Session.SendNotification("Ocorreu um erro ao encontrar o perfil do usuário.");
                return;
            }

            List<Group> groups = RetroEnvironment.GetGame().GetGroupManager().GetGroupsForUser(targetData.Id);

            int friendCount = 0;
            using (IQueryAdapter dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT COUNT(0) FROM `messenger_friendships` WHERE (`user_one_id` = @userid OR `user_two_id` = @userid)");
                dbClient.AddParameter("userid", userID);
                friendCount = dbClient.getInteger();
            }

            Session.SendMessage(new ProfileInformationComposer(targetData, Session, groups, friendCount));
        }
    }
}
