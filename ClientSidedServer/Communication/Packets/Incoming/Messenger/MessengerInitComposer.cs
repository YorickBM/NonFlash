using System;

namespace Retro.Communication.Packets.Outgoing.Messenger
{
	class MessengerInitComposer : ServerPacket
    {
        public MessengerInitComposer(Hotel.GameClients.GameClient Session)
            : base(ServerPacketHeader.MessengerInitMessageComposer)
        {
			WriteInteger(Convert.ToInt32(RetroEnvironment.GetGame().GetSettingsManager().TryGetValue("messenger.buddy_limit")));//Friends max.
			WriteInteger(300);
			WriteInteger(800);
			WriteInteger(1); // category count
			WriteInteger(1);
			WriteString("Groups");
        }
    }
}
