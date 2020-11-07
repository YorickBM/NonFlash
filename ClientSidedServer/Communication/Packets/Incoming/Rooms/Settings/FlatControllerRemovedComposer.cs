
using Retro.Hotel.Rooms;

namespace Retro.Communication.Packets.Outgoing.Rooms.Settings
{
	class FlatControllerRemovedComposer : ServerPacket
    {
        public FlatControllerRemovedComposer(Room Instance, int UserId)
            : base(ServerPacketHeader.FlatControllerRemovedMessageComposer)
        {
			WriteInteger(Instance.Id);
			WriteInteger(UserId);
        }
    }
}
