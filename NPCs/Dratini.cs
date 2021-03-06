﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PokeModRed.Items.Weapons;

namespace PokeModRed.NPCs
{
    public class Dratini : PokemonNPC
    {
        //constants unique to derived class
        public override float id { get { return 147.0f; } }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.width = 54;
            npc.height = 48;
            Main.npcFrameCount[npc.type] = 3;
        }

        public override float CanSpawn(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }
    }
}
