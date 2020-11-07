using Retro.Communication.Packets.Outgoing.Sound;

namespace Retro.Communication.Packets.Incoming.Sound
{
    class GetSongInfoEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new TraxSongInfoComposer());
        }
    }
}
