using UnityEngine;

/// <summary>
/// Při triggeru s hráčem aktivuje nebo deaktivuje vybrané objekty.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class TriggerActivationAndDeactivationObjects : MonoBehaviour
{
	public GameObject objectToActivate;
	public GameObject objectToDeactivate;

	private bool isScriptEnabled = true;

	public void SetScriptEnabled(bool enabled)
	{
		isScriptEnabled = enabled;
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!isScriptEnabled) return;

		else if (other.CompareTag("Player"))
		{
			if (objectToActivate != null)
				objectToActivate.SetActive(true);

			if (objectToDeactivate != null)
				objectToDeactivate.SetActive(false);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (!isScriptEnabled) return;

		else if (other.CompareTag("Player"))
		{
			if (objectToActivate != null)
				objectToActivate.SetActive(false);

			if (objectToDeactivate != null)
				objectToDeactivate.SetActive(true);
		}
	}
}
