using System;
using UnityEngine;


namespace UnityCommon.Variables
{
	[CreateAssetMenu(menuName = "Variables/LayerMask Variable", fileName = "New LayerMask Variable")]
	public class LayerMaskVariable : Variable<LayerMask>
	{

		public override bool CanBeBoundToPlayerPrefs() => false;

	}

	[Serializable]
	public class LayerMaskReference : Reference<LayerMask, LayerMaskVariable>
	{

	}



}
