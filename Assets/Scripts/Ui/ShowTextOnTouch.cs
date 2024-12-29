using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(LocalizeStringEvent))]

public class ShowTextOnTouch : MonoBehaviour
{
	[System.Serializable]
	public class LocalizedTextData
	{
		public string localizedTextKey;
		public float displayDuration;
		public bool autoClose;
		public float typingSpeed;
		public bool skippable; // Přidáno pole pro určení, zda je text přeskočitelný
	}

	public LocalizeStringEvent localizeStringEvent;
	public UITextWritter textWriter;
	public LocalizedTextData[] localizedTextData;
	public GameObject dialogWindow;
	public Button skipButton; // Přidáno tlačítko pro přeskočení textu přímo z editoru Unity

	private bool hasDisplayed = false;
	private BoxCollider2D boxCollider;
	private int currentIndex;

	private void Start()
	{
		boxCollider = GetComponent<BoxCollider2D>();
		if (skipButton != null)
		{
            skipButton.onClick.AddListener(SkipText);
        }
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && !hasDisplayed)
		{
			dialogWindow.SetActive(true);
			hasDisplayed = true;
			boxCollider.enabled = false;
			localizeStringEvent.enabled = true;
			currentIndex = 0;
			DisplayText();
		}
	}

	private void DisplayText()
	{
		if (currentIndex < localizedTextData.Length)
		{
			LocalizedTextData data = localizedTextData[currentIndex];
			currentIndex++;

			localizeStringEvent.OnUpdateString.AddListener(OnStringUpdate);

			localizeStringEvent.StringReference.TableEntryReference = data.localizedTextKey;
			localizeStringEvent.StringReference.RefreshString();

			StartCoroutine(WaitAndContinue(data.displayDuration, data.autoClose, data.typingSpeed, data.skippable));
		}
		else
		{
			dialogWindow.SetActive(false);
		}
	}

	private void OnStringUpdate(string localizedText)
	{
		localizeStringEvent.OnUpdateString.RemoveListener(OnStringUpdate);
		textWriter.ChangeText(localizedText, localizedTextData[currentIndex - 1].typingSpeed, localizedTextData[currentIndex - 1].typingSpeed);
	}

	private void SkipText()
	{
		if (currentIndex < localizedTextData.Length)
		{
			LocalizedTextData data = localizedTextData[currentIndex];
			currentIndex++;

			textWriter.ChangeText("", data.typingSpeed);
			StopAllCoroutines();

			if (data.autoClose)
			{
				DisplayText();
			}
			dialogWindow.SetActive(false);
		}
	}

	private IEnumerator WaitAndContinue(float duration, bool autoClose, float typingSpeed, bool skippable)
	{
		float timer = 0f;

		while (timer < duration)
		{
			if (skippable)
			{
				// Přeskočení textu pomocí stisknutí tlačítka
				yield return null;
			}

			timer += Time.deltaTime;
			yield return null;
		}

		if (autoClose)
		{
			textWriter.ChangeText("", typingSpeed);
		}

		DisplayText();
	}
}
