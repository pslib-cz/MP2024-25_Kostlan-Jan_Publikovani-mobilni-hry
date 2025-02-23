using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Okno smrti, které se ukáže pro hráčovi smrti.
/// </summary>
public class DeathMenu : MonoBehaviour
{
	public AudioClip deathMusicClip;
	public AudioClip normalMusicClip;
	public BannerAdsDeath bannerAdsDeath;

	private void Start()
	{
		Time.timeScale = 0f;
		AudioManager.instance.ChangeMusic(deathMusicClip);
		AudioListener.pause = true;
		Handheld.Vibrate();
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void GoToMainMenu()
	{
		bannerAdsDeath.DestroyAd();
		AudioListener.pause = false;
		Time.timeScale = 1f;
		SceneManager.LoadScene("MainMenu");
	}

	public void RestartLevel()
	{
		bannerAdsDeath.DestroyAd();
		AudioListener.pause = false;
		Time.timeScale = 1f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}