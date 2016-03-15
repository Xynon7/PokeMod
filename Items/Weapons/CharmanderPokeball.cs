using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PokeModRed.Items.Weapons {

	public class CharmanderPokeball : PokemonWeapon
	{
		public override float id {get{return 4f;}}
		
		public override void SetDefaults()
		{
			base.SetDefaults();
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Pokecase");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
