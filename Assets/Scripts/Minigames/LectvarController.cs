using Assets.Scripts.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Minigames
{
	public class LectvarController : MonoBehaviour, IMiniGame
	{
		[SerializeField] public Lectvar[] lectvars;
		private int currentStep = 0;
		public PlayMiniGame playMiniGame;
		public GameObject explosion;
		[SerializeField] private List<PotionAppearance> potionAppearances = new List<PotionAppearance>();

		private void Awake()
		{
			lectvars = FindObjectsByType<Lectvar>(FindObjectsSortMode.None);
			AssignRandomPotions();
		}

		public bool CheckCorrectPotion(int potionIndex, int targetIndex)
		{
			if (potionIndex == currentStep && potionIndex + 1 == targetIndex)
			{
				currentStep++;

				if (IsSequenceComplete())
				{
					EndGame();
				}
				return true;
			}
			ResetGame();
			return false;
		}

		public bool IsSequenceComplete()
		{
			return currentStep >= lectvars.Length - 1;
		}

		public void ResetGame()
		{
			explosion.SetActive(true);
			currentStep = 0;
			foreach (Lectvar potion in lectvars)
			{
				potion.ResetPotions();
			}
			Invoke("DisableExplosion", 0.5f);
		}

		private void DisableExplosion()
		{
			explosion.SetActive(false);
		}

		public void EndGame()
		{
			if (playMiniGame != null)
			{
				playMiniGame.EndMiniGame();
			}
		}

		private void AssignRandomPotions()
		{
			ShuffleList(potionAppearances);

			for (int i = 0; i < lectvars.Length; i++)
			{
				PotionAppearance appearance = potionAppearances[i];
				lectvars[i].SetPotionAppearance(appearance.sprite, i);
			}
		}


		private void ShuffleList<T>(List<T> list)
		{
			for (int i = list.Count - 1; i > 0; i--)
			{
				int j = Random.Range(0, i + 1);
				T temp = list[i];
				list[i] = list[j];
				list[j] = temp;
			}
		}

		public Color[] GetColorOrder()
		{
			Color[] colorOrder = new Color[potionAppearances.Count];
			for (int i = 0; i < potionAppearances.Count; i++)
			{
				colorOrder[i] = potionAppearances[i].color;
			}
			return colorOrder;
		}
	}
}

[System.Serializable]
public class PotionAppearance
{
	public Color color;
	public Sprite sprite;

	public PotionAppearance(Color color, Sprite sprite)
	{
		this.color = color;
		this.sprite = sprite;
	}
}