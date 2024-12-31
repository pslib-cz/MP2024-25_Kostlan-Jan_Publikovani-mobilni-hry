using UnityEngine;

/// <summary>
/// Náhodné generování potomků, jejich pořadí a umístnění.
/// </summary>
public class RandomizeChildren : MonoBehaviour
{
	private void Awake()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			int newSpot = Random.Range(0, transform.childCount);
			Vector3 temp = transform.GetChild(i).position;
			transform.GetChild(i).position = transform.GetChild(newSpot).position;
			transform.GetChild(newSpot).position = temp;
		}
	}
}
