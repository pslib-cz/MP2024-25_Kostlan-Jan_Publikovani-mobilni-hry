using Assets.Scripts.Enemies;
using UnityEngine;
using UnityEngine.Localization.Components;
using System.Collections;
using Assets.Scripts.Scenes;

/// <summary>
/// Vytvořená třída, kdybychom tam chtěli přidat další parametry pro lokalizaci.
/// </summary>
[System.Serializable]
public class LocalizedTextData
{
	public string localizedTextKey;
}

/// <summary>
/// Manažer na tutorial.
/// </summary>
[RequireComponent(typeof(LocalizeStringEvent))]
public class TutorialManager : MonoBehaviour
{
	private PlayerController2D playerController;
	public bool isClickedE;
	public string LevelName = "Intro";
	private int currentStep = 0;
	private bool isCompleted = false;
	private BottleTutorial bottleTutorial;
	private LocalizeStringEvent localizationStringEvent;
	private int roundsReposit;
	public LocalizedTextData[] localizedTextData;
	[SerializeField] private GameObject clickMouseMoveRight;
	[SerializeField] private GameObject clickMouseMoveDown;
	[SerializeField] private GameObject mobileImage;
	[SerializeField] private GameObject arrowLeft;
	[SerializeField] private GameObject arrowGunOut;
	[SerializeField] private GameObject arrowFire;
	[SerializeField] private GameObject arrowReload;
	[SerializeField] private GameObject arrowBottle;

	private void Awake()
	{
		bottleTutorial = FindFirstObjectByType<BottleTutorial>();
		playerController = FindFirstObjectByType<PlayerController2D>();
		localizationStringEvent = GetComponent<LocalizeStringEvent>();
		roundsReposit = playerController.GetNumberOfGunReposit();

		if (localizedTextData.Length is not 7)
		{
			Debug.LogError("Počet prvků není rovný počtu kroků");
		}
	}

	private void NextStep()
	{
		currentStep++;
		LocalizedTextData data = localizedTextData[currentStep];
		localizationStringEvent.StringReference.TableEntryReference = data.localizedTextKey;
		localizationStringEvent.StringReference.RefreshString();
	}

	void Update()
	{
		if (!isCompleted)
		{
			HandleTutorialSteps();
		}
	}

	void HandleTutorialSteps()
	{
		switch (currentStep)
		{
			case 0:
				// Získá se, že hráč se pohybuje.
				if (playerController.GetPlayerInput() != 0)
				{
					NextStep();
					clickMouseMoveRight.SetActive(false);
					clickMouseMoveDown.SetActive(true);
				}
				break;
			case 1:
				// Získá se, že hráč je pokrčený.
				if (playerController.IsPlayerCrouch() == true)
				{
					NextStep();
					clickMouseMoveDown.SetActive(false);
					mobileImage.SetActive(false);
					arrowLeft.SetActive(true);
				}
				break;
			case 2:
				// Hráč stiknul e.
				if (isClickedE == true)
				{
					NextStep();
					arrowGunOut.SetActive(true);
				}
				break;
			case 3:
				// Získá se, že hráč vytáhnul pistoli.
				if (playerController.gunout)
				{
					NextStep();
					arrowGunOut.SetActive(false);
					arrowFire.SetActive(true);
					bottleTutorial.gameObject.SetActive(true);
					arrowBottle.gameObject.SetActive(true);
				}
				break;
			case 4:
				// Lahev je zníčená.
				if (bottleTutorial.GetIsBroken() == true)
				{
					arrowFire.SetActive(false);
					NextStep();
					arrowReload.SetActive(true);
					arrowBottle.gameObject.SetActive(false);
				}
				break;
			case 5:
				// Je potřeba nabít pistoli.
				if (roundsReposit != playerController.GetNumberOfGunReposit())
				{
					arrowReload.SetActive(false);
					NextStep();
				}
				break;
			case 6:
				isCompleted = true;
				StartCoroutine(LoadLevelAfterDelay(2f)); // Načítání nové úrovně po 2 sekundách
				break;
		}
	}

	// Coroutine pro načítání nové scény po zpoždění
	private IEnumerator LoadLevelAfterDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
		PlayNewLevel();
	}

	private void PlayNewLevel()
	{
		var sceneManager = SceneManager.Instance;
		sceneManager.LoadLastScene();
	}

	public void SetClickedE()
	{
		isClickedE = true;
	}
}