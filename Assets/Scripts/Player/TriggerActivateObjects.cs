using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
	[RequireComponent(typeof(Collider2D))]
	public class TriggerActivateObjects : MonoBehaviour
	{
		[SerializeField] private GameObject[] objectsToActivate;
		[SerializeField] private UnityEvent onPlayerTrigger;

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.CompareTag("Player"))
			{
				if (objectsToActivate.Length != 0)
				{
					foreach (GameObject obj in objectsToActivate)
					{
						obj.SetActive(true);
					}
				}

				onPlayerTrigger.Invoke();
			}
		}
	}
}
