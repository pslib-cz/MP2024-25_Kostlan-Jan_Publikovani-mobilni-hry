using UnityEngine;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class MainMenu : MonoBehaviour
{
	private string _mStatus = "Ready";
	private string mSavedGameFilename = "MyCustomSaveGame";
	private ISavedGameMetadata mCurrentSavedGame = null;
	public GameObject dialog;
	public GameObject loadTutorialDialog;
	public GameObject loadGameChoiceDialog;
	public string tutorialScene = "tutorial";
	public Button removeAdsButton;
	public InPurchasingApp inPurchasingApp;
	[SerializeField] private string discordUrl;
	public void Awake()
	{
		if (PlayerPrefs.GetInt(PlayerPrefsKeys.HasAds, 0) == 1)
		{
			RemoveAds();
		}
		LoadLanguage();
	}

	public void Start()
	{

#if DEBUG
		PlayGamesPlatform.DebugLogEnabled = true;
#endif
		PlayGamesPlatform.Activate();
		AuthenticateUser();

		if (PlayerPrefs.GetInt(PlayerPrefsKeys.HasPlayedBefore, 0) == 0)
		{
			CheckForSavedGame();
		}
	}

	public void LoadLanguage()
	{
		string language = PlayerPrefs.GetString(PlayerPrefsKeys.Language, null);

		if (string.IsNullOrEmpty(language))
		{
			SystemLanguage systemLanguage = Application.systemLanguage;

			switch (systemLanguage)
			{
				case SystemLanguage.English:
					language = "en";
					break;
				case SystemLanguage.Czech:
					language = "cs";
					break;
				default:
					language = "en";
					break;
			}

			PlayerPrefs.SetString(PlayerPrefsKeys.Language, language);
			PlayerPrefs.Save();
		}

		var locale = LocalizationSettings.AvailableLocales.GetLocale(language);

		if (locale != null)
		{
			LocalizationSettings.SelectedLocale = locale;
			Debug.Log("Jazyk nastavený na: " + language);
		}
		else
		{
			Debug.LogError("Jazyk se nenašel: " + language);
		}
	}

	public void PlayNewGame()
	{
		SceneManager.LoadScene("Intro");
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void Browser()
	{
		Application.OpenURL("https://pixeldread.com/");
	}

	public void OpenDiscord()
	{
		Application.OpenURL(discordUrl);
	}

	public void LoadSceneByName(string name)
	{
		SceneManager.LoadScene(name);
	}

	public void RemoveAds()
	{
		removeAdsButton.gameObject.SetActive(false);
	}

	public void LoadSaveGame()
	{
		string lastScene = PlayerPrefs.GetString(PlayerPrefsKeys.LastScene);
		if (lastScene != "")
		{
			SceneManager.LoadScene(lastScene);
		}
		else
		{
			SceneManager.LoadScene("Intro");
		}
	}

	public void AuthenticateUser()
	{
		PlayGamesPlatform.Instance.Authenticate((signInStatus) =>
		{
			if (signInStatus == SignInStatus.Success)
			{
				Debug.Log("Přihlášení je úspěšné!");
				LoadRemoveAdsFromCloud();
			}
			else
			{
				Debug.LogError("Přihlášení se nepodařilo: " + signInStatus);
			}
		});
	}

	void LoadRemoveAdsFromCloud()
	{
		if (PlayGamesPlatform.Instance.IsAuthenticated())
		{
			PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConflictResolution(
				 "MyCustomSaveGame",
				 DataSource.ReadCacheOrNetwork,
				 ConflictResolutionStrategy.UseLongestPlaytime,
				 (status, game) =>
				 {
					 if (status == SavedGameRequestStatus.Success)
					 {
						 PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(game, (readStatus, data) =>
						 {
							 if (readStatus == SavedGameRequestStatus.Success && data != null)
							 {
								 string saveData = System.Text.Encoding.UTF8.GetString(data);
								 if (saveData.Contains("remove_ads=true"))
								 {
									 PlayerPrefs.SetInt(PlayerPrefsKeys.HasAds, 1);
									 PlayerPrefs.Save();
									 RemoveAds();
									 Debug.Log("Je to nakoupený od uživatele a uložený na jeho cloud.");
								 }
							 }
							 else
							 {
								 Debug.LogError("Selhalo čtení dat o reklamách.");
							 }
						 });
					 }
					 else
					 {
						 Debug.LogError("Selhalo přečtení dat z google cloud.");
					 }
				 });
		}
	}

	public void OnSignInResult(SignInStatus signInStatus)
	{
		if (signInStatus == SignInStatus.Success)
		{
			_mStatus = "Authenticated. Hello, " + PlayGamesPlatform.Instance.localUser.userName + " (" + PlayGamesPlatform.Instance.localUser.id + ")";
			Debug.Log(_mStatus);

			CheckForSavedGame();
		}
		else
		{
			_mStatus = "*** Failed to authenticate with " + signInStatus.ToString() + ". Possible issue: ";
			if (signInStatus == SignInStatus.Canceled)
			{
				_mStatus += "User canceled the login.";
			}
			else if (signInStatus == SignInStatus.InternalError)
			{
				_mStatus += "An internal error occurred in Google Play Games.";
			}
			else
			{
				_mStatus += "An unknown error occurred.";
			}

			Debug.LogError(_mStatus);
		}

		ShowEffect(signInStatus == SignInStatus.Success);
	}

	public void ShowSavedGameUI()
	{
		PlayGamesPlatform.Instance.SavedGame.ShowSelectSavedGameUI(
			"Saved Game UI",
			10,
			false,
			false,
			(status, savedGame) =>
			{
				_mStatus = "UI Status: " + status;
				if (savedGame != null)
				{
					_mStatus += "Retrieved saved game with description: " + savedGame.Description;
					mCurrentSavedGame = savedGame;
				}
				Debug.Log(_mStatus);
			});
	}

	private void CheckForSavedGame()
	{
		OpenSavedGame(mSavedGameFilename);
	}

	private void OpenSavedGame(string filename)
	{
		PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConflictResolution(
			filename,
			DataSource.ReadCacheOrNetwork,
			ConflictResolutionStrategy.UseLongestPlaytime,
			OnSavedGameOpened);
	}

	private void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
	{
		if (status == SavedGameRequestStatus.Success)
		{
			mCurrentSavedGame = game;

			bool saveExists = game.TotalTimePlayed > TimeSpan.Zero || game.LastModifiedTimestamp != DateTime.MinValue;

			if (PlayerPrefs.GetInt(PlayerPrefsKeys.HasPlayedBefore, 0) == 0)
			{
				if (saveExists)
				{
					loadGameChoiceDialog.SetActive(true);
				}
				else
				{
					loadTutorialDialog.SetActive(true);
				}
			}
			PlayerPrefs.SetInt(PlayerPrefsKeys.HasPlayedBefore, 1);
		}
		else
		{
			Debug.LogError("Selhalo otevření hry.");
			loadTutorialDialog.SetActive(true);
			PlayerPrefs.SetInt(PlayerPrefsKeys.HasPlayedBefore, 1);
		}
	}

	public void LoadSavedGameData()
	{
		if (mCurrentSavedGame != null)
		{
			PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(mCurrentSavedGame, OnSavedGameDataRead);
		}
	}

	private void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] data)
	{
		if (status == SavedGameRequestStatus.Success && data != null)
		{
			string savedLevel = System.Text.Encoding.UTF8.GetString(data);
			SceneManager.LoadScene(savedLevel);
		}
		else
		{
			Debug.LogError("Selhalo přečtení herních dat.");
			ShowDialogTutorial();
		}
	}

	private void ShowEffect(bool success)
	{
		Camera.main.backgroundColor = success ? new Color(0.0f, 0.0f, 0.8f, 1.0f) : new Color(0.8f, 0.0f, 0.0f, 1.0f);
	}

	public void ShowDialogTutorial()
	{
		dialog.SetActive(true);
	}

	public void StartTutorial()
	{
		SceneManager.LoadScene(tutorialScene);
	}

	public void OnChooseLoadLastSave(bool load)
	{
		if (load)
		{
			LoadSavedGameData();
		}
		else
		{
			DeleteSavedGame();
		}

		loadGameChoiceDialog.SetActive(false);
	}

	private void DeleteSavedGame()
	{
		if (mCurrentSavedGame != null)
		{
			PlayGamesPlatform.Instance.SavedGame.Delete(mCurrentSavedGame);
			Debug.Log("Uložení hry selhalo");
		}
	}

	public void BuyRemoveAds()
	{
		if (inPurchasingApp != null)
		{
			inPurchasingApp.BuyNonConsumable();
		}
		else
		{
			Debug.LogError("InPurchasingApp is not assigned.");
		}
	}

	public void OnAdsRemoved()
	{
		RemoveAds();
		Debug.Log("Reklama byla úspěšně odstraněná!");
	}
}