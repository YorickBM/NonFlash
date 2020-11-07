using Retro.Communication.Packets.Outgoing.Help.Helpers;
using Retro.Hotel.GameClients;
using Retro.Hotel.Helpers;

namespace Retro.Communication.Packets.Incoming.Help.Helpers
{
    class HelperSessioChatSendMessageEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var Element = HelperToolsManager.GetElement(Session);
            var message = Packet.PopString();
            if (Element.OtherElement != null)
            {
                Session.SendMessage(new HelperSessionSendChatComposer(Session.GetHabbo().Id, message));
                Element.OtherElement.Session.SendMessage(new HelperSessionSendChatComposer(Session.GetHabbo().Id, message));
            }
            else
            {
                Session.SendMessage(new CallForHelperErrorComposer(0));
            }
        }
    }
}
