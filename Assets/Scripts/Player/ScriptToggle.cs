using System.Collections;
using UnityEngine;

/// <summary>
/// Skript, který zníčí určitý skript po určité době.
/// </summary>
public class ScriptToggle : MonoBehaviour
{
	public MonoBehaviour scriptToToggle;
	public float delay = 5f;

	void Start()
	{
		StartCoroutine(ToggleScript());
	}

	IEnumerator ToggleScript()
	{
		scriptToToggle.enabled = false;
		yield return new WaitForSeconds(delay);
		scriptToToggle.enabled = true;
		Destroy(this);
	}
}