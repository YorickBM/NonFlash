using Etap.Communication.Packets.Outgoing.Navigator;
using Etap.Hotel.GameClients;
using Etap.Utilities;

namespace Etap.Communication.Packets.Incoming.Navigator
{
    class NavigatorMetaDataParserComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int count = Packet.PopInt(); Logger.DebugWarn("Count:", count);
            for (int i = 0; i < count; i++)
            {
                int id = Packet.PopInt(); //Id
                string searchCode = Packet.PopString(); //Search code
                string filter = Packet.PopString(); //Filter
                string localization = Packet.PopString(); //localization

                Logger.DebugWarn("id", id);
                Logger.DebugWarn("searchCode", searchCode);
                Logger.DebugWarn("filter", filter);
                Logger.DebugWarn("localization", localization);

                Session.SendPacket(new NavigatorSearchEvent(searchCode, ""));
            }
        }
    }
}
