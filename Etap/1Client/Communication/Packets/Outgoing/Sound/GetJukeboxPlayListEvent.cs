using Retro.Communication.Packets.Outgoing.Sound;
using Retro.Hotel.GameClients;


namespace Retro.Communication.Packets.Incoming.Sound
{
    class GetJukeboxPlayListEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session.GetHabbo().CurrentRoom != null)
                Session.SendMessage(new SetJukeboxPlayListComposer(Session.GetHabbo().CurrentRoom));
            RetroEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_MusicPlayer", 1);
        }
    }
}
