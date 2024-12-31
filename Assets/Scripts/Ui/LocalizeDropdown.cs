using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

[RequireComponent(typeof(Dropdown))]
[AddComponentMenu("Localization/Localize Dropdown")]
public class LocalizeDropdown : MonoBehaviour
{
	// todo opravit tuto chybu skrz jazyky, když někdo klikne nastavení, nastaví se mu jazyk čeština.
	[Serializable]
	public class LocalizedDropdownOption
	{
		public LocalizedString text;
	}

	public List<LocalizedDropdownOption> options;
	public int selectedOptionIndex;
	private Locale currentLocale;
	private Dropdown Dropdown => GetComponent<Dropdown>();

	private void Start()
	{
		getLocale();
		UpdateDropdown(currentLocale);
	}

	private void OnEnable() => LocalizationSettings.SelectedLocaleChanged += UpdateDropdown;
	private void OnDisable() => LocalizationSettings.SelectedLocaleChanged -= UpdateDropdown;
	void OnDestroy() => LocalizationSettings.SelectedLocaleChanged -= UpdateDropdown;

	private void getLocale()
	{
		var locale = LocalizationSettings.SelectedLocale;
		if (currentLocale != null && locale != currentLocale)
		{
			currentLocale = locale;
		}
	}

	private void UpdateDropdown(Locale locale)
	{
		selectedOptionIndex = Dropdown.value;
		Dropdown.ClearOptions();

		for (int i = 0; i < options.Count; i++)
		{
			String localizedText = options[i].text.GetLocalizedString();
			Dropdown.options.Add(new Dropdown.OptionData(localizedText));
		}

		Dropdown.value = selectedOptionIndex;
		Dropdown.RefreshShownValue();
	}
}