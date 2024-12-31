using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	internal class Balloon : MonoBehaviour
	{
		public float speedX = 2f; // Rychlost pohybu na ose X
		public float speedY = 1f; // Maximální rychlost pohybu na ose Y
		public float yChangeInterval = 1f; // Interval změny směru na ose Y

		private float targetYDirection; // Cílový směr na ose Y
		private float yVelocity; // Aktuální rychlost na ose Y
		private Vector3 targetPosition; // Cílová pozice balónku

		void Start()
		{
			// Nastavte náhodnou cílovou pozici na ose Y a vzdálenou cílovou pozici na ose X
			targetPosition = new Vector3(UnityEngine.Random.Range(10f, 20f), UnityEngine.Random.Range(-4f, 4f), transform.position.z);
			StartCoroutine(ChangeYDirection());
		}

		void Update()
		{
			// Pohyb na ose X
			transform.position -= Vector3.right * speedX * Time.deltaTime;

			// Pohyb na ose Y s přechodem mezi směry
			yVelocity = Mathf.Lerp(yVelocity, targetYDirection * speedY, Time.deltaTime);
			transform.position += Vector3.up * yVelocity * Time.deltaTime;

			// Pokud se balónek přiblíží k cíli na ose X, zastavte ho nebo nastavte nový cíl
			if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
			{
				targetPosition = new Vector3(UnityEngine.Random.Range(10f, 20f), UnityEngine.Random.Range(-4f, 4f), transform.position.z);
			}
		}

		private IEnumerator ChangeYDirection()
		{
			while (true)
			{
				// Náhodně nastavte směr pohybu na ose Y (nahoru/dolu)
				targetYDirection = UnityEngine.Random.Range(-1f, 1f);
				yield return new WaitForSeconds(yChangeInterval);
			}
		}
	}
}
