using Retro.Communication.Packets.Outgoing;

namespace Retro.Communication.Packets.Incoming.Rooms.Chat
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