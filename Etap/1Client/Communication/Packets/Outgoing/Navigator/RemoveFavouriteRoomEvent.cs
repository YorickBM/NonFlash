using Retro.Communication.Packets.Outgoing.Navigator;

using Retro.Database.Interfaces;
using Retro.Hotel.GameClients;
using Retro.Hotel.Users;
using Retro.Communication.Packets.Incoming;

namespace Retro.Communication.Packets.Incoming.Navigator
{
    public class RemoveFavouriteRoomEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int Id = Packet.PopInt();

            Session.GetHabbo().FavoriteRooms.Remove(Id);
            Session.SendMessage(new UpdateFavouriteRoomComposer(Id, false));

            using (IQueryAdapter dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("DELETE FROM user_favorites WHERE user_id = " + Session.GetHabbo().Id + " AND room_id = " + Id + " LIMIT 1");
            }
        }
    }
}