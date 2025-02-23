using UnityEngine;

/// <summary>
/// Effekt na výherní los.
/// </summary>
public class ScratchCardEffect : MonoBehaviour
{
	public GameObject maskPrefab;
	private bool isPressed = false;

	void Update()
	{
		var mousePos = Input.mousePosition;
		mousePos.z = 2;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);

		if (isPressed)
		{
			GameObject maskSprite = Instantiate(maskPrefab, mousePos, Quaternion.identity);
			maskSprite.transform.parent = gameObject.transform;
		}

		if (Input.GetMouseButtonDown(0))
		{
			Invoke("Reveal", 10);
			isPressed = true;
		}

		else if (Input.GetMouseButtonDown(1))
		{
			{
				isPressed = false;
			}
		}
	}

	void Reveal()
	{
		Destroy(this.gameObject);
	}
}