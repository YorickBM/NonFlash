using Etap.Communication.Packets;
using Etap.Hotel.GameClients;
using Etap.Communication.Packets.Incoming;

namespace Retro.Communication.Packets.Incoming.Inventory.Badges
{
    class BadgesComposer : IPacketEvent
    {
        /*public BadgesComposer(GameClient Session)
            : base(ServerPacketHeader.BadgesMessageComposer)
        {
            List<Badge> EquippedBadges = new List<Badge>();

			WriteInteger(Session.GetHabbo().GetBadgeComponent().Count);
            foreach (Badge Badge in Session.GetHabbo().GetBadgeComponent().GetBadges().ToList())
            {
				WriteInteger(1);
				WriteString(Badge.Code);

                if (Badge.Slot > 0)
                    EquippedBadges.Add(Badge);
            }

			WriteInteger(EquippedBadges.Count);
            foreach (Badge Badge in EquippedBadges)
            {
				WriteInteger(Badge.Slot);
				WriteString(Badge.Code);
            }
        }*/

        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int badges = Packet.PopInt();
            for(int i = 0; i < badges; i++)
            {
                int x0 = Packet.PopInt(); // 1
                string badgeCode = Packet.PopString();
            }

            int equipedBadges = Packet.PopInt();
            for(int i = 0; i < equipedBadges; i++)
            {
                int slot = Packet.PopInt();
                string badgeCode = Packet.PopString();
            }
        }
    }
}
