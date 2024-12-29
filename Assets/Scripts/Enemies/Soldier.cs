using UnityEngine;

/// <summary>
/// voják, který reaguje na stealf systém.
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]

public class Soldier : MonoBehaviour
{
	[Header("Soldier Settings")]
	public PlayerController2D playerController;
	public Animator animator;
	public int x = 9;
	public int shotStyle;
	public bool recharge;
	public bool dead;
	public bool hurt;
	public Transform player;

	[Header("Movement Settings")]
	public float walkSpeed = 1.0f;
	public float runSpeed = 3.0f;
	public float idleTime = 2.0f;
	public float lookRange = 10f;
	public float attackRange = 8f;

	[Header("Sound Settings")]
	public AudioClip firingSound;
	public AudioClip deathSound;
	public AudioClip walkingSound;
	public AudioClip runningSound;

	private SpriteRenderer spriteRenderer;
	private Vector2 direction;
	private float nextActionTime;

	private AudioSource firingAudioSource;
	private AudioSource movementAudioSource;
	private bool isShootingAnimationPlaying = false;
	private bool mFacingRight;

	private bool isAlerted;

	private enum State
	{
		Idle,
		Walking,
		Running,
		Firing
	}

	private State state = State.Idle;

	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		shotStyle = Random.Range(0, x);
		nextActionTime = Time.time + idleTime;
		direction = Vector2.right;
		spriteRenderer.flipX = direction.x < 0;
		mFacingRight = direction.x > 0;

		// Initialize AudioSources
		firingAudioSource = gameObject.AddComponent<AudioSource>();
		firingAudioSource.clip = firingSound;
		firingAudioSource.spatialBlend = 1.0f;
		firingAudioSource.volume = 0.5f;

		movementAudioSource = gameObject.AddComponent<AudioSource>();
		movementAudioSource.spatialBlend = 1.0f;
		movementAudioSource.volume = 0.5f;
	}

	void Update()
	{
		if (dead)
		{
			animator.SetBool("Dead", true);
			return;
		}

		animator.SetBool("Dead", false);

		if (player != null && playerController.isPlayerVisible || isAlerted)
		{
			Vector2 playerDirection = (player.position - transform.position).normalized;

			if (Vector2.Distance(transform.position, player.position) <= lookRange &&
				(mFacingRight && playerDirection.x < 0 || !mFacingRight && playerDirection.x > 0) || isAlerted)
			{
				isAlerted = true;
				state = State.Running;

				if (Vector2.Distance(transform.position, player.position) <= attackRange)
				{
					state = State.Firing;
					if (isShootingAnimationPlaying)
					{
						playerController.HandlePlayerDeath();
					}
				}
			}
			else if (Time.time >= nextActionTime)
			{
				state = (state == State.Idle) ? State.Walking : State.Idle;
				nextActionTime = Time.time + idleTime;

				if (state == State.Walking)
				{
					direction = Random.value < 0.3 ? Vector2.left : Vector2.right;
				}
			}
		}
		else if (Time.time >= nextActionTime)
		{
			state = (state == State.Idle) ? State.Walking : State.Idle;
			nextActionTime = Time.time + idleTime;

			if (state == State.Walking)
			{
				direction = Random.value < 0.7 ? Vector2.left : Vector2.right;
			}
		}

		switch (state)
		{
			case State.Idle:
				animator.SetBool("Run", false);
				animator.SetBool("Walk", false);
				StopMovementSound();
				break;
			case State.Walking:
				animator.SetBool("Run", false);
				animator.SetBool("Walk", true);
				Patrol(walkSpeed);
				PlayMovementSound(walkingSound);
				break;
			case State.Running:
				animator.SetBool("Run", true);
				animator.SetBool("Walk", false);
				ChasePlayer(runSpeed);
				PlayMovementSound(runningSound);
				break;
			case State.Firing:
				animator.SetBool("Run", false);
				animator.SetBool("Walk", false);
				ShotStyle();
				animator.SetInteger("ShotStyle", shotStyle);
				PlayFiringSound();
				break;
		}

		animator.SetBool("Recharge", recharge);
		animator.SetBool("Hurt", hurt);

		if (shotStyle == 9) x = 8;
	}

	void Patrol(float speed)
	{
		RaycastHit2D groundInfo = Physics2D.Raycast(transform.position, direction, 1f);
		if (!groundInfo.collider)
		{
			direction = -direction;
			spriteRenderer.flipX = direction.x < 0;
		}
		else
		{
			transform.Translate(direction * (speed * Time.deltaTime));
			FlipCharacter(direction.x);
		}
	}

	void ChasePlayer(float speed)
	{
		Vector2 currentPosition = transform.position;
		Vector2 chaseDirection = (player.position - transform.position).normalized;

		float newY = currentPosition.y;

		transform.position = Vector2.MoveTowards(new Vector2(currentPosition.x, newY), new Vector2(player.position.x, newY), speed * Time.deltaTime);

		if (chaseDirection.x < 0)
		{
			spriteRenderer.flipX = false;
		}
		else if (chaseDirection.x > 0)
		{
			spriteRenderer.flipX = true;
		}
	}

	void ShotStyle()
	{
		shotStyle = Random.Range(0, x + 1);
	}

	void PlayFiringSound()
	{
		if (!firingAudioSource.isPlaying)
		{
			firingAudioSource.Play();
		}
	}

	void PlayMovementSound(AudioClip clip)
	{
		if (movementAudioSource.clip != clip || !movementAudioSource.isPlaying)
		{
			movementAudioSource.clip = clip;
			movementAudioSource.Play();
		}
	}

	void StopMovementSound()
	{
		if (movementAudioSource.isPlaying)
		{
			movementAudioSource.Stop();
		}
	}

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

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackRange);

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, lookRange);
	}

	public void StartShootingAnimation()
	{
		isShootingAnimationPlaying = true;
	}

	public void StopShootingAnimation()
	{
		isShootingAnimationPlaying = false;
	}
}
