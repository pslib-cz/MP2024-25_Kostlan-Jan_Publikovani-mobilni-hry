using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class BlinkingLight : MonoBehaviour
{
	public Light2D light2D;
	public float minIntensity = 0.0f;
	public float maxIntensity = 0.5f;
	public float blinkSpeed = 1.0f;

	private Coroutine blinkCoroutine;

	void OnEnable()
	{
		if (light2D == null)
		{
			light2D = GetComponent<Light2D>();
		}

		if (light2D != null && light2D.enabled)
		{
			blinkCoroutine = StartCoroutine(BlinkRoutine());
		}
	}

	void OnDisable()
	{
		if (blinkCoroutine != null)
		{
			StopCoroutine(blinkCoroutine);
			light2D.intensity = minIntensity;
		}
	}

	private IEnumerator BlinkRoutine()
	{
		while (true)
		{
			light2D.intensity = (light2D.intensity == maxIntensity) ? minIntensity : maxIntensity;
			yield return new WaitForSeconds(1 / blinkSpeed);
		}
	}
}