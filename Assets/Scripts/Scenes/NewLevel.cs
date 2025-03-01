using UnityEngine;
using Assets.Scripts.Scenes;

/// <summary>
/// Pokud se triggrem dotkne hráč, mění scénu podle zadaného parametru.
/// </summary>
public class NewLevel : MonoBehaviour
{
	public string scene;

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.CompareTag("Player"))
		{
			if (SceneManager.Instance != null)
			{
				SceneManager.Instance.SceneLoad(scene);
				Destroy(gameObject);
			}
		}
	}
}