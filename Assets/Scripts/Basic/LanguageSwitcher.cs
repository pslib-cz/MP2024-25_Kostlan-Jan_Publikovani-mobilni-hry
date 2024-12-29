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
		/// Mění jazyk podle indexu.
		/// </summary>
		public void ChangeLanguage(int addIndex)
		{
			int currentIndex = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);

			int newIndex = (currentIndex + addIndex) % LocalizationSettings.AvailableLocales.Locales.Count;
			Locale newLocale = LocalizationSettings.AvailableLocales.Locales[newIndex];
			LocalizationSettings.SelectedLocale = newLocale;
			Debug.Log("Language changed to: " + newLocale.ToString());
		}
	}
}