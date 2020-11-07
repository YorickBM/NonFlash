using System;
using System.Collections.Generic;
using Retro.Communication.Packets.Incoming;
using Retro.Hotel.GameClients;
using Retro.Utilities;

namespace Retro.Communication.Packets.Outgoing.Moderation
{
    class ModeratorInitComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int tickets = Packet.PopInt();
            Logger.DebugWarn("Moderation Init Composer (ticket)->", tickets);

            for (int ticket = 0; ticket < tickets; ticket++)
            {
                int id = Packet.PopInt(); // Id
                int tabId = Packet.PopInt(); // Tab ID
                int type = Packet.PopInt(); // Type
                int catagory = Packet.PopInt(); // Category
                int overflowFixWithTimeStamp = Packet.PopInt(); // This should fix the overflow?
                int priority = Packet.PopInt(); // Priority
                int senderId = Packet.PopInt(); // Sender ID
                int basic1 = Packet.PopInt(); //Just a basic 1
                string senderName = Packet.PopString();
                int reporterId = Packet.PopInt(); // Reported ID
                string reporterName = Packet.PopString(); // Reported Name
                int moderatorId = Packet.PopInt(); // Moderator ID
                string moderatorName = Packet.PopString(); // Mod Name
                string issue = Packet.PopString(); // Issue
                int roomId = Packet.PopInt(); // Room Id
                int loop = Packet.PopInt();//LOOP (Basic 0)

                Logger.DebugWarn("Moderation Init Composer -->", id, tabId, type, catagory, overflowFixWithTimeStamp, priority,
                    senderId, basic1, senderName, reporterId, reporterName, moderatorId, moderatorName, issue, roomId, loop);
            }

            int presets = Packet.PopInt();
            Logger.DebugWarn("Moderation Init Composer (user)->", presets);
            for (int preset = 0; preset < presets; preset++)
            {
                string pre = Packet.PopString();
                Logger.DebugWarn("Moderation Init Composer -->", pre);
            }

            // TODO: Figure out
            int x0 = Packet.PopInt();
            {
                //Loop a string.
            }

            Logger.DebugWarn("Moderation Init Composer (bools)->", x0);
            bool ticketRight = Packet.PopBoolean(); // Ticket right
            bool chatlogs = Packet.PopBoolean(); // Chatlogs

            bool userActions = Packet.PopBoolean(); // User actions alert etc
            bool kickUser = Packet.PopBoolean(); // Kick users
            bool banUser = Packet.PopBoolean(); // Ban users
            bool caution = Packet.PopBoolean(); // Caution etc

            bool x1 = Packet.PopBoolean(); // ???
            Logger.DebugWarn("Moderation Init Composer -->", ticketRight, chatlogs, userActions, kickUser, banUser, caution, x1);

            int roomPresets = Packet.PopInt();
            Logger.DebugWarn("Moderation Init Composer (Room)->", roomPresets);
            for (int preset = 0; preset < roomPresets; preset++)
            {
                string pre = Packet.PopString();
                Logger.DebugWarn("Moderation Init Composer -->",pre);
            }
        }
    }
}