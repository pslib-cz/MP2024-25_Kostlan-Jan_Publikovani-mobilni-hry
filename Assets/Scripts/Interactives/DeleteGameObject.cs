using UnityEngine;

/// <summary>
/// Odstraní daný objekt, pri kolizi s hrácem.
/// </summary>
public class DeleteGameObject : MonoBehaviour
{
	public GameObject DeletedGameObject;

	public void DeleteObject()
	{
		Destroy(DeletedGameObject);
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			DeleteObject();
		}
	}
}