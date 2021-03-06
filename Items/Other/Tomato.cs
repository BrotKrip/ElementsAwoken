﻿using ElementsAwoken.NPCs.Bosses.Azana;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ElementsAwoken.Items.Other
{
    public class Tomato : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 20;

            item.damage = 1;

            item.noMelee = true;
            item.noUseGraphic = true;
            item.consumable = true;

            item.useAnimation = 12;
            item.useTime = 12;
            item.useStyle = 1;
            item.useTime = 12;
            item.UseSound = SoundID.Item1;

            item.value = Item.buyPrice(0, 0, 0, 5);
            item.rare = 0;

            item.shoot = mod.ProjectileType("TomatoP");
            item.shootSpeed = 4f;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tomato");
            Tooltip.SetDefault("you have a tomato now, what are you going to do with it?\nprobably nothing");
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<Azana>()))
            {
                speedX *= 2f;
                speedY *= 2f;
            }
            return true;
        }
    }
}
