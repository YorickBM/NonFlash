using Retro.Hotel.Rooms.Polls;
using Retro.Communication.Packets.Outgoing.Rooms.Polls;

namespace Retro.Communication.Packets.Incoming.Rooms.Polls
{
    class PollStartEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient session, ClientPacket packet)
        {
            int pollId = packet.PopInt();

            RoomPoll poll = null;
            if (!RetroEnvironment.GetGame().GetPollManager().TryGetPoll(pollId, out poll))
                return;

            session.SendMessage(new PollContentsComposer(poll));
        }
    }
}
