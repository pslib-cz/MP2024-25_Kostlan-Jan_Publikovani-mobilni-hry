using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Automatický mění scénu při inicializaci.
/// </summary>
public class AutomaticSceneSwitch : MonoBehaviour
{
	public float timeToSwitch = 5.0f;
	public string sceneToSwitchTo = "ZaMestem";
	public InputActionReference skipAction;

	private void Start()
	{
		StartCoroutine(WaitAndSwitchScene());
	}

	private void OnEnable()
	{
		skipAction.action.Enable();
		skipAction.action.performed += OnSkipActionPerformed;
	}

	private void OnDisable()
	{
		skipAction.action.Disable();
		skipAction.action.performed -= OnSkipActionPerformed;
	}

	private void OnSkipActionPerformed(InputAction.CallbackContext context)
	{
		SceneManager.LoadScene(sceneToSwitchTo);
	}

	private IEnumerator WaitAndSwitchScene()
	{
		yield return new WaitForSeconds(timeToSwitch);

		SceneManager.LoadScene(sceneToSwitchTo);
	}
}
