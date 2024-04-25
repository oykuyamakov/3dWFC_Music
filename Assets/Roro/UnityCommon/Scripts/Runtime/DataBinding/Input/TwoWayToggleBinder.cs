using Sirenix.OdinInspector;
using UnityCommon.DataBinding;
using UnityCommon.Runtime.Variables;
using UnityCommon.Variables;
using UnityEngine.UI;

namespace UnityCommon.Runtime.DataBinding
{
	public class TwoWayToggleBinder : BoolObserver
	{
		[Required]
		public Toggle toggle;

		public bool savePrefsOnInput = true;

#if UNITY_EDITOR
		protected override void OnValidate()
		{
			base.OnValidate();

			toggle = GetComponent<Toggle>();
		}
#endif

		protected override void Start()
		{
			base.Start();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			toggle.onValueChanged.AddListener(OnToggleInput);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			toggle.onValueChanged.RemoveListener(OnToggleInput);
		}


		private void OnToggleInput(bool val)
		{
			data.Value = negate ? !val : val;
			Variable.SavePlayerPrefs();
		}

		protected override void OnValueChanged(ValueChangedEvent<bool> evt)
		{
			base.OnValueChanged(evt);

			evt.value = negate ? !evt.value : evt.value;

			toggle.SetIsOnWithoutNotify(evt.value);
		}
	}
}
