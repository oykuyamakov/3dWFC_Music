using System.Linq;
using System.Text.RegularExpressions;

namespace UnityCommon.Runtime.Extensions
{
	public static class StringExtensions
	{
		private static readonly char[] SimplifyAllowedChars = {'_', '-'};

		public static string Simplify(this string x)
		{
			return new string(x.ToLower().Trim()
			                   .Select(c => char.IsLetterOrDigit(c) || SimplifyAllowedChars.Contains(c) ? c : '_')
			                   .ToArray());
		}
		
		
		public static string SplitCamelCase(this string input)
		{
			return System.Text.RegularExpressions.Regex
			             .Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
		}

		public static int ExtractInt(this string s)
		{
			return int.Parse(Regex.Match(s, @"\d+").Value);
		}

	}
}
