using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tato třída není používaná.
/// </summary>
public class KeyboardSettings : MonoBehaviour
{
	public Text moveForwardText;
	public Text moveBackwardText;
	public Text moveLeftText;
	public Text moveRightText;

	private string moveForwardKey = "MoveForwardKey";
	private string moveBackwardKey = "MoveBackwardKey";
	private string moveLeftKey = "MoveLeftKey";
	private string moveRightKey = "MoveRightKey";

	private bool isChangingControls = false;
	private KeyCode currentKeyCode;

	private void Start()
	{
		LoadKeys();
	}

	private void Update()
	{
		CheckForInput();
	}

	private void CheckForInput()
	{
		if (isChangingControls && Input.anyKeyDown)
		{
			foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
			{
				if (Input.GetKeyDown(keyCode))
				{
					if (IsKeyCodeAlreadyUsed(keyCode))
					{
						// Uložit klávesu pro vybranou akci
						SaveKeyCodeForKey(currentKeyCode);
					}
					else
					{
						currentKeyCode = keyCode;
						UpdateText();
					}
					isChangingControls = false;
					break;
				}
			}
		}
	}

	private void SaveKeyCodeForKey(KeyCode keyCode)
	{
		PlayerPrefs.SetString(GetCurrentPlayerPrefKey(), keyCode.ToString());
		LoadKeys();
	}

	private void LoadKeys()
	{
		UpdateText();
	}

	private bool IsKeyCodeAlreadyUsed(KeyCode keyCode)
	{
		return keyCode == (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(moveForwardKey))
			|| keyCode == (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(moveBackwardKey))
			|| keyCode == (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(moveLeftKey))
			|| keyCode == (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(moveRightKey));
	}

	public void OnChangeControlsButtonClick(string actionKey)
	{
		isChangingControls = true;
		switch (actionKey)
		{
			case "MoveForward":
				currentKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(moveForwardKey));
				break;
			case "MoveBackward":
				currentKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(moveBackwardKey));
				break;
			case "MoveLeft":
				currentKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(moveLeftKey));
				break;
			case "MoveRight":
				currentKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(moveRightKey));
				break;
			default:
				break;
		}

		// Přidáme kontrolu, zda je klíč platný, pokud ne, nastavíme výchozí klávesu (W, S, A, D) 
		if (!IsValidKeyCode(currentKeyCode))
		{
			currentKeyCode = GetDefaultKeyCode(actionKey);
		}
	}

	// Zkontroluje, zda je klávesa platná v KeyCode enumu
	private bool IsValidKeyCode(KeyCode keyCode)
	{
		return System.Enum.IsDefined(typeof(KeyCode), keyCode);
	}

	// Získá výchozí klávesu pro danou akci
	private KeyCode GetDefaultKeyCode(string actionKey)
	{
		switch (actionKey)
		{
			case "MoveForward":
				return KeyCode.W;
			case "MoveBackward":
				return KeyCode.S;
			case "MoveLeft":
				return KeyCode.A;
			case "MoveRight":
				return KeyCode.D;
			default:
				return KeyCode.W;
		}
	}

	private void UpdateText()
	{
		moveForwardText.text = PlayerPrefs.GetString(moveForwardKey);
		moveBackwardText.text = PlayerPrefs.GetString(moveBackwardKey);
		moveLeftText.text = PlayerPrefs.GetString(moveLeftKey);
		moveRightText.text = PlayerPrefs.GetString(moveRightKey);
	}

	private string GetCurrentPlayerPrefKey()
	{
		switch (currentKeyCode)
		{
			case KeyCode.W:
				return moveForwardKey;
			case KeyCode.S:
				return moveBackwardKey;
			case KeyCode.A:
				return moveLeftKey;
			case KeyCode.D:
				return moveRightKey;
			default:
				return moveForwardKey;
		}
	}
}