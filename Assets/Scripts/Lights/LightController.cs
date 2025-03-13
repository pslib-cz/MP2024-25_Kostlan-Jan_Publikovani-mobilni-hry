using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Kontroller světla.
/// </summary>
[RequireComponent(typeof(Light2D))]
public class LightController : MonoBehaviour
{
	[Header("Light Settings")]
	[SerializeField] private Light2D lightSource;
	[SerializeField] private List<Color> colors = new List<Color>();
	[SerializeField] private float rotationSpeed = 1f;
	[SerializeField] private float maxRotationAngle = 60f;
	[SerializeField] private float gizmoRadius = 2f;

	private Color lastColor;
	private Quaternion initialRotation;

	private void Awake()
	{
		lightSource = GetComponentInChildren<Light2D>();
		initialRotation = lightSource.transform.localRotation;
	}

	private void Start()
	{
		SelectRandomColor();
	}

	private void Update()
	{
		OscillateRotation();
		ChangeLightColor();
	}

	private void OscillateRotation()
	{
		float angle = Mathf.Sin(Time.time * rotationSpeed) * maxRotationAngle;
		lightSource.transform.localRotation = initialRotation * Quaternion.Euler(0f, 0f, angle); // Přičteme oscilaci k původní rotaci
	}

	private void ChangeLightColor()
	{
		if (Time.time % 2 < Time.deltaTime)
		{
			SelectRandomColor();
		}
	}

	private void SelectRandomColor()
	{
		Color newColor;
		do
		{
			newColor = colors[Random.Range(0, colors.Count)];
		} while (newColor == lastColor);

		lastColor = newColor;
		lightSource.color = newColor;
	}

	private void OnDrawGizmos()
	{

		Gizmos.color = Color.yellow;

		// Získání rotace na základě aktuální oscilace
		float angle = Mathf.Sin(Time.time * rotationSpeed) * maxRotationAngle;

		// Výpočet směrů pro maximální rotace relativně k původní rotaci
		Vector3 leftDirection = (initialRotation * Quaternion.Euler(0, 0, maxRotationAngle)) * Vector3.right;
		Vector3 rightDirection = (initialRotation * Quaternion.Euler(0, 0, -maxRotationAngle)) * Vector3.right;

		// Aktuální směr
		Vector3 currentDirection = (initialRotation * Quaternion.Euler(0, 0, angle)) * Vector3.right;

		// Vykreslení linií pro rozsah rotace
		Gizmos.DrawLine(lightSource.transform.position, lightSource.transform.position + leftDirection * gizmoRadius);
		Gizmos.DrawLine(lightSource.transform.position, lightSource.transform.position + rightDirection * gizmoRadius);

		// Vykreslení aktuální rotace
		Gizmos.color = Color.red;
		Gizmos.DrawLine(lightSource.transform.position, lightSource.transform.position + currentDirection * gizmoRadius);
	}
}
