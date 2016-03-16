using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PokeModRed.NPCs {

	public class Gyarados : PokemonNPC
	{
		public override float id {get{return 130f;}}
		
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.width = 20;
			npc.height = 20;
			Main.npcFrameCount[npc.type] = 3;
		}
	}
}
