using System.Collections.Generic;
using Retro.Hotel.Achievements;
using Retro.Communication.Packets.Outgoing.Talents;

namespace Retro.Communication.Packets.Incoming.Talents
{
    class GetTalentTrackEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            string Type = Packet.PopString();

            List<Talent> talents = RetroEnvironment.GetGame().GetTalentManager().GetTalents(Type, -1);

            if (talents == null)
                return;

            Session.SendMessage(new TalentTrackComposer(Session, Type, talents));
        }
    }
}
