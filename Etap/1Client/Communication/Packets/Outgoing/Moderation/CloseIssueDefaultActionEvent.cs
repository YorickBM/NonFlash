using Retro.Hotel.GameClients;

namespace Retro.Communication.Packets.Incoming.Moderation
{
    class CloseIssueDefaultActionEvent : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            if (session == null || session.GetHabbo() == null)
                return;
        }
    }
}
