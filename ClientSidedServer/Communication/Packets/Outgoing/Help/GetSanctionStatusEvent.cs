using Retro.Hotel.GameClients;
using Retro.Communication.Packets.Outgoing.Help;

namespace Retro.Communication.Packets.Incoming.Help
{
    class GetSanctionStatusEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new SanctionStatusComposer(Session));
        }
    }
}
