using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ModLoader;

namespace PokeModRed.Sounds.Item
{
	public class id25 : ModSound
	{
		public override void PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
		{
			soundInstance = sound.CreateInstance();
			soundInstance.Volume = volume;
			soundInstance.Pan = pan;
			soundInstance.Pitch = 1.0f;
			Main.PlaySoundInstance(soundInstance);
		}
	}
}
