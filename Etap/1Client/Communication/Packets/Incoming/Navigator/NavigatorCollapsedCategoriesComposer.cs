using Etap.Hotel.GameClients;
using Etap.Utilities;

namespace Etap.Communication.Packets.Incoming.Navigator
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
