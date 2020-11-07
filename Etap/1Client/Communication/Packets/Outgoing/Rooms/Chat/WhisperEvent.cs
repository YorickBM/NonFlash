using Etap.Communication.Packets.Outgoing;

namespace Etap.Communication.Packets.Incoming.Rooms.Chat
{
    public class WhisperEvent : ServerPacket
    {
        public WhisperEvent(string username, string message, int colour) : base (ServerPacketHeader.WhisperMessageEvent)
        {
            base.WriteString(username + " " + message); //User + " " + Message
            base.WriteInteger(colour);
        }
    }
}