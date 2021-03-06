using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PokeModRed.NPCs
{
	public class Vulpix : PokemonNPC
	{	
		//constants unique to derived class
		public override float id {get{return 37.0f;}}
		public override int shoot {get{return mod.ProjectileType("Ember");}}
		
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.width = 42;
			npc.height = 42;
			Main.npcFrameCount[npc.type] = 3;
		}
		
		public override float CanSpawn(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.spawnTileY < Main.rockLayer && Main.dayTime ? 1f * base.CanSpawn(spawnInfo): 0f;
		}
	}
}
