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

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (!TouchInput() || Touchscreen.current == null) return;

		var touch = Touchscreen.current.primaryTouch;

		if (touch.press.isPressed)
		{
			if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
			{
				startTouchPosition = touch.position.ReadValue();
				isTouching = true;
			}
			else if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved && isTouching)
			{
				DetectSwipe();
				endTouchPosition = touch.position.ReadValue();
			}
		}
		else if (isTouching && touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended)
		{
			endTouchPosition = Vector2.zero;
			startTouchPosition = Vector2.zero;
			isTouching = false;
		}
	}

	private void FixedUpdate()
	{
		Debug.Log($"Start: {startTouchPosition}, End: {endTouchPosition}");
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
}