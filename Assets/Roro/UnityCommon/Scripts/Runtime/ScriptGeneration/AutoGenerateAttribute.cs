using System;

namespace UnityCommon.ScriptGeneration
{

	[AttributeUsage(AttributeTargets.Class)]
	public class AutoGenerateAttribute : Attribute
	{

		internal string[] typeNames;
		internal string typeNameFormat;
		internal bool mergeFiles;

		public AutoGenerateAttribute(string typeNameFormat, bool mergeFiles, params string[] typeNames)
		{
			this.mergeFiles = mergeFiles;
			this.typeNameFormat = typeNameFormat;
			this.typeNames = typeNames;
		}

	}

}
