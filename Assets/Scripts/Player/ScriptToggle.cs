using System.Collections;
using UnityEngine;

/// <summary>
/// Autoshut down script
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