using Retro.Hotel.GameClients;
using Retro.Utilities;

namespace Retro.Communication.Packets.Incoming.Navigator
{
    class FavouritesComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int x = Packet.PopInt();
            int favouritesIDs = Packet.PopInt();
            Logger.DebugWarn("Favourites Composer ->", x, favouritesIDs);
            for (int id = 0; id < favouritesIDs; id++){
                int favId = Packet.PopInt();
                Logger.DebugWarn("Favourites Composer -->", favId);
            }
        }
    }
}
