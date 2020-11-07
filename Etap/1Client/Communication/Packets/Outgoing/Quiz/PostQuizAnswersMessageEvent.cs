using Retro.Hotel.GameClients;
using Retro.Communication.Packets.Outgoing.Quiz;

namespace Retro.Communication.Packets.Incoming.Quiz
{
    class PostQuizAnswersMessageEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new PostQuizAnswersMessageComposer(Session));
        }
    }
}
