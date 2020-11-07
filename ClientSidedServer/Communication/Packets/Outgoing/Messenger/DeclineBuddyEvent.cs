namespace Retro.Communication.Packets.Incoming.Messenger
{
    class DeclineBuddyEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || Session.GetHabbo().GetMessenger() == null)
                return;

            bool DeclineAll = Packet.PopBoolean();
            int Amount = Packet.PopInt();
         
            if (!DeclineAll)
            {
                int RequestId = Packet.PopInt();
                Session.GetHabbo().GetMessenger().HandleRequest(RequestId);
            }
            else
                Session.GetHabbo().GetMessenger().HandleAllRequests();          
        }
    }
}
