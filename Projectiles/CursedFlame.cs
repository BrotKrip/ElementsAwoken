﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ElementsAwoken.Projectiles
{
    public class CursedFlame : ModProjectile
    {
    	
        public override void SetDefaults()
        {
            projectile.width = 6;
            projectile.height = 12;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 120;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cursed Flames");
        }
        public override void AI()
        {
			if (projectile.velocity.X != projectile.velocity.X)
			{
				projectile.velocity.X = projectile.velocity.X * -0.1f;
			}
			if (projectile.velocity.X != projectile.velocity.X)
			{
				projectile.velocity.X = projectile.velocity.X * -0.5f;
			}
			if (projectile.velocity.Y != projectile.velocity.Y && projectile.velocity.Y > 1f)
			{
				projectile.velocity.Y = projectile.velocity.Y * -0.5f;
			}
			projectile.ai[0] += 1f;
			if (projectile.ai[0] > 5f)
			{
				projectile.ai[0] = 5f;
				if (projectile.velocity.Y == 0f && projectile.velocity.X != 0f)
				{
					projectile.velocity.X = projectile.velocity.X * 0.97f;
					if ((double)projectile.velocity.X > -0.01 && (double)projectile.velocity.X < 0.01)
					{
						projectile.velocity.X = 0f;
						projectile.netUpdate = true;
					}
				}
				projectile.velocity.Y = projectile.velocity.Y + 0.2f;
			}
			projectile.rotation += projectile.velocity.X * 0.1f;
			if (projectile.ai[1] == 0f && projectile.type >= 326 && projectile.type <= 328)
			{
				projectile.ai[1] = 1f;
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 13);
			}
			int num1 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 75, 0f, 0f, 100, default(Color), 1f);
			Dust expr_8976_cp_0 = Main.dust[num1];
			expr_8976_cp_0.position.X = expr_8976_cp_0.position.X - 2f;
			Dust expr_8994_cp_0 = Main.dust[num1];
			expr_8994_cp_0.position.Y = expr_8994_cp_0.position.Y + 2f;
			Main.dust[num1].scale += (float)Main.rand.Next(50) * 0.01f;
			Main.dust[num1].noGravity = true;
			Dust expr_89E7_cp_0 = Main.dust[num1];
			expr_89E7_cp_0.velocity.Y = expr_89E7_cp_0.velocity.Y - 2f;
			if (Main.rand.Next(2) == 0)
			{
				int num2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 75, 0f, 0f, 100, default(Color), 1f);
                Dust expr_8A4E_cp_0 = Main.dust[num2];
                expr_8A4E_cp_0.position.X = expr_8A4E_cp_0.position.X - 2f;
                Dust expr_8A6C_cp_0 = Main.dust[num2];
                expr_8A6C_cp_0.position.Y = expr_8A6C_cp_0.position.Y + 2f;
                Main.dust[num2].scale += 0.3f + (float)Main.rand.Next(50) * 0.01f;
                Main.dust[num2].noGravity = true;
                Main.dust[num2].velocity *= 0.1f;
            }
			if ((double)projectile.velocity.Y < 0.25 && (double)projectile.velocity.Y > 0.15)
			{
				projectile.velocity.X = projectile.velocity.X * 0.8f;
			}
			projectile.rotation = -projectile.velocity.X * 0.05f;
			if (projectile.velocity.Y > 16f)
			{
				projectile.velocity.Y = 16f;
				return;
			}
        }
        
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.penetrate == 0)
            {
                projectile.Kill();
            }
            return false;
        }
    }
}