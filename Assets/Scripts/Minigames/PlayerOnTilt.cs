using Assets.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Kontroller pro hráče na úroveň ontilt.
/// </summary>
public class PlayerOnTilt : MonoBehaviour
{
	public float forceAmount = 50f;
	[SerializeField] private Rigidbody2D tilt;
	[SerializeField] private PlayerInputs controls;
	[SerializeField] private MoveUp dangerWater;
	[SerializeField] private GameObject howToPlay;
	[SerializeField] private GameObject generateObjects;
	[SerializeField] private bool playerClick = false;
	private Accelerometer accelerometer;

	private void Awake()
	{
		controls = new PlayerInputs();
	}

	#region Controls
	private void OnEnable()
	{
		controls.Minigames.ClickToTop.performed += HandleMoveToTop;
		controls.Minigames.MoveRight.performed += HandleMoveRight;
		controls.Minigames.MoveLeft.performed += HandleMoveLeft;
		controls.Minigames.ClickToTop.Enable();
		controls.Minigames.MoveRight.Enable();
		controls.Minigames.MoveLeft.Enable();
	}

	private void OnDisable()
	{
		controls.Minigames.ClickToTop.performed -= HandleMoveToTop;
		controls.Minigames.MoveRight.performed -= HandleMoveRight;
		controls.Minigames.MoveLeft.performed -= HandleMoveLeft;
		controls.Minigames.ClickToTop.Disable();
		controls.Minigames.MoveRight.Disable();
		controls.Minigames.MoveLeft.Disable();
	}

	#endregion
	void Start()
	{

		if (Accelerometer.current != null)
		{
			accelerometer = Accelerometer.current;
			InputSystem.EnableDevice(accelerometer);
		}
		else
		{
			Debug.LogError("Device does not support accelerometer or it is unavailable.");
#if !UNITY_EDITOR
                //SceneManager.Instance.SceneLoad(sceneNext);
#endif
		}
	}

	private void HandleMoveToTop(InputAction.CallbackContext obj)
	{
		if (playerClick == false)
		{
			playerClick = true;
			dangerWater.enabled = true;
			howToPlay.SetActive(false);
			generateObjects.SetActive(true);
		}

		Vector3 newPosition = transform.localPosition;
		newPosition.y += 0.1f;
		transform.localPosition = newPosition;
	}

	private void FixedUpdate()
	{
		if (playerClick)
		{
			transform.localPosition += new Vector3(0, -0.01f, 0) * Time.fixedDeltaTime;
		}
	}

	private void HandleMoveRight(InputAction.CallbackContext obj)
	{
		tilt.AddForce(Vector2.right * forceAmount, ForceMode2D.Force);
	}

	private void HandleMoveLeft(InputAction.CallbackContext obj)
	{
		tilt.AddForce(Vector2.left * forceAmount, ForceMode2D.Force);
	}
}