using Retro.Hotel.Users.Effects;
using Retro.Communication.Packets.Outgoing.Inventory.AvatarEffects;

namespace Retro.Communication.Packets.Incoming.Inventory.AvatarEffects
{
    class AvatarEffectActivatedEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int EffectId = Packet.PopInt();

            AvatarEffect Effect = Session.GetHabbo().Effects().GetEffectNullable(EffectId, false, true);

            if (Effect == null || Session.GetHabbo().Effects().HasEffect(EffectId, true))
            {
                return;
            }

            if (Effect.Activate())
            {
                Session.SendMessage(new AvatarEffectActivatedComposer(Effect));
            }
        }
    }
}
