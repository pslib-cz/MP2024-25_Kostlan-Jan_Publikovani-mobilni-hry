using Assets.Scripts.Player;
using UnityEngine;

/// <summary>
/// Skript pro umístění trubek na herní plochu.
/// </summary>
public class PipePlacer : MonoBehaviour
{
	public GridManager gridManager;
	public Camera mainCamera;

	private PlayerInputs controls;

	private void Awake()
	{
		controls = InputManager.Instance.Controls;
		controls.Minigames.PlacePipe.performed += ctx => OnPlacePipe();
	}

	private void OnEnable()
	{
		controls.Minigames.Enable();
	}

	private void OnDisable()
	{
		controls.Minigames.Disable();
	}

	private void OnDestroy()
	{
		if (controls != null)
		{
			controls.Minigames.PlacePipe.performed -= ctx => OnPlacePipe();
			controls.Dispose();
		}
	}

	private void OnPlacePipe()
	{
		Vector2 mousePosition = controls.Minigames.MousePosition.ReadValue<Vector2>();
		Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.nearClipPlane));

		int row = Mathf.RoundToInt(worldPosition.y);
		int col = Mathf.RoundToInt(worldPosition.x);

		if (controls.Minigames.PlaceBentPipe.IsPressed())
		{
			gridManager.PlacePipe(row, col, gridManager.bentPipePrefab);
		}
		else
		{
			gridManager.PlacePipe(row, col, gridManager.straightPipePrefab);
		}
	}
}