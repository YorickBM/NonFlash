using System;
using System.Linq;
using System.Collections.Generic;
using Retro.Hotel.Items.Televisions;
using Retro.Communication.Packets.Outgoing.Rooms.Furni.YouTubeTelevisions;

namespace Retro.Communication.Packets.Incoming.Rooms.Furni.YouTubeTelevisions
{
	class YouTubeGetNextVideo : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            ICollection<TelevisionItem> Videos = RetroEnvironment.GetGame().GetTelevisionManager().TelevisionList;

            if (Videos.Count == 0)
            {
                Session.SendNotification("Oh, it looks like the hotel manager has not added any videos for you to see! :(");
                return;
            }

            int ItemId = Packet.PopInt();
            int Next = Packet.PopInt();

            TelevisionItem Item = null;
            Dictionary<int, TelevisionItem> dict = RetroEnvironment.GetGame().GetTelevisionManager()._televisions;
            foreach (TelevisionItem value in RandomValues(dict).Take(1))
            {
                Item = value;
            }

            if(Item == null)
            {
                Session.SendNotification("Oops! It seems like there was a problem!");
                return;
            }

            Session.SendMessage(new GetYouTubeVideoComposer(ItemId, Item.YouTubeId));
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