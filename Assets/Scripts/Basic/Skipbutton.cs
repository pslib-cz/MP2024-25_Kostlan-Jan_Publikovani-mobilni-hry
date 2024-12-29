using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Basic
{
	/// <summary>
	/// Tlačítko na přeskakování scén.
	/// </summary>
	public class SkipButton : MonoBehaviour
	{
		public TimelineController timelineController;

		void Start()
		{
			Button button = GetComponent<Button>();
			button.onClick.AddListener(SkipTimeline);
		}

		public void SkipTimeline()
		{
			timelineController.shouldSkip = true;
			gameObject.SetActive(false);
		}
	}
}