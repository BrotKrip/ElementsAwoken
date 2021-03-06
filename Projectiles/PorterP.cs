﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ElementsAwoken.Projectiles
{
    public class PorterP : ModProjectile
    {

        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;

            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.ranged = true;

            projectile.penetrate = 1;
            projectile.timeLeft = 200;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Porter");
        }
        public override void AI()
        {
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

            if (Main.rand.Next(3) == 0)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.PinkFlame);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = 1f;
                Main.dust[dust].velocity *= 0.1f;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            bool immune = false;
            foreach (int k in ElementsAwoken.instakillImmune)
            {
                if (target.type == k)
                {
                    immune = true;
                }
            }
            if (!immune && target.active && target.damage > 0 && !target.dontTakeDamage && !target.boss && target.lifeMax < 1000)
            {
                Vector2 newPos = new Vector2(target.Center.X, target.Center.Y);
                bool isntColliding = false;
                int num = 0;
                while (!isntColliding && num < 300)
                {
                    num++;
                    newPos = new Vector2(target.Center.X + Main.rand.Next(-300, 300), target.Center.Y + Main.rand.Next(-300, 300));
                    Point newPoint = newPos.ToTileCoordinates();
                    bool colliding = Main.tile[newPoint.X, newPoint.Y].nactive() && Main.tileSolid[(int)Main.tile[newPoint.X, newPoint.Y].type] && !Main.tileSolidTop[(int)Main.tile[newPoint.X, newPoint.Y].type] && Main.tile[newPoint.X, newPoint.Y].type != TileID.Rope;
                    if (!colliding)
                    {
                        isntColliding = true;
                    }
                }
                ProjectileUtils.OutwardsCircleDust(projectile, DustID.PinkFlame, 36, 3f, targetX: target.Center.X, targetY: target.Center.Y);
                target.Center = newPos;
                ProjectileUtils.OutwardsCircleDust(projectile, DustID.PinkFlame, 36, 3f, targetX: target.Center.X, targetY: target.Center.Y);
            }
        }
    }
}