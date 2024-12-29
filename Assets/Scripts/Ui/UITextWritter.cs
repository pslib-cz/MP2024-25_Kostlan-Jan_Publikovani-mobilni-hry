using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UITextWritter : MonoBehaviour
{
	public Text txt;
	public AudioSource typingAudioSource;
	public AudioClip typingSound;
	private string story;
	private float initialDelay;
	private float characterDelay;
	private Coroutine currentCoroutine;

	public void ChangeText(string _text, float _initialDelay = 0f, float _characterDelay = 0.05f)
	{
		txt.text = "";
		if (currentCoroutine != null)
		{
			StopCoroutine(currentCoroutine);
		}

		story = _text;
		initialDelay = _initialDelay;
		characterDelay = _characterDelay;

		txt.text = ""; // Ensure the text is cleared before starting a new text animation
		currentCoroutine = StartCoroutine(PlayText());
	}

	IEnumerator PlayText()
	{
		yield return new WaitForSeconds(initialDelay);

		// Start playing the typing sound in a loop
		PlayTypingSound();

		for (int i = 0; i < story.Length; i++)
		{
			txt.text += story[i];
			yield return new WaitForSeconds(characterDelay);
		}

		StopTypingSound();

		currentCoroutine = null;
	}

	private void PlayTypingSound()
	{
		if (typingAudioSource != null && typingSound != null)
		{
			typingAudioSource.clip = typingSound;
			typingAudioSource.loop = true;
			typingAudioSource.pitch = 0.5f / characterDelay;
			typingAudioSource.volume = typingAudioSource.volume / 4f; 
			typingAudioSource.Play();
		}
	}

	private void StopTypingSound()
	{
		if (typingAudioSource != null)
		{
			typingAudioSource.Stop();
		}
	}
}
