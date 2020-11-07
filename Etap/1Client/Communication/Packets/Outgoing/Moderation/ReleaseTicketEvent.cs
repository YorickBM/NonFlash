using Retro.Communication.Packets.Outgoing.Moderation;
using Retro.Hotel.Moderation;

namespace Retro.Communication.Packets.Incoming.Moderation
{
    class ReleaseTicketEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
                return;

            int Amount = Packet.PopInt();

            for (int i = 0; i < Amount; i++)
            {
                ModerationTicket Ticket = null;
                if (!RetroEnvironment.GetGame().GetModerationManager().TryGetTicket(Packet.PopInt(), out Ticket))
                    continue;

                Ticket.Moderator = null;
                RetroEnvironment.GetGame().GetClientManager().SendMessage(new ModeratorSupportTicketComposer(Session.GetHabbo().Id, Ticket), "mod_tool");
            }
        }
    }
}