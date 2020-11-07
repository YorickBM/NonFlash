namespace Retro.Communication.Packets.Incoming.Quests
{
	class CancelQuestEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            RetroEnvironment.GetGame().GetQuestManager().CancelQuest(Session, Packet);
        }
    }
}
