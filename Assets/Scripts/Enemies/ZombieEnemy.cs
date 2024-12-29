using Assets.Scripts.Interfaces;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Základní zombie nepřítel.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class ZombieEnemy : EnemyBase
{
	[Header("Movement Settings")]
	[SerializeField] private float moveSpeed = 2f;
	[SerializeField] private float runSpeed = 5f;

	[Header("Audio Clips")]
	[SerializeField] private AudioClip walkClip;
	[SerializeField] private AudioClip runClip;
	[SerializeField] private AudioClip attackClip;

	[Header("Idle and Walk Timers")]
	[SerializeField] private float idleTime = 2f;
	[SerializeField] private float walkTime = 5f;
	private float currentIdleTime;
	private float currentWalkTime;

	[Header("Detection Settings")]
	[SerializeField] private bool mFacingRight = true;
	[SerializeField] private float frontDetectionDistance = 10f;
	[SerializeField] private float frontDetectionWidth = 5f;
	[SerializeField] private float backDetectionDistance = 5f;
	[SerializeField] private float backDetectionWidth = 3f;
	[SerializeField] private float attackRange = 2f;
	[SerializeField] private float attackWidth = 1f;
	[SerializeField] private bool isAlert;

	private bool dead;
	private bool isIdle;
	private bool isWalkingRight;
	private Transform target;
	private Animator animator;
	private Rigidbody2D rb;
    private BoxCollider2D mainBoxCollider;
    private BoxCollider2D secondaryBoxCollider;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();


        BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();
        if (colliders.Length > 1)
        {
            mainBoxCollider = colliders[0];
            secondaryBoxCollider = colliders[1];
        }
        else
        {
            Debug.LogError("ZombieEnemy has less than two BoxCollider2D components.");
        }
    }

    private void Update()
	{
		if (!dead)
		{
			Vector2 frontBoxPosition = (Vector2)transform.position + (mFacingRight ? Vector2.right : Vector2.left) * frontDetectionDistance / 2;
			Vector2 backBoxPosition = (Vector2)transform.position - (mFacingRight ? Vector2.right : Vector2.left) * backDetectionDistance / 2;
			Vector2 attackBoxPosition = (Vector2)transform.position + (mFacingRight ? Vector2.right : Vector2.left) * attackRange / 2;

			Collider2D[] frontDetections = Physics2D.OverlapBoxAll(frontBoxPosition, new Vector2(frontDetectionDistance, frontDetectionWidth), 0);
			Collider2D[] backDetections = Physics2D.OverlapBoxAll(backBoxPosition, new Vector2(backDetectionDistance, backDetectionWidth), 0);
			Collider2D[] attackDetections = Physics2D.OverlapBoxAll(attackBoxPosition, new Vector2(attackRange, attackWidth), 0);

			bool playerInFront = Array.Exists(frontDetections, detection => detection.gameObject == target.gameObject);
			bool playerBehind = Array.Exists(backDetections, detection => detection.gameObject == target.gameObject);
			bool playerInRange = Array.Exists(attackDetections, detection => detection.gameObject == target.gameObject);

			if (playerInRange)
			{
				Attack();
			}
			else if (playerInFront || playerBehind)
			{
				if (playerBehind)
				{
					Flip();
				}
				Run();
				isAlert = true;
			}
			else
			{
				isAlert = false;
				IdleWalk();
				if ((isWalkingRight && !mFacingRight) || (!isWalkingRight && mFacingRight))
				{
					Flip();
				}
			}
		}
	}

	private void Run()
	{
		animator.SetBool("Run", true);
		PlaySound(runClip); // Play running sound
		var position = transform.position;
		var position1 = target.position;
		position = Vector2.MoveTowards(position, position1, runSpeed * Time.deltaTime);
		transform.position = position;
	}

	private void Attack()
	{
		animator.SetTrigger("Attack");
		PlaySound(attackClip); // Play attack sound
		isAttack = true;
	}

	private void IdleWalk()
	{
		if (!isAlert)
		{
			currentIdleTime -= Time.deltaTime;
			currentWalkTime -= Time.deltaTime;

			if (currentIdleTime <= 0 && currentWalkTime <= 0)
			{
				isIdle = Random.value > 0.5;
				currentIdleTime = isIdle ? Random.Range(1f, idleTime) : 0;
				currentWalkTime = isIdle ? 0 : Random.Range(1f, walkTime);

				if (isIdle)
				{
					animator.SetBool("Walk", false);
					audioSource.Stop(); // Stop any current walking sound
				}
				else
				{
					animator.SetBool("Walk", true);
					isWalkingRight = Random.Range(0, 2) < 1;
					PlaySound(walkClip); // Play walking sound
				}
			}

			if (!isIdle)
			{
				if (isWalkingRight)
				{
					transform.Translate(Vector2.right * (moveSpeed * Time.deltaTime));
				}
				else
				{
					transform.Translate(Vector2.left * (moveSpeed * Time.deltaTime));
				}
			}
		}
	}

	private void Flip()
	{
		mFacingRight = !mFacingRight;

		var transform1 = transform;
		Vector3 theScale = transform1.localScale;
		theScale.x *= -1;
		transform1.localScale = theScale;
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player") && dead == false)
		{
			isAttack = true;
		}

		else if (other.gameObject.CompareTag("Car") && dead == false)
		{
            CarController car = other.gameObject.GetComponent<CarController>();

			if(car.speed == 0)
			{
				car.EndGame();
			}
			else
			{
				TakeDamage(20);
			}
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.GetContact(0).normal.Normalize();
    }

    private void OnDrawGizmosSelected()
	{
		Vector2 frontBoxPosition = (Vector2)transform.position + (mFacingRight ? Vector2.right : Vector2.left) * frontDetectionDistance / 2;
		Vector2 backBoxPosition = (Vector2)transform.position - (mFacingRight ? Vector2.right : Vector2.left) * backDetectionDistance / 2;
		Vector2 attackBoxPosition = (Vector2)transform.position + (mFacingRight ? Vector2.right : Vector2.left) * attackRange / 2;

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(frontBoxPosition, new Vector2(frontDetectionDistance, frontDetectionWidth));
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(backBoxPosition, new Vector2(backDetectionDistance, backDetectionWidth));
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(attackBoxPosition, new Vector2(attackRange, attackWidth));
	}

	public override void TakeDamage(int damage)
	{
		animator.SetBool("Dead", true);
		PlayDeathSound();
        dead = true;
        secondaryBoxCollider.enabled = false;

        gameObject.layer = LayerMask.NameToLayer("DeathEnemy");
    }
}