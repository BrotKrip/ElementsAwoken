﻿using ElementsAwoken.Items.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ElementsAwoken.NPCs.Bosses.Ancients
{
    [AutoloadBossHead]
    public class AncientAmalgam : ModNPC
    {
        public float originX = 0;
        public float originY = 0;

        public bool canDie = false;
        public float deathTimer = 0;
        bool spawnedHands = false;

        public float[] shootTimer = new float[4];
        public float numRadialProj = 30;
        public int projectileBaseDamage = 200;
        public int invertAttack = 1;
        public Vector2 playerOrigin = new Vector2();

        public bool[] hasInverted = new bool[Main.maxProjectiles];

        public int previousAttackNum = 0;

        public Vector2 toPlayerDash = new Vector2();
        public override void SetDefaults()
        {
            npc.width = 140;
            npc.height = 144;

            npc.aiStyle = -1;

            npc.lifeMax = 1200000;
            npc.damage = 200;
            npc.defense = 90;
            npc.knockBackResist = 0f;

            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.netAlways = true;

            npc.HitSound = SoundID.NPCHit5;

            npc.scale *= 1.3f;
            npc.alpha = 255; // starts transparent
            npc.value = Item.buyPrice(1, 0, 0, 0);
            npc.npcSlots = 1f;

            music = MusicID.LunarBoss;
            //music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/InfernaceTheme");

            // all EA modded buffs (unless i forget to add new ones)
            npc.buffImmune[mod.BuffType("IceBound")] = true;
            npc.buffImmune[mod.BuffType("ExtinctionCurse")] = true;
            npc.buffImmune[mod.BuffType("HandsOfDespair")] = true;
            npc.buffImmune[mod.BuffType("EndlessTears")] = true;
            npc.buffImmune[mod.BuffType("AncientDecay")] = true;
            npc.buffImmune[mod.BuffType("SoulInferno")] = true;
            npc.buffImmune[mod.BuffType("DragonFire")] = true;
            npc.buffImmune[mod.BuffType("Discord")] = true;
            // all vanilla buffs
            for (int num2 = 0; num2 < 206; num2++)
            {
                npc.buffImmune[num2] = true;
            }
            bossBag = mod.ItemType("AncientsBag");
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Ancient Amalgamate");
            Main.npcFrameCount[npc.type] = 5;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = 1500000;
            npc.damage = 250;
            npc.defense = 120;
            if (MyWorld.awakenedMode)
            {
                npc.lifeMax = 1750000;
                npc.damage = 300;
                npc.defense = 130;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            npc.spriteDirection = npc.direction;
            npc.frameCounter++;

            if (npc.frameCounter > 6)
            {
                npc.frame.Y = npc.frame.Y + frameHeight;
                npc.frameCounter = 0.0;
            }

            if (npc.frame.Y >= frameHeight * Main.npcFrameCount[npc.type])
            {
                npc.frame.Y = 0;
            }
        }

        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            player.AddBuff(BuffID.OnFire, 180, false);
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            if (npc.ai[0] < 180 || npc.alpha > 100) return false;
            return base.CanHitPlayer(target, ref cooldownSlot);
        }
        public override void NPCLoot()
        {
            if (Main.rand.Next(10) == 0)
            {
                //Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AncientsTrophy"));
            }
            if (Main.rand.Next(10) == 0)
            {
                
                //Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AncientsMask"));
           Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<ElderSignet>());
            }

            if (Main.expertMode)
            {
                npc.DropBossBags();

            }
            else
            {
                int choice = Main.rand.Next(3);
                if (choice == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Chromacast"));
                }
                if (choice == 1)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Shimmerspark"));
                }
                if (choice == 2)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TheFundamentals"));
                }
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CrystalAmalgamate"), 1);
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AncientShard"), Main.rand.Next(5, 8));
            }

        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = mod.ItemType("EpicHealingPotion");
        }
        public override bool CheckActive()
        {
            return false;
        }

        public override void AI()
        {
            npc.TargetClosest(true);
            Player P = Main.player[npc.target];
            Lighting.AddLight(npc.Center, 1f, 1f, 1f);

            // despawn if no players
            if (!P.active || P.dead)
            {
                npc.TargetClosest(true);
                if (!P.active || P.dead)
                {
                    npc.localAI[0]++;
                    npc.velocity.Y = npc.velocity.Y + 0.11f;
                    if (npc.localAI[0] >= 300)
                    {
                        npc.active = false;
                    }
                }
                else
                    npc.localAI[0] = 0;
            }
            if (npc.ai[0] < 180)
            {
                if (npc.ai[0] == 0)
                {
                    originX = P.Center.X;
                    originY = P.Center.Y;

                }
                if (npc.ai[0] < 60)
                {
                    MoonlordDeathDrama.RequestLight(1f, npc.Center);
                    npc.alpha = 255;
                    if (npc.ai[0] == 59)
                    {
                        Main.PlaySound(SoundLoader.customSoundType, (int)npc.position.X, (int)npc.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/NPC/AncientMergeFall"));

                        for (int i = 0; i < Main.maxProjectiles; i++)
                        {
                            if (Main.projectile[i].type == mod.ProjectileType("IzarisShard") ||
                                Main.projectile[i].type == mod.ProjectileType("KirveinShard") ||
                                Main.projectile[i].type == mod.ProjectileType("KrecheusShard") ||
                                Main.projectile[i].type == mod.ProjectileType("XernonShard") ||
                                Main.projectile[i].type == mod.ProjectileType("ShardBase"))
                            {
                                Main.projectile[i].Kill();
                            }
                        }
                    }
                }
                else
                {
                    if (!spawnedHands && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        npc.TargetClosest(true);
                        spawnedHands = true;

                        int num = NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2)), (int)npc.position.Y + npc.height / 2, mod.NPCType("AncientAmalgamFist"), npc.whoAmI, 0f, 0f, 0f, 0f, 255);
                        Main.npc[num].ai[0] = -1f;
                        Main.npc[num].ai[1] = (float)npc.whoAmI;
                        Main.npc[num].target = npc.target;
                        Main.npc[num].netUpdate = true;
                        num = NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2)), (int)npc.position.Y + npc.height / 2, mod.NPCType("AncientAmalgamFist"), npc.whoAmI, 0f, 0f, 0f, 0f, 255);
                        Main.npc[num].ai[0] = 1f;
                        Main.npc[num].ai[1] = (float)npc.whoAmI;
                        Main.npc[num].ai[3] = 150f; // ai timer offset so they arent exactly the same
                        Main.npc[num].target = npc.target;
                        Main.npc[num].netUpdate = true;
                    }

                    npc.alpha = 0;
                    Vector2 target = new Vector2(originX, originY - 250);
                    Vector2 toTarget = new Vector2(target.X - npc.Center.X, target.Y - npc.Center.Y);
                    toTarget.Normalize();
                    if (Vector2.Distance(target, npc.Center) > 5)
                    {
                        npc.velocity = toTarget * 6;
                    }
                    else
                    {
                        npc.velocity *= 0f;
                    }
                }
                npc.ai[0]++;
            }
            else
            {
                if (npc.ai[1] == 0)
                {
                    int attackNum = -1;
                    while (attackNum == previousAttackNum || attackNum == -1)
                    {
                        if (npc.life > npc.lifeMax * 0.85f)
                        {
                            attackNum = Main.rand.Next(0, 7); // 0 - 6
                        }
                        else if (npc.life <= npc.lifeMax * 0.85f && npc.life > npc.lifeMax * 0.5f)
                        {
                            attackNum = Main.rand.Next(1, 7); // 1 - 6
                        }
                        else if (npc.life <= npc.lifeMax * 0.5f && npc.life > npc.lifeMax * 0.25f)
                        {
                            attackNum = Main.rand.Next(2, 10);// 2 - 9
                        }
                        else if (npc.life <= npc.lifeMax * 0.25f)
                        {
                            attackNum = Main.rand.Next(3, 12); // 3 - 11
                            while (attackNum == 4 || attackNum == 9)
                            {
                                attackNum = Main.rand.Next(3, 12); // 3 - 11
                            }
                            if (attackNum == 7)
                            {
                                attackNum = 11;
                            }
                        }
                        else
                        {
                            attackNum = 0;
                            ElementsAwoken.DebugModeText("Error selecting attack"); // this shouldnt occur anymore
                            break;
                        }
                    }
                    if (npc.localAI[1] == 0)
                    {
                        npc.localAI[1]++;
                        attackNum = 0;
                    }
                    npc.ai[2] = attackNum;
                    previousAttackNum = attackNum;

                    invertAttack = Main.rand.Next(2) == 0 ? -1 : 1;
                    playerOrigin = P.Center;
                }
                if (npc.alpha > 0 && npc.ai[2] != 3 && npc.ai[2] != 5 && npc.ai[2] != 8)
                {
                    npc.alpha -= 5;
                }

                npc.ai[1]++;
                // move and swing hands
                if (npc.ai[2] == 0)
                {
                    float speed = 0.25f;
                    float playerX = P.Center.X;
                    float playerY = P.Center.Y - 250f;
                    Move(P, speed, new Vector2(playerX, playerY));

                    if (npc.ai[1] > 450)
                    {
                        npc.ai[1] = 0;
                        npc.ai[3] = 0;
                        ResetShootTimers();
                    }
                }
                // create a circle of kunais
                else if (npc.ai[2] == 1)
                {
                    npc.velocity = Vector2.Zero;

                    shootTimer[0]--;
                    shootTimer[1]--;
                    if (shootTimer[1] <= 0)
                    {
                        shootTimer[1] = 80;
                        numRadialProj = Main.rand.Next(20, 30);
                    }
                    if (shootTimer[0] <= 0 && shootTimer[1] <= 42)
                    {
                        int projDamage = Main.expertMode ? (int)(projectileBaseDamage * 0.75f) : projectileBaseDamage;
                        float rotation = MathHelper.ToRadians(360);
                        for (int i = 0; i < numRadialProj; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(2, 2).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numRadialProj - 1))) * 2.5f;
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("CrystallineKunaiHostileNG"), projDamage, 2f, Main.myPlayer);
                        }
                        shootTimer[0] = 14;
                    }
                    if (npc.ai[1] > 400)
                    {
                        npc.ai[1] = 0;
                        npc.ai[3] = 0;
                        ResetShootTimers();
                    }
                }
                // spin and block
                else if (npc.ai[2] == 2)
                {
                    npc.velocity = Vector2.Zero;

                    // hands spin around
                    npc.ai[3]++;
                    if (npc.ai[3] >= 60)
                    {
                        for (int i = 0; i < Main.maxProjectiles; i++)
                        {
                            Projectile oProj = Main.projectile[i];
                            if (Vector2.Distance(oProj.Center, npc.Center) <= 300 && !hasInverted[i] && oProj.active && !oProj.minion && !ProjectileID.Sets.MinionSacrificable[oProj.type])
                            {
                                oProj.velocity.X *= -1;
                                oProj.velocity.Y *= -1;
                                oProj.friendly = false;
                                oProj.hostile = true;
                                hasInverted[i] = true;
                            }
                            if (!oProj.active)
                            {
                                hasInverted[oProj.whoAmI] = false;
                            }
                        }
                    }
                    if (npc.ai[1] > 300)
                    {
                        npc.ai[1] = 0;
                        npc.ai[3] = 0;
                        ResetShootTimers();
                    }
                }
                // projections from the side
                else if (npc.ai[2] == 3)
                {
                    if (npc.alpha == 0)
                    {
                        Main.PlaySound(29, (int)npc.Center.X, (int)npc.Center.Y, 105, 1f, 0f);
                    }
                    if (npc.alpha < 255)
                    {
                        npc.alpha += 5;
                    }
                    else
                    {
                        npc.Center = P.Center - new Vector2(0, 1000);
                    }
                    shootTimer[0]--;
                    if (shootTimer[0] <= 0)
                    {
                        float speed = 6f;
                        for (int i = 1; i < 5; i++) // start at 1 so they dont overlap
                        {
                            Projectile.NewProjectile(playerOrigin.X - 1000 * invertAttack, playerOrigin.Y - i * 300, speed * invertAttack, 0f, mod.ProjectileType("AncientProjection"), projectileBaseDamage, 0f, 0, Main.rand.Next(4));
                            Projectile.NewProjectile(playerOrigin.X - 1000 * invertAttack, playerOrigin.Y + i * 300, speed * invertAttack, 0f, mod.ProjectileType("AncientProjection"), projectileBaseDamage, 0f, 0, Main.rand.Next(4));
                        }
                        Projectile.NewProjectile(playerOrigin.X - 1000 * invertAttack, playerOrigin.Y, speed * invertAttack, 0f, mod.ProjectileType("AncientProjection"), projectileBaseDamage, 0f, 0, Main.rand.Next(4));

                        shootTimer[0] = 45;
                    }
                    if (npc.ai[1] > 300)
                    {
                        npc.ai[1] = 0;
                        npc.ai[3] = 0;
                        ResetShootTimers();
                    }
                }
                // clusters
                else if (npc.ai[2] == 4)
                {
                    float speed = 0.25f;
                    float playerX = P.Center.X;
                    float playerY = P.Center.Y - 450f;
                    Move(P, speed, new Vector2(playerX, playerY));

                    if (Main.rand.Next(5) == 0)
                    {
                        int distance = 500;
                        Projectile.NewProjectile(P.Center.X + Main.rand.NextFloat(-distance, distance), P.Center.Y + Main.rand.NextFloat(-distance, distance), 0f, 0f, mod.ProjectileType("CrystalCluster"), projectileBaseDamage, 0f, Main.myPlayer);
                    }
                    if (Main.rand.Next(30) == 0)
                    {
                        Projectile.NewProjectile(P.Center.X, P.Center.Y, 0f, 0f, mod.ProjectileType("CrystalCluster"), projectileBaseDamage, 0f, Main.myPlayer);
                    }
                    if (npc.ai[1] > 450)
                    {
                        npc.ai[1] = 0;
                        npc.ai[3] = 0;
                        ResetShootTimers();
                    }
                }
                // teleport and dash
                else if (npc.ai[2] == 5)
                {
                    if (npc.ai[3] == 0)
                    {
                        int dist = 500;
                        double angle = Main.rand.NextDouble() * 2d * Math.PI;
                        Vector2 offset = new Vector2((float)Math.Sin(angle) * dist, (float)Math.Cos(angle) * dist);
                        Dust dust = Main.dust[Dust.NewDust(P.Center + offset, 0, 0, 6, 0, 0, 100)];
                        npc.Center = P.Center + offset;
                        toPlayerDash = P.Center - npc.Center;

                        for (int i = 0; i < Main.npc.Length; i++)
                        {
                            NPC fist = Main.npc[i];
                            if (fist.type == mod.NPCType("AncientAmalgamFist") && fist.ai[1] == npc.whoAmI && fist.active)
                            {
                                int fistPos = fist.ai[0] == 1 ? 90 : -120;
                                fist.position.X = npc.Center.X + fistPos - fist.width / 2;
                                fist.position.Y = npc.Center.Y + 140 - fist.height / 2;
                            }
                        }
                    }

                    npc.ai[3]++;
                    if (npc.ai[3] >= 75)
                    {
                        if (npc.alpha < 255)
                        {
                            npc.alpha += 15;
                        }
                        if (npc.alpha >= 255)
                        {
                            npc.ai[3] = 0;
                        }
                    }
                    else
                    {
                        if (npc.alpha > 0)
                        {
                            npc.alpha -= 15;
                        }
                        toPlayerDash.Normalize();
                        npc.velocity = toPlayerDash * 12f;
                    }
                    if (npc.ai[1] > 300)
                    {
                        npc.ai[1] = 0;
                        npc.ai[3] = 0;
                        ResetShootTimers();
                    }
                }
                // shoot down homing projectiles 
                else if (npc.ai[2] == 6)
                {
                    if (npc.ai[3] == 0)
                    {
                        Vector2 target = P.Center + new Vector2(400 * invertAttack, -300);
                        Vector2 toTarget = new Vector2(target.X - npc.Center.X, target.Y - npc.Center.Y);
                        toTarget.Normalize();
                        if (Vector2.Distance(target, npc.Center) > 20)
                        {
                            npc.velocity = toTarget * 16;
                        }
                        else
                        {
                            npc.ai[3]++;
                        }
                    }
                    if (npc.ai[3] != 0)
                    {
                        npc.ai[3]++;
                        shootTimer[0]--;

                        npc.velocity.X = -8f * invertAttack;
                        npc.velocity.Y = 0f;

                        if (shootTimer[0] <= 0)
                        {
                            Projectile kunai = Main.projectile[Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 8f, mod.ProjectileType("CrystallineKunaiHostileExplosive"), projectileBaseDamage, 2f, 0, 1)];
                            kunai.timeLeft = 60;
                            shootTimer[0] = 15;
                        }
                    }
                    if (npc.ai[3] >= 90)
                    {
                        npc.ai[1] = 0;
                        npc.ai[3] = 0;
                        ResetShootTimers();
                    }
                }
                // kunai galaxy
                else if (npc.ai[2] == 7)
                {
                    npc.velocity *= 0f;
                    if (npc.ai[3] == 0)
                    {
                        Vector2 target = P.Center;
                        Vector2 toTarget = new Vector2(target.X - npc.Center.X, target.Y - npc.Center.Y);
                        toTarget.Normalize();
                        if (Vector2.Distance(target, npc.Center) > 300)
                        {
                            npc.velocity = toTarget * 13;
                        }
                        else
                        {
                            npc.ai[3]++;
                        }
                    }
                    // create projections
                    else if (npc.ai[3] == 1)
                    {
                        int innerCount = 15;
                        for (int l = 0; l < innerCount; l++)
                        {
                            float distance = 360 / innerCount;
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, mod.ProjectileType("AncientProjectionSwirl"), projectileBaseDamage, 5f, 0, l * distance, npc.whoAmI);
                        }
                        int outerCount = 20;
                        for (int l = 0; l < outerCount; l++)
                        {
                            float distance = 360 / outerCount;
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, mod.ProjectileType("AncientProjectionSwirl2"), projectileBaseDamage, 5f, 0, l * distance, npc.whoAmI);
                        }
                        npc.ai[3]++;
                    }
                    if (npc.ai[3] == 2)
                    {
                        shootTimer[0]--;
                        Vector2 offset = new Vector2(400, 0);
                        float rotateSpeed = MathHelper.ToRadians(0.88f);
                        shootTimer[1] += rotateSpeed;

                        if (shootTimer[0] <= 0)
                        {
                            Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 20);
                            float numProj = 5f;
                            float projOffset = MathHelper.ToRadians(360f) / numProj;
                            for (int i = 0; i < numProj; i++)
                            {
                                Vector2 shootTarget1 = npc.Center + offset.RotatedBy(shootTimer[1] + (projOffset * i));
                                float rotation = (float)Math.Atan2(npc.Center.Y - shootTarget1.Y, npc.Center.X - shootTarget1.X);
                                for (int k = 0; k < 2; k++)
                                {
                                    float Speed = k == 0 ? 7 : 4;
                                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1), mod.ProjectileType("CrystallineKunaiHostileNG"), projectileBaseDamage, 0f, 0);
                                }
                            }
                            shootTimer[0] = 12;
                        }
                    }

                    if (npc.ai[1] > 450)
                    {
                        npc.ai[1] = 0;
                        npc.ai[3] = 0;
                        ResetShootTimers();

                        for (int i = 0; i < Main.maxProjectiles; i++)
                        {
                            Projectile other = Main.projectile[i];
                            if ((other.type == mod.ProjectileType("AncientProjectionSwirl") || other.type == mod.ProjectileType("AncientProjectionSwirl2")) && other.ai[1] == npc.whoAmI && other.active)
                            {
                                other.Kill();
                            }
                        }
                    }                  
                }
                // teleport and shoot
                else if (npc.ai[2] == 8)
                {
                    npc.velocity *= 0f;

                    npc.ai[3]++;
                    int intervalTime = 45;
                    int alphaChangeRate = 5;
                    int xDist = 300;
                    if (npc.life <= npc.lifeMax / 4)
                    {
                        intervalTime = 30;
                        alphaChangeRate = 15;
                        xDist = 350;
                    }
                    if (npc.life <= npc.lifeMax / 10)
                    {
                        intervalTime = 15;
                        alphaChangeRate = 20;
                        xDist = 450;
                    }
                    if (npc.ai[3] >= intervalTime)
                    {
                        if (npc.alpha < 255)
                        {
                            npc.alpha += alphaChangeRate;
                        }
                        else
                        {
                            npc.Center = new Vector2(P.Center.X - (Main.rand.Next(2) == 0 ? -xDist : xDist), P.Center.Y + Main.rand.Next(-400, 400));
                            for (int i = 0; i < Main.npc.Length; i++)
                            {
                                NPC fist = Main.npc[i];
                                if (fist.type == mod.NPCType("AncientAmalgamFist") && fist.ai[1] == npc.whoAmI && fist.active)
                                {
                                    int fistPos = fist.ai[0] == 1 ? 90 : -120;
                                    fist.position.X = npc.Center.X + fistPos - fist.width / 2;
                                    fist.position.Y = npc.Center.Y + 140 - fist.height / 2;
                                }
                            }
                            npc.ai[3] = 0;
                        }
                    }
                    else
                    {
                        if (npc.alpha > 0)
                        {
                            npc.alpha -= alphaChangeRate;
                        }
                    }
                    if (npc.ai[3] == (int)(intervalTime / 2))
                    {
                        float projSpeed = 12f;
                        float rotation = (float)Math.Atan2(npc.Center.Y - P.Center.Y, npc.Center.X - P.Center.X);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)((Math.Cos(rotation) * projSpeed) * -1), (float)((Math.Sin(rotation) * projSpeed) * -1), mod.ProjectileType("CrystallineKunaiHostileNG"), projectileBaseDamage, 5f, Main.myPlayer);
                    }
                    if (npc.ai[1] > 600)
                    {
                        npc.ai[1] = 0;
                        npc.ai[3] = 0;
                        ResetShootTimers();
                    }
                }
                // exploding projectiles
                else if (npc.ai[2] == 9)
                {
                    shootTimer[0]--;
                    if (npc.life <= npc.lifeMax * 0.35f)
                    {
                        if (Main.rand.Next(50) == 0)
                        {
                            Projectile kunai = Main.projectile[Projectile.NewProjectile(P.Center.X + Main.rand.Next(-1000, 1000), P.Center.Y - 1500, Main.rand.NextFloat(-2, 2), 9f, mod.ProjectileType("CrystallineKunaiHostileExplosive"), projectileBaseDamage, 5f, Main.myPlayer)];
                            kunai.timeLeft = 180;
                        }
                    }
                    if (shootTimer[0] <= 0)
                    {
                        float projSpeed = 12f;
                        float rotation = (float)Math.Atan2(npc.Center.Y - P.Center.Y, npc.Center.X - P.Center.X);
                        Projectile kunai = Main.projectile[Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)((Math.Cos(rotation) * projSpeed) * -1), (float)((Math.Sin(rotation) * projSpeed) * -1), mod.ProjectileType("CrystallineKunaiHostileExplosive"), projectileBaseDamage, 5f, Main.myPlayer)];
                        kunai.timeLeft = 60;
                        shootTimer[0] = 60;
                    }

                    float speed = 0.15f;
                    float playerX = P.Center.X;
                    float playerY = P.Center.Y;
                    Move(P, speed, new Vector2(playerX, playerY));

                    if (npc.ai[1] > 600)
                    {
                        npc.ai[1] = 0;
                        npc.ai[3] = 0;
                        ResetShootTimers();
                    }
                }
                // clusters and exploding
                else if (npc.ai[2] == 10)
                {
                    float speed = 0.15f;
                    float playerX = P.Center.X;
                    float playerY = P.Center.Y;
                    Move(P, speed, new Vector2(playerX, playerY));

                    shootTimer[0]--;


                    if (Main.rand.Next(75) == 0)
                    {
                        Projectile kunai = Main.projectile[Projectile.NewProjectile(P.Center.X + Main.rand.Next(-1000, 1000), P.Center.Y - 1500, Main.rand.NextFloat(-2, 2), 9f, mod.ProjectileType("CrystallineKunaiHostileExplosive"), projectileBaseDamage, 5f, Main.myPlayer)];
                        kunai.timeLeft = 180;
                    }

                    if (shootTimer[0] <= 0)
                    {
                        float projSpeed = 12f;
                        float rotation = (float)Math.Atan2(npc.Center.Y - P.Center.Y, npc.Center.X - P.Center.X);
                        Projectile kunai = Main.projectile[Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)((Math.Cos(rotation) * projSpeed) * -1), (float)((Math.Sin(rotation) * projSpeed) * -1), mod.ProjectileType("CrystallineKunaiHostileExplosive"), projectileBaseDamage, 5f, Main.myPlayer)];
                        kunai.timeLeft = 75;
                        shootTimer[0] = 60;
                    }

                    if (Main.rand.Next(10) == 0)
                    {
                        int distance = 500;
                        Projectile.NewProjectile(P.Center.X + Main.rand.NextFloat(-distance, distance), P.Center.Y + Main.rand.NextFloat(-distance, distance), 0f, 0f, mod.ProjectileType("CrystalCluster"), projectileBaseDamage, 0f, Main.myPlayer);
                    }
                    if (Main.rand.Next(32) == 0)
                    {
                        Projectile.NewProjectile(P.Center.X, P.Center.Y, 0f, 0f, mod.ProjectileType("CrystalCluster"), projectileBaseDamage, 0f, Main.myPlayer);
                    }
                    if (npc.ai[1] > 450)
                    {
                        npc.ai[1] = 0;
                        npc.ai[3] = 0;
                        ResetShootTimers();
                    }
                }
                // kunai flower
                else if (npc.ai[2] == 11)
                {
                    npc.velocity *= 0f;
                    if (npc.ai[3] == 0)
                    {
                        Vector2 target = P.Center;
                        Vector2 toTarget = new Vector2(target.X - npc.Center.X, target.Y - npc.Center.Y);
                        toTarget.Normalize();
                        if (Vector2.Distance(target, npc.Center) > 300)
                        {
                            npc.velocity = toTarget * 13;
                        }
                        else
                        {
                            npc.ai[3]++;
                        }
                    }
                    else if (npc.ai[3] == 1)
                    {
                        int innerCount = 15;
                        for (int l = 0; l < innerCount; l++)
                        {
                            float distance = 360 / innerCount;
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, mod.ProjectileType("AncientProjectionSwirl"), projectileBaseDamage, 5f, 0, l * distance, npc.whoAmI);
                        }
                        int outerCount = 20;
                        for (int l = 0; l < outerCount; l++)
                        {
                            float distance = 360 / outerCount;
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, mod.ProjectileType("AncientProjectionSwirl2"), projectileBaseDamage, 5f, 0, l * distance, npc.whoAmI);
                        }
                        npc.ai[3]++;
                    }
                    if (npc.ai[3] == 2)
                    {
                        shootTimer[0]--;
                        Vector2 offset = new Vector2(400, 0);
                        float rotateSpeed = 0.015f;
                        shootTimer[1] += rotateSpeed;
                        shootTimer[2] -= rotateSpeed;

                        float Speed = 7;

                        if (shootTimer[0] <= 0)
                        {
                            Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 20);
                            float numProj = 5f;
                            float projOffset = MathHelper.ToRadians(360f) / numProj;
                            for (int i = 0; i < numProj; i++)
                            {
                                Vector2 shootTarget1 = npc.Center + offset.RotatedBy(shootTimer[1] + (projOffset * (float)i) * (Math.PI * 2 / 8));
                                float rotation = (float)Math.Atan2(npc.Center.Y - shootTarget1.Y, npc.Center.X - shootTarget1.X);
                                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1), mod.ProjectileType("CrystallineKunaiHostileNG"), projectileBaseDamage, 0f, 0);
                            }
                            for (int i = 0; i < numProj; i++)
                            {
                                Vector2 shootTarget1 = npc.Center + offset.RotatedBy(shootTimer[2] + (projOffset * (float)i) * (Math.PI * 2 / 8));
                                float rotation = (float)Math.Atan2(npc.Center.Y - shootTarget1.Y, npc.Center.X - shootTarget1.X);
                                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1), mod.ProjectileType("CrystallineKunaiHostileNG"), projectileBaseDamage, 0f, 0);
                            }
                            shootTimer[0] = 12;
                        }
                    }

                    if (npc.ai[1] > 450)
                    {
                        npc.ai[1] = 0;
                        npc.ai[3] = 0;
                        ResetShootTimers();

                        for (int i = 0; i < Main.maxProjectiles; i++)
                        {
                            Projectile other = Main.projectile[i];
                            if ((other.type == mod.ProjectileType("AncientProjectionSwirl") || other.type == mod.ProjectileType("AncientProjectionSwirl2")) && other.ai[1] == npc.whoAmI && other.active)
                            {
                                other.Kill();
                            }
                        }
                    }
                }
            }
        }
        private void ResetShootTimers()
        {
            shootTimer[0] = 0;
            shootTimer[1] = 0;
            shootTimer[2] = 0;
            shootTimer[3] = 0;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Main.PlaySound(SoundLoader.customSoundType, (int)npc.position.X, (int)npc.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/NPC/AncientDeath"));

                NPC deathAni = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("AncientAmalgamDeath"))];
                deathAni.Center = npc.Center;
                /*for (int i = 0; i < Main.npc.Length; i++)
                {
                    NPC fist = Main.npc[i];
                    if (fist.type == mod.NPCType("AncientAmalgamFist") && fist.ai[1] == npc.whoAmI && fist.active)
                    {
                        fist.ai[1] = deathAni.whoAmI; // just makes the parent a random npc :shrug:
                    }
                }*/
            }
        }

        private void MoveDirect(Player P, float moveSpeed)
        {
            Vector2 toTarget = new Vector2(P.Center.X - npc.Center.X, P.Center.Y - npc.Center.Y);
            toTarget = new Vector2(P.Center.X - npc.Center.X, P.Center.Y - npc.Center.Y);
            toTarget.Normalize();
            npc.velocity = toTarget * moveSpeed;
        }

        private void Move(Player P, float speed, Vector2 target)
        {
            Vector2 desiredVelocity = target - npc.Center;
            if (Main.expertMode) speed *= 1.1f;
            if (MyWorld.awakenedMode) speed *= 1.1f;
            if (Vector2.Distance(P.Center, npc.Center) >= 2500) speed = 2;

            if (npc.velocity.X < desiredVelocity.X)
            {
                npc.velocity.X = npc.velocity.X + speed;
                if (npc.velocity.X < 0f && desiredVelocity.X > 0f)
                {
                    npc.velocity.X = npc.velocity.X + speed;
                }
            }
            else if (npc.velocity.X > desiredVelocity.X)
            {
                npc.velocity.X = npc.velocity.X - speed;
                if (npc.velocity.X > 0f && desiredVelocity.X < 0f)
                {
                    npc.velocity.X = npc.velocity.X - speed;
                }
            }
            if (npc.velocity.Y < desiredVelocity.Y)
            {
                npc.velocity.Y = npc.velocity.Y + speed;
                if (npc.velocity.Y < 0f && desiredVelocity.Y > 0f)
                {
                    npc.velocity.Y = npc.velocity.Y + speed;
                    return;
                }
            }
            else if (npc.velocity.Y > desiredVelocity.Y)
            {
                npc.velocity.Y = npc.velocity.Y - speed;
                if (npc.velocity.Y > 0f && desiredVelocity.Y < 0f)
                {
                    npc.velocity.Y = npc.velocity.Y - speed;
                    return;
                }
            }
            float slowSpeed = Main.expertMode ? 0.93f : 0.95f;
            if (MyWorld.awakenedMode) slowSpeed = 0.92f;
            int xSign = Math.Sign(desiredVelocity.X);
            if ((npc.velocity.X < xSign && xSign == 1) || (npc.velocity.X > xSign && xSign == -1)) npc.velocity.X *= slowSpeed;

            int ySign = Math.Sign(desiredVelocity.Y);
            if (MathHelper.Distance(target.Y, npc.Center.Y) > 1000)
            {
                if ((npc.velocity.X < ySign && ySign == 1) || (npc.velocity.X > ySign && ySign == -1)) npc.velocity.Y *= slowSpeed;
            }
        }
    }
}
