using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	[RequireComponent(typeof(AudioSource))]
	public class DoorTelepoted : MonoBehaviour
	{
		PlayerController2D player;
		public DoorTelepoted TeleportTo;
		public AudioClip DoorSoundOpenClose;
		public float timeToChangeToTransparent = 0.8f;
		private AudioSource audioSource;
		private Image image;

		private void Awake()
		{
			player = FindFirstObjectByType<PlayerController2D>();
			audioSource = GetComponent<AudioSource>();

			// Najdeme GameObject "BlackImage" a získáme komponentu Image
			GameObject canvasObject = GameObject.Find("BlackImage");
			if (canvasObject != null)
			{
				image = canvasObject.GetComponent<Image>();
				// Dále můžeš s tímto obrázkem pracovat
			}
			else
			{
				Debug.LogError("BlackImage nebyl nalezen.");
			}
		}

		public void TeloportedPlayer()
		{
			Color newColor = image.color;
			newColor.a = 1f;
			image.color = newColor;
			StartCoroutine(DeactivatedImage());
			audioSource.PlayOneShot(DoorSoundOpenClose);
			player.transform.position = TeleportTo.transform.position;
		}

		/// <summary>
		/// With boxcolliders determine areas to spawn leaves.
		/// </summary>
		private IEnumerator DeactivatedImage()
		{
			yield return new WaitForSeconds(timeToChangeToTransparent);
			Color newColor = image.color;
			newColor.a = 0f;
			image.color = newColor;
		}
	}
}