using Etap.Hotel.GameClients;
using Etap.ImagesCode;

namespace Etap.Communication.Packets.Incoming.Navigator
{
    class NavigatorPreferencesComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int posX = Packet.PopInt(); //Position X
            int posY = Packet.PopInt(); //Position Y

            int width = Packet.PopInt(); //Width
            int height = Packet.PopInt(); //Height

            ///bool x2 = Packet.PopBoolean();
            ///int x3 = Packet.PopInt();
            ///
            GameScreenManager.Instance.GetNavigatorManager().SetPosition(posX, posY);
            GameScreenManager.Instance.GetNavigatorManager().SetSize(width, height);
            GameScreenManager.Instance.GetNavigatorManager().OpenNavigator();
        }
    }
}

