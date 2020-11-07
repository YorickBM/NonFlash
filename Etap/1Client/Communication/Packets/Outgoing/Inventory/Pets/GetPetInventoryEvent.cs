using System.Collections.Generic;

using Retro.Hotel.Rooms.AI;
using Retro.Communication.Packets.Outgoing.Inventory.Pets;

namespace Retro.Communication.Packets.Incoming.Inventory.Pets
{
    class GetPetInventoryEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session.GetHabbo().GetInventoryComponent() == null)
                return;

            ICollection<Pet> Pets = Session.GetHabbo().GetInventoryComponent().GetPets();
            Session.SendMessage(new PetInventoryComposer(Pets));
        }
    }
}