﻿using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ElementsAwoken.NPCs.Town
{
    [AutoloadHead]
    public class Alchemist : ModNPC
    {
        public override bool Autoload(ref string name)
        {
            return mod.Properties.Autoload;
        }
        public override string Texture
        {
            get
            {
                return "ElementsAwoken/NPCs/Town/Alchemist";
            }
        }
        public override void SetDefaults()
        {
            npc.townNPC = true;
            npc.friendly = true;
            npc.width = 40;
            npc.height = 40;
            npc.aiStyle = 7;
            npc.damage = 10;
            npc.defense = 30;
            npc.lifeMax = 250;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0.5f;
            NPCID.Sets.AttackFrameCount[npc.type] = 4;
            NPCID.Sets.DangerDetectRange[npc.type] = 700;
            NPCID.Sets.AttackType[npc.type] = 0;
            NPCID.Sets.AttackTime[npc.type] = 90;
            NPCID.Sets.AttackAverageChance[npc.type] = 30;
            animationType = NPCID.Wizard;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Alchemist");
            Main.npcFrameCount[npc.type] = 25;
        }
        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            if (NPC.downedBoss2)
            {
                return true;
            }
            return false;
        }
        public override bool CheckConditions(int left, int right, int top, int bottom)
        {
            return true;
        }

        public override string TownNPCName()
        {
            switch (WorldGen.genRand.Next(4))
            {
                case 0:
                    return "Saul";
                case 1:
                    return "Darius";
                case 2:
                    return "Eliseo";
                case 3:
                    return "Cadmus";
                default:
                    return "Saul";
            }
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 30;
            knockback = 4f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 6;
            randExtraCooldown = 10;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = mod.ProjectileType("SuperStinkPotion");
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 10f;
            randomOffset = 2f;
        }

        const int NumberShops = 4;
        static int shopNumber = 0;
        public string text = "";
        public string shopText = "";

        public override string GetChat()
        {
            switch (Main.rand.Next(6))
            {
                case 0: text = "No matter how large the wound, healing potions shall fix it."; break;
                case 1: text = "All your potion needs, taken care of right here."; break;
                case 2: text = "I travel the world searching for ingredients for my potions, its not an easy task."; break;
                case 3: text = "I come from a dark world..."; break;
                case 4: text = "Be careful out there, I hate fighting. I'd much rather talk my way out of it."; break;
                case 5: text = "Going truffle worm hunting? I suggest an invisibility potion."; break;
                default:
                    return "default";
            }
            string addText = "";

            if (ModContent.GetInstance<Config>().alchemistPotions)
            {
                if (shopNumber == 0)
                {
                    shopText = "Health and Mana";
                }
                if (shopNumber == 1)
                {
                    shopText = "Combat";
                }
                if (shopNumber == 2)
                {
                    shopText = "Other";
                }
                if (shopNumber == 3)
                {
                    shopText = "Transmutation";
                }
                addText = "\n\nShop: " + shopText;
            }
            return text + addText;
        }
        // Multiple Shops code. multishop
        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
            if (shopNumber == 3) button = "Convert";
            if (ModContent.GetInstance<Config>().alchemistPotions) button2 = "Next Shop";
            else button2 = "Transmutation";
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                if (ModContent.GetInstance<Config>().alchemistPotions)
                {
                    if (shopNumber < 3) shop = true;
                    else
                    {
                        Main.playerInventory = true;
                        Main.npcChatText = "";
                        ModContent.GetInstance<ElementsAwoken>().AlchemistUserInterface.SetState(new UI.AlchemistUI());
                    }
                }
                else
                {
                    shop = true;
                }
            }
            else
            {
                if (ModContent.GetInstance<Config>().alchemistPotions)
                {
                    shopNumber = shopNumber + 1 % NumberShops;
                    if (shopNumber > 3)
                    {
                        shopNumber = 0;
                    }
                    if (shopNumber == 0)
                    {
                        shopText = "Health and Mana";
                    }
                    if (shopNumber == 1)
                    {
                        shopText = "Combat";
                    }
                    if (shopNumber == 2)
                    {
                        shopText = "Other";
                    }
                    if (shopNumber == 3)
                    {
                        shopText = "Transmutation";
                    }
                    Main.npcChatText = text + "\n\nShop: " + shopText;
                }
                else
                {
                    Main.playerInventory = true;
                    Main.npcChatText = "";
                    ModContent.GetInstance<ElementsAwoken>().AlchemistUserInterface.SetState(new UI.AlchemistUI());
                }
            }
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            if (ModContent.GetInstance<Config>().alchemistPotions)
            {
                if (shopNumber == 0) // healing and mana
                {
                    #region Healing Potions
                    shop.item[nextSlot].SetDefaults(ItemID.LesserHealingPotion);
                    shop.item[nextSlot].shopCustomPrice = 7500;
                    nextSlot++;
                    if (NPC.downedBoss2) // && !Main.hardMode
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.HealingPotion);
                        shop.item[nextSlot].shopCustomPrice = 10000;
                        nextSlot++;
                    }
                    if (Main.hardMode)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.GreaterHealingPotion);
                        shop.item[nextSlot].shopCustomPrice = 75000;
                        nextSlot++;
                    }
                    if (NPC.downedAncientCultist)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.SuperHealingPotion);
                        shop.item[nextSlot].shopCustomPrice = 100000;
                        nextSlot++;
                    }
                    if (NPC.downedMoonlord)
                    {
                        shop.item[nextSlot].SetDefaults(ModLoader.GetMod("ElementsAwoken").ItemType("EpicHealingPotion"));
                        shop.item[nextSlot].shopCustomPrice = 200000;
                        nextSlot++;
                    }
                    #endregion
                    #region Mana Potions
                    shop.item[nextSlot].SetDefaults(ItemID.LesserManaPotion);
                    shop.item[nextSlot].shopCustomPrice = 7500;
                    nextSlot++;
                    if (NPC.downedBoss2)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.ManaPotion);
                        shop.item[nextSlot].shopCustomPrice = 10000;
                        nextSlot++;
                    }
                    if (Main.hardMode)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.GreaterManaPotion);
                        shop.item[nextSlot].shopCustomPrice = 75000;
                        nextSlot++;
                    }
                    if (NPC.downedMechBossAny)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.SuperManaPotion);
                        shop.item[nextSlot].shopCustomPrice = 100000;
                        nextSlot++;
                    }
                    #endregion
                    #region other
                    if (NPC.downedBoss1)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.LesserRestorationPotion);
                        shop.item[nextSlot].shopCustomPrice = 10000;
                        nextSlot++;
                        shop.item[nextSlot].SetDefaults(ItemID.RestorationPotion);
                        shop.item[nextSlot].shopCustomPrice = 20000;
                        nextSlot++;
                        shop.item[nextSlot].SetDefaults(ItemID.RegenerationPotion);
                        shop.item[nextSlot].shopCustomPrice = 10000;
                        nextSlot++;
                        shop.item[nextSlot].SetDefaults(ItemID.ManaRegenerationPotion);
                        shop.item[nextSlot].shopCustomPrice = 10000;
                        nextSlot++;
                    }
                    #endregion
                }
                else if (shopNumber == 1) // combat
                {
                    #region Defense
                    if (NPC.downedBoss1)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.IronskinPotion);
                        shop.item[nextSlot].shopCustomPrice = 10000;
                        nextSlot++;
                    }
                    if (NPC.downedBoss2)
                    {
                        shop.item[nextSlot].SetDefaults(ModLoader.GetMod("ElementsAwoken").ItemType("DemonPhilter"));
                        shop.item[nextSlot].shopCustomPrice = 30000;
                        nextSlot++;
                    }
                    if (Main.hardMode)
                    {
                        shop.item[nextSlot].SetDefaults(ModLoader.GetMod("ElementsAwoken").ItemType("HellFury"));
                        shop.item[nextSlot].shopCustomPrice = 40000;
                        nextSlot++;
                    }
                    if (NPC.downedMoonlord)
                    {
                        shop.item[nextSlot].SetDefaults(ModLoader.GetMod("ElementsAwoken").ItemType("CelestialEmpowerment"));
                        shop.item[nextSlot].shopCustomPrice = 100000;
                        nextSlot++;
                    }
                    #endregion
                    #region Other
                    shop.item[nextSlot].SetDefaults(ItemID.ArcheryPotion);
                    shop.item[nextSlot].shopCustomPrice = 10000;
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(ItemID.EndurancePotion);
                    shop.item[nextSlot].shopCustomPrice = 10000;
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(ItemID.ThornsPotion);
                    shop.item[nextSlot].shopCustomPrice = 10000;
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(ItemID.TitanPotion);
                    shop.item[nextSlot].shopCustomPrice = 10000;
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(ItemID.SwiftnessPotion);
                    shop.item[nextSlot].shopCustomPrice = 10000;
                    nextSlot++;
                    if (NPC.downedBoss2)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.AmmoReservationPotion);
                        shop.item[nextSlot].shopCustomPrice = 10000;
                        nextSlot++;
                    }
                    if (NPC.downedBoss3)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.SummoningPotion);
                        shop.item[nextSlot].shopCustomPrice = 10000;
                        nextSlot++;
                        shop.item[nextSlot].SetDefaults(ItemID.GravitationPotion);
                        shop.item[nextSlot].shopCustomPrice = 10000;
                        nextSlot++;
                        shop.item[nextSlot].SetDefaults(ItemID.HeartreachPotion);
                        shop.item[nextSlot].shopCustomPrice = 10000;
                        nextSlot++;
                        shop.item[nextSlot].SetDefaults(ItemID.MagicPowerPotion);
                        shop.item[nextSlot].shopCustomPrice = 10000;
                        nextSlot++;
                        shop.item[nextSlot].SetDefaults(ItemID.WrathPotion);
                        shop.item[nextSlot].shopCustomPrice = 10000;
                        nextSlot++;
                    }
                    if (Main.hardMode)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.InfernoPotion);
                        shop.item[nextSlot].shopCustomPrice = 30000;
                        nextSlot++;
                        shop.item[nextSlot].SetDefaults(ItemID.RagePotion);
                        shop.item[nextSlot].shopCustomPrice = 30000;
                        nextSlot++;
                        shop.item[nextSlot].SetDefaults(ItemID.LifeforcePotion);
                        shop.item[nextSlot].shopCustomPrice = 30000;
                        nextSlot++;
                    }
                    #endregion
                }
                else if (shopNumber == 2) // other
                {
                    #region Other
                    shop.item[nextSlot].SetDefaults(ItemID.RecallPotion);
                    shop.item[nextSlot].shopCustomPrice = 10000;
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(ItemID.WormholePotion);
                    shop.item[nextSlot].shopCustomPrice = 10000;
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(ItemID.InvisibilityPotion);
                    shop.item[nextSlot].shopCustomPrice = 10000;
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(ItemID.BuilderPotion);
                    shop.item[nextSlot].shopCustomPrice = 10000;
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(ItemID.FeatherfallPotion);
                    shop.item[nextSlot].shopCustomPrice = 10000;
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(ItemID.HunterPotion);
                    shop.item[nextSlot].shopCustomPrice = 10000;
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(ItemID.TrapsightPotion); // dangersense
                    shop.item[nextSlot].shopCustomPrice = 10000;
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(ItemID.MiningPotion);
                    shop.item[nextSlot].shopCustomPrice = 10000;
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(ItemID.NightOwlPotion);
                    shop.item[nextSlot].shopCustomPrice = 10000;
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(ItemID.ShinePotion);
                    shop.item[nextSlot].shopCustomPrice = 10000;
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(ItemID.ThornsPotion);
                    shop.item[nextSlot].shopCustomPrice = 10000;
                    nextSlot++;
                    if (NPC.savedAngler)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.CratePotion);
                        shop.item[nextSlot].shopCustomPrice = 10000;
                        nextSlot++;
                        shop.item[nextSlot].SetDefaults(ItemID.FishingPotion);
                        shop.item[nextSlot].shopCustomPrice = 10000;
                        nextSlot++;
                        shop.item[nextSlot].SetDefaults(ItemID.FlipperPotion);
                        shop.item[nextSlot].shopCustomPrice = 10000;
                        nextSlot++;
                        shop.item[nextSlot].SetDefaults(ItemID.SonarPotion);
                        shop.item[nextSlot].shopCustomPrice = 10000;
                        nextSlot++;
                        shop.item[nextSlot].SetDefaults(ItemID.GillsPotion);
                        shop.item[nextSlot].shopCustomPrice = 10000;
                        nextSlot++;
                    }
                    if (NPC.downedBoss3)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.BattlePotion);
                        shop.item[nextSlot].shopCustomPrice = 10000;
                        nextSlot++;
                        shop.item[nextSlot].SetDefaults(ItemID.CalmingPotion);
                        shop.item[nextSlot].shopCustomPrice = 10000;
                        nextSlot++;
                        shop.item[nextSlot].SetDefaults(ItemID.GravitationPotion);
                        shop.item[nextSlot].shopCustomPrice = 10000;
                        nextSlot++;
                        shop.item[nextSlot].SetDefaults(ItemID.HeartreachPotion);
                        shop.item[nextSlot].shopCustomPrice = 10000;
                        nextSlot++;
                        shop.item[nextSlot].SetDefaults(ItemID.ObsidianSkinPotion);
                        shop.item[nextSlot].shopCustomPrice = 10000;
                        nextSlot++;
                        shop.item[nextSlot].SetDefaults(ItemID.WarmthPotion);
                        shop.item[nextSlot].shopCustomPrice = 10000;
                        nextSlot++;
                        shop.item[nextSlot].SetDefaults(ItemID.SpelunkerPotion);
                        shop.item[nextSlot].shopCustomPrice = 10000;
                        nextSlot++;
                    }
                    if (NPC.downedMechBossAny)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.TeleportationPotion);
                        shop.item[nextSlot].shopCustomPrice = 50000;
                        nextSlot++;
                    }
                    shop.item[nextSlot].SetDefaults(ItemID.GenderChangePotion);
                    shop.item[nextSlot].shopCustomPrice = 100000;
                    nextSlot++;
                    if (NPC.downedBoss3)
                    {
                        shop.item[nextSlot].SetDefaults(ModLoader.GetMod("ElementsAwoken").ItemType("Chaospotion"));
                        shop.item[nextSlot].shopCustomPrice = 50000;
                        nextSlot++;
                    }
                    if (NPC.downedMechBossAny)
                    {
                        shop.item[nextSlot].SetDefaults(ModLoader.GetMod("ElementsAwoken").ItemType("HavocPotion"));
                        shop.item[nextSlot].shopCustomPrice = 100000;
                        nextSlot++;
                    }
                    if (NPC.downedMoonlord)
                    {
                        shop.item[nextSlot].SetDefaults(ModLoader.GetMod("ElementsAwoken").ItemType("CalamityPotion"));
                        shop.item[nextSlot].shopCustomPrice = 250000;
                        nextSlot++;
                    }
                    #endregion
                }
            }
            else
            {
                shop.item[nextSlot].SetDefaults(ItemID.DaybloomSeeds);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 5, 0);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.MoonglowSeeds);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 5, 0);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.BlinkrootSeeds);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 5, 0);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.DeathweedSeeds);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 5, 0);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.WaterleafSeeds);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 5, 0);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.ShiverthornSeeds);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 5, 0);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.FireblossomSeeds);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 5, 0);
                nextSlot++;
                nextSlot++;
                nextSlot++;
                nextSlot++;

                if (NPC.downedSlimeKing)
                {
                    shop.item[nextSlot].SetDefaults(ItemID.DayBloomPlanterBox);
                    nextSlot++;
                }
                if (NPC.downedQueenBee)
                {
                    shop.item[nextSlot].SetDefaults(ItemID.MoonglowPlanterBox);
                    nextSlot++;
                }
                if (NPC.downedBoss1)
                {
                    shop.item[nextSlot].SetDefaults(ItemID.BlinkrootPlanterBox);
                    nextSlot++;
                }
                if (NPC.downedBoss2)
                {
                    if (WorldGen.crimson)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.CrimsonPlanterBox);
                    }
                    else
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.CorruptPlanterBox);
                    }
                    nextSlot++;
                }
                if (NPC.downedBoss3)
                {
                    shop.item[nextSlot].SetDefaults(ItemID.WaterleafPlanterBox);
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(ItemID.ShiverthornPlanterBox);
                    nextSlot++;
                }
                if (Main.hardMode)
                {
                    shop.item[nextSlot].SetDefaults(ItemID.FireBlossomPlanterBox);
                    nextSlot++;
                }
                if (Main.moonPhase == 1) shop.item[20].SetDefaults(ModLoader.GetMod("ElementsAwoken").ItemType("AlchemistsTimer"));
            }
        }
    }
}
