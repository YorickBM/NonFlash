using Retro.Communication.Packets.Outgoing.Catalog;
using Retro.Hotel.GameClients;
using Retro.Hotel.Items;

namespace Retro.Communication.Packets.Incoming.Catalog
{
    public class GetSellablePetBreedsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string Type = Packet.PopString();

            ItemData Item = RetroEnvironment.GetGame().GetItemManager().GetItemByName(Type);
            if (Item == null)
                return;

            int PetId = Item.BehaviourData;

            Session.SendMessage(new SellablePetBreedsComposer(Type, PetId, RetroEnvironment.GetGame().GetCatalog().GetPetRaceManager().GetRacesForRaceId(PetId)));
        }
    }
}