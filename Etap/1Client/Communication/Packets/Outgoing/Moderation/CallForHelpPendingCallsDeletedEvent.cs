using Retro.Communication.Packets.Outgoing.Moderation;
using Retro.Hotel.GameClients;
using Retro.Hotel.Moderation;

namespace Retro.Communication.Packets.Incoming.Moderation
{
    class CallForHelpPendingCallsDeletedEvent : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            if (session == null || session.GetHabbo() == null)
                return;
            
            if (RetroEnvironment.GetGame().GetModerationManager().UserHasTickets(session.GetHabbo().Id))
            {
                ModerationTicket PendingTicket = RetroEnvironment.GetGame().GetModerationManager().GetTicketBySenderId(session.GetHabbo().Id);
                if (PendingTicket != null)
                {
                    PendingTicket.Answered = true;
                    RetroEnvironment.GetGame().GetClientManager().SendMessage(new ModeratorSupportTicketComposer(session.GetHabbo().Id, PendingTicket), "mod_tool");
                }
            }
        }
    }
}
