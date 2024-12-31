using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjectsSpawner : MonoBehaviour
{
	public List<Texture2D> texturesToSpawn; // Textury pro ostatní objekty
	public Texture2D carTexture; // Textura pro auto
	public float initialSpawnInterval = 2f;
	public float minSpawnInterval = 0.1f;
	public float intervalDecreaseRate = 0.95f;
	public float spawnRangeX = 10f;
	public float deleteObjectAfter = 4f;
	private float currentSpawnInterval;

	void Start()
	{
		currentSpawnInterval = initialSpawnInterval;

		// Spawni auto jednou na začátku
		SpawnCar();

		// Začni spawnovat ostatní objekty
		StartCoroutine(SpawnObjects());
	}

	void SpawnCar()
	{
		Vector3 spawnPosition = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), transform.position.y, 0f) + transform.position;

		GameObject car = new GameObject("Car");
		car.transform.localScale = new Vector3(30, 30, 1);
		car.tag = "Obstacle";
		SpriteRenderer renderer = car.AddComponent<SpriteRenderer>();
		renderer.sprite = TextureToSprite(carTexture);
		renderer.sortingOrder = 1;

		PolygonCollider2D carCollider = car.AddComponent<PolygonCollider2D>();
		carCollider.isTrigger = true;

		Rigidbody2D carRigidbody = car.AddComponent<Rigidbody2D>();
		carRigidbody.gravityScale = 2f;
		car.transform.position = spawnPosition;

		StartCoroutine(DestroyAfterDelay(car, 15));
	}

	IEnumerator SpawnObjects()
	{
		while (true)
		{
			SpawnObject();
			yield return new WaitForSeconds(currentSpawnInterval);
			currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval * intervalDecreaseRate);
		}
	}

	void SpawnObject()
	{
		// Generování náhodné pozice na ose X, pevná osa Y
		Vector3 spawnPosition = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), transform.position.y, 0f);

		int randomIndex = Random.Range(0, texturesToSpawn.Count);
		Texture2D selectedTexture = texturesToSpawn[randomIndex];

		GameObject spawnedObject = new GameObject("FallingObject");
		spawnedObject.transform.localScale = new Vector3(10, 10, 1);
		spawnedObject.tag = "Obstacle";
		SpriteRenderer renderer = spawnedObject.AddComponent<SpriteRenderer>();
		renderer.sprite = TextureToSprite(selectedTexture);
		renderer.sortingOrder = 0;

		PolygonCollider2D boxCollider = spawnedObject.AddComponent<PolygonCollider2D>();
		boxCollider.isTrigger = true;

		Rigidbody2D rigidbodyObject = spawnedObject.AddComponent<Rigidbody2D>();
		rigidbodyObject.gravityScale = 0.5f;

		spawnedObject.transform.position = spawnPosition;

		StartCoroutine(DestroyAfterDelay(spawnedObject, deleteObjectAfter));
	}

	IEnumerator DestroyAfterDelay(GameObject obj, float delay)
	{
		yield return new WaitForSeconds(delay);
		Destroy(obj);
	}

	Sprite TextureToSprite(Texture2D texture)
	{
		Rect rect = new Rect(0, 0, texture.width, texture.height);
		return Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position + new Vector3(-spawnRangeX, 0f, 0f), transform.position + new Vector3(spawnRangeX, 0f, 0f));
		Gizmos.DrawCube(transform.position + new Vector3(-spawnRangeX, 0f, 0f), new Vector3(0.5f, 0.5f, 0.5f));
		Gizmos.DrawCube(transform.position + new Vector3(spawnRangeX, 0f, 0f), new Vector3(0.5f, 0.5f, 0.5f));
	}
}
