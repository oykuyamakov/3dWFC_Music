using System;
using System.Globalization;
using UnityEngine;


namespace UnityCommon.Variables
{
	[CreateAssetMenu(menuName = "Variables/Long Variable", fileName = "New Long Variable")]
	public class LongVariable : Variable<long>
	{
		public override string Serialize()
		{
			return Value.ToString(CultureInfo.InvariantCulture);
		}

		public override void Deserialize(string s)
		{
			value = long.Parse(s, CultureInfo.InvariantCulture);
		}

		public void Add(int val)
		{
			Value += val;
		}
	}

	[Serializable]
	public class LongReference : Reference<long, LongVariable>
	{
	}
}
