using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;
using Etap.Utilities;

namespace Etap.Communication.Packets.Outgoing.Navigator
{
	class NavigatorSettingsComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int HomeRoom1 = Packet.PopInt();
            int HomeRoom2 = Packet.PopInt();

            Logger.DebugWarn("Navigator Settings Composer ->", HomeRoom1, HomeRoom2);
        }
    }
}
