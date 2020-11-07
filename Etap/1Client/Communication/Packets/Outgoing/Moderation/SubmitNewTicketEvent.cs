using Etap.Communication.Packets.Outgoing;

namespace Etap.Communication.Packets.Incoming.Moderation
{
    class SubmitNewTicketEvent : ServerPacket
    {
        public SubmitNewTicketEvent() : base(ServerPacketHeader.SubmitNewTicketMessageEvent)
        {
            base.WriteString("My Message"); //Message
            base.WriteInteger(1); //Catagory
            base.WriteInteger(2); //Reported User ID
            base.WriteInteger(1); //Type
            base.WriteInteger(2); //MessageCount

            base.WriteInteger(0); //Message Nun ?!?!?
            base.WriteString("First Chat Message"); //Message
            base.WriteInteger(1); //Message Nun ?!?!?
            base.WriteString("Second Chat Message"); //Message


        }
    }
}
