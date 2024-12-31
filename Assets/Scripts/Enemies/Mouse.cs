using Assets.Scripts.Interfaces;
using UnityEngine;

/// <summary>
/// Základní nepřímo definovaný nepřítel
/// </summary>
public class Mouse : EnemyBase
{
	public float speed = 0.01f;
	public Sprite deathSprite;
	private SpriteRenderer spriteRenderer;
	private BoxCollider2D[] boxColliders;
	private Rigidbody2D rb;
	private Transform target;
	private bool facingRight = true;
	private Animator animator;
	private bool dead;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		boxColliders = GetComponents<BoxCollider2D>();
		rb = GetComponent<Rigidbody2D>();
	}

	void Start()
	{
		GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
		if (playerObject != null)
		{
			target = playerObject.transform;
		}
		else
		{
			Debug.LogError("Nelze najít objekt s tagem 'Player' v scéně.");
		}
		isAttack = true;
	}

	void Update()
	{
		if (target != null && dead == false)
		{
			// Pohyb směrem k hráči
			transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

			// Zjištění, zda je hráč vlevo nebo vpravo
			bool playerIsToRight = target.position.x > transform.position.x;

			// Pokud je hráč na opačné straně, otočíme se
			if (playerIsToRight && !facingRight)
			{
				Flip();
			}

			else if (!playerIsToRight && facingRight)
			{
				Flip();
			}
		}
	}

	private void Flip()
	{
		// Otočení nepřítele
		facingRight = !facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

	public override void TakeDamage(int damage)
	{
		dead = true;
		PlayDeathSound();
		rb.gravityScale = 0;
		foreach (var collider in boxColliders)
		{
			collider.enabled = false;
		}
		animator.enabled = false;
		spriteRenderer.sprite = deathSprite;
	}
}