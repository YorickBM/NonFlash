using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Retro.Core;
using Retro.Hotel.Rooms;
using Retro.Hotel.GameClients;

namespace Retro.Communication.Packets.Incoming.Moderation
{
    class ModerationKickEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().GetPermissions().HasRight("mod_kick"))
                return;

            int UserId = Packet.PopInt();
            string Message = Packet.PopString();

            GameClient Client = RetroEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
            if (Client == null || Client.GetHabbo() == null || Client.GetHabbo().CurrentRoomId < 1 || Client.GetHabbo().Id == Session.GetHabbo().Id)
                return;

            if (Client.GetHabbo().Rank >= Session.GetHabbo().Rank)
            {
                Session.SendNotification(RetroEnvironment.GetGame().GetLanguageManager().TryGetValue("moderation.kick.disallowed"));
                return;
            }

            Room Room = null;
            if (!RetroEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;
            
            Room.GetRoomUserManager().RemoveUserFromRoom(Client, true, false);
        }
    }
}
