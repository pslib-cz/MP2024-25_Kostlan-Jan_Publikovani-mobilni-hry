using UnityEngine;
namespace Assets.Scripts.Audio
{
    public class UnityEditorAudioManager: MonoBehaviour
    {

        [SerializeField] private AudioManager audioManager;

        private void Awake()
        {
            if (FindObjectOfType<AudioManager>() == null)
            {
                GameObject audioManager = new GameObject("AudioManager");
            }
        }
    }
}
