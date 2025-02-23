using UnityEngine;

/// <summary>
/// Pin slouží pro minihru lockpicking.
/// </summary>
public class Pin : MonoBehaviour
{
	public bool isPicked = false;
	public Vector3 inicializationPosition;
	// Upravit, když responzivní design!
	private const float YPosition = 800f;

	public void Start()
	{
		inicializationPosition = transform.position;
	}

	public void PinUp()
	{
		if (isPicked) return;

		isPicked = true;
		transform.position = new Vector3(transform.position.x, YPosition, transform.position.z);
	}

	public void ResetPin()
	{
		isPicked = false;
		transform.position = inicializationPosition;
	}
}