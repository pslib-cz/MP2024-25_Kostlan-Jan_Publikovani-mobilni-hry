using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParallaxBackgroundY : MonoBehaviour
{
	public ParallaxCameraY parallaxCamera;
	List<ParallaxLayerY> parallaxLayers = new List<ParallaxLayerY>();

	void Start()
	{
		if (parallaxCamera == null)
			parallaxCamera = Camera.main.GetComponent<ParallaxCameraY>();

		if (parallaxCamera != null)
			parallaxCamera.onCameraTranslate += Move;

		SetLayers();

	}

	void SetLayers()
	{
		parallaxLayers.Clear();

		for (int i = 0; i < transform.childCount; i++)
		{
			ParallaxLayerY layer = transform.GetChild(i).GetComponent<ParallaxLayerY>();

			if (layer != null)
			{
				layer.name = "Layer-" + i;
				parallaxLayers.Add(layer);
			}
		}
	}

	void Move(float delta)
	{
		foreach (ParallaxLayerY layer in parallaxLayers)
		{
			layer.Move(delta);
		}

	}
}