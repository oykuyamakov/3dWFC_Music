using UnityCommon.Singletons;

namespace UnityCommon.Modules
{
	public abstract class Module<T> : SingletonBehaviour<T> where T : Module<T>
	{
		protected virtual void Awake()
		{
			if (!SetupInstance())
				return;
		}

		public abstract void OnEnable();

		public abstract void OnDisable();

		public abstract void Update();
		public abstract void LateUpdate();
	}
}
