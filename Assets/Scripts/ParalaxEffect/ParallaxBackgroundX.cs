using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParallaxBackgroundX : MonoBehaviour
{
	public ParallaxCameraX parallaxCamera;
	List<ParallaxLayerX> parallaxLayers = new List<ParallaxLayerX>();

	void Start()
	{
		if (parallaxCamera == null)
			parallaxCamera = Camera.main.GetComponent<ParallaxCameraX>();

		if (parallaxCamera != null)
			parallaxCamera.onCameraTranslate += Move;

		SetLayers();
	}

	void SetLayers()
	{
		parallaxLayers.Clear();

		for (int i = 0; i < transform.childCount; i++)
		{
			ParallaxLayerX layer = transform.GetChild(i).GetComponent<ParallaxLayerX>();

			if (layer != null)
			{
				layer.name = "Layer-" + i;
				parallaxLayers.Add(layer);
			}
		}
	}

	void Move(float delta)
	{
		foreach (ParallaxLayerX layer in parallaxLayers)
		{
			if (layer != null)
			{
				layer.Move(delta);
			}
			else
			{
				//Debug.LogWarning("Parallax layer is null. Skipping...");
			}
		}
	}
}