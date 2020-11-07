using Etap.Communication.Packets;
using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;
using System.Collections.Generic;

namespace Retro.Communication.Packets.Incoming.Inventory.AvatarEffects
{
    class AvatarEffectsComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int count = Packet.PopInt();

            for (int i = 0; i < count; i++)
            {
                ///WriteInteger(Effect.SpriteId);//Effect Id
                ///WriteInteger(0);//Type, 0 = Hand, 1 = Full
                ///WriteInteger((int)Effect.Duration);
                ///WriteInteger(Effect.Activated ? Effect.Quantity - 1 : Effect.Quantity);
                ///WriteInteger(Effect.Activated ? (int)Effect.TimeLeft : -1);
                ///WriteBoolean(false);//Permanent
            }
        }
    }
}
