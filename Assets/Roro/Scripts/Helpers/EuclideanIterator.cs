using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Helpers
{
	public static class EuclideanIterator
	{
		public static IEnumerable<int> BeginIterate(int w, int h, int i)
		{
			int x = i % w;
			int y = i / w;

			return Enumerable.Range(0, w * h).OrderBy(k => UnityEngine.Random.Range(0, 9999))
			                 .OrderBy(k => Mathf.Pow((k % w) - x, 2) + Mathf.Pow((k / w) - y, 2));
		}

		public static IEnumerable<int> GetNeighbors(int w, int h, int i)
		{
			int x = i % w;
			int y = i / w;

			if (x + 1 < w)
				yield return y * w + x + 1;

			if (x - 1 >= 0)
				yield return y * w + x - 1;

			if (y + 1 < h)
				yield return (y + 1) * w + x;

			if (y - 1 >= 0)
				yield return (y - 1) * w + x;
		}

		public static IEnumerable<int> GetNeighborsWCorners(int w, int h, int i)
		{
			int x = i % w;
			int y = i / w;

			if (x + 1 < w)
				yield return y * w + x + 1;

			if (x - 1 >= 0)
				yield return y * w + x - 1;

			if (y + 1 < h)
				yield return (y + 1) * w + x;

			if (y - 1 >= 0)
				yield return (y - 1) * w + x;

			if (x + 1 < w && y + 1 < h)
				yield return (y + 1) * w + x + 1;

			if (x - 1 >= 0 && y - 1 >= 0)
				yield return (y - 1) * w + x - 1;

			if (y + 1 < h && x - 1 >= 0)
				yield return (y + 1) * w + (x - 1);

			if (y - 1 >= 0 && x + 1 < w)
				yield return (y - 1) * w + x + 1;
		}
	}
}
