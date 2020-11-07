using Retro.Communication.Packets.Outgoing.Rooms.Furni;
using Retro.Hotel.Items.Crafting;

namespace Retro.Communication.Packets.Incoming.Rooms.Furni
{
    class GetCraftingItemEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            var result = Packet.PopString();

            CraftingRecipe recipe = null;
            foreach (CraftingRecipe Receta in RetroEnvironment.GetGame().GetCraftingManager().CraftingRecipes.Values)
            {
                if (Receta.Result.Contains(result))
                {
                    recipe = Receta;
                    break;
                }
            }

            var Final = RetroEnvironment.GetGame().GetCraftingManager().GetRecipe(recipe.Id);

            Session.SendMessage(new CraftingResultComposer(recipe, true));
            Session.SendMessage(new CraftableProductsComposer());
        }
    }
}