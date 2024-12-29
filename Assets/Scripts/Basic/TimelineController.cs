using UnityEngine;
using UnityEngine.Playables;

namespace Assets.Scripts.Basic
{
	/// <summary>
	/// Správce pro timeline, který se stárá o přeskočitelnost.
	/// </summary>
	public class TimelineController : MonoBehaviour
	{
		public bool shouldSkip = false;
		private PlayableDirector playableDirector;
		public PlayerController2D scriptToActive;

		void Awake()
		{
			playableDirector = GetComponent<PlayableDirector>();
		}

		void Update()
		{
			if (shouldSkip)
			{
				SkipTimeline();
            }
		}

		public void SkipTimeline()
		{
			playableDirector.time = playableDirector.duration;
			playableDirector.Play();
			shouldSkip = false;

			if (scriptToActive != null)
			{
				scriptToActive.enabled = true;
			}
		}
	}
}