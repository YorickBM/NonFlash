
using System.Collections.Generic;

using Retro.Hotel.Games;
using Retro.Communication.Packets.Outgoing.GameCenter;

namespace Retro.Communication.Packets.Incoming.GameCenter
{
    class GetGameListingEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            ICollection<GameData> Games = RetroEnvironment.GetGame().GetGameDataManager().GameData;

            Session.SendMessage(new GameListComposer(Games));
        }
    }
}
