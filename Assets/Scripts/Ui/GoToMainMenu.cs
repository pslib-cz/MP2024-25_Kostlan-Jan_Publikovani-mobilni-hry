using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMainMenu : MonoBehaviour
{
	// todo vyřešit uložení hry do google play, pokud to má hráč aktivovaný.
	public void OnButtonGoToMainMenu()
	{
		PlayerPrefs.SetString(PlayerPrefsKeys.LastScene, "");
		PlayerPrefs.Save();

		SceneManager.LoadScene("MainMenu");
		Debug.Log("tady se dostal");
	}
}