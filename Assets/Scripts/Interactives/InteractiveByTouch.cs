using UnityEngine;
using UnityEngine.Events;
namespace Assets.Scripts.Interactives
{
    /// <summary>
    /// Reaguje na kliknutí hráče a spustí dané interakce.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class InteractiveByTouch : MonoBehaviour
    {
        public Animator objectAnimator;
        public UnityEvent onInteract;
        private static readonly int Clicken = Animator.StringToHash("Clicken");
        [SerializeField] private bool interactOnlyOnce = false;
        private bool hasInteracted = false;
        public GameObject showImage;
        private AudioSource audioSource;
        public AudioClip clip;

        public void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void Interact()
        {

            if (interactOnlyOnce && hasInteracted)
            {
                return;
            }

            if (objectAnimator != null)
            {
                objectAnimator.SetBool(Clicken, true);
            }

            onInteract?.Invoke();

            if (interactOnlyOnce)
            {
                hasInteracted = true;
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Interact();
            }
        }

        public void ResetInteraction()
        {
            hasInteracted = false;
            if (objectAnimator != null)
            {
                objectAnimator.SetBool(Clicken, false);
            }
        }
    }
}