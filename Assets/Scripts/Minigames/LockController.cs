using Assets.Scripts.Interfaces;
using UnityEngine;

public class LockController : MonoBehaviour, IMiniGame
{
	// automatické generování velikosti paklíče, vzdálenosti atd, to by se hodilo spíše než to natvrdo mít definované!
	public Pin[] pins;
	Lockpick lockpick;
	public PlayMiniGame playMiniGame;

	void Start()
	{
		// tohle je krutý s tím findobjectinactive.
		pins = FindObjectsByType<Pin>(FindObjectsInactive.Include, FindObjectsSortMode.None); ResetPins();
	}

	public void ResetPins()
	{
		foreach (Pin pin in pins)
		{
			pin.ResetPin();
		}
	}

	public void AreAllPinsPicked()
	{
		foreach (Pin pin in pins)
		{
			if (!pin.isPicked)
			{
				return;
			}
		}
		EndMiniGame();
	}

	public void EndMiniGame()
	{
		if (playMiniGame != null)
		{
			playMiniGame.EndMiniGame();

		}
	}
}