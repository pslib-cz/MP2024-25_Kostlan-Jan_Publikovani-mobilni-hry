using UnityEngine;
using System.Collections;

public class DarkerScene : MonoBehaviour
{
	private Color startColor = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
	private Color targetColor = new Color32(0x00, 0x00, 0x00, 0xFF);
	private Color finalColor = new Color32(0x62, 0x62, 0x62, 0xFF);

	public float darkeningDuration = 5f;
	public float blackScreenDuration = 2f;
	public float lighteningDuration = 3f;

	public void Start()
	{
		StartCoroutine(AfterDeath());
	}

	private IEnumerator AfterDeath()
	{
		// zvážit náročnost / výsledek
		Renderer[] renderers = FindObjectsOfType<Renderer>();

		float elapsedTime = 0f;
		while (elapsedTime < darkeningDuration)
		{
			foreach (Renderer renderer in renderers)
			{
				renderer.material.color = Color.Lerp(startColor, targetColor, elapsedTime / darkeningDuration);
			}
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		foreach (Renderer renderer in renderers)
		{
			renderer.material.color = targetColor;
		}

		yield return new WaitForSeconds(blackScreenDuration);

		elapsedTime = 0f;
		while (elapsedTime < lighteningDuration)
		{
			foreach (Renderer renderer in renderers)
			{
				renderer.material.color = Color.Lerp(targetColor, finalColor, elapsedTime / lighteningDuration);
			}
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		foreach (Renderer renderer in renderers)
		{
			renderer.material.color = finalColor;
		}
	}
}
