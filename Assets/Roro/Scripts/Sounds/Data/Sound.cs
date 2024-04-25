using System;
using Roro.Scripts.Serialization;
using UnityEngine;

namespace Roro.Scripts.Sounds.Data
{
	[CreateAssetMenu(menuName = "Sound", fileName = "_Sound")]
	public class Sound : ScriptableObject
	{
		[SerializeField]
		private AudioClip m_AudioClip;

		[SerializeField]
		private float m_Volume = 0.75f;

		[SerializeField]
		private float m_Pitch = 1f;
		
		[SerializeField]
		public bool Loop = false;

		private int m_Id = -1;

		public int GetId()
		{
			if (m_Id != -1)
				return m_Id;
			
			var identifier = SerializationWizard.Default.GetInt("Loop_Identifier", 0);
			m_Id = identifier;
			SerializationWizard.Default.SetInt("Loop_Identifier", identifier+1);
			SerializationWizard.Default.Push();
			return m_Id;
		}

		public AudioClip Clip   => m_AudioClip;
		public float     Volume => m_Volume;
		public float     Pitch  => m_Pitch;

	}
}
