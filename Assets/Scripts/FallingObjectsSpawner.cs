using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	/// <summary>
	/// Spawnuje padající objekty
	/// </summary>
	public class FallingObjectsSpawner : MonoBehaviour
	{
		public List<Texture2D> texturesToSpawn;
		public float initialSpawnInterval = 2f;
		public float minSpawnInterval = 0.1f;
		public float intervalDecreaseRate = 0.95f;
		public float spawnRangeX = 8f;
		public float deleteObjectAfter = 8f;
		private float currentSpawnInterval;

		void Start()
		{
			currentSpawnInterval = initialSpawnInterval;

			// Začni spawnovat ostatní objekty
			StartCoroutine(SpawnObjects());
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
			Vector3 spawnPosition = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), transform.position.y, 0f);

			int randomIndex = Random.Range(0, texturesToSpawn.Count);
			Texture2D selectedTexture = texturesToSpawn[randomIndex];

			GameObject spawnedObject = new GameObject("FallingObject");
			spawnedObject.transform.localScale = new Vector3(5, 5, 1);
			spawnedObject.tag = "Obstacle";
			SpriteRenderer renderer = spawnedObject.AddComponent<SpriteRenderer>();
			renderer.sprite = TextureToSprite(selectedTexture);
			renderer.sortingOrder = 0;

			PolygonCollider2D boxCollider = spawnedObject.AddComponent<PolygonCollider2D>();
			boxCollider.isTrigger = true;

			Rigidbody2D rigidbodyObject = spawnedObject.AddComponent<Rigidbody2D>();
			rigidbodyObject.gravityScale = 0.3f;

			float randomRotation = Random.Range(0f, 360f);
			spawnedObject.transform.rotation = Quaternion.Euler(0f, 0f, randomRotation);

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
}