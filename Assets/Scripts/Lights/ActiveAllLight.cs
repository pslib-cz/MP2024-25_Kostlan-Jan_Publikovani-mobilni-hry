using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Aktivuje všechny světla s maskou Light..
/// </summary>
public class ActiveAllLight : MonoBehaviour
{
	private List<Light2D> lights = new List<Light2D>();
	[SerializeField] private GameObject playerLight2D;

	void Awake()
	{
		Light2D[] allLights = FindObjectsByType<Light2D>(FindObjectsSortMode.None);
		int lightLayer = LayerMask.NameToLayer("Light");

		foreach (Light2D light in allLights)
		{
			if (light.gameObject.layer == lightLayer)
			{
				lights.Add(light);
			}
		}
	}

	private void Start()
	{
		ActivatedAllLights(false);
	}

	public void ActivatedAllLights(bool activate)
	{
		foreach (Light2D light in lights)
		{
			light.enabled = activate;
		}
		playerLight2D.SetActive(!activate);
	}
}
