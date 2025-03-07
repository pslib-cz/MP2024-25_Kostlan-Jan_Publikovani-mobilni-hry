using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingsMenu : MonoBehaviour
{

	public Image panel; // Odkaz na panel, který chcete změnit barvu
	public Color fullscreenColor; // Barva pro režim celé obrazovky
	public Color windowedColor; // Barva pro režim okna

	public Dropdown resolutionDropdown;
	public Resolution[] resolutions;
	public Scrollbar musicVolumeSlider;
	public Scrollbar sfxVolumeSlider;


	void Awake()
	{
		resolutions = Screen.resolutions;
		resolutionDropdown.ClearOptions();
		List<string> options = new List<string>();

		int currentResolutionIndex = 0;
		for (int i = 0; i < resolutions.Length; i++)
		{
			string option = resolutions[i].width + " x " + resolutions[i].height;
			options.Add(option);

			if (resolutions[i].width == Screen.currentResolution.width &&
				resolutions[i].height == Screen.currentResolution.height)
			{
				currentResolutionIndex = i;
			}
		}
		resolutionDropdown.AddOptions(options);
		resolutionDropdown.value = PlayerPrefs.GetInt(PlayerPrefsKeys.Resolution, currentResolutionIndex); // Načteme uložené rozlišení
		resolutionDropdown.RefreshShownValue();
	}

	private void Start()
	{
		if (AudioManager.instance != null)
		{
			musicVolumeSlider.value = PlayerPrefs.GetFloat(PlayerPrefsKeys.MusicVolume, AudioManager.instance.defaultVolume);
			sfxVolumeSlider.value = PlayerPrefs.GetFloat(PlayerPrefsKeys.SFXVolume, AudioManager.instance.defaultVolume);
		}
	}

	public void SetMusicVolume()
	{
		Debug.Log("nastavení zvuku");
		float volume = musicVolumeSlider.value;
		AudioManager.instance.SetMusicVolume(volume);
	}

	public void SetSfxVolume()
	{
		Debug.Log("nastavení zvuku");
		float volume = sfxVolumeSlider.value;
		AudioManager.instance.SetSFXVolume(volume);
	}

	public void SetResolution(int resolutionIndex)
	{
		Resolution resolution = resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

		// Uložíme nastavení rozlišení
		PlayerPrefs.SetInt(PlayerPrefsKeys.Resolution, resolutionIndex);
	}

	public void SetQuality(int qualityIndex)
	{
		QualitySettings.SetQualityLevel(qualityIndex);

		// Uložíme nastavení kvality
		PlayerPrefs.SetInt(PlayerPrefsKeys.Quality, qualityIndex);
	}

	public void SetFullscreen(bool isFullscreen)
	{
		Debug.Log("změna na fullscreen nebo naopak?");
		Screen.fullScreen = isFullscreen;

		// Změňte barvu panelu
		if (panel != null)
		{
			panel.color = isFullscreen ? fullscreenColor : windowedColor;
		}

		// Uložíme nastavení režimu celé obrazovky
		PlayerPrefs.SetInt(PlayerPrefsKeys.FullScreen, isFullscreen ? 1 : 0);
	}
}