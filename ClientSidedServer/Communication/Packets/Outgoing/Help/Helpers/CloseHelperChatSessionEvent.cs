using Retro.Hotel.GameClients;
using Retro.Hotel.Helpers;

namespace Retro.Communication.Packets.Incoming.Help.Helpers
{
    class CloseHelperChatSessionEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var Element = HelperToolsManager.GetElement(Session);

            if (Element != null)
            {
                Element.End();
                if (Element.OtherElement != null)
                    Element.OtherElement.End();
            }
        }
    }
}
