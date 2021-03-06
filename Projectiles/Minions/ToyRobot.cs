﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ElementsAwoken.Projectiles.Minions
{
    public class ToyRobot : ModProjectile
    {

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 40;

            projectile.aiStyle = 67;
            aiType = ProjectileID.OneEyedPirate;

            projectile.netImportant = true;
            projectile.minion = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;

            projectile.timeLeft *= 5;
            projectile.minionSlots = 1f;
            projectile.penetrate = -1;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Toy Robot");
            Main.projFrames[projectile.type] = 15;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.penetrate == 0)
            {
                projectile.Kill();
            }
            return false;
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = (MyPlayer)player.GetModPlayer(mod, "MyPlayer");
            player.AddBuff(mod.BuffType("ToyRobotBuff"), 3600);
            if (player.dead)
            {
                modPlayer.toyRobot = false;
            }
            if (modPlayer.toyRobot)
            {
                projectile.timeLeft = 2;
            }
            projectile.localAI[0] = 0; // responsible for pooping
            // platform collision
            Vector2 platform = projectile.Bottom / 16;
            Tile platformTile = Framing.GetTileSafely((int)platform.X, (int)platform.Y);
            if (TileID.Sets.Platforms[platformTile.type] && player.Center.Y < projectile.Center.Y && platformTile.active() && projectile.ai[0] != 1) projectile.velocity.Y = 0;
        }
    }
}