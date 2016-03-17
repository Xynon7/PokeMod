using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PokeModRed.NPCs.Trainers
{
    public class GymLeaderBrockBattle : ModNPC
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            name = "Rock Gym Leader Battle";
            return mod.Properties.Autoload;
        }

        public override void SetDefaults()
        {
            npc.name = "Rock Gym Leader Battle";
            npc.displayName = "Brock, the Rock Gym Leader";
            npc.boss = true;
            npc.dontTakeDamage = true;
            npc.width = 18;
            npc.height = 40;
            npc.aiStyle = -1;
            npc.damage = 10;
            npc.defense = 10;
            npc.lifeMax = 250;
            npc.npcSlots = 15f;
            npc.soundHit = 1;
            npc.soundKilled = 1;
            npc.buffImmune[24] = true;
            music = MusicID.Boss2;
            npc.knockBackResist = 0f;
            Main.npcFrameCount[npc.type] = 25;
            NPCID.Sets.ExtraFramesCount[npc.type] = 9;
            NPCID.Sets.AttackFrameCount[npc.type] = 4;
            NPCID.Sets.DangerDetectRange[npc.type] = 700;
            NPCID.Sets.AttackType[npc.type] = 0;
            NPCID.Sets.AttackTime[npc.type] = 90;
            NPCID.Sets.AttackAverageChance[npc.type] = 30;
            animationType = NPCID.Guide;
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
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.625f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.6f);
        }

        public override void AI()
        {
            if (npc.localAI[0] == 0f)
            {
                /*
                for (int k = 0; k < 5; k++)
                {
                    int poke1 = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("Bulbasaur"));
                    Main.npc[poke1].ai[0] = npc.whoAmI;
                    Main.npc[poke1].ai[1] = k;
                    Main.npc[poke1].ai[2] = 50 * (k + 1);
                    if (k == 2)
                    {
                        Main.npc[poke1].damage += 20;
                    }
                    CaptiveElement.SetPosition(Main.npc[captive]);
                    Main.npc[poke1].netUpdate = true;
                }
                npc.netUpdate = true;
                npc.localAI[0] = 1f;
                */
            }
        }
    }
}
