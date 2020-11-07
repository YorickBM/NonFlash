using Retro.Communication.Packets.Incoming;
using Retro.Hotel.GameClients;

namespace Retro.Communication.Packets.Outgoing.Handshake
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
        }
    }
}