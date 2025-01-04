using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi.SavedGame;
using System.Text;
using GooglePlayGames.BasicApi;

namespace Assets.Scripts
{
	public class InicializationScene : MonoBehaviour
	{
		private Image image;
		private float fadeDuration = 5f;
		private string savedGameName = "MyCustomSaveGame";
		[SerializeField] private bool saveSceneToPrefs = true;

		private Coroutine fadeCoroutine;

		private void Awake()
		{
			GameObject canvasObject = GameObject.Find("BlackImage");
			if (canvasObject != null)
			{
				image = canvasObject.GetComponent<Image>();
				image.color = new Color(0, 0, 0, 1);
			}
			else
			{
				Debug.LogError("BlackImage nebyl nalezen.");
			}
		}

		void Start()
		{
			string currentSceneName = SceneManager.GetActiveScene().name;
			if (saveSceneToPrefs)
			{
				PlayerPrefs.SetString(PlayerPrefsKeys.LastScene, currentSceneName);
				PlayerPrefs.Save();
				SaveCurrentSceneToGooglePlay(currentSceneName);
			}

			if (image != null)
			{
				fadeCoroutine = StartCoroutine(FadeToTransparent(fadeDuration));
			}
		}

		public void PauseFade()
		{
			if (fadeCoroutine != null)
			{
				StopCoroutine(fadeCoroutine);
				fadeCoroutine = null;
			}
		}

		public void ResumeFade()
		{
			if (fadeCoroutine == null && image != null)
			{
				fadeCoroutine = StartCoroutine(FadeToTransparent(fadeDuration));
			}
		}

		private IEnumerator FadeToTransparent(float duration)
		{
			Color color = image.color;
			float startAlpha = color.a;
			float time = 0;

			while (time < duration)
			{
				time += Time.unscaledDeltaTime;
				color.a = Mathf.Lerp(startAlpha, 0, time / duration);
				image.color = color;
				yield return null;
			}

			color.a = 0;
			image.color = color;
		}

		private void SaveCurrentSceneToGooglePlay(string sceneName)
		{
			if (PlayGamesPlatform.Instance.IsAuthenticated())
			{
				PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConflictResolution(
					savedGameName,
					DataSource.ReadCacheOrNetwork,
					ConflictResolutionStrategy.UseLongestPlaytime,
					(status, game) =>
					{
						if (status == SavedGameRequestStatus.Success)
						{
							SaveSceneData(sceneName, game);
						}
						else
						{
							Debug.LogError("Failed to open saved game for writing.");
						}
					});
			}
			else
			{
				Debug.LogWarning("User is not authenticated, cannot save game.");
			}
		}

		private void SaveSceneData(string sceneName, ISavedGameMetadata game)
		{
			byte[] data = Encoding.UTF8.GetBytes(sceneName);

			SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder()
				.WithUpdatedDescription("Saved at " + System.DateTime.Now)
				.Build();

				PlayGamesPlatform.Instance.SavedGame.CommitUpdate(
				game,
				update,
				data,
				(status, updatedGame) =>
				{
					if (status == SavedGameRequestStatus.Success)
					{
						Debug.Log("Scene saved successfully to Google Play.");
					}
					else
					{
						Debug.LogError("Failed to save scene to Google Play.");
					}
				});
		}
	}
}