﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;

namespace ElementsAwoken.Tiles
{
    public class HavocBanner : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileLavaDeath[Type] = false;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Width = 2;

            AddMapEntry(new Color(217, 137, 85));
            disableSmartCursor = true;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16,16 };
            TileObjectData.addTile(Type);
			animationFrameHeight = 36;
        }

 
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 16, mod.ItemType("HavocBannerItem"));
        }
        public override void NearbyEffects(int i, int j, bool closer)
        {
            Player player = Main.LocalPlayer;
            if (closer && !player.dead && player.active)
            {
                player.AddBuff(mod.BuffType("HavocBannerBuff"), 60);
            }
        }
    }
}