using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Ui
{
	public class LoadScene: MonoBehaviour
	{
		public string SceneName;
		public void LoadSceneGame()
		{
			SceneManager.LoadScene(SceneName);
		}
	}
}