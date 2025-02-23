using System.Collections;
using UnityEngine;

/// <summary>
/// Generátor listů na stromě.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class LeafGenerator : MonoBehaviour
{
	public GameObject leafPrefab;
	public float spawnInterval = 1f;
	public float groundLevel = 0f;

	void Start()
	{
		StartCoroutine(SpawnLeaves());
	}

	/// <summary>
	/// Za pomocí box collideru generuje listy v oblasti.
	/// </summary>
	private IEnumerator SpawnLeaves()
	{
		BoxCollider2D spawnArea = GetComponent<BoxCollider2D>();

		while (true)
		{
			// Vygeneruj náhodnou pozici v oblasti
			Vector2 spawnPosition = new Vector2(
				Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
				Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y)
			);

			// Vytvoř nový list
			GameObject leaf = Instantiate(leafPrefab, spawnPosition, Quaternion.identity);

			// Předání groundLevel listu
			LeafFall leafFall = leaf.GetComponent<LeafFall>();
			if (leafFall != null)
			{
				leafFall.Initialize(groundLevel);
			}

			StartCoroutine(DestroyAfterDelay(leaf, 6f));

			yield return new WaitForSeconds(spawnInterval);
		}
	}

	/// <summary>
	/// Zničí objekt po zadaném čase.
	/// </summary>
	IEnumerator DestroyAfterDelay(GameObject obj, float delay)
	{
		yield return new WaitForSeconds(delay);
		Destroy(obj);
	}

	/// <summary>
	/// Vykreslí oblast, ve které se listy generují.
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