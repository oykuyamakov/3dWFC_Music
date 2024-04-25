using Events;

namespace SceneManagement
{
	public class SceneChangeRequestEvent : Event<SceneChangeRequestEvent>
	{
		public SceneId sceneId = SceneId.None;
		
		public static SceneChangeRequestEvent Get(SceneId sceneId)
		{
			var evt = GetPooledInternal();
			evt.sceneId = sceneId;
			return evt;
		}
	}
}
