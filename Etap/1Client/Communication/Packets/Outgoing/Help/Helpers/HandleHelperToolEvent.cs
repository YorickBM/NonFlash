using Retro.Hotel.GameClients;
using Retro.Communication.Packets.Outgoing.Help.Helpers;
using Retro.Hotel.Helpers;
using Retro.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Retro.Communication.Packets.Incoming.Help.Helpers
{
    class HandleHelperToolEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session.GetHabbo().Rank > 2 || Session.GetHabbo()._guidelevel > 0)
            {

                var onDuty = Packet.PopBoolean();
                var isGuide = Packet.PopBoolean();
                var isHelper = Packet.PopBoolean();
                var isGuardian = Packet.PopBoolean();
                if (onDuty)
                    HelperToolsManager.AddHelper(Session, isHelper, isGuardian, isGuide);
                else
                    HelperToolsManager.RemoveHelper(Session);
                Session.SendMessage(new HandleHelperToolComposer(onDuty));
            }
            else
            {
                Session.SendMessage(new RoomNotificationComposer("Oeps, je kunt deze tool niet gebruiken!", ""));
            }

        }
    }
}
