using Etap.Communication.Packets.Outgoing;

namespace Etap.Communication.Packets.Outgoing.Navigator
{
    public class InitializeNewNavigatorEvent : ServerPacket
    {
        public InitializeNewNavigatorEvent() : base (ServerPacketHeader.InitializeNewNavigatorMessageEvent)
        {

        }
    }
}
