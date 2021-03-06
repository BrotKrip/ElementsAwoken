﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ElementsAwoken.Items.Materials
{
    public class MysticLeaf : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 999;
            item.value = Item.buyPrice(0, 5, 0, 0);
            item.rare = 6;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Biomatter");
            Tooltip.SetDefault("");
        }

    }
}
