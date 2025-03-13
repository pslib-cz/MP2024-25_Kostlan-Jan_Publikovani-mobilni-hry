using UnityEngine;

namespace Assets.Scripts
{
	/// <summary>
	/// Stoupání po ose y nahoru.
	/// </summary>
	public class MoveUp : MonoBehaviour
	{
		public float slowSpeed = 0.5f;
		public float fastSpeed = 2.0f;
		public float switchTime = 0.5f;
		private float currentSpeed;
		private float timeElapsed;

		void Start()
		{
			currentSpeed = slowSpeed;
			timeElapsed = 0.0f;
		}

		void Update()
		{
			timeElapsed += Time.deltaTime;

			if (timeElapsed >= switchTime)
			{
				currentSpeed = fastSpeed;
			}

			transform.Translate(Vector3.up * currentSpeed * Time.deltaTime);
		}
	}
}
