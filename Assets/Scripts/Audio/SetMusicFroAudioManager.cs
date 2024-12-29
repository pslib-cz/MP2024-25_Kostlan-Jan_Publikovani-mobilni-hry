using UnityEngine;

namespace Assets.Scripts.Audio
{
    /// <summary>
    /// Mění hudbu pro AudioManger při inicializaci.
    /// </summary>
    public class SetMusicFroAudioManager: MonoBehaviour
    {
        private AudioManager audioManager;
        public AudioClip audioClip;
        public void Start()
        {
#if UNITY_EDITOR
            if (FindObjectOfType<AudioManager>() == null)
            {
                GameObject audioManager = new GameObject("AudioManager");
            }
#endif
            audioManager = FindObjectOfType<AudioManager>();
            audioManager.ChangeMusic(audioClip);
        }
    }
}