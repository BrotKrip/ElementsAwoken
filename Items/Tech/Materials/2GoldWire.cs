﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ElementsAwoken.Items.Tech.Materials
{
    public class GoldWire : ModItem
    {
        // T2

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 999;
            item.value = Item.buyPrice(0, 1, 0, 0);
            item.rare = 1;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gold Wire");
            Tooltip.SetDefault("Used for tech crafting");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("ElementsAwoken:GoldBar", 4);
            recipe.AddIngredient(null, "CopperWire", 5);
            recipe.AddIngredient(null, "LensFragment", 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 5);
            recipe.AddRecipe();
        }
    }
}
