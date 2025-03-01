using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Ads;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Scenes
{
	/// <summary>
	/// Skript pro práci se scénami.
	/// </summary>
	/// todo proč mi to nevydělává :(
	public class SceneManager : MonoBehaviour
	{
		public const string introScene = "Intro";
		private Image image;
		private string targetSceneName;
		private const float fadeDuration = 3f;

		public static SceneManager Instance;
		[SerializeField] private RewardAds adPrefab;
		private float gameStartTime;
		private const float adTriggerTime = 900f;

		private void Start()
		{
			gameStartTime = Time.time;
		}

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Destroy(gameObject);
				return;
			}

			Instance = this;
			DontDestroyOnLoad(gameObject);

			UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (CanShowAd())
			{
				CreateRewardAdsObject();
				gameStartTime = Time.time;

			}
		}

		private bool CanShowAd()
		{
			// Zkontroluje, zda hráč hraje déle než 30 minut
			return Time.time - gameStartTime >= adTriggerTime;
		}


		public void SceneLoad(string sceneName)
		{
			FindBlackImage();
			targetSceneName = sceneName;
			StartCoroutine(LoadSceneAsync(sceneName));
		}

		public void LoadLastScene()
		{
			if (PlayerPrefs.HasKey(PlayerPrefsKeys.LastScene))
			{
				string lastScene = PlayerPrefs.GetString(PlayerPrefsKeys.LastScene);
				targetSceneName = lastScene;

				FindBlackImage();
				StartCoroutine(LoadSceneAsync(lastScene));
			}
			else
			{
				targetSceneName = introScene;
				StartCoroutine(LoadSceneAsync(introScene));
			}
		}

		private void FindBlackImage()
		{
			GameObject canvasObject = GameObject.Find("BlackImage");
			if (canvasObject != null)
			{
				image = canvasObject.GetComponent<Image>();
				image.color = new Color(0, 0, 0, 0);
			}
			else
			{
				Debug.LogError("BlackImage nebyl nalezen.");
			}
		}

		private IEnumerator LoadSceneAsync(string sceneName)
		{
			AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
			asyncLoad.allowSceneActivation = false;

			Coroutine fadeCoroutine = StartCoroutine(FadeToOpaque(fadeDuration));

			while (!asyncLoad.isDone && asyncLoad.progress < 0.9f)
			{
				yield return null;
			}

			yield return fadeCoroutine;

			asyncLoad.allowSceneActivation = true;

		}

		private IEnumerator FadeToOpaque(float duration)
		{
			Color color = image.color;
			float startAlpha = 0;
			float time = 0;

			color.a = startAlpha;
			image.color = color;

			while (time < duration)
			{
				time += Time.unscaledDeltaTime;
				color.a = Mathf.Lerp(startAlpha, 1, time / duration);
				image.color = color;
				yield return null;
			}

			color.a = 1;
			image.color = color;
		}

		private void CreateRewardAdsObject()
		{
			if (adPrefab != null)
			{
				Instantiate(adPrefab, Vector3.zero, Quaternion.identity);
			}
			else
			{
				Debug.LogError("Ad Prefab není nastaven v SceneManageru.");
			}
		}
	}
}