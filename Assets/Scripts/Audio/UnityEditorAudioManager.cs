using UnityEngine;
namespace Assets.Scripts.Audio
{
	/// <summary>
	/// Hledá AudioManager.
	/// Todo je to už zbytečné!
	/// </summary>
	public class UnityEditorAudioManager : MonoBehaviour
	{
		[SerializeField] private AudioManager audioManager;

		private void Awake()
		{
			if (FindFirstObjectByType<AudioManager>() == null)
			{
				GameObject audioManager = new GameObject("AudioManager");
			}
		}
	}
}
