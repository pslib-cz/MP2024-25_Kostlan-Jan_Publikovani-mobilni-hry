using GoogleMobileAds.Api;
using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

namespace Assets.Scripts.Ads
{
	/// <summary>
	/// Odměňovací reklama, která odemkne další průchod hry.
	/// </summary>
	public class RewardAds : MonoBehaviour
	{
#if UNITY_ANDROID
		private string _rewardAdUnitId = "ca-app-pub-6609788058532191/6413512520";
#else
		private string _rewardAdUnitId = "unused";
#endif

		private RewardedAd _rewardedAd;
		public Button adObject;
		public Text buttonTextadObject;
		public float adCooldownTime = 30f;
		private float lastAdTime;
		private InicializationScene InicializationScene;
		[SerializeField] private LocalizeStringEvent localizedStringEvent;
		[SerializeField] private string adReady = "AdReady";
		[SerializeField] private string adCooldown = "AdCooldown";
		private InPurchasingApp InPurchasingApp;
		private LocalizedString localizedString;

		private void Awake()
		{
			localizedString = localizedStringEvent.StringReference;
			InPurchasingApp = GetComponent<InPurchasingApp>();
		}

		public void Start()
		{
			if (PlayerPrefs.GetInt(PlayerPrefsKeys.HasAds, 0) == 1)
			{
				Time.timeScale = 1f;
				return;
			}
			Debug.Log(PlayerPrefs.GetInt(PlayerPrefsKeys.HasAds, 0));

			MobileAds.Initialize(initStatus =>
			{
				LoadRewardedAd();
			});

			InicializationScene.PauseFade();

			lastAdTime = Time.realtimeSinceStartup - adCooldownTime;
		}

		private void Update()
		{
			int remainingTime = Mathf.CeilToInt(adCooldownTime - (Time.realtimeSinceStartup - lastAdTime));

			if (remainingTime <= 0)
			{
				// Cooldown skončil -> tlačítko se aktivuje
				adObject.interactable = true;
				localizedStringEvent.StringReference.TableEntryReference = adReady;
				localizedStringEvent.StringReference.Arguments = new object[] { }; // Bez argumentů
			}
			else
			{
				// Cooldown stále běží -> tlačítko zůstává disabled
				adObject.interactable = false;
				localizedStringEvent.StringReference.TableEntryReference = adCooldown;
				if (localizedString.TryGetValue("remainingTime", out var variable) && variable is IntVariable intVariable)
				{
					intVariable.Value = remainingTime;
				}
			}

			// Aktualizujeme zobrazený text
			localizedStringEvent.RefreshString();
		}

		/// <summary>
		/// Načte odměnovou reklamu.
		/// </summary>
		public void LoadRewardedAd()
		{
			if (_rewardedAd != null)
			{
				_rewardedAd.Destroy();
				_rewardedAd = null;
			}

			Debug.Log("Loading the rewarded ad.");

			var adRequest = new AdRequest();

			RewardedAd.Load(_rewardAdUnitId, adRequest,

				(RewardedAd ad, LoadAdError error) =>
				{
					if (error != null || ad == null)
					{
						Debug.LogError("Rewarded ad failed to load an ad " +
	 "with error : " + error);
						return;
					}

					Debug.Log("Rewarded ad loaded with response : "
 + ad.GetResponseInfo());

					_rewardedAd = ad;
				});
		}


		/// <summary>
		/// Zobrazuje odměnovou reklamu.
		/// </summary>
		public void ShowRewardedAd()
		{
			const string rewardMsg =
				"Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

			if (_rewardedAd != null && _rewardedAd.CanShowAd())
			{
				_rewardedAd.Show((Reward reward) =>
				{
					Debug.Log(string.Format(rewardMsg, reward.Type, reward.Amount));
					HandleUserEarnedReward(this, reward);
				});
			}
		}

		public void BuyCoffee()
		{
			InPurchasingApp.BuyNonConsumable();
		}

		/// <summary>
		/// Zpracování odměny hráče.
		/// </summary>
		public void HandleUserEarnedReward(object sender, Reward args)
		{
			Time.timeScale = 1f;

			InicializationScene inicializationScene = GetComponent<InicializationScene>();
			if (inicializationScene != null)
			{
				inicializationScene.ResumeFade();
			}

			Destroy(gameObject);
		}


		private void RegisterEventHandlers(RewardedAd ad)
		{
			ad.OnAdPaid += (AdValue adValue) =>
			{
				Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
					adValue.Value,
					adValue.CurrencyCode));
			};
			ad.OnAdImpressionRecorded += () =>
			{
				Debug.Log("Rewarded ad recorded an impression.");
			};
			ad.OnAdClicked += () =>
			{
				Debug.Log("Rewarded ad was clicked.");
			};
			ad.OnAdFullScreenContentOpened += () =>
			{
				Debug.Log("Rewarded ad full screen content opened.");
			};
			ad.OnAdFullScreenContentClosed += () =>
			{
				Debug.Log("Rewarded ad full screen content closed.");
			};
			ad.OnAdFullScreenContentFailed += (AdError error) =>
			{
				Debug.LogError("Rewarded ad failed to open full screen content " +
  "with error : " + error);
			};
		}

		private void RegisterReloadHandler(RewardedAd ad)
		{
			ad.OnAdFullScreenContentClosed += () =>
			{
				Debug.Log("Rewarded Ad full screen content closed.");

				LoadRewardedAd();
			};
			ad.OnAdFullScreenContentFailed += (AdError error) =>
			{
				Debug.LogError("Rewarded ad failed to open full screen content " +
  "with error : " + error);

				LoadRewardedAd();
			};
		}
	}
}