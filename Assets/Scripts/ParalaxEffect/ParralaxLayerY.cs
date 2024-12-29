
using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayerY : MonoBehaviour
{
	public float parallaxFactor;

	public void Move(float delta)
	{
		Vector3 newPos = transform.localPosition;
		newPos.y -= delta * parallaxFactor;

		transform.localPosition = newPos;
	}

}