using UnityEngine;
using GoogleMobileAds.Ump.Api;
using GoogleMobileAds.Api;
using UnityEngine.UI;

public class AdConsentManager : MonoBehaviour
{
    private static AdConsentManager instance;
    private ConsentForm consentForm;

    public static AdConsentManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("Přidsání instance");
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
		RequestConsentInfoUpdate();
    }

    private void RequestConsentInfoUpdate()
    {
        ConsentRequestParameters requestParameters = new ConsentRequestParameters
        {
            TagForUnderAgeOfConsent = false
        };

        ConsentInformation.Update(requestParameters, (formError) =>
        {
            if (formError != null)
            {
                return;
            }

            if (ConsentInformation.IsConsentFormAvailable())
            {
                LoadConsentForm();
            }
            else
            {
                ConfigureAds();
            }
        });

		Canvas consentCanvas = GameObject.Find("ConsentForm(Clone)")?.GetComponent<Canvas>(); CanvasScaler scaler = consentCanvas.GetComponent<CanvasScaler>(); scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight; scaler.matchWidthOrHeight = 0.6f;
	}

    private void LoadConsentForm()
    {
        ConsentForm.Load((loadedForm, loadError) =>
        {
            if (loadError != null)
            {
				ConfigureAds();
				return;
            }

            consentForm = loadedForm;
			ShowConsentForm();
        });
	}

    private void ShowConsentForm()
    {
        if (consentForm == null)
        {
            return;
        }

        consentForm.Show((showError) =>
        {
            if (showError != null)
            {
                return;
            }

            ConfigureAds();
        });
    }

	private void ConfigureAds()
	{
		RequestConfiguration requestConfiguration = new RequestConfiguration
		{
			TagForUnderAgeOfConsent = TagForUnderAgeOfConsent.False,
			MaxAdContentRating = MaxAdContentRating.G
		};

		if (!IsPersonalizedAdsAllowed())
		{
			requestConfiguration.MaxAdContentRating = MaxAdContentRating.G;
		}

		MobileAds.SetRequestConfiguration(requestConfiguration);
		MobileAds.Initialize((InitializationStatus initStatus) =>
		{

		});
	}

	public bool IsPersonalizedAdsAllowed()
	{
		var consentStatus = ConsentInformation.ConsentStatus;
		return consentStatus == ConsentStatus.Obtained;
	}


    public AdRequest GetAdRequest()
    {
        return new AdRequest();
    }

    public void ResetConsentInformation()
    {
        ConsentInformation.Reset();
        RequestConsentInfoUpdate();
    }

	public void ShowConsentFormAgain()
	{
		ConsentForm.Load((loadedForm, loadError) =>
		{
			if (loadError != null)
			{
				Debug.LogError("Nepodařilo se načíst formulář: " + loadError.Message);
				return;
			}

			consentForm = loadedForm;

			if (consentForm != null)
			{
				consentForm.Show((showError) =>
				{
					if (showError != null)
					{
						Debug.LogError("Chyba při zobrazování formuláře: " + showError.Message);
						return;
					}

					ConfigureAds();
				});
			}
		});

		Canvas consentCanvas = GameObject.Find("ConsentForm(Clone)")?.GetComponent<Canvas>(); CanvasScaler scaler = consentCanvas.GetComponent<CanvasScaler>(); scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight; scaler.matchWidthOrHeight = 0.6f;
	}
}