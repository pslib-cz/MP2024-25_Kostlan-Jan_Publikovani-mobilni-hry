using UnityEngine;

namespace Assets.Scripts.Scenes
{
	/// <summary>
	/// Spustí novou úroveň bez kolize.
	/// </summary>
	public class NewLevelWithoutCollision : MonoBehaviour
	{
		public string scene;

		void Start()
		{
			if (SceneManager.Instance != null)
			{
				SceneManager.Instance.SceneLoad(scene);
				Destroy(gameObject);
			}
		}
	}
}