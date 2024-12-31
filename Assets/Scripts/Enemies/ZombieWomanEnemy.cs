using Assets.Scripts.Interfaces;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Téměř totožný jako zombie s rozdílem, že má delay s útokem při křičení.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class ZombieWomanEnemy : EnemyBase
{
	[Header("Movement Settings")]
	[SerializeField] private float moveSpeed = 2f;
	[SerializeField] private float runSpeed = 10f;

	[Header("Audio Clips")]
	[SerializeField] private AudioClip walkClip;
	[SerializeField] private AudioClip runClip;
	[SerializeField] private AudioClip attackClip;
	[SerializeField] private AudioClip screamClip;

	[Header("Detection Settings")]
	[SerializeField] private bool isFacingRight = true;
	[SerializeField] private float frontDetectionDistance = 10f;
	[SerializeField] private float frontDetectionWidth = 5f;
	[SerializeField] private float backDetectionDistance = 5f;
	[SerializeField] private float backDetectionWidth = 3f;
	[SerializeField] private float attackRange = 2f;
	[SerializeField] private float attackWidth = 1f;
	[SerializeField] private bool isAlert;

	[Header("Idle and Walk Timers")]
	[SerializeField] private float idleTime = 2f;
	[SerializeField] private float walkTime = 5f;
	private float currentIdleTime;
	private float currentWalkTime;

	private bool isScream = true;
	private bool dead;
	private bool isIdle;
	private bool isWalkingRight;
	private Transform target;
	private Animator animator;

	private void Awake()
	{
		target = GameObject.FindGameObjectWithTag("Player").transform;
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (!dead)
		{
			Vector2 frontBoxPosition = (Vector2)transform.position + (isFacingRight ? Vector2.right : Vector2.left) * frontDetectionDistance / 2;
			Vector2 backBoxPosition = (Vector2)transform.position - (isFacingRight ? Vector2.right : Vector2.left) * backDetectionDistance / 2;
			Vector2 attackBoxPosition = (Vector2)transform.position + (isFacingRight ? Vector2.right : Vector2.left) * attackRange / 2;
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
				isAlert = true;
				isAttack = false;
				Run();
			}
			else
			{
				isAttack = false;
				isAlert = false;
				IdleWalk();
				if ((isWalkingRight && !isFacingRight) || (!isWalkingRight && isFacingRight))
				{
					Flip();
				}
			}
		}
	}

	private void Run()
	{

		if (isScream)
		{
			animator.SetBool("Scream", true);
			animator.SetBool("Run", true);
			StartCoroutine(NoMoving());
			isScream = false;
		}

		if (!isScream)
		{

			Vector2 currentPosition = transform.position;
			Vector2 targetPosition = target.position;
			targetPosition.y = currentPosition.y;

			Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetPosition, runSpeed * Time.deltaTime);

			transform.position = newPosition;
		}
	}
	IEnumerator NoMoving()
	{
		runSpeed = 0;
		yield return new WaitForSeconds(1);
		runSpeed = 10f;

	}

	private void Attack()
	{

		animator.SetTrigger("Attack");
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
				}
				else
				{
					animator.SetBool("Walk", true);
					isWalkingRight = Random.Range(0, 2) < 1;
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
		isFacingRight = !isFacingRight;
		var transform1 = transform;
		Vector3 theScale = transform1.localScale;
		theScale.x *= -1;
		transform1.localScale = theScale;
	}

	public override void TakeDamage(int damage)
	{
		animator.SetBool("Dead", true);
		PlayDeathSound();
		dead = true;

		gameObject.layer = LayerMask.NameToLayer("DeathEnemy");

	}
	private void OnDrawGizmosSelected()
	{
		Vector2 frontBoxPosition = (Vector2)transform.position + (isFacingRight ? Vector2.right : Vector2.left) * frontDetectionDistance / 2;
		Vector2 backBoxPosition = (Vector2)transform.position - (isFacingRight ? Vector2.right : Vector2.left) * backDetectionDistance / 2;
		Vector2 attackBoxPosition = (Vector2)transform.position + (isFacingRight ? Vector2.right : Vector2.left) * attackRange / 2;

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(frontBoxPosition, new Vector2(frontDetectionDistance, frontDetectionWidth));
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(backBoxPosition, new Vector2(backDetectionDistance, backDetectionWidth));
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(attackBoxPosition, new Vector2(attackRange, attackWidth));
	}
}