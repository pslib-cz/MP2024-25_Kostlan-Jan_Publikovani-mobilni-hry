using UnityEngine;
public class PipePlacer : MonoBehaviour
{
	public GridManager gridManager;
	public Camera mainCamera;

	private PlayerInputs controls;

	private void Awake()
	{
		controls = new PlayerInputs();
		controls.Minigames.PlacePipe.performed += ctx => OnPlacePipe();
	}

	private void OnEnable()
	{
		controls.Player.Enable();
	}

	private void OnDisable()
	{
		controls.Player.Disable();
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