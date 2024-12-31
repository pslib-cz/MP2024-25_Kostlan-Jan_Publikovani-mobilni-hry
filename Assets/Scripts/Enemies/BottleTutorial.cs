using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
	[RequireComponent(typeof(BoxCollider2D))]
	[RequireComponent(typeof(SpriteRenderer))]
	public class BottleTutorial : EnemyBase
	{
		private SpriteRenderer spriteRenderer;
		private BoxCollider2D boxCollider;
		private bool isBroken = false;
		[SerializeField] Sprite brokeBottle;

		private void Awake()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
			boxCollider = GetComponent<BoxCollider2D>();
		}

		public override void TakeDamage(int damage)
		{
			spriteRenderer.sprite = brokeBottle;
			boxCollider.enabled = false;
			isBroken = true;
		}

		public bool GetIsBroken()
		{
			return isBroken;
		}
	}
}
