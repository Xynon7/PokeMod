using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using PokeModRed.Items.Weapons;

namespace PokeModRed
{
	public class PokePlayer : ModPlayer
	{	
		//public Dictionary<string, bool> summonedPokemon;

		/*
		public override bool Autoload(ref string name)
		{
			summonedPokemon = new Dictionary<string, bool>();
			foreach (PokedexEntry item in Pokedex.pokedex.Values)
			{
				summonedPokemon.Add(item.Pokemon, false);
			}
			return true;
		}
		
		public override void UpdateDead()
		{
			// prepare the temp list
			List<KeyValuePair<string, bool>> list = new List<KeyValuePair<string, bool>>(summonedPokemon);
			// iterate through the list and then change the dictionary object
			foreach (KeyValuePair<string, bool> kvp in list)
			{
				summonedPokemon[kvp.Key] = false;
			}
		}
		*/
		
		public override void PreUpdate()
		{
			if (player.talkNPC > -1)
			{
				if (Main.npc[player.talkNPC].type == 107 && player.selectedItem == 58 && player.inventory[player.selectedItem].modItem != null)
				{
					PokemonWeapon pokeWeapon;
					pokeWeapon = player.inventory[player.selectedItem].modItem as PokemonWeapon;
					if (pokeWeapon != null)
					{
						player.talkNPC = -1;
					}
				}
			}
		}

		public override void SetupStartInventory(IList<Item> items)
		{
			Item item = new Item();
			item.SetDefaults(mod.ItemType("Pokecase"));
			item.stack = 1;
			items.Add(item);
		}
	}
}
