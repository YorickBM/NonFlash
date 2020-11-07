using Retro.Hotel.GameClients;
using Retro.Utilities;

namespace Retro.Communication.Packets.Incoming.Navigator
{
    class NavigatorMetaDataParserComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int count = Packet.PopInt(); Logger.DebugWarn("count", count);
            for (int i = 0; i < count; i++)
            {
                string searchCode = Packet.PopString(); Logger.DebugWarn("searchCode", searchCode);
                int savedCount = Packet.PopInt(); Logger.DebugWarn("savedCount", savedCount);
                for (int y = 0; y < savedCount; y++)
                {
                    int id = Packet.PopInt(); //Id
                    string savedSearchCode = Packet.PopString(); //Search code
                    string filter = Packet.PopString(); //Filter
                    string localization = Packet.PopString(); //localization

                    Logger.DebugWarn("id", id);
                    Logger.DebugWarn("savedSearchCode", savedSearchCode);
                    Logger.DebugWarn("filter", filter);
                    Logger.DebugWarn("localization", localization);
                }
            }
        }
    }
}
