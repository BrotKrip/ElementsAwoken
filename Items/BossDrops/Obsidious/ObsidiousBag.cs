﻿using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ElementsAwoken.Items.BossDrops.Obsidious
{
    public class ObsidiousBag : ModItem
    {
        public override void SetDefaults()
        {
            item.maxStack = 99;
            item.consumable = true;
            item.width = 24;
            item.height = 24;
            item.rare = 7;
            bossBagNPC = mod.NPCType("Obsidious");
            item.expert = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("Right click to open");
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            int choice = Main.rand.Next(4);
            if (choice == 0)
            {
                player.QuickSpawnItem(mod.ItemType("Magmarox"));      
            }
            if (choice == 1)
            {
                player.QuickSpawnItem(mod.ItemType("TerreneScepter"));
            }
            if (choice == 2)
            {
                player.QuickSpawnItem(mod.ItemType("Ultramarine"));
            }
            if (choice == 3)
            {
                player.QuickSpawnItem(mod.ItemType("VioletEdge"));
            }
            player.QuickSpawnItem(mod.ItemType("SacredCrystal"));
            if (Main.rand.Next(10) == 0)
            {
                player.QuickSpawnItem(mod.ItemType("ObsidiousMask"));
                player.QuickSpawnItem(mod.ItemType("ObsidiousRobes"));
                player.QuickSpawnItem(mod.ItemType("ObsidiousPants"));
            }
            if (Main.rand.Next(10) == 0)
            {
                player.QuickSpawnItem(mod.ItemType("ObsidiousTrophy"));
            }
        }
    }
}