using System;
using UnityEngine;


namespace UnityCommon.Variables
{
	[CreateAssetMenu(menuName = "Variables/GameObject Variable", fileName = "New GameObject Variable")]
	public class GameObjectVariable : Variable<GameObject>
	{

		public override bool CanBeBoundToPlayerPrefs()
		{
			return false;
		}


	}

	[Serializable]
	public class GameObjectReference : Reference<GameObject, GameObjectVariable>
	{

	}



}
