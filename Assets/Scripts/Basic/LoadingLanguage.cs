using UnityEngine;
using UnityEngine.Localization.Settings;

public static class PreloadLocalization
{
    /// <summary>
    /// Načte jazyk před spuštěním jakékoliv scény.
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitializeBeforeGameStarts()
    {
        var initializationOperation = LocalizationSettings.InitializationOperation;
        initializationOperation.Completed += op => InitializeLanguageSetting();
    }

    /// <summary>
    /// Vezme jazyk z PlayerPrefs a inicializuje daný jazyk.
    /// </summary>
    private static void InitializeLanguageSetting()
    {
        var savedLanguage = PlayerPrefs.GetString(PlayerPrefsKeys.Language, null);
        if (!string.IsNullOrEmpty(savedLanguage))
        {
            var savedLocale = LocalizationSettings.AvailableLocales.GetLocale(savedLanguage);
            if (savedLocale != null)
            {
                LocalizationSettings.SelectedLocale = savedLocale;
            }
            else
            {
                Debug.LogWarning("Uložený jazyk nenalezen: " + savedLanguage);
            }
        }

        var currentLocale = LocalizationSettings.SelectedLocale;
        if (currentLocale != null)
        {
            Debug.Log("Lokalizace před spuštěním hry nastavena na: " + currentLocale.Identifier.Code);
        }
        else
        {
            Debug.LogWarning("Před spuštěním hry není vybrán žádný jazyk.");
        }
    }
}
