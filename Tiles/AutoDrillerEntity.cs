﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ElementsAwoken.Tiles
{
    public class AutoDrillerEntity : ModTileEntity
    {
        public int digCooldown = 60;
        public Vector2 justDugOre = new Vector2();

        public string tileOwner = "No Owner";

        public bool enabled = true;
        public override void Update()
        {
            Vector2 tileCenter = new Vector2((Position.X + 2) * 16, (Position.Y + 2) * 16);
            Vector2 tileBottomCenter = new Vector2((Position.X + 2) * 16 + 8, (Position.Y + 5) * 16 + 8);

            Rectangle mouse = new Rectangle((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, 2, 2);
            Rectangle tileRect = new Rectangle(Position.X * 16, Position.X * 16, 5 * 16, 5 * 16);
            if (Main.mouseRightRelease && mouse.Intersects(tileRect))
            {
                if (enabled)
                {
                    enabled = false;
                }
                else
                {
                    enabled = true;
                }
            }

            Tile anchorLeft = Framing.GetTileSafely(Position.X, Position.Y + 5);
            Tile anchorRight = Framing.GetTileSafely(Position.X + 4, Position.Y + 5);
            if (!(Main.tileSolid[anchorLeft.type] && anchorLeft.active()) || !Main.tileSolid[anchorRight.type])
            {
                WorldGen.KillTile(Position.X, Position.Y);
            }

            Player player = null;
            bool isPlayerActive = false;
            if (player == null)
            {
                for (int k = 0; k < Main.player.Length; k++)
                {
                    Player temp = Main.player[k];
                    if (temp.name == tileOwner && temp.active)
                    {
                        player = temp;
                        isPlayerActive = true;
                        break;
                    }
                }
            }
            if (Main.time % 100 == 0) ElementsAwoken.DebugModeText(tileOwner);
            //if (Main.time % 500 == 0) tileOwner = "Mod Testing";

            if (isPlayerActive)
            {
                PlayerEnergy modPlayer = player.GetModPlayer<PlayerEnergy>(mod);

                if (enabled && modPlayer.energy >= 3)
                {
                    Point digLeft = new Point(Position.X + 1, Position.Y + 5);
                    Point bottomRight = new Point(Position.X + 3, Main.maxTilesY - 50);

                    digCooldown--;
                    if (digCooldown >= 55)
                    {
                        for (int k = 0; k < Main.item.Length; k++)
                        {
                            Item other = Main.item[k];
                            if (other.getRect().Intersects(new Rectangle((int)justDugOre.X, (int)justDugOre.Y, 16, 16)))
                            {
                                other.Center = tileCenter - new Vector2(32, 0);
                                other.velocity = new Vector2(-5, 0);
                            }

                        }
                    }
                    if (digCooldown <= 0)
                    {
                        for (int j = digLeft.Y; j <= bottomRight.Y; j++) // for each y, check x- normally done the other way around
                        {
                            for (int i = digLeft.X; i <= bottomRight.X; i++)
                            {
                                Tile t = Framing.GetTileSafely(i, j);
                                Vector2 dugCenter = new Vector2(i * 16 + 8, j * 16 + 8);
                                if (t.active())
                                {
                                    Player randP = Main.player[Main.myPlayer];
                                    randP.PickTile(i, j, 100);
                                    justDugOre = new Vector2(i * 16, j * 16);
                                    Vector2 justDugOreCenter = new Vector2(i * 16 + 8, j * 16 + 8);

                                    Main.PlaySound(2, (int)tileCenter.X, (int)tileCenter.Y, 91);
                                    float rotation = (float)Math.Atan2(tileBottomCenter.Y - justDugOreCenter.Y, tileBottomCenter.X - justDugOreCenter.X);
                                    Projectile.NewProjectile(tileBottomCenter.X, tileBottomCenter.Y, (float)((Math.Cos(rotation) * 5) * -1), (float)((Math.Sin(rotation) * 5) * -1), mod.ProjectileType("AutoDrillBeam"), 0, 0f, 0);

                                    digCooldown = 120;

                                    modPlayer.energy -= 3;
                                    return;
                                }
                            }
                        }
                    }
                }
            }           
        }
        public override TagCompound Save()
        {
            return new TagCompound {
                {"tileOwner", tileOwner},
            };
        }
        public override void Load(TagCompound tag)
        {
            tileOwner = tag.GetString("tileOwner");
        }
        public override void NetSend(BinaryWriter writer, bool lightSend)
        {
            writer.Write(tileOwner);
        }
        public override void NetReceive(BinaryReader reader, bool lightReceive)
        {
            tileOwner = reader.ReadString();
        }
        public override void OnNetPlace()
        {
            for (int k = 0; k < Main.player.Length; k++)
            {
                Player player = Main.player[k];
                if (player.active)
                {
                    PlayerUtils modPlayer = player.GetModPlayer<PlayerUtils>(mod);
                    if(modPlayer.placingAutoDriller > 0)
                    {
                        tileOwner = player.name;
                        NetworkText f = NetworkText.FromLiteral(tileOwner);
                        NetMessage.BroadcastChatMessage(f, new Color(255, 255, 255));
                    }
                    NetworkText text = NetworkText.FromLiteral("numpty");
                    NetMessage.BroadcastChatMessage(text, new Color(255, 255, 255));
                }
            }
        }
        private bool BlacklistedTile(Tile t)
        {
            if (t.type == TileID.Torches)
            {
                return true;
            }
            return false;
        }
        private void VanishItem(Item item)
        {
            item.type = 0;
            item.stack = 0;
            item.netID = 0;
            item.active = false;
        }

        public override bool ValidTile(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            return tile.active() && tile.type == mod.TileType<AutoDriller>() && tile.frameX == 0 && tile.frameY == 0;
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            //Main.NewText("i " + i + " j " + j + " t " + type + " s " + style + " d " + direction);
            //tileOwner = Main.LocalPlayer.name; // Hook_AfterPlacement is called on the client that places it, so LocalPlayer will be the owner

            if (Main.netMode == 1)
            {
                NetMessage.SendTileSquare(Main.myPlayer, i, j, 3);
                NetMessage.SendData(87, -1, -1, null, i, j, Type, 0f, 0, 0, 0);
                return -1;
            }
            int placedEntity = Place(i, j);
            AutoDrillerEntity AutoDrillTE = (AutoDrillerEntity)ByID[placedEntity];
            AutoDrillTE.tileOwner = Main.LocalPlayer.name;
            return placedEntity;
        }
    }
}