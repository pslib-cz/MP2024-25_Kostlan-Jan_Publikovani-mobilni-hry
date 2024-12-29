using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class IntensityColor : MonoBehaviour
	{
		public Image imageToModify;
		public float transitionTime = 6.0f;
		public Color originalColor = new (0f, 0f, 0f, 0.0f);

		private void Start()
		{
			StartCoroutine(ChangeColorOverTime());
		}

		private IEnumerator ChangeColorOverTime()
		{
			Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1.0f);

			yield return LerpColor(originalColor, targetColor, transitionTime);

			yield return new WaitForSeconds(1.0f);

			yield return LerpColor(targetColor, originalColor, transitionTime);

			imageToModify.color = originalColor;
		}

		private IEnumerator LerpColor(Color startColor, Color targetColor, float duration)
		{
			float elapsedTime = 0f;

			while (elapsedTime < duration)
			{
				float t = elapsedTime / duration;
				imageToModify.color = Color.Lerp(startColor, targetColor, t);
				elapsedTime += Time.deltaTime;
				yield return null;
			}

			imageToModify.color = targetColor;
		}
	}
}