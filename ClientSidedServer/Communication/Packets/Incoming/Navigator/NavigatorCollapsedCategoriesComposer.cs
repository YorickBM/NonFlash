using Retro.Hotel.GameClients;
using Retro.Utilities;

namespace Retro.Communication.Packets.Incoming.Navigator
{
    class NavigatorCollapsedCategoriesComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int x0 = Packet.PopInt();
            Logger.Debug("x0", x0);
        }
    }
}
