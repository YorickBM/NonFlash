using Etap.Communication.Packets.Outgoing;

namespace Etap.Communication.Packets.Incoming.Rooms.Chat
{
    class ChatEvent : ServerPacket
    {
        public ChatEvent(string Message, int Colour) : base(ServerPacketHeader.ChatMessageEvent)
        {
            WriteString(Message);
            WriteInteger(Colour);
        }
    }
}