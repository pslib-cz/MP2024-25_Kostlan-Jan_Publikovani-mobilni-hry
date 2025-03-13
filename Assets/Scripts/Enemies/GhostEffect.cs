using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
	/// <summary>
	/// Efekt nepřítele, který zmizí před hráčem.
	/// </summary>
	[RequireComponent(typeof(SpriteRenderer))]
	public class GhostEffect : MonoBehaviour
	{
		public float maxDistance = 10f;
		public float minDistance = 2f;
		public float minAlpha = 0.1f;

		private SpriteRenderer spriteRenderer;
		private Transform player;
		private EnemyBase enemyBase;

		void Start()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
			enemyBase = GetComponent<EnemyBase>();

			if (enemyBase != null)
			{
				enemyBase.isAttack = false;
			}

			// todo tohle je ošklivé!
			GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
			if (playerObject != null)
			{
				player = playerObject.transform;
			}
			else
			{
				Debug.LogError("Nelze najít objekt s tagem 'Player' ve scéně.");
			}
		}

		void Update()
		{
			if (player == null) return;

			float distance = Vector2.Distance(transform.position, player.position);

			if (distance < minDistance)
			{
				Destroy(gameObject);
			}

			float alpha = Mathf.Clamp(distance / maxDistance, minAlpha, 1f);

			Color newColor = spriteRenderer.color;
			newColor.a = alpha;
			spriteRenderer.color = newColor;
		}
	}
}
