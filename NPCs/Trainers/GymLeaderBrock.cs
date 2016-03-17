using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PokeModRed.NPCs.Trainers
{
    public class GymLeaderBrock : ModNPC
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            name = "Rock Gym Leader";
            return mod.Properties.Autoload;
        }
        public override void SetDefaults()
        {
            npc.name = "Rock Gym Leader";
            npc.townNPC = true;
            npc.friendly = true;
            npc.width = 18;
            npc.height = 40;
            npc.aiStyle = 7;
            npc.damage = 10;
            npc.defense = 15;
            npc.lifeMax = 250;
            npc.soundHit = 1;
            npc.soundKilled = 1;
            npc.knockBackResist = 0.5f;
            Main.npcFrameCount[npc.type] = 25;
            NPCID.Sets.ExtraFramesCount[npc.type] = 9;
            NPCID.Sets.AttackFrameCount[npc.type] = 4;
            NPCID.Sets.DangerDetectRange[npc.type] = 700;
            NPCID.Sets.AttackType[npc.type] = 0;
            NPCID.Sets.AttackTime[npc.type] = 90;
            NPCID.Sets.AttackAverageChance[npc.type] = 30;
            animationType = NPCID.Guide;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            int num = npc.life > 0 ? 1 : 5;
            for (int k = 0; k < num; k++)
            {
                // Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("Sparkle"));
                // Dont have a dust for him yet, leaving example code till we make one
            }
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            for (int k = 0; k < 255; k++)
            {
                Player player = Main.player[k];
                if (player.active)
                {
                    for (int j = 0; j < player.inventory.Length; j++)
                    {
                        if (player.inventory[j].type == mod.ItemType("PokeBall"))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public override bool CheckConditions(int left, int right, int top, int bottom)
        {
            int score = 0;
            for (int x = left; x <= right; x++)
            {
                for (int y = top; y <= bottom; y++)
                {
                    int type = Main.tile[x, y].type;
                    if (type == TileID.Stone || type == TileID.Chairs || type == TileID.WorkBenches || type == TileID.ClosedDoor || type == TileID.OpenDoor)
                    {
                        score++;
                    }
                    if (Main.tile[x, y].wall == WallID.Stone)
                    {
                        score+= 500;
                    }
                }
            }
            return score >= (right - left) * (bottom - top) / 2;
        }

        public override string TownNPCName()
        {
            return "Brock";
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frame.Width = 40;
            if (((int)Main.time / 10) % 2 == 0)
            {
                npc.frame.X = 40;
            }
            else
            {
                npc.frame.X = 0;
            }
        }

        public override string GetChat()
        {
            switch (Main.rand.Next(3))
            {
                case 0:
                    return "Are you ready to challenge me trainer?";
                case 1:
                    return "I will defeat you withth tough pokemon of the Rock Type!!!";
                default:
                    return "Come and battle me when you are ready!";
            }
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Battle!";
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                Main.NewText("You would battle, but thats not implemented yet!");
                npc.Transform(mod.NPCType("GymLeaderBrockBattle"));
            }
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 20;
            knockback = 4f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 30;
            randExtraCooldown = 30;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = mod.ProjectileType("PokeBall");
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 2f;
        }
    }
}
