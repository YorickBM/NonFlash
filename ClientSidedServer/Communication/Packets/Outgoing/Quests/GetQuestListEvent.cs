using System.Collections.Generic;
using Retro.Hotel.GameClients;
using Retro.Hotel.Quests;
using Retro.Communication.Packets.Incoming;

namespace Retro.Communication.Packets.Incoming.Quests
{
    public class GetQuestListEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            RetroEnvironment.GetGame().GetQuestManager().GetList(Session, null);
        }
    }
}