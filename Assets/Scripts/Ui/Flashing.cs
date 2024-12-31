using UnityEngine;
using UnityEngine.UI;

public class Flashing : MonoBehaviour
{
	public Image imageToModify;
	public float changeSpeed = 1.0f;

	void Update()
	{
		float targetAlpha = Mathf.PingPong(Time.time * changeSpeed, 0.5f);
		Color newColor = imageToModify.color;
		newColor.a = targetAlpha;
		imageToModify.color = newColor;
	}
}