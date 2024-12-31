using UnityEngine;

/// <summary>
/// Odstran� dan� objekt, pri kolizi s hr�cem.
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