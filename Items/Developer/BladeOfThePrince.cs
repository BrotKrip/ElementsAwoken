﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ElementsAwoken.Items.Developer
{
    public class BladeOfThePrince : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 250;
            item.melee = true;
            item.width = 40;
            item.height = 40;
            item.useTime = 12;
            item.useTurn = true;
            item.useAnimation = 12;
            item.useStyle = 1;
            item.knockBack = 6.5f;
            item.value = Item.buyPrice(1, 50, 0, 0);
            item.rare = 11;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("PrinceStrike");
            item.shootSpeed = 18f;

            item.GetGlobalItem<EATooltip>().developer = true;
            item.GetGlobalItem<EARarity>().rare = 12;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blade of the Raven Prince");
            Tooltip.SetDefault("Shoots a purple energy blast that rains homing sparkles when it hits an enemy\nIt is tainted with the raging blood of an long slain raven demon\nAn undescribeable desire for chaos and destruction dwells within your heart, feeling as it would darken by every passing second...\nBurst's developer weapon");
        }
        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[mod.ProjectileType("RavenPrince")] < 3)
            {
                Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, mod.ProjectileType("RavenPrince"), item.damage, item.knockBack, player.whoAmI);
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Pyroplasm", 25);
            recipe.AddIngredient(null, "Armageddon", 1);
            recipe.AddRecipeGroup("ElementsAwoken:GoldBar", 10);
            recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
            recipe.AddIngredient(ItemID.LunarBar, 8);
            recipe.AddIngredient(null, "VoiditeBar", 8);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
