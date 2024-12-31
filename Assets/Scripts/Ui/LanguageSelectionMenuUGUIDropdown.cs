using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class LanguageSelectionMenuUGUIDropdown : MonoBehaviour
{
	Dropdown m_Dropdown;
	AsyncOperationHandle m_InitializeOperation;

	private static readonly string LanguageKey = "Language";

	void Start()
	{
		m_Dropdown = GetComponent<Dropdown>();
		m_Dropdown.onValueChanged.AddListener(OnSelectionChanged);

		m_Dropdown.ClearOptions();
		m_Dropdown.options.Add(new Dropdown.OptionData("Loading..."));
		m_Dropdown.interactable = false;

		m_InitializeOperation = LocalizationSettings.SelectedLocaleAsync;
		if (m_InitializeOperation.IsDone)
		{
			InitializeCompleted(m_InitializeOperation);
		}
		else
		{
			m_InitializeOperation.Completed += InitializeCompleted;
		}
	}

	void InitializeCompleted(AsyncOperationHandle obj)
	{
		var options = new List<string>();
		int selectedOption = 0;
		var locales = LocalizationSettings.AvailableLocales.Locales;
		for (int i = 0; i < locales.Count; ++i)
		{
			var displayName = locales[i].Identifier.CultureInfo != null ? locales[i].Identifier.CultureInfo.NativeName : locales[i].ToString();
			options.Add(displayName);
		}

		if (options.Count == 0)
		{
			options.Add("No Locales Available");
			m_Dropdown.interactable = false;
		}
		else
		{
			m_Dropdown.interactable = true;
		}

		m_Dropdown.ClearOptions();
		m_Dropdown.AddOptions(options);

		// Načteme uložený jazyk
		var savedLanguage = PlayerPrefs.GetString(LanguageKey, null);
		if (!string.IsNullOrEmpty(savedLanguage))
		{
			var savedLocale = LocalizationSettings.AvailableLocales.GetLocale(savedLanguage);
			if (savedLocale != null)
			{
				LocalizationSettings.SelectedLocale = savedLocale;
				selectedOption = LocalizationSettings.AvailableLocales.Locales.IndexOf(savedLocale);
			}
		}
		else
		{
			// Pokud není uložený jazyk, zkontrolujeme zemi a nastavíme výchozí jazyk
			var currentCountry = new RegionInfo(CultureInfo.CurrentCulture.LCID).TwoLetterISORegionName;
			if (currentCountry == "CZ")
			{
				var czechLocale = LocalizationSettings.AvailableLocales.GetLocale("cs");
				if (czechLocale != null)
				{
					LocalizationSettings.SelectedLocale = czechLocale;
					selectedOption = LocalizationSettings.AvailableLocales.Locales.IndexOf(czechLocale);
				}
			}
			else
			{
				var englishLocale = LocalizationSettings.AvailableLocales.GetLocale("en");
				if (englishLocale != null)
				{
					LocalizationSettings.SelectedLocale = englishLocale;
					selectedOption = LocalizationSettings.AvailableLocales.Locales.IndexOf(englishLocale);
				}
			}
		}

		m_Dropdown.SetValueWithoutNotify(selectedOption);

		LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
	}

	void OnSelectionChanged(int index)
	{
		LocalizationSettings.SelectedLocaleChanged -= LocalizationSettings_SelectedLocaleChanged;

		var locale = LocalizationSettings.AvailableLocales.Locales[index];
		LocalizationSettings.SelectedLocale = locale;

		// Uložíme nastavení jazyka
		PlayerPrefs.SetString(LanguageKey, locale.Identifier.Code);

		LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
	}

	void LocalizationSettings_SelectedLocaleChanged(Locale locale)
	{
		var selectedIndex = LocalizationSettings.AvailableLocales.Locales.IndexOf(locale);
		m_Dropdown.SetValueWithoutNotify(selectedIndex);
	}
}