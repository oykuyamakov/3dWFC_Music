using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UnityCommon.RuntimeCollections
{
	[CreateAssetMenu(menuName = "Runtime Collection/String")]
	public class StringCollection : RuntimeCollection<string>
	{
        [TextArea]
		public string data;

        
        [Button]
		public void Parse()
        {

			var d = data.Trim().Replace("\r", "");
			var lines = d.Split('\n');

			Clear();

			AddRange(lines.Select(l => l.Trim()));
        }

	}
}
