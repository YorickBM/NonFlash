using Etap.Communication.Packets;
using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;
using Etap.Utilities;

namespace Retro.Communication.Packets.Incoming.Inventory.Purse
{
	class HabboActivityPointNotificationComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int balance = Packet.PopInt();
            int notif = Packet.PopInt();
            int type = Packet.PopInt();

            Logger.DebugWarn(balance, " <- amount, ", notif, " <- Notify, ", type, " <- Type");
        }
    }
}
