namespace Retro.Communication.Packets.Incoming.Misc
{
    class DisconnectEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.Disconnect();
        }
    }
}
