using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMainMenu : MonoBehaviour
{
	// todo vyřešit uložení hry do google play, pokud to má hráč aktivovaný.
	// todo už nevím, kde jsem to použil XD
	public void OnButtonGoToMainMenu()
	{
		PlayerPrefs.SetString(PlayerPrefsKeys.LastScene, "");
		PlayerPrefs.Save();

		SceneManager.LoadScene("MainMenu");
		Debug.Log("tady se dostal");
	}
}