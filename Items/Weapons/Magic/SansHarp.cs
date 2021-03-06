﻿using ElementsAwoken.Items.Materials;
using ElementsAwoken.Projectiles;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ElementsAwoken.Items.Weapons.Magic
{
    public class SansHarp : ModItem
    {
        private float[] pitches = new float[10] {  0.0f,  0.0f, 0.8f, 0.4f, 0.3f, 0.2f,  0.1f,  0.0f,  0.1f, 0.2f };
        private int[] useTimes = new int[10] { 8, 8, 16, 18, 14, 14, 12, 8, 8, 16 };
        // cant have notenum and useCD in here because when holding the item it is reverted back to a clone of its self for whatever reason- bug boi. tmod will fix later

        public override bool CloneNewInstances => true;

        public override void SetDefaults()
        {
            item.width = 50;
            item.height = 50;

            item.damage = 130;
            item.mana = 9;

            item.useTime = 6;
            item.useAnimation = 6;

            item.magic = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.holdStyle = 3;

            item.useStyle = 5;
            item.knockBack = 2;

            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = 8;

            item.shoot = ModContent.ProjectileType<SansNote>();
            item.shootSpeed = 9f;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Harp of Judgement");
            Tooltip.SetDefault("it's a beautiful day outside.\nbirds are singing, flowers are blooming...\non days like these, kids like you...\nshould be playing the harp. bing");
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

            float pitch = pitches[modPlayer.sansNote];
            Main.harpNote = pitch;
            Main.PlaySound(SoundID.Item26, player.position);
            modPlayer.sansUseCD= useTimes[modPlayer.sansNote];
            Projectile proj = Main.projectile[Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0.0f, 0.0f)];
            modPlayer.sansNote++;
            if (modPlayer.sansNote >= 10) modPlayer.sansNote = 0;
                return false;
        }
        public override void HoldItem(Player player)
        {
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

            if (player.whoAmI == Main.myPlayer) modPlayer.sansUseCD--;
        }
        public override bool CanUseItem(Player player)
        {
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

            if (modPlayer.sansUseCD > 0) return false;
            return base.CanUseItem(player);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MagicalHarp, 1);
            recipe.AddIngredient(ItemID.Bone, 32);
            recipe.AddIngredient(ModContent.ItemType<RoyalScale>(), 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();

        }
    }
}
