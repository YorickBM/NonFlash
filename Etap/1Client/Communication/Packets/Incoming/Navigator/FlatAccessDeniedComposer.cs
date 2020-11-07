using Etap.Communication.Packets;
using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;
using Etap.Utilities;

namespace Retro.Communication.Packets.Incoming.Navigator
{
	class FlatAccessDeniedComposer : IPacketEvent
    {
        /*public FlatAccessDeniedComposer(string Username)
            : base(ServerPacketHeader.FlatAccessDeniedMessageComposer)
        {
			WriteString(Username);
        }*/

        public void Parse(GameClient Session, ClientPacket packet)
        {
            string username = packet.PopString();
            Logger.Debug(username);
        }
    }
}
