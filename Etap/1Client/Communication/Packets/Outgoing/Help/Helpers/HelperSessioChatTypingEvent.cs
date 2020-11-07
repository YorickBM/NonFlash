using Retro.Hotel.GameClients;

namespace Retro.Communication.Packets.Incoming.Help.Helpers
{
    class HelperSessioChatTypingEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var element = Hotel.Helpers.HelperToolsManager.GetElement(Session);
            if (element != null && element.OtherElement != null)
                element.OtherElement.Session.SendMessage(new Outgoing.Help.Helpers.HelperSessionChatIsTypingComposer(Packet.PopBoolean()));
        }
    }
}
