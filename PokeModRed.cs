using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace PokeModRed
{
	public class PokeModRed : Mod
	{
		public override void SetModInfo(out string name, ref ModProperties properties)
		{
			name = "PokeModRed";
			properties.Autoload = true;
			properties.AutoloadGores = true;
			properties.AutoloadSounds = true;
		}
		
		public override void Load()
		{
			Pokedex.DoNothing();
		}
		
		public override void ChatInput(string text)
		{
			if (text[0] != '/')
			{
				return;
			}
			text = text.Substring(1);
			int index = text.IndexOf(' ');
			string command;
			string[] args;
			if (index < 0)
			{
				command = text;
				args = new string[0];
			}
			else
			{
				command = text.Substring(0, index);
				args = text.Substring(index + 1).Split(' ');
			}
			if (command == "pokedex")
			{
				PokedexCommand(args);
			}
		}
		
		private void PokedexCommand(string[] args)
		{
		int id;
			if (args.Length == 0 || !Int32.TryParse(args[0], out id))
			{
				Main.NewText("Usage: /pokedex [number]");
				Main.NewText("Input the number as the National Pokedex number");
				return;
			} else {
				PokedexEntry entry;
				if (Pokedex.pokedex.TryGetValue((float)id, out entry))
				{
					Main.NewText(entry.Print());
				} else {
					Main.NewText("No Pokemon found by that ID");
				}
			}
			
		}
	}
}