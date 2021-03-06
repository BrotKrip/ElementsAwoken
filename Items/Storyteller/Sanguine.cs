﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ElementsAwoken.Items.Storyteller
{
    public class Sanguine : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 50;
            item.height = 50;

            item.damage = 18;
            item.knockBack = 2;
            item.mana = 6;

            item.useTime = 14;
            item.useAnimation = 14;
            item.useStyle = 5;
            Item.staff[item.type] = true;

            item.magic = true;
            item.noMelee = true;
            item.autoReuse = true;

            item.value = Item.buyPrice(0, 5, 0, 0);
            item.rare = 2;

            item.UseSound = SoundID.Item8;
            item.shoot = mod.ProjectileType("BloodshotP");
            item.shootSpeed = 3f;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sanguine");
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(12));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;
            return true;
        }
    }
}
