using Assets.Scripts.Interfaces;
using UnityEngine;

public class LockController : MonoBehaviour, IMiniGame
{
	// automatické generování velikosti paklíče, vzdálenosti atd, to by se hodilo spíše než to natvrdo mít definované!
	public Pin[] pins;
	public PlayMiniGame playMiniGame;

	void Start()
	{
	  // natvrdo definovaný locker. Musí se jmenovat nějaký prvek s piny Locker.
	  GameObject locker = GameObject.Find("Locker");
	  // tohle je krutý s tím findobjectinactive.
	  pins = locker.GetComponentsInChildren<Pin>(true);
	}

	void OnDisable()
	{
		ResetPins();
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