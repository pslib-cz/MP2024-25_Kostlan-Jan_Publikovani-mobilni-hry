using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// When player dies, show DeathScreen
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

	/// <summary>
	/// Exit game.
	/// </summary>
	public void QuitGame()
	{
		Application.Quit();
	}

	/// <summary>
	/// Load MainMenu
	/// </summary>
	public void GoToMainMenu()
	{
		bannerAdsDeath.DestroyAd();
		AudioListener.pause = false;
		Time.timeScale = 1f;
		SceneManager.LoadScene("MainMenu");
	}

	/// <summary>
	/// Restart current level
	/// </summary>
	public void RestartLevel()
	{
		bannerAdsDeath.DestroyAd();
		AudioListener.pause = false;
		Time.timeScale = 1f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}