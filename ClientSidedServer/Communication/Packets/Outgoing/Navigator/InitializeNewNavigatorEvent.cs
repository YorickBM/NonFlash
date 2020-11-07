using Retro.Communication.Packets.Outgoing;

namespace Retro.Communication.Packets.Outgoing.Navigator
{
    public class InitializeNewNavigatorEvent : ServerPacket
    {
        public InitializeNewNavigatorEvent() : base (ServerPacketHeader.InitializeNewNavigatorMessageEvent)
        {

        }
    }
}
