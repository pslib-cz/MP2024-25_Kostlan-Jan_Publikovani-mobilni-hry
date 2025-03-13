using Assets.Scripts.Attributes;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
	/// <summary>
	/// Nepřítel s jednoduchou animací.
	/// </summary>
	[RequireTag("Enemy")]
	[RequireComponent(typeof(Animator))]
	public class Worn: EnemyBase
	{
		public float speedAnimation = 1.0f;
		private Animator animator;

		private void Awake()
		{
			animator = GetComponent<Animator>();
			animator.speed = speedAnimation;
		}

		public void Start()
		{
			isAttack = true;
		}
	}
}