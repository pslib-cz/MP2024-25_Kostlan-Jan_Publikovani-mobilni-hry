using Assets.Scripts.Extension;
using Assets.Scripts.Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Assets.Scripts
{
	public class BalloonManager : MonoBehaviour
	{
		public Light2D playerLight;
		private List<Balloon> balloonList = new List<Balloon>();
		private int balloonsToActivate = 2;
		private const int maxBalloonsToActivate = 7;
		private const float delayBetweenWaves = 20f;
		private const float lightOffDuration = 1f; // Světlo zhasne na 1 sekundu
		public string levelName = "";

		private void Start()
		{
			FindAllBalloons();
			balloonList.Shuffle();
			StartCoroutine(ActivateBalloonsOverTime());
		}

		/// <summary>
		/// Najde všechny balónky ve scéně
		/// </summary>
		private void FindAllBalloons()
		{
			balloonList.Clear();
			foreach (Transform child in transform)
			{
				Balloon balloon = child.GetComponent<Balloon>();
				if (balloon != null)
				{
					balloonList.Add(balloon);
				}
			}
		}
		private IEnumerator ActivateBalloonsOverTime()
		{
			while (balloonsToActivate <= maxBalloonsToActivate)
			{
				// Zhasneme hráčovo světlo
				if (playerLight != null)
				{
					playerLight.enabled = false;
				}

				yield return new WaitForSeconds(lightOffDuration); // Počkáme 1 sekundu

				// Rozsvítíme hráčovo světlo zpět
				if (playerLight != null)
				{
					playerLight.enabled = true;
				}

				int activatedCount = 0;

				foreach (Balloon balloon in balloonList)
				{
					if (balloon != null && !balloon.isAttack)
					{
						balloon.SetAggressive(true);
						activatedCount++;

						if (activatedCount >= balloonsToActivate)
							break;
					}
				}

				yield return new WaitForSeconds(delayBetweenWaves);

				balloonsToActivate += 1;
			}
			SceneManager.Instance.SceneLoad(levelName);
		}
	}
}