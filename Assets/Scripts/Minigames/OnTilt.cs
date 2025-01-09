using Assets.Scripts.Scenes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
	public class OnTilt : MonoBehaviour
	{
		private Accelerometer accelerometer;
		public float tiltSpeed = 2f;
		public float jumpForce = 1f;
		public float maxRotationZ = 45f;
		public float gravityStrength = 1f;
		[SerializeField] private PlayerInputs controls;
		[SerializeField] GameObject Player;
		[SerializeField] private Rigidbody2D PlayerRb;
		[SerializeField] private string sceneNext = "HospitalOutside";
		// řešíme to přes bool, ale kdyby to bylo neoptimální, tak by se to mohlo řešit přes aktivování a deaktivování skriptu.
		[SerializeField] private bool playerClick = false;
		[SerializeField] private MoveUp dangerWater;
		[SerializeField] private GameObject howToPlay;
		[SerializeField] private GameObject generateObjects;

		private void Awake()
		{
			controls = new PlayerInputs();
			PlayerRb = Player.GetComponent<Rigidbody2D>();
		}

		#region Controls
		private void OnEnable()
		{
			controls.Minigames.ClickToTop.performed += HandleMoveToTop;
			controls.Minigames.ClickToTop.Enable();
		}

		private void OnDisable()
		{
			controls.Minigames.ClickToTop.performed -= HandleMoveToTop;
			controls.Minigames.ClickToTop.Disable();
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
				// řešíme pro případ, kdyby hráč neměl zapnutý accelerometr.
#if !UNITY_EDITOR
					SceneManager.Instance.SceneLoad(sceneNext);
#endif
			}
		}

		private void FixedUpdate()
		{
			if (playerClick)
			{
				Player.transform.localPosition += Vector3.down * gravityStrength * Time.deltaTime;
			}
		}

		private void Update()
		{
			if (playerClick)
			{
				// udělat to tak, že budeme mít nějakou hodnotu, která hodnotí, kde hráč může být maximálně po ose x nebo to řešit ve formě box collideru
				Vector3 acceleration = Vector3.zero;
				acceleration = accelerometer.acceleration.ReadValue();
				float tilt = acceleration.x;
				float currentZRotation = gameObject.transform.rotation.eulerAngles.z;
				float newZRotation = currentZRotation + (tilt * tiltSpeed);
				if (newZRotation > 180) newZRotation -= 360; // Pro práci s negativními úhly
				newZRotation = Mathf.Clamp(newZRotation, -maxRotationZ, maxRotationZ);
				gameObject.transform.rotation = Quaternion.Euler(0, 0, newZRotation);
			}
		}
	}
}