using Etap.Communication.Packets.Incoming;
using Etap.Engine.User;
using Etap.Hotel.GameClients;
using Etap.Utilities;

namespace Etap.Communication.Packets.Outgoing.Handshake
{
    public class UserObjectComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int Id = Packet.PopInt();
            string Username = Packet.PopString();
            string Look = Packet.PopString();
            string Gender = Packet.PopString();
            string Motto = Packet.PopString();
            string h = Packet.PopString();
            bool w = Packet.PopBoolean();
            int Respect = Packet.PopInt();
            int DailyRespectPoints = Packet.PopInt();
            int DailyPetRespectPoints = Packet.PopInt();
            bool x = Packet.PopBoolean(); // Friends stream active
            string LastOnline = Packet.PopString(); // last online?
            bool CanChangeName = Packet.PopBoolean(); // Can change name
            bool z = Packet.PopBoolean();

            int Credits = Packet.PopInt();
            int Duckets = Packet.PopInt();
            int Diamonds = Packet.PopInt();

            RetroEnvironment.GetGame().GetClientManager().RegisterClient(Session, Id, Username);
            GameClient session = RetroEnvironment.GetGame().GetClientManager().GetClientByUsername(Username);
            if (Session != null) Logger.Debug("SuccesFully Registerd Client");

            Logger.Info(Credits, " - ", Duckets, " - ", Diamonds, "");
            User usr = new User(Credits, Duckets, Diamonds, 0);
            session.SetUser(usr);
        }
    }
}