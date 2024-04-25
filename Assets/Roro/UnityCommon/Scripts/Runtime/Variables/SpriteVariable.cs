using System;
using UnityEngine;


namespace UnityCommon.Variables
{
	[CreateAssetMenu(menuName = "Variables/Sprite Variable", fileName = "New Sprite Variable")]
	public class SpriteVariable : Variable<Sprite>
	{

		public override bool CanBeBoundToPlayerPrefs()
		{
			return false;
		}

	}

	[Serializable]
	public class SpriteReference : Reference<Sprite, SpriteVariable>
	{

	}



}
