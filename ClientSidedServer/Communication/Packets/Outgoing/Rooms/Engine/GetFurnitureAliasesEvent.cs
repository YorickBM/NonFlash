using Retro.Hotel.GameClients;
using Retro.Communication.Packets.Outgoing.Rooms.Engine;

namespace Retro.Communication.Packets.Incoming.Rooms.Engine
{
    class GetFurnitureAliasesEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new FurnitureAliasesComposer());
        }
    }
}
