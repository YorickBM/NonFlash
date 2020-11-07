

namespace Retro.Communication.Packets.Incoming.Misc
{
    class ClientVariablesEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            string GordanPath = Packet.PopString();
            string ExternalVariables = Packet.PopString();
        }
    }
}
