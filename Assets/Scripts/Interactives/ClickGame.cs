using UnityEngine;
using UnityEngine.InputSystem;
using Assets.Scripts.Interfaces;

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
        controls = new PlayerInputs();
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
