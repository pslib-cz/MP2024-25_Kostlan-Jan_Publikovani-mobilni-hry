using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Audio
{
	/// <summary>
	/// Poustí zvuk při kliknutí na tlačítko.
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	[RequireComponent(typeof(Button))]
	public class ButtonAudioPlayer : MonoBehaviour
	{
		private AudioSource audioSource;
		private Button button;
		private void Awake()
		{
			audioSource = GetComponent<AudioSource>();
			button = GetComponent<Button>();
			button.onClick.AddListener(PlayAudio);
		}

		private void PlayAudio()
		{
			audioSource.Play();
		}
	}
}