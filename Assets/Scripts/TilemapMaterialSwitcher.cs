using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapMaterialSwitcher : MonoBehaviour
{
	[SerializeField] private Material visibleMaterial;
	[SerializeField] private Material invisibleMaterial;
	private TilemapRenderer tilemapRenderer;
	[SerializeField] private float timeToSwitch = 5f;
	[SerializeField] private float timeToSwitchBack = 0.2f;

	void Start()
	{
		tilemapRenderer = GetComponent<TilemapRenderer>();
		StartCoroutine(SwitchMaterialRoutine());
	}

	IEnumerator SwitchMaterialRoutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(timeToSwitch);
			tilemapRenderer.material = visibleMaterial;
			yield return new WaitForSeconds(timeToSwitchBack);
			tilemapRenderer.material = invisibleMaterial;
		}
	}
}