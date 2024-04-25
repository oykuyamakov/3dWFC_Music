using System;

namespace UnityCommon.Utilities
{
	public static class TimeUtility
	{
		public static long UnixMilliseconds => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		public static long UnixSeconds      => DateTimeOffset.UtcNow.ToUnixTimeSeconds();
	}
}
