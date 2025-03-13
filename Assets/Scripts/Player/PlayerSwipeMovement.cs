using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// Tenhle skript využívá gesta pro pohyb hráče na obrazovce.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerSwipeMovement : MonoBehaviour
{
	public float speed = 5f;
	private Vector2 startTouchPosition, endTouchPosition;
	private Rigidbody2D rb;
	private bool isTouching;
	public DeathMenu DeathScreen;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{

		if (TouchInput())
		{
			var touch = Touchscreen.current.primaryTouch;

			if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
			{
				startTouchPosition = touch.position.ReadValue();
				isTouching = true;
			}

			else if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved && isTouching)
			{
				endTouchPosition = touch.position.ReadValue();
				DetectSwipe();
			}

			else
			{
				endTouchPosition = Vector2.zero;
				startTouchPosition = Vector2.zero;
				isTouching = false;
				Move(Vector2.zero);
			}
		}
	}

	private void DetectSwipe()
	{
		Vector2 swipeDirection = endTouchPosition - startTouchPosition;

		if (swipeDirection.magnitude < 50f) return;

		if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
		{
			Move(swipeDirection.x > 0 ? Vector2.right : Vector2.left);
		}
		else
		{
			Move(swipeDirection.y > 0 ? Vector2.up : Vector2.down);
		}
	}

	public bool TouchInput()
	{
		return EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject();
	}

	private void Move(Vector2 direction)
	{
		rb.linearVelocity = direction * speed;
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
	}

	public void HandlePlayerDeath()
	{
		DeathScreen.gameObject.SetActive(true);
	}
}