﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ElementsAwoken.Projectiles.Minions
{
    public class IcicleMinion : ModProjectile
    {

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.netImportant = true;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.minionSlots = 1f;
            projectile.timeLeft = 18000;
            projectile.alpha = 100;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
            projectile.penetrate = -1;
            projectile.timeLeft *= 5;
            projectile.minion = true;
            aiType = 388;
            projectile.aiStyle = 66;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubble");
        }
        public override void AI()
        {
            projectile.rotation += projectile.velocity.X * 0.04f;
            bool flag64 = projectile.type == mod.ProjectileType("IcicleMinion");
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            player.AddBuff(mod.BuffType("IcicleBuff"), 3600);
            if (flag64)
            {
                if (player.dead)
                {
                    modPlayer.icicleMinion = false;
                }
                if (modPlayer.icicleMinion)
                {
                    projectile.timeLeft = 2;
                }
            }
            ProjectileUtils.PushOtherEntities(projectile);

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