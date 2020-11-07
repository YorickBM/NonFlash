using Etap.Hotel.GameClients;
using Etap.Utilities;

namespace Etap.Communication.Packets.Incoming.Navigator
{
    class FavouritesComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int x = Packet.PopInt();
            int favouritesIDs = Packet.PopInt();
            for (int id = 0; id < favouritesIDs; id++){
                int favId = Packet.PopInt();
            }
        }
    }
}
