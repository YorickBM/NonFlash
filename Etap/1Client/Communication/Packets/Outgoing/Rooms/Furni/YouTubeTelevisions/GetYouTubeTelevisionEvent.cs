using System;
using System.Linq;
using System.Collections.Generic;
using Retro.Hotel.Items.Televisions;
using Retro.Communication.Packets.Outgoing.Rooms.Furni.YouTubeTelevisions;

namespace Retro.Communication.Packets.Incoming.Rooms.Furni.YouTubeTelevisions
{
	class GetYouTubeTelevisionEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            int ItemId = Packet.PopInt();
            ICollection<TelevisionItem> Videos = RetroEnvironment.GetGame().GetTelevisionManager().TelevisionList;
            if (Videos.Count == 0)
            {
                Session.SendNotification("Oh, it looks like the hotel manager has not added any videos for you to see! :(");
                return;
            }

            Dictionary<int, TelevisionItem> dict = RetroEnvironment.GetGame().GetTelevisionManager()._televisions;
            foreach (TelevisionItem value in RandomValues(dict).Take(1))
            {
                Session.SendMessage(new GetYouTubeVideoComposer(ItemId, value.YouTubeId));
            }

            Session.SendMessage(new GetYouTubePlaylistComposer(ItemId, Videos));
        }

        public IEnumerable<TValue> RandomValues<TKey, TValue>(IDictionary<TKey, TValue> dict)
        {
            Random rand = new Random();
            List<TValue> values = Enumerable.ToList(dict.Values);
            int size = dict.Count;
            while (true)
            {
                yield return values[rand.Next(size)];
            }
        }
    }
}