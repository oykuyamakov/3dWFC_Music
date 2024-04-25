using System.Collections;
using System.Collections.Generic;

namespace UnityCommon.Runtime.Collections
{
	public class PriorityList<T> : IEnumerable<T>
	{
		private List<T> m_Items;
		private List<int> m_Priorities;

		public int Count => m_Items.Count;

		public T this[int i]
		{
			get { return m_Items[i]; }
		}


		public PriorityList()
		{
			m_Items = new List<T>();
			m_Priorities = new List<int>();
		}

		public PriorityList(int capacity)
		{
			m_Items = new List<T>(capacity);
			m_Priorities = new List<int>(capacity);
		}

		public void Add(T item, int priority)
		{
			for (var i = 0; i < m_Priorities.Count; i++)
			{
				if (m_Priorities[i] > priority)
				{
					m_Items.Insert(i, item);
					m_Priorities.Insert(i, priority);
					return;
				}
			}

			m_Items.Add(item);
			m_Priorities.Add(priority);
		}

		public void Remove(T item)
		{
			var index = m_Items.IndexOf(item);
			if (index < 0)
				return;

			m_Items.RemoveAt(index);
			m_Priorities.RemoveAt(index);
		}

		public void Clear()
		{
			m_Items.Clear();
			m_Priorities.Clear();
		}

		public IEnumerator<T> GetEnumerator()
		{
			return m_Items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_Items.GetEnumerator();
		}
	}
}
