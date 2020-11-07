using Retro.Hotel.Moderation;
using Retro.Communication.Packets.Outgoing.Moderation;

namespace Retro.Communication.Packets.Incoming.Moderation
{
    class PickTicketEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
                return;

            int Junk = Packet.PopInt();//??
            int TicketId = Packet.PopInt();

            ModerationTicket Ticket = null;
            if (!RetroEnvironment.GetGame().GetModerationManager().TryGetTicket(TicketId, out Ticket))
                return;

            Ticket.Moderator = Session.GetHabbo();
            RetroEnvironment.GetGame().GetClientManager().SendMessage(new ModeratorSupportTicketComposer(Session.GetHabbo().Id, Ticket), "mod_tool");
        }
    }
}
