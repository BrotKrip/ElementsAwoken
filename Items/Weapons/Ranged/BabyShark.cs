﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ElementsAwoken.Items.Weapons.Ranged
{
    public class BabyShark : ModItem
    {

        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 16; 
            
            item.damage = 10;
            item.knockBack = 1.05f;


            item.useTime = 20;
            item.useAnimation = 20;

            item.useStyle = 5;

            item.noMelee = true;
            item.noUseGraphic = true;
            item.channel = true;
            item.ranged = true;
            item.autoReuse = true;

            item.value = Item.sellPrice(0, 0, 10, 0);
            item.rare = 1;

            item.UseSound = SoundID.Item10;

            item.shootSpeed = 15f;
            item.shoot = mod.ProjectileType("BabySharkP");
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Baby Shark");
            Tooltip.SetDefault("Animal and child abuse, all in one\nYou monster...");
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
    }
}
