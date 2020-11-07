using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;
using Etap.Utilities;

namespace Etap.Communication.Packets.Outgoing.Notifications
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
