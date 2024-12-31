using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Efekt průběžného zčervavání obrazovky.
/// </summary>
public class FlashingEnemy : MonoBehaviour
{
	public GameObject enemy;
	public Image imageToModify;
	public float changeSpeed = 1.0f;

	void Update()
	{
		// Pokud je nepřátelský objekt neaktivní
		if (!enemy.activeSelf)
		{
			gameObject.SetActive(false);
			return;
		}

		float targetAlpha = Mathf.PingPong(Time.time * changeSpeed, 0.5f);
		Color newColor = imageToModify.color;
		newColor.a = targetAlpha;
		imageToModify.color = newColor;
	}
}