using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Events;
using Roro.Scripts.Serialization;
using Roro.Scripts.Sounds.Data;
using Roro.Scripts.Sounds.Helpers;
using Roro.Scripts.Utility;
using UnityCommon.Singletons;
using UnityEngine;

namespace Roro.Scripts.Sounds.Core
{
	[DefaultExecutionOrder(ExecOrder.SoundManager)]
	[RequireComponent(typeof(AudioSource))]
	public class SoundManager : SingletonBehaviour<SoundManager>
	{
		[SerializeField]
		private Transform m_LoopSourcesParent;
		[SerializeField]
		private Transform m_OneShotSourcesParent;

		[SerializeField] 
		private AudioSource m_MainSongSource;

		private List<AudioSource> m_LoopAudioSources => m_LoopSourcesParent.GetComponents<AudioSource>().ToList();

		private List<AudioSource> m_OneShotAudioSources => m_OneShotSourcesParent.GetComponents<AudioSource>().ToList();
		
		private float m_SfxVolume => m_SerializationWizard.GetInt("Sfx Volume", 10) / 10f;
		private float m_MusicVolume => m_SerializationWizard.GetInt("Music Volume", 10) / 10f;
		private bool m_AudioDisabled => m_SerializationWizard.GetBool("Audio Disabled", false);

		private Dictionary<int, AudioSource> m_SourcesbyLoopIds = new Dictionary<int, AudioSource>();
		
		private SerializationWizard m_SerializationWizard = SerializationWizard.Default;

		private int m_SourceIndex;
		
		private Sound m_CurrentMainSound;

		private Tween m_MainAudioTweener;
		
		private void Awake()
		{
			if (!SetupInstance())
				return;

			m_SourceIndex = 0;
			
			GEM.AddListener<SoundPlayEvent>(OnSoundPlayEvent);
			GEM.AddListener<SoundStopEvent>(OnSoundStopEvent);
		}


		private void OnSoundPlayEvent(SoundPlayEvent evt)
		{
			if (evt.MainSong)
			{
				TryPlayMainSong(evt.Sound, m_MusicVolume);
				return;
			}
			if (evt.Loop)
			{
				PlayLoop(evt.Sound, m_MusicVolume);
			}
			else
			{
				PlayOneShot(evt.Sound, m_SfxVolume);
			}
		}
		
		private void TryPlayMainSong(Sound sound, float volume = 1f, float pitch = 1f)
		{
			m_MainAudioTweener?.Kill();
			m_MainAudioTweener = m_MainSongSource.DOFade(0, 0.1f).OnComplete( () => PlayMainSong(sound,volume,pitch));
		}
		
		private void PlayMainSong(Sound sound, float volume = 1f, float pitch = 1f)
		{
			//looping the main theme with force?
			m_MainSongSource.loop = true;
			if (!sound || !sound.Clip || sound.Volume < 1e-2f)
			{
				Debug.Log($"Ignoring sound {sound.name}");
				return;
			}

			m_MainSongSource.Play(sound, volume, pitch);
			m_CurrentMainSound = sound;
		}

		private void OnSoundStopEvent(SoundStopEvent evt)
		{
			StopSound(evt.LoopIndex);
		}

		public void StopSound(int index)
		{
			if(!m_SourcesbyLoopIds.ContainsKey(index))
				return;
			
			Debug.Log("Stopping" + index);

			
			m_SourcesbyLoopIds[index].Stop();
		}
		
		public void OnMusicVolumeChange()
		{
			var currentVolume = m_CurrentMainSound != null ? m_CurrentMainSound.Volume : 1;
			m_MainSongSource.volume = m_MusicVolume * currentVolume;
		}
		
		private AudioSource GetSource()
		{
			var src = m_OneShotAudioSources[m_SourceIndex++];
			m_SourceIndex %= m_OneShotAudioSources.Count;
			return src;
		}
		
		public void Reset()
		{
			Debug.Log("Resetting Sounds");
			
			m_OneShotAudioSources.ForEach(source =>
			{
				if (source.isPlaying)
				{
					source.DOFade(0, 0.1f); //.OnComplete(() => source.loop = false);
				}
			
			});
			m_LoopAudioSources.ForEach(source =>
			{
				if (source.isPlaying)
				{
					source.DOFade(0, 0.1f); //.OnComplete(() => source.loop = false);
				}
			
			});
		}

		public void PlayOneShot(Sound sound, float volume = 1f, float pitch = 1f)
		{
			if (m_AudioDisabled)
				return;
			
			if (!sound || !sound.Clip || sound.Volume < 1e-2f)
			{
				Debug.Log($"Ignoring sound {sound.name}");
				return;
			}

			var src = GetSource();
			src.PlayOneShot(sound, volume, pitch);
		}
		public void PlayLoop(Sound sound, float volume = 1f, float pitch = 1f)
		{
			if (m_AudioDisabled)
				return;
			
			Debug.Log("Playing Loop" + sound.name);
			
			if (!sound || !sound.Clip || sound.Volume < 1e-2f)
			{
				Debug.Log($"Ignoring sound {sound.name}");
				return;
			}
			

			if (m_SourcesbyLoopIds.ContainsKey(sound.GetId()))
			{
				var usedSrc = m_SourcesbyLoopIds[sound.GetId()];
				if(!usedSrc.isPlaying)
					m_SourcesbyLoopIds[sound.GetId()].Pause();
			}
			else
			{
				var src = GetSource();
				// TODO 
				src.loop = true;
				m_SourcesbyLoopIds.Add(sound.GetId(), src);
				
				src.Play(sound, volume, pitch);
			}
			
		}
		
		
		private void OnDestroy()
		{
			GEM.RemoveListener<SoundPlayEvent>(OnSoundPlayEvent);
			GEM.RemoveListener<SoundStopEvent>(OnSoundStopEvent);
		}
		
		
#if UNITY_EDITOR

		public void GetSounds()
		{
			
		}
#endif
		

	}
}

//Difference between audiosource.Play() vs .PlayOneShot() is you can play multiple sounds with PlayOneShot()
// but wont be able to stop them.