using Retro.Communication.Packets.Outgoing.Notifications;
using Retro.Hotel.GameClients;
using Retro.Core;

namespace Retro.Communication.Packets.Incoming.Moderation
{
    class AmbassadorAlert : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session.GetHabbo().Rank < ExtraSettings.AmbassadorMinRank) return;
            int userId = Packet.PopInt();
            GameClient user = RetroEnvironment.GetGame().GetClientManager().GetClientByUserID(userId);
            if (user == null) return;
            user.SendMessage(new SuperNotificationComposer("", "${notification.ambassador.alert.warning.title}", "${notification.ambassador.alert.warning.message}", "", ""));
        }
    }
}