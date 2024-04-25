using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityCommon.Runtime.Extensions
{
	public static class CollectionExtensions
	{
		public static Dictionary<TVal, TKey> ReverseMap<TKey, TVal>(this Dictionary<TKey, TVal> source)
		{
			var rev = new Dictionary<TVal, TKey>();
			foreach (var entry in source)
			{
				rev[entry.Value] = entry.Key;
			}

			return rev;
		}

		public static IEnumerable<T> RandomTake<T>(this IEnumerable<T> source, int n)
		{
			try
			{
				return source.OrderBy(el => UnityEngine.Random.Range(0, 999999)).Take(n);
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
		{
			return source.OrderBy(item => UnityEngine.Random.Range(0, 999999));
		}

		public static IEnumerable<T> TakeSafe<T>(this IEnumerable<T> source, int maxCount)
		{
			var count = source.Count();
			if (count < maxCount)
				maxCount = count;

			return source.Take(maxCount);
		}

		public static IEnumerable<T> Except<T>(this IEnumerable<T> source, T element)
		{
			return source.Except(new[] {element});
		}

		public static IEnumerable<T> ArgDistinct<T, TArg>(this IEnumerable<T> source, Func<T, TArg> selector)
		{
			HashSet<TArg> hash = new HashSet<TArg>();
			foreach (var item in source)
			{
				if (hash.Add(selector(item)))
				{
					yield return item;
				}
			}
		}

		public static int LastIndexOf<T>(this IEnumerable<T> source, Func<T, bool> condition)
		{
			int i = 0;
			foreach (var item in source)
			{
				if (condition(item))
				{
					return i;
				}

				i++;
			}

			return -1;
		}

		public static int FirstIndexOf<T>(this IEnumerable<T> source, Func<T, bool> condition, int startIndex)
		{
			int i = 0;
			foreach (var item in source)
			{
				if (i < startIndex)
				{
					i++;
					continue;
				}

				if (condition(item))
				{
					return i;
				}

				i++;
			}

			return -1;
		}

		public static T MinBy<T, TArg>(this IEnumerable<T> source, Func<T, TArg> selector)
			where TArg : IComparable<TArg>
		{
			if (!source.Any())
				return default;

			bool first = true;
			T minObj = default(T);
			TArg minKey = default(TArg);
			foreach (var item in source)
			{
				if (first)
				{
					minObj = item;
					minKey = selector(minObj);
					first = false;
				}
				else
				{
					TArg currentKey = selector(item);
					if (currentKey.CompareTo(minKey) > 0)
					{
						minKey = currentKey;
						minObj = item;
					}
				}
			}

			if (first)
				throw new InvalidOperationException("Sequence is empty.");
			return minObj;
		}

		public static T MaxBy<T, TArg>(this IEnumerable<T> source, Func<T, TArg> selector)
			where TArg : IComparable<TArg>
		{
			if (!source.Any())
				return default;

			bool first = true;
			T maxObj = default(T);
			TArg maxKey = default(TArg);
			foreach (var item in source)
			{
				if (first)
				{
					maxObj = item;
					maxKey = selector(maxObj);
					first = false;
				}
				else
				{
					TArg currentKey = selector(item);
					if (currentKey.CompareTo(maxKey) > 0)
					{
						maxKey = currentKey;
						maxObj = item;
					}
				}
			}

			if (first)
				throw new InvalidOperationException("Sequence is empty.");
			return maxObj;
		}

		public static IEnumerable<T> NonNull<T>(this IEnumerable<T> source) where T : class
		{
			return source.Where(item => !ReferenceEquals(null, item));
		}

		public static IEnumerable<T> NonDefault<T>(this IEnumerable<T> source)
		{
			return source.Where(item => !Equals(default, item));
		}

		public static T GetAndRemoveAt<T>(this IList<T> source, int index)
		{
			var item = source[index];
			source.RemoveAt(index);
			return item;
		}

		public static void RemoveRange<T>(this IList<T> source, IEnumerable<T> removed)
		{
			foreach (var item in removed)
			{
				source.Remove(item);
			}
		}

		public static T ArgMin<T, TArg>(this IEnumerable<T> source, Func<T, TArg> selector)
		{
			return source.Select(item => (selector(item), item)).Min().item;
		}

		public static T ArgMax<T, TArg>(this IEnumerable<T> source, Func<T, TArg> selector)
		{
			return source.Select(item => (selector(item), item)).Max().item;
		}

		public static void AddByCapacity<T>(this List<T> source, T element)
		{
			if(source.Count == source.Capacity)
				return;
			
			source.Add(element);
		}

		public static void AddDistinct<T>(this List<T> source, T element)
		{
			if(source.Contains(element))
				return;
			
			source.Add(element);
		}
		
		public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
		{
			if(source.ContainsKey(key))
				source[key] = value;
			else
				source.Add(key, value);
		}
	}
}
