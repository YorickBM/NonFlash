using Retro.Hotel.GameClients;
using Retro.Communication.Packets.Outgoing.Handshake;
using Retro.Communication.Packets.Outgoing;
using Retro.Communication.Packets.Outgoing.Users;

namespace Retro.Communication.Packets.Incoming.Users
{
    public class ScrGetUserInfoEvent : ServerPacket
    {
        public ScrGetUserInfoEvent() : base(ServerPacketHeader.ScrGetUserInfoMessageEvent)
        {
        }
    }
}
