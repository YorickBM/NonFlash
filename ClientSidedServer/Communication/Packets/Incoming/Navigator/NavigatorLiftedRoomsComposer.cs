using Retro.Hotel.GameClients;
using Retro.Utilities;

namespace Retro.Communication.Packets.Incoming.Navigator
{
    class NavigatorLiftedRoomsComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int count = Packet.PopInt();
            int flatId = Packet.PopInt();
            int x0 = Packet.PopInt();
            string image = Packet.PopString();
            string caption = Packet.PopString();

            Logger.DebugWarn("count", count);
            Logger.DebugWarn("flatId", flatId);
            Logger.DebugWarn("x0", x0);
            Logger.DebugWarn("image", image);
            Logger.DebugWarn("caption", caption);
        }
    }
}
