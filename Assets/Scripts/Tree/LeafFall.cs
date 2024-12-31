using UnityEngine;

public class LeafFall : MonoBehaviour
{
	public float minFallSpeed = 0.3f;
	public float maxFallSpeed = 1f;
	public float minSpinSpeed = 0.3f;
	public float maxSpinSpeed = 1f;

	private float fallSpeed;
	private float spinSpeed;
	private float groundLevel;

	void Start()
	{
		// Nastav náhodné rychlosti pádu a otáčení v rozsahu mezi minimem a maximem.
		fallSpeed = Random.Range(minFallSpeed, maxFallSpeed);
		spinSpeed = Random.Range(minSpinSpeed, maxSpinSpeed);

		// Najdi LeafGenerator v scéně a získej z něj hodnotu groundLevel.
		LeafGenerator leafGenerator = FindObjectOfType<LeafGenerator>();
		if (leafGenerator != null)
		{
			groundLevel = leafGenerator.groundLevel;
		}
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