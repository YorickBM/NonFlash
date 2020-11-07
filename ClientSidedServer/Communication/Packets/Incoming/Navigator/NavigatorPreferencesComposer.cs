using Retro.Hotel.GameClients;

namespace Retro.Communication.Packets.Incoming.Navigator
{
    class NavigatorPreferencesComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int x0 = Packet.PopInt(); //Header Width??
            int x1 = Packet.PopInt(); //Header Height??

            int width = Packet.PopInt(); //Width
            int height = Packet.PopInt(); //Height

            bool x2 = Packet.PopBoolean();
            int x3 = Packet.PopInt();


        }
    }
}

