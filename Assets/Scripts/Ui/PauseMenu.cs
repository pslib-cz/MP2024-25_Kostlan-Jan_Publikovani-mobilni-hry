using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	public InputActionReference pauseAction;
	public static bool GameIsPaused;
	public GameObject PauseMenuUI;
	public GameObject SettingsMenuUI;
	[SerializeField] private BannerAdsPause bannerAdsPause;

	private void OnEnable()
	{
		pauseAction.action.performed += OnPause;
		pauseAction.action.Enable();
    }

	private void OnDisable()
	{
		pauseAction.action.performed -= OnPause;
		pauseAction.action.Disable();
	}

    private void OnPause(InputAction.CallbackContext context)
	{
		Debug.Log("stiknuta klavesa");
		// Zkontroluje stav hry a podle toho přepíná mezi pauzou a obnovením
		if (GameIsPaused)
		{
			Resume();
		}
		else
		{
			Pause();
		}
	}

	public void Resume()
	{
		PauseMenuUI.SetActive(false);
		SettingsMenuUI.SetActive(false);
        AudioListener.pause = false;
        Time.timeScale = 1f;
        GameIsPaused = false;
		bannerAdsPause.DestroyAd();
        bannerAdsPause.enabled = false;
    }

	public void Pause()
	{
		bannerAdsPause.enabled = true;
		PauseMenuUI.SetActive(true);
		Time.timeScale = 0f;
		AudioListener.pause = true;
        GameIsPaused = true;
	}

	public void RestartLevel()
	{
        AudioListener.pause = false;
        bannerAdsPause.DestroyAd();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		Time.timeScale = 1f;
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void LoadMainMenu()
	{
        // natvrdo definovaná scéna.
        bannerAdsPause.DestroyAd();
        AudioListener.pause = false;
        SceneManager.LoadScene("MainMenu");
		Time.timeScale = 1f;
	}
}
