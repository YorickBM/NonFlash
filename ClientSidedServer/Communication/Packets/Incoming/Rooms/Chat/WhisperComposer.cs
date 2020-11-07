using Retro.Hotel.GameClients;

namespace Retro.Communication.Packets.Incoming.Rooms.Chat
{
	public class WhisperComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int virtualId = Packet.PopInt();
            string message = Packet.PopString();
            int emotion = Packet.PopInt();
            int color = Packet.PopInt();

            int x0 = Packet.PopInt();
            int x1 = Packet.PopInt();
        }
    }
}