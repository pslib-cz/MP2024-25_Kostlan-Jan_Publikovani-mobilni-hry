using UnityEngine;
namespace Assets.Scripts.Scenes
{
	/// <summary>
	/// Manažer na načtení scény.
	/// </summary>
	public class LoadSceneBySceneManager : MonoBehaviour
	{
		public static void LoadScene(string sceneName)
		{
			SceneManager.Instance.SceneLoad(sceneName);
		}
	}
}