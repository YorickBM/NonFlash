using Etap.Communication.Packets.Outgoing;

namespace Etap.Communication.Packets.Outgoing.Navigator
{
    public class SaveNavigatorPositionEvent : ServerPacket
    {
        public SaveNavigatorPositionEvent(int x, int y) : base (ServerPacketHeader.SaveNavigatorPositionEvent)
        {
            base.WriteInteger(x);
            base.WriteInteger(y);
        }
    }
}
