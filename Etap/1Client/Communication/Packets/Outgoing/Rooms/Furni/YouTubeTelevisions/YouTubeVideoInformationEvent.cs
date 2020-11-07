using System.Linq;

using Retro.Hotel.Items.Televisions;
using Retro.Communication.Packets.Outgoing.Rooms.Furni.YouTubeTelevisions;

namespace Retro.Communication.Packets.Incoming.Rooms.Furni.YouTubeTelevisions
{
	class YouTubeVideoInformationEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int ItemId = Packet.PopInt();
            string VideoId = Packet.PopString();

            foreach (TelevisionItem Tele in RetroEnvironment.GetGame().GetTelevisionManager().TelevisionList.ToList())
            {
                if (Tele.YouTubeId != VideoId)
                    continue;

                Session.SendMessage(new GetYouTubeVideoComposer(ItemId, Tele.YouTubeId));
            }
        }
    }
}