using UnityEngine;
using Assets.Scripts.Scenes;
public class NewLevel : MonoBehaviour
{
	public string myscene;

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.CompareTag("Player"))
		{
			if (SceneManager.Instance != null)
			{
				SceneManager.Instance.SceneLoad(myscene);
			}
		}
	}
}