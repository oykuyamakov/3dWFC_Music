using System.Collections.Generic;
using UnityCommon.Variables;

namespace UnityCommon.Modules
{
	internal class VariablesModule : Module<VariablesModule>
	{
		public Dictionary<int, Variable> variables;
		public List<Variable> prefsVariables;
		
		public override void OnEnable()
		{
		}

		public override void OnDisable()
		{
		}

		public override void Update()
		{
		}

		public override void LateUpdate()
		{
		}
	}
}
