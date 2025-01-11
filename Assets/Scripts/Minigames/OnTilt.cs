using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
	public class OnTilt : MonoBehaviour
	{
		private Accelerometer accelerometer;
		public float tiltSpeed = 2f;
		public float jumpForce = 1f;
		public float gravityStrength = 1f;

		public float maxRotation = 50f;
		public float minRotation = -50f;


		[SerializeField] private PlayerInputs controls;
		[SerializeField] private GameObject Player;
		[SerializeField] private Rigidbody2D PlayerRb;
		[SerializeField] private string sceneNext = "HospitalOutside";
		[SerializeField] private bool playerClick = false;
		[SerializeField] private MoveUp dangerWater;
		[SerializeField] private GameObject howToPlay;
		[SerializeField] private GameObject generateObjects;
		[SerializeField] private float minX = -12f;
		[SerializeField] private float maxX = 12f;

		private void Awake()
		{
			controls = new PlayerInputs();
			PlayerRb = Player.GetComponent<Rigidbody2D>();
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

		private void HandleMoveToTop(InputAction.CallbackContext obj)
		{
			if (Player!)
			{
				playerClick = true;
				dangerWater.enabled = true;
				howToPlay.SetActive(false);
				generateObjects.SetActive(true);
			}

			Player.transform.localPosition = new Vector3(Player.transform.localPosition.x, Player.transform.localPosition.y + jumpForce, Player.transform.localPosition.z);
		}

		private void HandleMoveRight(InputAction.CallbackContext obj)
		{

			if (Player.transform.position.x < maxX)
			{
				float currentZRotation = gameObject.transform.eulerAngles.z;
				if (currentZRotation > 180) currentZRotation -= 360; 

				float newZRotation = Mathf.Clamp(currentZRotation + tiltSpeed, minRotation, maxRotation);
				gameObject.transform.rotation = Quaternion.Euler(0, 0, newZRotation);
			}
		}

		private void HandleMoveLeft(InputAction.CallbackContext obj)
		{
			if (Player.transform.position.x > minX)
			{
				float currentZRotation = gameObject.transform.eulerAngles.z;
				if (currentZRotation > 180) currentZRotation -= 360;

				float newZRotation = Mathf.Clamp(currentZRotation - tiltSpeed, minRotation, maxRotation);
				gameObject.transform.rotation = Quaternion.Euler(0, 0, newZRotation);
			}
		}


		private void Start()
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

		private void FixedUpdate()
		{
			if (playerClick)
			{
				Player.transform.localPosition += Vector3.down * gravityStrength * Time.deltaTime;

				float clampedX = Mathf.Clamp(Player.transform.position.x, minX, maxX);
				Player.transform.position = new Vector3(clampedX, Player.transform.position.y, Player.transform.position.z);

			}
		}

		private void Update()
		{
			if (playerClick && accelerometer != null)
			{
				Vector3 acceleration = accelerometer.acceleration.ReadValue();
				float tilt = acceleration.x;
				float currentZRotation = gameObject.transform.rotation.eulerAngles.z;

				if (currentZRotation > 180) currentZRotation -= 360;

				float newZRotation = currentZRotation + (tilt * tiltSpeed);

				newZRotation = Mathf.Clamp(newZRotation, minRotation, maxRotation);

				if (tilt > 0 && Player.transform.position.x < maxX)
				{
					gameObject.transform.rotation = Quaternion.Euler(0, 0, newZRotation);
				}
				else if (tilt < 0 && Player.transform.position.x > minX)
				{
					gameObject.transform.rotation = Quaternion.Euler(0, 0, newZRotation);
				}
			}
		}

		// Metoda pro vykreslení hranic v editoru
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine(new Vector3(minX, -60, 0), new Vector3(minX, 100, 0));
			Gizmos.DrawLine(new Vector3(maxX, -60, 0), new Vector3(maxX, 100, 0));
		}
	}
}