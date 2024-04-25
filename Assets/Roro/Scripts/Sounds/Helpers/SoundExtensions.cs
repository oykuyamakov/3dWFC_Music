using Roro.Scripts.Sounds.Data;
using UnityEngine;

namespace Roro.Scripts.Sounds.Helpers
{
	public static class SoundExtensions
	{
		public static void PlayOneShot(this AudioSource src, Sound sound, float volume = 1f, float pitch = 1f)
		{
			src.pitch = sound.Pitch * pitch;
			src.loop = sound.Loop;
			src.PlayOneShot(sound.Clip, sound.Volume * volume);
		}
		public static void Play(this AudioSource src, Sound sound, float volume = 1f, float pitch = 1f)
		{
			src.pitch = sound.Pitch * pitch;
			src.loop = sound.Loop;
			src.clip = sound.Clip;
			src.volume = sound.Volume * volume;
			src.Play();
		}
	}
}
