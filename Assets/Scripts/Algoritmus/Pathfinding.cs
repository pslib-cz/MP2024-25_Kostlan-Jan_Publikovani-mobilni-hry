using Assets.Scripts.Algoritmus;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Algoritmus na hledání cest, kde je potřeba počáteční vektork, konečný vektor, výška, šířka a již "prošlé" nody.
/// </summary>
public class Pathfinding
{
	/// <summary>
	/// Algoritmus na hledání cest, kde je potřeba počáteční vektork, konečný vektor, výška, šířka a již "prošlé" nody.
	/// </summary>
	public static List<Vector2Int> FindPath(Vector2Int start, Vector2Int end, int width, int height, HashSet<Vector2Int> occupiedNodes = null)
	{
		var openSet = new List<Node>();
		var closedSet = new HashSet<Node>();

		var startNode = new Node { Position = start, GCost = 0, HCost = GetHeuristic(start, end) };
		var endNode = new Node { Position = end };

		openSet.Add(startNode);

		while (openSet.Count > 0)
		{
			var currentNode = GetLowestFCostNode(openSet);
			if (currentNode.Position.Equals(endNode.Position))
				return ReconstructPath(currentNode);

			openSet.Remove(currentNode);
			closedSet.Add(currentNode);

			foreach (var neighbor in GetNeighbors(currentNode, width, height, occupiedNodes))
			{
				if (closedSet.Contains(neighbor))
					continue;

				var tentativeGCost = currentNode.GCost + 1;

				if (tentativeGCost < neighbor.GCost || !openSet.Contains(neighbor))
				{
					neighbor.GCost = tentativeGCost;
					neighbor.HCost = GetHeuristic(neighbor.Position, endNode.Position);
					neighbor.Parent = currentNode;

					if (!openSet.Contains(neighbor))
						openSet.Add(neighbor);
				}
			}
		}

		return null;
	}

	/// <summary>
	/// Získá všechny sousedy v konkrétním node.
	/// </summary>
	private static List<Node> GetNeighbors(Node node, int width, int height, HashSet<Vector2Int> occupiedNodes)
	{
		var neighbors = new List<Node>();

		var directions = new Vector2Int[]
		{
				Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
		};

		foreach (var direction in directions)
		{
			var newPosition = node.Position + direction;

			if (newPosition.x >= 0 && newPosition.x < width && newPosition.y >= 0 && newPosition.y < height &&
				 (occupiedNodes == null || !occupiedNodes.Contains(newPosition)))
			{
				neighbors.Add(new Node { Position = newPosition });
			}
		}

		return neighbors;
	}

	/// <summary>
	/// Získá nekratší cestu skrze hodnocení.
	/// </summary>
	private static Node GetLowestFCostNode(List<Node> nodes)
	{
		Node lowestFCostNode = nodes[0];

		foreach (var node in nodes)
		{
			if (node.FCost < lowestFCostNode.FCost ||
				 node.FCost == lowestFCostNode.FCost && node.HCost < lowestFCostNode.HCost)
			{
				lowestFCostNode = node;
			}
		}

		return lowestFCostNode;
	}

	/// <summary>
	/// Vytvoří cestu
	/// </summary>
	private static List<Vector2Int> ReconstructPath(Node endNode)
	{
		var path = new List<Vector2Int>();
		var currentNode = endNode;

		while (currentNode != null)
		{
			path.Add(currentNode.Position);
			currentNode = currentNode.Parent;
		}

		path.Reverse();
		return path;
	}

	private static float GetHeuristic(Vector2Int a, Vector2Int b)
	{
		return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); // Manhattan distance
	}
}