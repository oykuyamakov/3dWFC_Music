using Sirenix.OdinInspector;
using UnityEngine;

namespace Events
{
	[CreateAssetMenu(menuName = "Event Context", fileName = "New Event Context")]
	public class EventContext : ScriptableObject
	{

		[Button(ButtonSizes.Large)]
		public void SendEvent()
		{
			EventExtensions.SendEvent(this);
		}

	}
}
