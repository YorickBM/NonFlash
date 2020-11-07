using Retro.Hotel.Items.Crafting;

namespace Retro.Communication.Packets.Outgoing.Rooms.Furni
{
	class CraftingResultComposer : ServerPacket
    {
        public CraftingResultComposer(CraftingRecipe recipe, bool success)
            : base(ServerPacketHeader.CraftingResultMessageComposer)
        {
			WriteBoolean(success);
			WriteString(recipe.Result);
			WriteString(recipe.Result);
        }
    }
}