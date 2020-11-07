using Retro.Communication.Packets.Outgoing.Talents;

namespace Retro.Communication.Packets.Incoming.Talents
{
	class RetrieveCitizenshipStatus : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            string Type = Packet.PopString();

            Session.SendMessage(new TalentTrackLevelComposer(Session, Type));
        }
    }
}
