using UnityEngine;

namespace Assets.Scripts.Player
{
	/// <summary>
	/// Kontroller hráče, který slouží pro úroveň onTilt. Reaguje na kolize smrtí.
	/// </summary>
	public class OnTiltPlayer : MonoBehaviour
	{
		public DeathMenu DeathScreen;

		void OnTriggerEnter2D(Collider2D collider)
		{

			if (collider.gameObject.CompareTag("Obstacle"))
			{
				HandlePlayerDeath();
			}
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (collision.gameObject.CompareTag("Obstacle"))
			{
				HandlePlayerDeath();
			}
		}

		public void HandlePlayerDeath()
		{
			DeathScreen.gameObject.SetActive(true);
			Time.timeScale = 0f;
		}
	}
}