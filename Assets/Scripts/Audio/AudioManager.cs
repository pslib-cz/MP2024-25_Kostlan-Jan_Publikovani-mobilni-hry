using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Má informace o audiu, jako je například hlasitost, nastavování hlasitosti a nastavování hudby.
/// </summary>
public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

	public AudioMixer audioMixer;

	public float defaultVolume = 0.7f;

	[Header("MusicSettings")]
	public AudioClip defaultMusicClip;
	public AudioSource musicSource;

	private void Awake()
	{
		musicSource.ignoreListenerPause = true;
	}

	private void Start()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
			LoadVolumeSettings();
		}
		else
		{
			Destroy(gameObject);
		}
		PlayMusic();

	}

	public void SetMusicVolume(float volume)
	{
		volume = Mathf.Clamp(volume, 0.0f, 1.0f);
		float dBValue = GetdBValue(volume);
		audioMixer.SetFloat(PlayerPrefsKeys.MusicVolume, dBValue);
		PlayerPrefs.SetFloat(PlayerPrefsKeys.MusicVolume, volume);
		Debug.Log($"Music Volume set to: {volume}");
	}

	public void SetSFXVolume(float volume)
	{
		volume = Mathf.Clamp(volume, 0.0f, 1.0f);
		float dBValue = GetdBValue(volume);
		audioMixer.SetFloat(PlayerPrefsKeys.SFXVolume, dBValue);
		PlayerPrefs.SetFloat(PlayerPrefsKeys.SFXVolume, volume);
		Debug.Log($"SFX Volume set to: {volume}");
	}

	private float GetdBValue(float volume)
	{
		return Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20;
	}

	private void LoadMusicPlayerPrefs()
	{
		if (PlayerPrefs.HasKey(PlayerPrefsKeys.MusicVolume))
		{
			float musicVolume = PlayerPrefs.GetFloat(PlayerPrefsKeys.MusicVolume, defaultVolume);  // Nastavení výchozí hodnoty
			float dBValue = GetdBValue(musicVolume);
			Debug.Log($"Loaded Music Volume: {musicVolume}, dB: {dBValue}");
			audioMixer.SetFloat(PlayerPrefsKeys.MusicVolume, dBValue);
		}
		else
		{
			SetMusicVolume(defaultVolume);  // Pokud není klíč, nastavíme defaultní hlasitost
		}
	}

	private void LoadSFXPlayerPrefs()
	{
		if (PlayerPrefs.HasKey(PlayerPrefsKeys.SFXVolume))
		{
			float sfxVolume = PlayerPrefs.GetFloat(PlayerPrefsKeys.SFXVolume, defaultVolume);  // Nastavení výchozí hodnoty
			float dBValue = GetdBValue(sfxVolume);
			Debug.Log($"Loaded SFX Volume: {sfxVolume}, dB: {dBValue}");
			audioMixer.SetFloat(PlayerPrefsKeys.SFXVolume, dBValue);
		}
		else
		{
			SetSFXVolume(defaultVolume);  // Pokud není klíč, nastavíme defaultní hlasitost
		}
	}

	private void LoadVolumeSettings()
	{
		LoadMusicPlayerPrefs();
		LoadSFXPlayerPrefs();
	}

	#region MusicSettings

	public void PlayMusic()
	{
		musicSource.clip = defaultMusicClip;
		musicSource.Play();
	}

	public void ChangeMusic(AudioClip musicClip)
	{
		musicSource.clip = musicClip;
		musicSource.Play();
	}

	public void StopMusic()
	{
		musicSource.Stop();
	}

	public void PauseMusic()
	{
		if (musicSource.isPlaying)
		{
			musicSource.Pause();
		}
	}

	public void ContinueMusic()
	{
		if (!musicSource.isPlaying)
		{
			musicSource.UnPause();
		}
	}

	#endregion
}