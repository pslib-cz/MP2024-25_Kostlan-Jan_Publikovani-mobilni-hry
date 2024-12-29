using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayMiniGame : MonoBehaviour
{
	private PlayerController2D playerController;

	[SerializeField] GameObject minigame;
	[SerializeField] GameObject bariel;
	[SerializeField] GameObject objectToShow;
    [SerializeField] UnityEvent eventsAfterGame;
	[SerializeField] Button exitButton;

    private void Awake()
	{
		playerController = FindAnyObjectByType<PlayerController2D>();
        var button = Instantiate(exitButton, minigame.transform);
        button.onClick.AddListener(ExitMiniGame);
    }

    public void StartMiniGame()
	{
		minigame.SetActive(true);
		playerController.DisableControls();
	}

	public void ExitMiniGame()
	{
		Debug.Log("změna");
		playerController.EnableControls();
		minigame.SetActive(false);
    }

	public void EndMiniGame()
	{
        eventsAfterGame?.Invoke();
        if (bariel != null)
		{
			bariel.SetActive(false);
		}

		if (objectToShow != null)
		{
			objectToShow.SetActive(true);
		}
        playerController.EnableControls();

        minigame.SetActive(false);

	}
}
