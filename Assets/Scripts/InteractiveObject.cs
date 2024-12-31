using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class InteractiveObject : MonoBehaviour
{
	public Animator objectAnimator;
	[SerializeField] private bool isPlayerInRange;
	[SerializeField] private bool interactionKeyPressed;
	public UnityEvent onInteract;
	private static readonly int Clicken = Animator.StringToHash("Clicken");
	[SerializeField] private bool interactOnlyOnce = false;
	private bool hasInteracted = false;
	public GameObject showImage;
	private AudioSource audioSource;
	public AudioClip clip;
	private BoxCollider2D boxcollider2D;

	public void EndMinigameInteractive()
	{
		interactOnlyOnce = true;
		showImage.SetActive(false);
		boxcollider2D.enabled = false;

	}

	public void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		boxcollider2D = GetComponent<BoxCollider2D>();

	}

	public void Interact()
	{
		if (isPlayerInRange)
		{

			// Trigger the animation.
			if (objectAnimator != null)
			{
				objectAnimator.SetBool(Clicken, true);
			}
			// Mark interaction as happened if it's a one-time interaction.
			if (interactOnlyOnce)
			{
				boxcollider2D.enabled = false;
				Destroy(showImage);
			}
			// Invoke the UnityEvent.
			onInteract?.Invoke();

		}
	}

	/// <summary>
	/// Change state.
	/// </summary>
	/// <param name="state"></param>
	public void SetInteractionKeyState(bool state)
	{
		interactionKeyPressed = state;
	}

	/// <summary>
	/// Player is in range.
	/// </summary>
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			isPlayerInRange = true;
			if (showImage != null)
			{
				showImage.SetActive(true);

				audioSource.clip = clip;
				audioSource.Play();
			}
		}
	}

	/// <summary>
	/// Player is not in range.
	/// </summary>
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			isPlayerInRange = false;
			if (showImage != null)
			{
				showImage.SetActive(false);
			}
		}
	}

	/// <summary>
	/// Resets the interaction, allowing it to be triggered again if interactOnlyOnce is true.
	/// </summary>
	public void ResetInteraction()
	{
		hasInteracted = false;

		if (objectAnimator != null)
		{
			objectAnimator.SetBool(Clicken, false);
		}
	}
}
