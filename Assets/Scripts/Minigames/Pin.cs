using UnityEngine;

public class Pin : MonoBehaviour
{
	public bool isPicked = false;

	public void PinUp()
	{
		if (isPicked) return;

		isPicked = true;
		transform.position = new Vector3(transform.position.x, 740f, transform.position.z);
		Debug.Log("Pin zvednut!");
	}

	public void ResetPin()
	{
		isPicked = false;
		transform.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
	}
}