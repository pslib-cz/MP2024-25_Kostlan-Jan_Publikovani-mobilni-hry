using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace Assets.Scripts.Interactives
{
	/// <summary>
	/// Aktviuje danou scénu po určeném čase.
	/// </summary>
	public class ActivedThingsAfterTime : MonoBehaviour
	{
		public float delay = 5f;
		public UnityEvent thingsToActivated;

		public void StartActivatedThings()
		{
			StartCoroutine(ActivatedThings());
		}

		IEnumerator ActivatedThings()
		{
			yield return new WaitForSeconds(delay);
			thingsToActivated.Invoke();
		}
	}
}
