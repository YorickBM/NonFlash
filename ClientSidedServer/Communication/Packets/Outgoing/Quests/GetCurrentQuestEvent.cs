namespace Retro.Communication.Packets.Incoming.Quests
{
	class GetCurrentQuestEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            RetroEnvironment.GetGame().GetQuestManager().GetCurrentQuest(Session, Packet);
        }
    }
}
