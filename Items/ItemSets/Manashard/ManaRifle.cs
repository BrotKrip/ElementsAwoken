﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ElementsAwoken.Items.ItemSets.Manashard
{
    public class ManaRifle : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 66;
            item.height = 34;
            
            item.damage = 42;
            item.knockBack = 3.75f;

            item.useTime = 4;
            item.reuseDelay = 10;
            item.useAnimation = 9;
            item.useStyle = 5;

            item.noMelee = true;
            item.ranged = true;
            item.autoReuse = true;

            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 5;

            item.UseSound = SoundID.Item11;
            item.shoot = 10;
            item.shootSpeed = 12f;
            item.useAmmo = 97;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mana Rifle");
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-12, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(2));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("ManaRound"), damage, knockBack, player.whoAmI, 0.0f, 0.0f);
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Manashard", 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
