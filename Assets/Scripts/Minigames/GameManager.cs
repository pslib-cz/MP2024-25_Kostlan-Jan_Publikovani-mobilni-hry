using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[Header("Pipes prefabs")]
	public GameObject straightPipePrefab;
	public GameObject cornerPipePrefab;

	// možnost přidat další typy trubek.
	/*
    public GameObject tShapePipePrefab;

    public GameObject cornerShapePipePrefab;
    public GameObject emptyPipePrefab;*/

	[Header("Start/End Pipes positions")]
	public GameObject PositionStartObject;
	public GameObject PositionEndObject;
	[SerializeField] private int rowStartObject;
	[SerializeField] private int rowEndObject;

	[Header("Grid Settings")]
	public int gridWidth = 5;
	public int gridHeight = 5;
	public float spacing = 100f;
	public Canvas canvas;
	public float startPositionXOffset = 120f;

	[Header("Object with pipes")]
	public GameObject PipesHolder;

	private List<PipeScript> pipes = new List<PipeScript>();
	private List<(PipeType, Vector2Int)> generatePathWithPipes;

	[SerializeField] PlayMiniGame playMiniGame;

	private Color startColor = Color.blue;
	private Color endColor = Color.red;
	private Color pathColor = Color.green;

	Vector2Int point1;
	Vector2Int point2;

	void Start()
	{
		if (PipesHolder == null)
		{
			PipesHolder = new GameObject("PipesHolder");
		}

		rowStartObject = UnityEngine.Random.Range(0, gridHeight);
		rowEndObject = UnityEngine.Random.Range(0, gridHeight);

		GeneratePath();
		GenerateGrid();
		//HighlightPath();
		CheckAllConnections();
	}

	//void HighlightPath()
	//{
	//    foreach (var pipeData in generatePathWithPipes)
	//    {
	//        var pipe = pipes.FirstOrDefault(p => p.gridX == pipeData.Item2.x && p.gridY == pipeData.Item2.y);
	//        if (pipe != null)
	//        {
	//            // Najdeme komponentu Image na objektu nebo v jeho child objektech
	//            var imageComponent = pipe.GetComponent<Image>();
	//            if (imageComponent == null)
	//            {
	//                imageComponent = pipe.GetComponentInChildren<Image>();
	//            }

	//            // Pokud nalezneme Image komponentu, změníme barvu
	//            if (imageComponent != null)
	//            {
	//                imageComponent.color = Color.yellow; // Nastavení oranžové barvy
	//            }
	//            else
	//            {
	//                Debug.LogWarning("Pipe does not have an Image component attached on any child.");
	//            }
	//        }
	//    }
	//}

	/// <summary>
	/// Generování gridu pro pathfinding.
	/// </summary>
	void GenerateGrid()
	{
		// získáme pozici pipesholderu
		Vector3 startPosition = PipesHolder.transform.position;

		for (int y = 0; y < gridHeight; y++)
		{
			for (int x = 0; x < gridWidth; x++)
			{
				// přidáme k tomu spacing k pozici pipeholderu
				Vector3 position = startPosition + new Vector3(x * spacing, y * spacing, 0);
				GameObject pipePrefab;

				var pathPoint = generatePathWithPipes.FirstOrDefault(p => p.Item2.x == x && p.Item2.y == y);

				if (pathPoint != default)
				{
					pipePrefab = GetPrefabByPipeType(pathPoint.Item1);
				}
				else
				{
					pipePrefab = GetRandomPipePrefab();
				}

				// Instantiate the pipe and add it to the grid
				GameObject newPipe = Instantiate(pipePrefab, position, Quaternion.identity, PipesHolder.transform);
				PipeScript pipeScript = newPipe.GetComponent<PipeScript>();

				pipeScript.gridX = x;
				pipeScript.gridY = y;
				pipeScript.UpdateOpenDirections();


				pipes.Add(pipeScript);
			}
		}

		var startPipe = pipes.FirstOrDefault(pipe => pipe.gridX == 0 && pipe.gridY == rowStartObject);
		var endPipe = pipes.FirstOrDefault(pipe => pipe.gridX == 5 && pipe.gridY == rowEndObject);

		var nevim = canvas.scaleFactor;
		var nevimddkl = canvas.pixelRect;

		Vector3 newPosition = new Vector3(startPipe.transform.position.x * nevim, startPipe.transform.position.y * nevim, startPipe.transform.position.z);
		Instantiate(PositionStartObject, newPosition, Quaternion.identity, canvas.transform);

		Vector3 endPositionPositionStart = new Vector3(endPipe.transform.position.x * nevim, endPipe.transform.position.y * nevim, startPipe.transform.position.z);
		Instantiate(PositionEndObject, endPositionPositionStart, Quaternion.identity, canvas.transform);
	}
	GameObject GetPrefabByPipeType(PipeType pipeType)
	{
		switch (pipeType)
		{
			case PipeType.Straight:
				return straightPipePrefab;
			case PipeType.Corner:
				return cornerPipePrefab;
			// Add cases for other pipe types if necessary
			default:
				return null;
		}
	}

	/// <summary>
	/// Algoritmus na generování cest.
	/// </summary>
	private void GeneratePath()
	{
		var startPipe = new Vector2Int(0, rowStartObject);
		var endPipe = new Vector2Int(gridWidth - 1, rowEndObject);

		if (startPipe == null || endPipe == null)
		{
			Debug.LogError("Start or End pipe not found.");
			return;
		}

		Vector2Int point1;
		Vector2Int point2;
		List<Vector2Int> fullPath = null;

		while (fullPath == null)
		{
			do
			{
				point1 = new Vector2Int(UnityEngine.Random.Range(1, gridWidth - 1), UnityEngine.Random.Range(0, gridHeight));
			} while (point1.Equals(new Vector2Int(startPipe.x, startPipe.y)));

			do
			{
				point2 = new Vector2Int(UnityEngine.Random.Range(1, gridWidth - 1), UnityEngine.Random.Range(0, gridHeight));
			} while (point2.Equals(new Vector2Int(endPipe.x, endPipe.y)) || point2.Equals(point1));

			Debug.Log(point1 + " point1");
			Debug.Log(point2 + " point2");
			HashSet<Vector2Int> occupiedNodes = new HashSet<Vector2Int>();

			var path1 = Pathfinding.FindPath(new Vector2Int(startPipe.x, startPipe.y), point1, gridWidth, gridHeight, occupiedNodes);
			if (path1 == null)
			{
				Debug.LogWarning("No path found from start to point1, regenerating points.");
				continue;
			}

			foreach (var node in path1)
			{
				occupiedNodes.Add(node);
			}


			var path2 = Pathfinding.FindPath(point1, point2, gridWidth, gridHeight, occupiedNodes);
			if (path2 == null)
			{
				Debug.LogWarning("No path found from point1 to point2, regenerating points.");
				continue;
			}

			foreach (var node in path2)
			{
				occupiedNodes.Add(node);
			}

			var path3 = Pathfinding.FindPath(point2, new Vector2Int(endPipe.x, endPipe.y), gridWidth, gridHeight, occupiedNodes);
			if (path3 == null)
			{
				Debug.LogWarning("No path found from point2 to end, regenerating points.");
				continue;
			}


			fullPath = path1.Concat(path2).Concat(path3).Distinct().ToList();

			foreach (var item in fullPath)
			{
				Debug.Log(item);
			}
		}

		List<PipeType> segmentTypes = DetermineSegmentTypes(fullPath);

		generatePathWithPipes = new List<(PipeType segmentType, Vector2Int point)>();

		for (int i = 0; i < fullPath.Count; i++)
		{
			var point = fullPath[i];
			var segmentType = segmentTypes[i];

			generatePathWithPipes.Add((segmentType, point));
		}
	}

	public static List<PipeType> DetermineSegmentTypes(List<Vector2Int> path)
	{
		var segmentTypes = new List<PipeType>();

		if (path.Count < 2)
		{
			return segmentTypes;
		}

		Vector2Int first = path[0];
		Vector2Int second = path[1];
		if (second.x > first.x)
		{
			segmentTypes.Add(PipeType.Straight);
		}
		else if (second.y != first.y)
		{
			segmentTypes.Add(PipeType.Corner);
		}
		else
		{
			segmentTypes.Add(PipeType.Straight);
		}

		for (int i = 1; i < path.Count - 1; i++)
		{
			Vector2Int prev = path[i - 1];
			Vector2Int curr = path[i];
			Vector2Int next = path[i + 1];

			if (IsStraight(prev, curr, next))
			{
				segmentTypes.Add(PipeType.Straight);
			}
			else
			{
				segmentTypes.Add(PipeType.Corner);
			}
		}

		Vector2Int last = path[path.Count - 1];
		Vector2Int secondLast = path[path.Count - 2];
		if (secondLast.y != last.y)
		{
			segmentTypes.Add(PipeType.Corner);
		}
		else if (secondLast.x < last.x)
		{
			segmentTypes.Add(PipeType.Straight);
		}
		else
		{
			segmentTypes.Add(PipeType.Corner); // Default to corner if no conditions match
		}

		return segmentTypes;
	}

	private static bool IsStraight(Vector2Int p1, Vector2Int p2, Vector2Int p3)
	{
		return (p1.x == p2.x && p2.x == p3.x) || (p1.y == p2.y && p2.y == p3.y) ||
			   ((p1.x - p2.x) * (p2.y - p3.y) == (p2.x - p3.x) * (p1.y - p2.y));
	}

	GameObject GetRandomPipePrefab()
	{
		int randomPipeType = UnityEngine.Random.Range(0, 2);
		switch (randomPipeType)
		{
			case 0:
				return straightPipePrefab;
			case 1:
				return cornerPipePrefab;
			//case 2:
			//    return emptyPipePrefab;
			/*case 2:
                return tShapePipePrefab;
            default:
                return cornerShapePipePrefab;*/
			default:
				return null;
		}
	}

	private PipeDirection GetDirection(Vector2Int from, Vector2Int to)
	{
		if (from.x < to.x) return PipeDirection.Right;
		if (from.x > to.x) return PipeDirection.Left;
		if (from.y < to.y) return PipeDirection.Down;
		if (from.y > to.y) return PipeDirection.Up;
		throw new ArgumentException("Invalid direction");
	}

	private PipeDirection GetOppositeDirection(PipeDirection direction)
	{
		switch (direction)
		{
			case PipeDirection.Right: return PipeDirection.Left;
			case PipeDirection.Up: return PipeDirection.Down;
			case PipeDirection.Left: return PipeDirection.Right;
			case PipeDirection.Down: return PipeDirection.Up;
			default: return direction;
		}
	}
	public void CheckAllConnections()
	{
		Vector3 startPosition = PipesHolder.transform.position;

		var startPipe = pipes.FirstOrDefault(pipe => pipe.gridX == 0 && pipe.gridY == rowStartObject);
		bool correct = startPipe != null && startPipe.HasOpenDirection(PipeDirection.Left);

		var endPipe = pipes.FirstOrDefault(pipe => pipe.gridX == (gridWidth - 1) && pipe.gridY == rowEndObject);

		foreach (var pipe in pipes)
		{
			pipe.SetConnected(false);
		}

		var visitedPipes = new HashSet<PipeScript>();

		if (correct)
		{
			CheckPipeConnections(startPipe, visitedPipes);

			if (endPipe != null && endPipe.isConnected && endPipe.HasOpenDirection(PipeDirection.Right))
			{
				EndMiniGame();
			}
			else
			{
				Debug.Log("The pipes are not connected correctly.");
			}
		}
		else
		{
			Debug.Log("The start pipe is not correctly oriented.");
		}
	}

	private void CheckPipeConnections(PipeScript pipe, HashSet<PipeScript> visitedPipes)
	{
		if (!visitedPipes.Add(pipe))
		{
			return;
		}

		pipe.SetConnected(true);

		int positionX = pipe.gridX;
		int positionY = pipe.gridY;

		for (int i = 0; i < pipe.openDirections.Length; i++)
		{
			if (pipe.openDirections[i])
			{
				PipeDirection direction = (PipeDirection)i;
				PipeScript nextPipe = null;

				// Adjust the position based on the direction
				switch (direction)
				{
					case PipeDirection.Right:
						nextPipe = pipes.FirstOrDefault(p => p.gridX == positionX + 1 && p.gridY == positionY);
						break;
					case PipeDirection.Up:
						nextPipe = pipes.FirstOrDefault(p => p.gridX == positionX && p.gridY == positionY - 1);
						break;
					case PipeDirection.Left:
						nextPipe = pipes.FirstOrDefault(p => p.gridX == positionX - 1 && p.gridY == positionY);
						break;
					case PipeDirection.Down:
						nextPipe = pipes.FirstOrDefault(p => p.gridX == positionX && p.gridY == positionY + 1);
						break;
				}

				if (nextPipe != null)
				{
					bool isConnected = nextPipe.HasOpenDirection(GetOppositeDirection(direction));
					nextPipe.SetConnected(isConnected);

					if (isConnected)
					{
						CheckPipeConnections(nextPipe, visitedPipes);
					}
				}
			}
		}
	}

	/// <summary>
	/// Ukončení minihry
	/// </summary>
	public void EndMiniGame()
	{
		if (playMiniGame != null)
		{
			playMiniGame.EndMiniGame();
		}
	}
}