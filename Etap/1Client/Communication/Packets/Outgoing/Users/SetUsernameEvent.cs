namespace Retro.Communication.Packets.Incoming.Users
{
	class SetUsernameEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            string Username = Packet.PopString();
        }
    }
}
