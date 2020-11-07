using Retro.Communication.Packets.Outgoing.Rooms.Notifications;
using Retro.Hotel.GameClients;
using Retro.Hotel.Helpers;

namespace Retro.Communication.Packets.Incoming.Help.Helpers
{
    class FinishHelperSessionEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var Voted = Packet.PopBoolean();
            var Element = HelperToolsManager.GetElement(Session);
            if (Element is HelperCase)
            {
                if (Voted)
                    Element.OtherElement.Session.SendMessage(RoomNotificationComposer.SendBubble("Ambassadeur", "" + Session.GetHabbo().Username + ", Bedankt voor je deelname aan het Alfa-programma, U heeft het gebruikersprobleem succesvol aangepakt.", "catalog/open/habbiween"));
                else
                    Element.OtherElement.Session.SendMessage(RoomNotificationComposer.SendBubble("Ambassadeur", "" + Session.GetHabbo().Username + ", Bedankt voor je deelname aan het Alfa-programma, U heeft het gebruikersprobleem succesvol aangepakt.", "catalog/open/habbiween"));
            }

            Element.Close();
        }
    }
}
