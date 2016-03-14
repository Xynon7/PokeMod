using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PokeModRed.NPCs
{
	public class Diglett : PokemonNPC
	{	
		public override float id {get{return 50.0f;}}
		
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.width = 44;
			npc.height = 34;
			Main.npcFrameCount[npc.type] = 6;
		}

		public override float CanSpawn(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.spawnTileY < Main.rockLayer && !Main.dayTime ? 1f * base.CanSpawn(spawnInfo): 0f;
		}
	}
}
