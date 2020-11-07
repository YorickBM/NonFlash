using Retro.Communication.Packets.Outgoing.Sound;
using Retro.Hotel.GameClients;

namespace Retro.Communication.Packets.Incoming.Sound
{
    class LoadJukeboxDiscsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session.GetHabbo().CurrentRoom != null)
                Session.SendMessage(new LoadJukeboxUserMusicItemsComposer(Session.GetHabbo().CurrentRoom));
        }
    }
}
