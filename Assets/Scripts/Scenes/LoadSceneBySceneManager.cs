using UnityEngine;
namespace Assets.Scripts.Scenes
{
	public class LoadSceneBySceneManager : MonoBehaviour
	{
		public static void LoadScene(string sceneName)
		{
			SceneManager.Instance.SceneLoad(sceneName);
		}
	}
}