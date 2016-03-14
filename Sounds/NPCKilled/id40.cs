using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ModLoader;

namespace PokeModRed.Sounds.NPCKilled
{
	public class id40 : ModSound
	{
		public override void PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
		{
			soundInstance = sound.CreateInstance();
			soundInstance.Volume = volume;
			soundInstance.Pan = pan;
			soundInstance.Pitch = 0.8f;
			Main.PlaySoundInstance(soundInstance);
		}
	}
}
