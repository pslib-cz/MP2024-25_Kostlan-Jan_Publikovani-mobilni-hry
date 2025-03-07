using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace UVH.Localization
{
	/// <summary>
	/// Měnič jazyku na kliknutí.
	/// </summary>
	public class LanguageSwitcher : MonoBehaviour
	{
		/// <summary>
		/// Mění jazyk podle indexu a ukládá ho do PlayerPrefs.
		/// </summary>
		public void ChangeLanguage(int addIndex)
		{
			int currentIndex = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);
			int newIndex = (currentIndex + addIndex) % LocalizationSettings.AvailableLocales.Locales.Count;

			if (newIndex < 0)
			{
				newIndex += LocalizationSettings.AvailableLocales.Locales.Count;
			}

			Locale newLocale = LocalizationSettings.AvailableLocales.Locales[newIndex];
			LocalizationSettings.SelectedLocale = newLocale;

			// Uložení nového jazyka do PlayerPrefs
			PlayerPrefs.SetString(PlayerPrefsKeys.Language, newLocale.Identifier.Code);
			PlayerPrefs.Save();

			Debug.Log("Language changed to: " + newLocale.Identifier.Code);
		}
	}
}
