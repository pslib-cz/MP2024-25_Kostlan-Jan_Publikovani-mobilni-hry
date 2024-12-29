using UnityEngine;

namespace Assets.Scripts.Player
{
    public class OnTiltPlayer: MonoBehaviour
    {
        public OnTilt OnTilt;
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