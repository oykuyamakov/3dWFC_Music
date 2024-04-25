using System;
using UnityEngine;


namespace UnityCommon.Variables
{
	[CreateAssetMenu(menuName = "Variables/AnimationCurve Variable", fileName = "New AnimationCurve Variable")]
	public class AnimationCurveVariable : Variable<AnimationCurve>
	{

		public override bool CanBeBoundToPlayerPrefs() => false;


	}

	[Serializable]
	public class AnimationCurveReference : Reference<AnimationCurve, AnimationCurveVariable>
	{

	}



}
