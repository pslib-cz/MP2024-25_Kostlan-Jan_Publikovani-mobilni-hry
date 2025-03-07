using UnityEngine;

namespace Assets.Scripts.Player
{
	/// <summary>
	/// Input manažer používající lazy singleton.
	/// </summary>
	public class InputManager : MonoBehaviour
	{
		private static InputManager _instance;
		public static InputManager Instance
		{
			get
			{
				if (_instance == null)
				{
					GameObject obj = new GameObject("InputManager");
					_instance = obj.AddComponent<InputManager>();
					DontDestroyOnLoad(obj);
				}
				return _instance;
			}
		}

		public PlayerInputs Controls
		{ 
			get;
			private set;
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

			Controls = new PlayerInputs();
			Controls.Enable();
		}
	}
}