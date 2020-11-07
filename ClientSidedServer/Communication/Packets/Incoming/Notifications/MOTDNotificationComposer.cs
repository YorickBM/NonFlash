using Retro.Communication.Packets.Incoming;
using Retro.Hotel.GameClients;
using Retro.Utilities;

namespace Retro.Communication.Packets.Outgoing.Notifications
{
    class MOTDNotificationComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int x = Packet.PopInt();
            string message = Packet.PopString();
            Logger.DebugWarn(message);
        }
    }
}
