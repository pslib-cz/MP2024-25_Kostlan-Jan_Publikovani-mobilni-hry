using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class Lockpick : MonoBehaviour
{
	public float moveSpeed = 5f;
	public LayerMask pinLayer;
	public Transform child;
	public float pickWidth = 1f;
	public float castRadius = 5F;
	private PlayerInputs controls;
	private Vector3 originalPosition;
	private bool pinDetected = false;
	private Transform lastDetectedPin;
	private LockController lockController;
	private AudioSource audioSource;

	void Awake()
	{
		controls = new PlayerInputs();
		lockController = FindFirstObjectByType<LockController>();
		audioSource = GetComponent<AudioSource>();
	}

	void OnEnable()
	{
		controls.Minigames.ClickLockPick.performed += HandleInteraction;
		controls.Enable();
	}

	void OnDisable()
	{
		controls.Minigames.ClickLockPick.performed -= HandleInteraction;
		controls.Disable();
	}

	void Start()
	{
		originalPosition = transform.position;
	}

	void Update()
	{
		transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

		RaycastHit2D hit = Physics2D.CircleCast(child.position, castRadius, Vector2.up);

		if (hit.collider != null)
		{
			Pin pin = hit.collider.GetComponent<Pin>();
			if (pin != null && !pin.isPicked)
			{
				pinDetected = true;
				lastDetectedPin = hit.collider.transform;
			}
		}

		CheckForMissedPin();
	}

	void HandleInteraction(InputAction.CallbackContext context)
	{
		if (pinDetected && lastDetectedPin != null)
		{
			Pin pin = lastDetectedPin.GetComponent<Pin>();
			pin.PinUp();
			pinDetected = false;
			audioSource.Play();
			lockController.AreAllPinsPicked();
		}
		else
		{
			ResetGame();
		}
	}

	void CheckForMissedPin()
	{
		if (pinDetected && child.position.x > (lastDetectedPin.position.x + (castRadius * 10)))
		{
			ResetGame();
		}
	}

	void ResetGame()
	{
		Handheld.Vibrate();
		transform.position = originalPosition;
		pinDetected = false;
		lastDetectedPin = null;
		FindFirstObjectByType<LockController>().ResetPins();
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Vector3 direction = Vector3.up * 10f;
		Gizmos.DrawWireSphere(child.position, castRadius);
		Gizmos.DrawWireSphere(child.position + direction, castRadius);
		Gizmos.DrawLine(child.position, child.position + direction);
	}
}