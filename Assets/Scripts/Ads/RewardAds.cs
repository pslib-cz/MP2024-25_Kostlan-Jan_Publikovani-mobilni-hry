using GoogleMobileAds.Api;
using System;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace Assets.Scripts.Ads
{
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

		private void Awake()
		{
			InicializationScene = FindFirstObjectByType<InicializationScene>();
		}

		public void Start()
		{
			if (PlayerPrefs.GetInt(PlayerPrefsKeys.HasAds, 0) == 1)
			{
				Time.timeScale = 1f;
				return;
			}

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
				adObject.interactable = true;
				localizedStringEvent.StringReference.TableEntryReference = adReady;
			}
			else
			{
				adObject.interactable = false;
				localizedStringEvent.StringReference.TableEntryReference = adCooldown;
				localizedStringEvent.StringReference.Arguments = new object[] { remainingTime };
			}

			localizedStringEvent.RefreshString();
		}

		/// <summary>
		/// Načte odměnovou reklamu.
		/// </summary>
		/// <summary>
		/// Loads the rewarded ad.
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

		/// <summary>
		/// Zpracování odměny hráče.
		/// </summary>
		private void HandleUserEarnedReward(object sender, Reward args)
		{
			Debug.Log($"Player earned reward: {args.Amount} {args.Type}");


			Time.timeScale = 1f;
			InicializationScene.ResumeFade();

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
