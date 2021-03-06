using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ElementsAwoken.Projectiles
{
    public class VoidPortalSinewave : ModProjectile
    {
        public bool invert = false;
        public override string Texture { get { return "ElementsAwoken/Projectiles/Blank"; } }
        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;

            projectile.alpha = 255;
            projectile.timeLeft = 600;

            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.ranged = true;

            projectile.penetrate = 1;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Wave");
        }
        public override void AI()
        {
            if (projectile.localAI[0] == 0)
            {
                invert = Main.rand.Next(2) == 0 ? true : false;
                projectile.localAI[0]++;
            }

            int dustLength = 7;
            for (int i = 0; i < dustLength; i++)
            {
                Dust dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.PinkFlame)];
                dust.velocity = Vector2.Zero;
                dust.position -= projectile.velocity / dustLength * (float)i;
                dust.noGravity = true;
                dust.scale *= 0.7f;
            }
            float rotateIntensity = 6;
            if (invert)
            {
                rotateIntensity *= -1;
            }
            float waveTime = 16f;
            projectile.ai[0]++;
            if (projectile.ai[1] == 0) // this part is to fix the offset (it is still slightlyyyy offset)
            {
                if (projectile.ai[0] > waveTime * 0.5f)
                {
                    projectile.ai[0] = 0;
                    projectile.ai[1] = 1;
                }
                else
                {
                    Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(-rotateIntensity));
                    projectile.velocity = perturbedSpeed;
                }
            }
            else
            {
                if (projectile.ai[0] <= waveTime)
                {
                    Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(rotateIntensity));
                    projectile.velocity = perturbedSpeed;
                }
                else
                {
                    Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(-rotateIntensity));
                    projectile.velocity = perturbedSpeed;
                }
                if (projectile.ai[0] >= waveTime * 2)
                {
                    projectile.ai[0] = 0;
                }
            }
        }
    }
}