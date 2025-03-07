using UnityEngine;
using UnityEngine.InputSystem;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Player;

/// <summary>
/// Testovací minihry na klikutí, při kliknutí se ukončí hra.
/// </summary>
public class ClickGame : MonoBehaviour, IMiniGame
{
	public PlayMiniGame playMiniGame;
	private InputAction clickAction;
	private PlayerInputs controls;

	void Awake()
	{
		controls = InputManager.Instance.Controls;
	}

	void OnEnable()
	{
		controls.Enable();
		controls.Minigames.ClickLockPick.performed += HandleInteraction;
	}

	void OnDisable()
	{
		controls.Minigames.ClickLockPick.performed -= HandleInteraction;
		controls.Disable();
	}

	void OnDestroy()
	{

	}

	void HandleInteraction(InputAction.CallbackContext context)
	{
		EndMiniGame();
	}

	public void EndMiniGame()
	{
		if (playMiniGame != null)
		{
			playMiniGame.EndMiniGame();
		}
	}
}
