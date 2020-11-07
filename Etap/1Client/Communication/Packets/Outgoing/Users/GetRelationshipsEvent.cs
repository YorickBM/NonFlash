using System;
using System.Linq;
using Retro.Hotel.Users;
using Retro.Communication.Packets.Outgoing.Users;

namespace Retro.Communication.Packets.Incoming.Users
{
    class GetRelationshipsEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Habbo Habbo = RetroEnvironment.GetHabboById(Packet.PopInt());
            if (Habbo == null)
                return;

            var rand = new Random();
            Habbo.Relationships = Habbo.Relationships.OrderBy(x => rand.Next()).ToDictionary(item => item.Key, item => item.Value);

            int Loves = Habbo.Relationships.Count(x => x.Value.Type == 1);
            int Likes = Habbo.Relationships.Count(x => x.Value.Type == 2);
            int Hates = Habbo.Relationships.Count(x => x.Value.Type == 3);

            Session.SendMessage(new GetRelationshipsComposer(Habbo, Loves, Likes, Hates));
        }
    }
}
