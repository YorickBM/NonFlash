using Retro.Hotel.Users;
using Retro.Communication.Packets.Outgoing.Users;

namespace Retro.Communication.Packets.Incoming.Users
{
    class GetSelectedBadgesEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int UserId = Packet.PopInt();
            Habbo Habbo = RetroEnvironment.GetHabboById(UserId);
            if (Habbo == null)
                return;

            Session.SendMessage(new HabboUserBadgesComposer(Habbo));
        }
    }
}