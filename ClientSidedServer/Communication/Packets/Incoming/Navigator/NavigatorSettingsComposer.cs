using Retro.Communication.Packets.Incoming;
using Retro.Hotel.GameClients;
using Retro.Utilities;

namespace Retro.Communication.Packets.Outgoing.Navigator
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
