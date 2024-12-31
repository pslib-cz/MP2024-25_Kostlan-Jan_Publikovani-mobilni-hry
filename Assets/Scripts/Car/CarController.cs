using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
	public float speed;
	private Rigidbody2D rb;
	private Animator animator;
	[SerializeField] private float forwardSpeed = 10f;
	[SerializeField] private float reverseSpeed = 3f;
	[SerializeField] private float touchThreshold = 50f;
	[SerializeField] private PlayerInputs controls;
	[SerializeField] private PlayerController2D playerController2D;
	[SerializeField] private GameObject cineMachineCamera;
	[SerializeField] private GameObject imageCar;
	private Vector2 touchStartPosition;
	private bool isTouching = false;
	private float moveInput = 0f;

	private void Awake()
	{
		controls = new PlayerInputs();
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

	private void OnEnable()
	{
		controls.Player.Interact.performed += HandleInteraction;
		controls.Enable();
	}

	private void OnDisable()
	{
		controls.Player.Interact.performed -= HandleInteraction;
		controls.Disable();
	}

	private void Update()
	{
		Move();
	}

	public void Move()
	{
		if (TouchInput())
		{
			var touch = Touchscreen.current.primaryTouch;

			if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
			{
				touchStartPosition = touch.position.ReadValue();
				isTouching = true;
			}
			else if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved && isTouching)
			{
				Vector2 touchDelta = touch.position.ReadValue() - touchStartPosition;

				float deltaX = touchDelta.x;
				float deltaY = touchDelta.y;

				if (Mathf.Abs(deltaX) > touchThreshold)
				{
					moveInput = Mathf.Clamp(deltaX / Screen.width * 2f, -1f, 1f);
					HandleRunAndWalk(moveInput);
				}
			}
			else if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended || touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Canceled)
			{
				isTouching = false;
				moveInput = 0f;
			}
		}
	}

	private bool TouchInput()
	{
		return Touchscreen.current != null;
	}

	private void HandleRunAndWalk(float moveInput)
	{

		if (moveInput > 0)
		{
			speed = forwardSpeed * moveInput;
		}
		else
		{
			speed = reverseSpeed * moveInput;
		}

		rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);

		if (animator != null)
		{
			animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
		}
	}

	private void HandleInteraction(InputAction.CallbackContext context)
	{
		playerController2D.gameObject.transform.position = gameObject.transform.position;

		gameObject.SetActive(false);

		playerController2D.gameObject.SetActive(true);
		imageCar.gameObject.transform.position = gameObject.transform.position;
		imageCar.SetActive(true);

		cineMachineCamera.SetActive(false);
	}

	public void EndGame()
	{
		playerController2D.HandlePlayerDeath();
	}
}