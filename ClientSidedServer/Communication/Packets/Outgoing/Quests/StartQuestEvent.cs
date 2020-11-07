namespace Retro.Communication.Packets.Incoming.Quests
{
	class StartQuestEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int QuestId = Packet.PopInt();

            RetroEnvironment.GetGame().GetQuestManager().ActivateQuest(Session, QuestId);
        }
    }
}
