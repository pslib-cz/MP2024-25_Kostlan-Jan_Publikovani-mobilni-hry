using UnityEngine;

namespace Assets.Scripts.Scenes
{

	/// <summary>
	/// Pokud se triggeru dotkne hráč, mění scénu podle zadaného parametru.
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
}