
using Retro.Hotel.Users.Effects;

namespace Retro.Communication.Packets.Outgoing.Inventory.AvatarEffects
{
	class AvatarEffectExpiredComposer : ServerPacket
    {
        public AvatarEffectExpiredComposer(AvatarEffect Effect)
            : base(ServerPacketHeader.AvatarEffectExpiredMessageComposer)
        {
			WriteInteger(Effect.SpriteId);
        }
    }
}
