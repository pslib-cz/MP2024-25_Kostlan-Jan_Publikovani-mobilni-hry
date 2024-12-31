using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LeafGenerator : MonoBehaviour
{
	public GameObject leafPrefab;
	public float spawnInterval = 1f;
	public float groundLevel = 0f;

	/// <summary>
	/// When object active, start spawnleaves.
	/// </summary>
	void Start()
	{
		StartCoroutine(SpawnLeaves());
	}

	/// <summary>
	/// With boxcolliders determine areas to spawn leaves.
	/// </summary>
	private IEnumerator SpawnLeaves()
	{
		BoxCollider2D spawnArea = GetComponent<BoxCollider2D>();

		while (true)
		{
			Vector2 spawnPosition = new Vector2(
				Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
				Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y)
			);

			GameObject leaf = Instantiate(leafPrefab, spawnPosition, Quaternion.identity);
			yield return new WaitForSeconds(spawnInterval);
		}
	}

	/// <summary>
	/// Render spawnArea
	/// </summary>
	void OnDrawGizmos()
	{
		BoxCollider2D spawnArea = GetComponent<BoxCollider2D>();
		if (spawnArea != null)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(spawnArea.bounds.center, spawnArea.bounds.size);
		}

		Gizmos.color = Color.red;
		Gizmos.DrawLine(new Vector3(-1000, groundLevel, 0), new Vector3(1000, groundLevel, 0));
	}
}