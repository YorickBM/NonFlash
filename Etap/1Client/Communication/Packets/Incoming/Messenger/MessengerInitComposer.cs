using Etap.Communication.Packets;
using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;
using Etap.Utilities;
using System;

namespace Etap.Communication.Packets.Incoming.Messenger
{
	class MessengerInitComposer : IPacketEvent
    {
		public void Parse(GameClient Session, ClientPacket Packet)
		{
			int friendsMax = Packet.PopInt();
			int x1 = Packet.PopInt(); //300 -> width?
			int x2 = Packet.PopInt(); //800 -> height?
			int catagoryCount = Packet.PopInt(); //1
			int x3 = Packet.PopInt(); //1
			string x4 = Packet.PopString(); //Groups

			//TODO Create Messenger thingy
		}
	}
}
