using UnityEngine;

namespace Assets.Scripts.Basic
{
	/// <summary>
	/// Přeskočí danou scénu na určenou scénáý.
	/// </summary>
	public class SkipScene : MonoBehaviour
	{
		public string SkipSceneName = "MainMenu";
		public void SkippingScene()
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(SkipSceneName);
		}
	}
}