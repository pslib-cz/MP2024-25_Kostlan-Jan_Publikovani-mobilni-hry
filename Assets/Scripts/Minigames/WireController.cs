using Assets.Scripts.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Minigames
{
    public class WireController : MonoBehaviour, IMiniGame
    {
		List<Color> colorsWire = new List<Color>
		{
			Color.red,
            Color.green,
            Color.blue,
            Color.yellow,
            Color.magenta
        };
		public List<Wire> leftSideWires;
        public List<Wire> rightSideWires;

        public RectTransform leftSideContainer;
        public RectTransform rightSideContainer;

        public GameObject wirePrefab;
        public int numberOfWires = 4;
        private int connectedWires = 0;
        private float wireHeight = 0.25f;
        float spacing = 0.8f;

        [SerializeField] private PlayMiniGame playMiniGame;

        private void Start()
        {
            SetupWires();
        }

        public void CheckWinning(int number)
        {
            connectedWires += number;

            if (connectedWires == numberOfWires)
            {
                EndGame();
            }
        }

        private void SetupWires()
        {
            // Generování náhodných barev drátů
            List<Color> wireColors = GetSelectedWireColors(numberOfWires);
            wireHeight = wireHeight / numberOfWires;

            for (int i = 0; i < numberOfWires; i++)
            {
                // Nastavení levého drátu
                GameObject leftWireObject = Instantiate(wirePrefab, leftSideContainer);
                RectTransform leftWireRectTransform = leftWireObject.GetComponent<RectTransform>();
                Wire leftWire = leftWireObject.GetComponent<Wire>();
                leftWire.wireColor = wireColors[i];
                leftWire.wireManager = this;
                leftSideWires.Add(leftWire);

                float topPosition = (wireHeight * i) - (spacing * i); // Horní pozice
                float bottomPosition = topPosition - wireHeight; // Dolní pozice

                leftWireRectTransform.anchorMin = new Vector2(0.5f, bottomPosition);
                leftWireRectTransform.anchorMax = new Vector2(0.5f, topPosition);

                // Nastavení pravého drátu
                GameObject rightWireObject = Instantiate(wirePrefab, rightSideContainer);
                RectTransform rightWireRectTransform = rightWireObject.GetComponent<RectTransform>();
                Wire rightWire = rightWireObject.GetComponent<Wire>();
                rightWire.wireColor = wireColors[i];
                rightWire.wireManager = this;
                rightSideWires.Add(rightWire);

                rightWireRectTransform.anchorMin = new Vector2(0.5f, bottomPosition);
                rightWireRectTransform.anchorMax = new Vector2(0.5f, topPosition);
            }

            ShuffleWires(leftSideWires); // Náhodně zamícháme dráty na levé straně
        }


		// Generujeme náhodné barvy pro dráty
		private List<Color> GetSelectedWireColors(int count)
		{
			List<Color> selectedColors = new List<Color>();

			for (int i = 0; i < count; i++)
			{
				selectedColors.Add(colorsWire[i % colorsWire.Count]); // Cykluje přes předem definované barvy
			}

			return selectedColors;
		}

		// zamícháme dráty z vybrané strany.
		private void ShuffleWires(List<Wire> wireList)
        {
            // Zamícháme seznam drátů
            for (int i = wireList.Count - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                Wire temp = wireList[i];
                wireList[i] = wireList[randomIndex];
                wireList[randomIndex] = temp;
            }

            for (int i = 0; i < wireList.Count; i++)
            {
                Wire wire = wireList[i];
                RectTransform wireRectTransform = wire.GetComponent<RectTransform>();

                float topPosition = (wireHeight * i) - (spacing * i);
                float bottomPosition = topPosition - wireHeight;

                wireRectTransform.anchorMin = new Vector2(wireRectTransform.anchorMin.x, bottomPosition);
                wireRectTransform.anchorMax = new Vector2(wireRectTransform.anchorMax.x, topPosition);

                // Přesuňte objekt v hierarchii, aby odpovídal pořadí v seznamu
                wire.transform.SetSiblingIndex(i);
            }
        }

        // Konec hry, když jsou všechny dráty správně propojeny
        private void EndGame()
        {
            playMiniGame.EndMiniGame();
        }
    }
}
