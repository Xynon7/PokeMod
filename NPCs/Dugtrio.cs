using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PokeModRed.NPCs
{
	public class Dugtrio : PokemonNPC
	{	
		public override float id {get{return 51.0f;}}
		
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.width = 64;
			npc.height = 54;
			Main.npcFrameCount[npc.type] = 6;
		}

		public override float CanSpawn(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.spawnTileY < Main.rockLayer && !Main.dayTime ? 1f * base.CanSpawn(spawnInfo): 0f;
		}
	}
}
