using UnityEngine;

namespace Assets.Scripts.Player
{
	public class InputManager : MonoBehaviour
	{
		private static InputManager _instance;
		public static InputManager Instance => _instance ?? CreateInstance();

		public PlayerInputs Controls { get; private set; }

		private static InputManager CreateInstance()
		{
			GameObject obj = new GameObject("InputManager");
			_instance = obj.AddComponent<InputManager>();
			DontDestroyOnLoad(obj);
			_instance.InitializeControls(); // Inicializace Controls
			return _instance;
		}

		private void Awake()
		{
			if (_instance != null && _instance != this)
			{
				Destroy(gameObject);
				return;
			}

			_instance = this;
			DontDestroyOnLoad(gameObject);
			InitializeControls();
		}

		private void InitializeControls()
		{
			if (Controls == null)
			{
				Controls = new PlayerInputs();
				Controls.Enable();
			}
		}
	}
}
