using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UnityCommon.Runtime.Utility
{
	[DefaultExecutionOrder(-10)]
	public class ParticleSystemController : MonoBehaviour
	{
		[SerializeField]
		private ParticleSystem[] systems;

		private List<float> defaultEmissions;
		private List<float> defaultOverDistanceEmissions;

		private bool isPlaying;

		public Color color
		{
			get { return systems[0].main.startColor.color; }
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			systems = GetComponentsInChildren<ParticleSystem>();
		}
#endif

		private void Awake()
		{
			isPlaying = systems.First().emission.enabled;

			defaultEmissions = systems.Select(s => s.emission.rateOverTimeMultiplier).ToList();

			defaultOverDistanceEmissions = systems.Select(s => s.emission.rateOverDistanceMultiplier).ToList();
		}

		[Button]
		public void Play()
		{
			isPlaying = true;

			foreach (var system in systems)
			{
				system.Play();
				var emission = system.emission;
				emission.enabled = true;
			}
		}

		[Button]
		public void Pause()
		{
			isPlaying = false;

			foreach (var system in systems)
			{
				system.Play();
				var emission = system.emission;
				emission.enabled = false;
			}
		}

		public void SetEmissionMultiplier(float speed)
		{
			if (defaultEmissions == null)
				defaultEmissions = systems.Select(s => s.emission.rateOverTimeMultiplier).ToList();

			if (defaultOverDistanceEmissions == null)
				defaultOverDistanceEmissions = systems.Select(s => s.emission.rateOverDistanceMultiplier).ToList();

			for (var index = 0; index < systems.Length; index++)
			{
				var system = systems[index];
				if (system.isPlaying == false)
					system.Play();

				var emission = system.emission;
				emission.rateOverTimeMultiplier = defaultEmissions[index] * speed;
				emission.rateOverDistanceMultiplier = defaultOverDistanceEmissions[index] * speed;
			}
		}

		public void Stop()
		{
			foreach (var system in systems)
			{
				system.Stop();
			}
		}
	}
}
