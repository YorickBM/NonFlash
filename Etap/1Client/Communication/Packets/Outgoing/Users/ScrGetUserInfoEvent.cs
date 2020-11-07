using Etap.Hotel.GameClients;
using Etap.Communication.Packets.Outgoing.Handshake;
using Etap.Communication.Packets.Outgoing;
using Etap.Communication.Packets.Outgoing.Users;

namespace Etap.Communication.Packets.Incoming.Users
{
    public class ScrGetUserInfoEvent : ServerPacket
    {
        public ScrGetUserInfoEvent() : base(ServerPacketHeader.ScrGetUserInfoMessageEvent)
        {
        }
    }
}
