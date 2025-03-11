using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using Assets.Scripts.Enums;
using UnityEngine.EventSystems;
using Assets.Scripts.Player;

/// <summary>
/// Nejzákladnější kontroller hráče, který slouží pro hlavní úrovně.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController2D : MonoBehaviour
{
	[Header("References")]
	private Animator animator;
	public AudioSource gunAudioSource;
	public AudioSource footStepsAudioSource;
	public AudioClip footstepClip;
	public AudioClip shootClip;
	public AudioClip reloadClip;
	public DeathMenu DeathScreen;
	public Text zobrazovaninaboju;

	[Header("Settings")]
	[SerializeField] private float shootDistance = 10f;
	public float MaxStamina = 100f;
	public int initialRounds = 3;
	public int initialRoundsDeposit = 8;
	[SerializeField] public bool mFacingRight = true;

	[Header("Movement")]
	[SerializeField] private float runSpeed = 20f;
	[SerializeField] private float walkSpeed = 10f;
	[SerializeField] private float slowSpeed = 8f;
	private Rigidbody2D rb;
	private InteractiveObject currentInteractiveObject;
	[NonSerialized] public bool isPlayerVisible = true;
	private bool crouch;
	public bool gunout;
	private bool run;
	private bool reload;
	[SerializeField] private bool isHidden = false;
	private float moveInput;
	[SerializeField] private float speed;
	private int rounds;
	private int roundsDeposit;
	private bool isCanGetUp = true;
	private int speedHash = Animator.StringToHash("Speed");

	[Header("Jump Settings")]
	[SerializeField] private float jumpForce = 3f;
	[SerializeField] private LayerMask groundLayer;

	private Vector2 touchStartPosition;
	private bool isTouching = false;
	private const float touchThreshold = 0.1f;
	private const float touchThresholdDown = 200f;
	[SerializeField] private PlayerInputs controls;
	[SerializeField] private bool iscanMove = true;

	public PlayerState currentState;

	private void Awake()
	{
		controls = InputManager.Instance.Controls;
		rb = GetComponent<Rigidbody2D>();
		rounds = initialRounds;
		roundsDeposit = initialRoundsDeposit;
		currentState = PlayerState.Walking;
		AktualizovatTextovaPole();
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		HandleTouchInput();
		moveInput = Mathf.Clamp(moveInput, -1f, 1f);
		animator.SetFloat(speedHash, Mathf.Abs(moveInput));

		if (Mathf.Abs(moveInput) > 0 && currentState != PlayerState.Crouching && currentState != PlayerState.Running)
		{
			if (!gunout)
			{
				ChangePlayerState(PlayerState.Walking);
			}
		}

		FlipCharacter(moveInput);

		// přesunout na update.
		if (moveInput != 0 && !footStepsAudioSource.isPlaying && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
		{
			PlaySoundFootSteps(footstepClip);
		}
		// když skok nebo se hráč nepohybuje.
		else if (moveInput == 0 || Mathf.Abs(rb.linearVelocity.y) >= 0.01f)
		{
			StopSoundFootSteps();
		}
	}

	private void FixedUpdate()
	{
		rb.linearVelocity = new Vector2(moveInput * speed / 10, rb.linearVelocity.y);
	}

	#region Controls
	private void OnEnable()
	{
		controls.Player.Interact.performed += HandleInteraction;
		controls.Player.Crouch.performed += HandleCrouch;
		controls.Player.Reload.performed += HandleReload;
		controls.Player.Shoot.performed += HandleShoot;
		controls.Player.GunOut.performed += HandleGunOut;
		controls.Player.Moving.performed += HandleMoveInput;

		controls.Enable();
	}

	private void OnDisable()
	{
		controls.Player.Interact.performed -= HandleInteraction;
		controls.Player.Crouch.performed -= HandleCrouch;
		controls.Player.Reload.performed -= HandleReload;
		controls.Player.Shoot.performed -= HandleShoot;
		controls.Player.GunOut.performed -= HandleGunOut;
		controls.Player.Moving.performed -= HandleMoveInput;

		controls.Disable();
	}

	private void OnDestroy()
	{
		if (controls != null)
		{
			controls.Player.Interact.performed -= HandleInteraction;
			controls.Player.Crouch.performed -= HandleCrouch;
			controls.Player.Reload.performed -= HandleReload;
			controls.Player.Shoot.performed -= HandleShoot;
			controls.Player.GunOut.performed -= HandleGunOut;
			controls.Player.Moving.performed -= HandleMoveInput;

			controls.Disable();
		}
	}

	public void DisableControls()
	{
		controls.Disable();
		iscanMove = false;
		moveInput = 0;
		zobrazovaninaboju.gameObject.SetActive(false);

#if PLATFORM_ANDROID
		FindAnyObjectByType<MobileInputsUI>().gameObject.SetActive(false);
#endif
	}

	public void EnableControls()
	{
		controls.Enable();
		iscanMove = true;
		zobrazovaninaboju.gameObject.SetActive(true);

#if PLATFORM_ANDROID
		GameObject parent = GameObject.Find("Canvas");
		if (parent != null)
		{
			MobileInputsUI mobileInputsUI = parent.GetComponentInChildren<MobileInputsUI>(true);
			if (mobileInputsUI != null)
			{
				mobileInputsUI.gameObject.SetActive(true);
			}
		}
#endif
	}
	#endregion

	private void HandleTouchInput()
	{
		// Tuhle část skrz možnout budoucí složitosti jsem se rozhodl řešit kódově bez newinputsystemu za pomocí souboru nebo za ui kliknutí.
		if (TouchInput() && iscanMove)
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

				float deltaX = touchDelta.x * 3;
				float deltaY = touchDelta.y;

				if (Mathf.Abs(deltaX) > touchThreshold)
				{
					moveInput = Mathf.Clamp(deltaX / Screen.width * 2.5f, -1f, 1f);
					HandleRunAndWalk(moveInput);
				}

				if (Mathf.Abs(deltaY) > touchThresholdDown)
				{
					HandleCrouchAndStand(deltaY);
				}
			}

			else if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended || touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Canceled)
			{
				isTouching = false;
				moveInput = 0f;
			}
		}
	}

	private void HandleRunAndWalk(float moveInput)
	{
		if (PlayerState.Crouching == currentState)
		{
			return;
		}

		else if (Mathf.Abs(moveInput) > 0.3f)
		{
			ChangePlayerState(PlayerState.Running);

			animator.SetBool("Run", true);
		}

		else if (Mathf.Abs(moveInput) > 0.1f)
		{
			ChangePlayerState(PlayerState.Walking);
			animator.SetBool("Run", false);
		}
	}

	public bool TouchInput()
	{
		if (EventSystem.current.IsPointerOverGameObject())
		{
			return false;
		}

		return Touchscreen.current != null;
	}

	private void HandleCrouchAndStand(float crouchInput)
	{
		if (isCanGetUp)
		{
			if (crouchInput < 0.1f)
			{
				animator.SetBool("Crouch", true);
				crouch = true;
				ChangePlayerState(PlayerState.Crouching);

			}
			else if (crouchInput > -0.1f)
			{
				animator.SetBool("Crouch", false);
				crouch = false;
				ChangePlayerState(PlayerState.Walking);
			}
		}
	}

	private void HandleMoveInput(InputAction.CallbackContext context)
	{
		Vector2 input = context.ReadValue<Vector2>();
		moveInput = input.x;

		if (moveInput > 0 && !mFacingRight)
		{
			Flip();
		}
		else if (moveInput < 0 && mFacingRight)
		{
			Flip();
		}
	}

	private void HandleCrouch(InputAction.CallbackContext context)
	{
		if (isHidden)
		{
			isPlayerVisible = !isPlayerVisible;
		}

		ChangePlayerState(currentState == PlayerState.Crouching ? (run ? PlayerState.Running : PlayerState.Walking) : PlayerState.Crouching);

		ToggleAnimationState(ref crouch, "Crouch");
	}

	private void HandleShoot(InputAction.CallbackContext context)
	{
		if (gunout && rounds > 0 && moveInput == 0)
		{
			animator.SetBool("Shot", true);
		}
	}

	private void HandleGunOut(InputAction.CallbackContext context)
	{
		ToggleAnimationState(ref gunout, "GunOut");
	}

	private void HandleRun()
	{
		ChangePlayerState(currentState == PlayerState.Walking ? PlayerState.Running : PlayerState.Walking);

		ToggleAnimationState(ref run, "Run");
	}

	private void HandleReload(InputAction.CallbackContext context)
	{
		if (gunout)
		{
			Reload();
		}
	}

	private void HandleInteraction(InputAction.CallbackContext context)
	{
		if (currentInteractiveObject != null)
		{
			currentInteractiveObject.SetInteractionKeyState(true);
			currentInteractiveObject.Interact();
		}
	}

	private void ChangePlayerState(PlayerState newState)
	{
		currentState = newState;

		switch (currentState)
		{
			case PlayerState.Walking:
				WalkingMode();
				break;
			case PlayerState.Running:
				RunMode();
				break;
			case PlayerState.Crouching:
				SlowMode();
				break;
			default:
				WalkingMode();
				break;
		}
	}

	#region Fliping
	private void FlipCharacter(float moveDirection)
	{
		if ((moveDirection > 0 && !mFacingRight) || (moveDirection < 0 && mFacingRight))
		{
			Flip();
		}
	}

	private void Flip()
	{
		mFacingRight = !mFacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	#endregion
	private void ToggleAnimationState(ref bool state, string animation)
	{
		state ^= true;
		animator.SetBool(animation, state);
	}

	#region SpeedChanging
	private void SlowMode()
	{
		speed = slowSpeed;
	}

	private void NoMoving()
	{
		speed = 0;
	}

	private void RunMode()
	{
		speed = runSpeed;
	}

	private void WalkingMode()
	{
		speed = walkSpeed;
	}
	#endregion
	#region Shooting
	private void AktualizovatTextovaPole()
	{
		zobrazovaninaboju.text = rounds.ToString() + " / " + roundsDeposit.ToString();
	}

	public void Shoot()
	{
		gunAudioSource.clip = shootClip;
		gunAudioSource.Play();
		rounds -= 1;

		Vector2 shootDirection = mFacingRight ? Vector2.right : Vector2.left;

		int layerMask = ~(1 << LayerMask.NameToLayer("Player")) & ~(1 << LayerMask.NameToLayer("Barier"));

		RaycastHit2D hit = Physics2D.Raycast(transform.position, shootDirection, shootDistance, layerMask);

		if (hit.collider != null)
		{
			if (hit.collider.CompareTag("Enemy"))
			{
				EnemyBase enemy = hit.collider.GetComponent<EnemyBase>();
				if (enemy != null)
				{
					enemy.TakeDamage(1);
				}
			}
		}

		animator.SetBool("Shot", false);

		AktualizovatTextovaPole();
	}

	public void Reload()
	{
		if (rounds < 3 && roundsDeposit > 0)
		{
			int neededRounds = 3 - rounds;
			int roundsToLoad = Mathf.Min(neededRounds, roundsDeposit);

			rounds += roundsToLoad;
			roundsDeposit -= roundsToLoad;

			gunAudioSource.clip = reloadClip;
			gunAudioSource.Play();
			AktualizovatTextovaPole();
		}
		ToggleAnimationState(ref reload, "Reload");
	}


	#endregion
	#region Triggers
	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.CompareTag("Interactive"))
		{
			currentInteractiveObject = collider.gameObject.GetComponent<InteractiveObject>();
		}
		if (collider.CompareTag("Schovavani"))
		{
			isHidden = true;
			if (crouch)
			{
				isPlayerVisible = false;
			}
		}
		else if (collider.gameObject.CompareTag("Obstacle"))
		{
			HandlePlayerDeath();
		}
	}

	private void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.CompareTag("Schovavani"))
		{
			isHidden = false;
			if (crouch)
			{
				isPlayerVisible = true;
			}
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Enemy"))
		{
			EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
			if (enemy.isAttack)
			{
				HandlePlayerDeath();
			}
		}

		if (collision.CompareTag("Schovavani"))
		{
			if (crouch)
			{
				isPlayerVisible = false;
			}
			else
			{
				isPlayerVisible = true;
			}
		}
	}
	#endregion
	public void HandlePlayerDeath()
	{
		DeathScreen.gameObject.SetActive(true);
	}

	public bool IsPlayerVisible()
	{
		return isPlayerVisible;
	}

	public void CanGetUp()
	{
		isCanGetUp = true;

		if (crouch == true)
		{
			ToggleAnimationState(ref crouch, "Crouch");
		}
	}

	public void GetDown()
	{
		isCanGetUp = false;
		ChangePlayerState(PlayerState.Crouching);
		if (crouch == false)
		{
			ToggleAnimationState(ref crouch, "Crouch");
		}
	}

	#region Sounds
	private void PlaySoundFootSteps(AudioClip clip)
	{
		footStepsAudioSource.clip = clip;
		footStepsAudioSource.Play();
	}

	private void StopSoundFootSteps()
	{
		footStepsAudioSource.Stop();
	}

	#endregion

	public void PerformJump()
	{
		rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
	}

	private void OnDrawGizmos()
	{
		Vector2 shootDirection = mFacingRight ? Vector2.right : Vector2.left;
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, shootDirection * shootDistance);
	}

	/// <summary>
	/// Deaktivování světla u hráče.
	/// </summary>
	/// todo opravit¨, co jsem tím myslel?
	public void DeactivateLight(GameObject player)
	{
		Light playerLight = gameObject.GetComponentInChildren<Light>();
		if (playerLight != null)
		{
			playerLight.enabled = false;
		}
	}

	#region Tutorial
	public int GetRounds()
	{
		return rounds;
	}

	public float GetPlayerInput()
	{
		return moveInput;
	}

	public bool IsPlayerCrouch()
	{
		return crouch;
	}

	public int GetNumberOfGunReposit()
	{
		return roundsDeposit;
	}
	#endregion
}