using System;
using System.Collections.Generic;
using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;
using Etap.Utilities;

namespace Etap.Communication.Packets.Outgoing.Moderation
{
    class ModeratorInitComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int tickets = Packet.PopInt();

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
            }

            int presets = Packet.PopInt();
            for (int preset = 0; preset < presets; preset++)
            {
                string pre = Packet.PopString();
            }

            // TODO: Figure out
            int x0 = Packet.PopInt();
            {
                //Loop a string.
            }

            bool ticketRight = Packet.PopBoolean(); // Ticket right
            bool chatlogs = Packet.PopBoolean(); // Chatlogs

            bool userActions = Packet.PopBoolean(); // User actions alert etc
            bool kickUser = Packet.PopBoolean(); // Kick users
            bool banUser = Packet.PopBoolean(); // Ban users
            bool caution = Packet.PopBoolean(); // Caution etc

            bool x1 = Packet.PopBoolean(); // ???

            int roomPresets = Packet.PopInt();
            for (int preset = 0; preset < roomPresets; preset++)
            {
                string pre = Packet.PopString();
            }
        }
    }
}