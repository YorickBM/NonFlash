using Etap.Communication.Packets.Outgoing;

namespace Retro.Communication.Packets.Outgoing.Misc
{
    class ClientVariablesEvent : ServerPacket
    {
        public ClientVariablesEvent() : base(ServerPacketHeader.ClientVariablesMessageEvent)
        {
            base.WriteString("Gordon/Path/Here");
            base.WriteString("ExternalVariables/Url/Here");
        }
    }
}
