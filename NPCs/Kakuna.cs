using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PokeModRed.NPCs
{
	public class Kakuna : PokemonNPC
	{
		public override float id {get{return 14.0f;}}
		
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.width = 30;
			npc.height = 44;
			Main.npcFrameCount[npc.type] = 2;
		}
		
		public override float CanSpawn(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.spawnTileY < Main.rockLayer && !Main.dayTime ? 1f * base.CanSpawn(spawnInfo): 0f;
		}		
	}
}