using UnityEngine;

namespace Assets.Scripts.Enemies
{
	/// <summary>
	/// Jednoduchý skript netopýra nepoužívající enemybase.
	/// </summary>
	public class Bat : MonoBehaviour
	{
		public float speed = 2.0f;
		public bool moveRight = true;

		void Update()
		{
			float direction = moveRight ? 1 : -1;

			transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
		}

		public void ChangeDirection()
		{
			moveRight = !moveRight;
		}
	}
}