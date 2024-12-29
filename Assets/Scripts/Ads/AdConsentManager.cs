using UnityEngine;
using GoogleMobileAds.Ump.Api;
using UnityEngine.UI;

public class AdConsentManager : MonoBehaviour
{
    private ConsentForm consentForm;

    public void Start()
    {
        RequestConsentInfoUpdate();
        Canvas consentCanvas = GameObject.Find("ConsentForm(Clone)")?.GetComponent<Canvas>();
        CanvasScaler scaler = consentCanvas.GetComponent<CanvasScaler>();
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        return;
    }

    private void RequestConsentInfoUpdate()
    {
        // Vytvoří parametry pro požadavek na informace o souhlasu.
        ConsentRequestParameters requestParameters = new ConsentRequestParameters
        {
            TagForUnderAgeOfConsent = false // Změňte na true, pokud cílíte na děti.
        };

        // Aktualizuje informace o souhlasu.
        ConsentInformation.Update(requestParameters, (formError) =>
        {
            if (formError != null)
            {
                Debug.LogError("Error updating consent info: " + formError.Message);
                return;
            }

            // Zkontroluje, zda je potřeba zobrazit formulář souhlasu.
            if (ConsentInformation.IsConsentFormAvailable())
            {
                LoadConsentForm();
            }
        });
    }

    private void LoadConsentForm()
    {
        // Načte formulář souhlasu.
        ConsentForm.Load((loadedForm, loadError) =>
        {
            if (loadError != null)
            {
                Debug.LogError("Error loading consent form: " + loadError.Message);
                return;
            }

            consentForm = loadedForm;

            // Zobrazí formulář souhlasu, pokud je k dispozici.
            ShowConsentForm();
        });
    }

    private void ShowConsentForm()
    {
        if (consentForm == null)
        {
            Debug.LogError("Consent form is not loaded.");
            return;
        }

        consentForm.Show((showError) =>
        {
            if (showError != null)
            {
                Debug.LogError("Error showing consent form: " + showError.Message);
                return;
            }

            // Po zobrazení formuláře se chování aplikace může změnit na základě preferencí uživatele.
            HandleConsentDecision();
        });
    }

    private void HandleConsentDecision()
    {
        if (ConsentInformation.ConsentStatus == ConsentStatus.Required)
        {
            Debug.Log("Consent is required but not given.");
        }
        else if (ConsentInformation.ConsentStatus == ConsentStatus.Obtained)
        {
            Debug.Log("User has given consent.");
        }
        else if (ConsentInformation.ConsentStatus == ConsentStatus.NotRequired)
        {
            Debug.Log("Consent is not required.");
        }
    }

    public void ResetConsent()
    {
        // Resetuje informace o souhlasu (např. pro testování).
        ConsentInformation.Reset();
        Debug.Log("Consent information reset.");
    }
}
