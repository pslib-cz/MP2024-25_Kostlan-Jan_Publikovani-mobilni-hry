using UnityEngine;

/// <summary>
/// Skript pro padájící list ze stromu.
/// </summary>
public class LeafFall : MonoBehaviour
{
	public float minFallSpeed = 0.3f;
	public float maxFallSpeed = 1f;
	public float minSpinSpeed = 0.3f;
	public float maxSpinSpeed = 1f;

	private float fallSpeed;
	private float spinSpeed;
	private float groundLevel;

	public void Initialize(float groundLevel)
	{
		this.groundLevel = groundLevel;

		// Nastav náhodné rychlosti pádu a otáčení
		fallSpeed = Random.Range(minFallSpeed, maxFallSpeed);
		spinSpeed = Random.Range(minSpinSpeed, maxSpinSpeed);
	}

	void Update()
	{
		// Pokud je list nad zemí, pokračuj v pádu.
		if (transform.position.y > groundLevel)
		{
			transform.position -= Vector3.up * (fallSpeed * Time.deltaTime);
			transform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime);
		}
	}
}