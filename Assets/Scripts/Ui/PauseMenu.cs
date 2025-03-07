using Assets.Scripts.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	public static bool GameIsPaused;
	public GameObject PauseMenuUI;
	public GameObject SettingsMenuUI;
	[SerializeField] private BannerAdsPause bannerAdsPause;
	[SerializeField] private PlayerInputs controls;

	private void Awake()
	{
		controls = InputManager.Instance.Controls;
	}

	private void OnEnable()
	{
		controls.Player.Pause.performed += OnPause;
		controls.Player.Pause.Enable();
	}

	private void OnDisable()
	{
		controls.Player.Pause.performed -= OnPause;
		controls.Player.Pause.Disable();
	}

	private void OnDestroy()
	{
		controls.Player.Pause.performed -= OnPause;
		controls.Player.Pause.Disable();
	}

	private void OnPause(InputAction.CallbackContext context)
	{
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
		AudioManager.instance.ContinueMusic();
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
		AudioManager.instance.PauseMusic();
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
