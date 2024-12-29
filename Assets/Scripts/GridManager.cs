using UnityEngine;

public class GridManager : MonoBehaviour
{
	public int rows = 5;
	public int cols = 5;
	public float tileSize = 1.0f;
	public GameObject straightPipePrefab;
	public GameObject bentPipePrefab;

	private GameObject[,] grid;

	void Start()
	{
		CreateGrid();
	}

	void CreateGrid()
	{
		grid = new GameObject[rows, cols];
		Vector3 origin = transform.position;

		for (int row = 0; row < rows; row++)
		{
			for (int col = 0; col < cols; col++)
			{
				Vector3 tilePosition = origin + new Vector3(col * tileSize, row * tileSize, 0);
				grid[row, col] = Instantiate(straightPipePrefab, tilePosition, Quaternion.identity);
			}
		}
	}

	public void PlacePipe(int row, int col, GameObject pipePrefab)
	{
		if (grid[row, col] != null)
		{
			Destroy(grid[row, col]);
		}
		Vector3 position = new Vector3(col * tileSize, row * tileSize, 0);
		grid[row, col] = Instantiate(pipePrefab, position, Quaternion.identity);
	}
}
