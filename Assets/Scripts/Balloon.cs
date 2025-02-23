using System.Collections;
using UnityEngine;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts
{
	public class Balloon : EnemyBase
	{
		public float speedX = 2f;
		public float speedY = 1f;
		public float yChangeInterval = 1f;

		private float yVelocity;
		private Transform player;
		private Animator animator;
		private bool isAggressive = false;
		private const float moveSpeed = 2f;

		[SerializeField] private bool mFacingRight = false;

		private void Awake()
		{
			player = GameObject.FindGameObjectWithTag("Player").transform;
			animator = GetComponent<Animator>();
			audioSource = GetComponent<AudioSource>();
		}

		void Start()
		{
			StartCoroutine(ChangeYDirection());
		}

		private IEnumerator ChangeYDirection()
		{
			while (!isAggressive)
			{
				float targetYDirection = Random.Range(-0.5f, 0.5f);
				float targetXDirection = Random.Range(-0.5f, 0.5f);

				float interval = Random.Range(2f, 4f);

				Vector3 newTarget = new Vector3(transform.position.x + targetXDirection * speedX,
												transform.position.y + targetYDirection * speedY,
												transform.position.z);

				StartCoroutine(MoveToTarget(newTarget, interval));

				yield return new WaitForSeconds(interval);
			}
		}

		private IEnumerator MoveToTarget(Vector3 target, float duration)
		{
			float time = 0;
			Vector3 startPosition = transform.position;

			while (time < duration)
			{
				transform.position = Vector3.Lerp(startPosition, target, time / duration);
				time += Time.deltaTime;
				yield return null;
			}
		}

		private void Update()
		{
			if (isAggressive)
			{
				// Pronásleduje hráče po ose X i Y
				transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
			}

			FlipTowardsPlayer();
		}

		private void FlipTowardsPlayer()
		{
			if (player.position.x > transform.position.x && mFacingRight)
			{
				Flip();
			}
			else if (player.position.x < transform.position.x && !mFacingRight)
			{
				Flip();
			}
		}

		/// <summary>
		/// Změní agresivitu balónu. Musí být voláno z jiného skriptu.
		/// </summary>
		/// <param name="state">True pro agresivní režim, false pro pasivní režim</param>
		public void SetAggressive(bool state)
		{
			isAggressive = state;
			animator.SetBool("isAgressive", state);
			isAttack = state;

			if (state)
			{
				StopCoroutine(ChangeYDirection());
			}
			else
			{
				StartCoroutine(ChangeYDirection());
			}
		}

		private void Flip()
		{
			mFacingRight = !mFacingRight;
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}

		public override void TakeDamage(int damage)
		{
			if (isAggressive) // Lze zničit pouze pokud je agresivní
			{
				health -= damage;
				if (health <= 0)
				{
					Die();
				}
			}
		}

		private void Die()
		{
			PlayDeathSound();
			Destroy(gameObject);
		}
	}
}
