using System;
using System.Linq;
using System.Collections.Generic;

using Retro.Hotel.Items;
using Retro.Communication.Packets.Outgoing.Inventory.Furni;

using Retro.Communication.Packets.Outgoing.Rooms.Furni;
using Retro.Hotel.Items.Crafting;
using Retro.Database.Interfaces;
using System.Data;

namespace Retro.Communication.Packets.Incoming.Rooms.Furni
{
	class SetCraftingRecipeEvent : IPacketEvent
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
            if (Final == null) return;
            Session.SendMessage(new CraftingRecipeComposer(Final));
            if (Final != null)
            {
                int craftingTable = Packet.PopInt();

                List<Item> items = new List<Item>();

                var count = Packet.PopInt();
                for (var i = 1; i <= count; i++)
                {
                    var id = Packet.PopInt();

                    var item = Session.GetHabbo().GetInventoryComponent().GetItem(id);
                    if (item == null || items.Contains(item))
                        return;

                    items.Add(item);
                }

                foreach (var Receta in RetroEnvironment.GetGame().GetCraftingManager().CraftingRecipes)
                {
                    bool found = false;

                    foreach (var item in Receta.Value.ItemsNeeded)
                    {
                        if (item.Value != items.Count(item2 => item2.GetBaseItem().ItemName == item.Key))
                        {
                            found = false;
                            break;
                        }
                        found = true;
                    }

                    if (found == false)
                        continue;

                    recipe = Receta.Value;
                    break;
                }

                if (recipe == null) return;
                ItemData resultItem = RetroEnvironment.GetGame().GetItemManager().GetItemByName(recipe.Result);
                if (resultItem == null) return;
                bool success = true;
                foreach (var need in recipe.ItemsNeeded)
                {
                    for (var i = 1; i <= need.Value; i++)
                    {
                        ItemData item = RetroEnvironment.GetGame().GetItemManager().GetItemByName(need.Key);
                        if (item == null)
                        {
                            success = false;
                            continue;
                        }

                        var inv = Session.GetHabbo().GetInventoryComponent().GetFirstItemByBaseId(item.Id);
                        if (inv == null)
                        {
                            success = false;
                            continue;
                        }

                        using (IQueryAdapter dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("SELECT * FROM `items` WHERE user_id = '" + Session.GetHabbo() + "'");
                            DataRow Table = dbClient.getRow();
                        }
                        if (success)
                        {
                        using (var dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor()) dbClient.RunQuery("DELETE FROM `items` WHERE `id` = '" + inv.Id + "' AND `user_id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                        Session.GetHabbo().GetInventoryComponent().RemoveItem(inv.Id);

                        Session.SendMessage(new CraftingResultComposer(recipe, true));
                        Session.SendMessage(new CraftableProductsComposer());
                        RetroEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_CrystalCracker", 1);
                        Session.SendNotification("Grandpa," + Session.GetHabbo().Username + " you crafted the item " + resultItem.Id + "!\n\n Status: " + Session.GetHabbo().craftHabbie + "!");

                        Session.GetHabbo().GetInventoryComponent().AddNewItem(0, resultItem.Id, "", 0, true, false, 0, 0);
                        Session.SendMessage(new FurniListUpdateComposer());
                        Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                        Session.GetHabbo().craftHabbie = false;
                        }
                    }
                }
            }
        }
    }
}