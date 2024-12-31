using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Components;
public class ShowText : MonoBehaviour
{
	[System.Serializable]
	public class LocalizedTextData
	{
		public string localizedTextKey;
		public float displayDuration;
		public bool autoClose;
		public float typingSpeed;
	}

	public LocalizeStringEvent localizeStringEvent;
	public UITextWritter textWriter;
	public LocalizedTextData[] localizedTextData;

	private int currentIndex;

	private void Start()
	{
		DisplayText();
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

			StartCoroutine(WaitAndContinue(data.displayDuration, data.autoClose, data.typingSpeed));
		}
	}

	private void OnStringUpdate(string localizedText)
	{
		localizeStringEvent.OnUpdateString.RemoveListener(OnStringUpdate);
		textWriter.ChangeText(localizedText, localizedTextData[currentIndex - 1].typingSpeed, localizedTextData[currentIndex - 1].typingSpeed);
	}

	private IEnumerator WaitAndContinue(float duration, bool autoClose, float typingSpeed)
	{
		yield return new WaitForSeconds(duration);

		if (autoClose)
		{
			textWriter.ChangeText("", typingSpeed);
		}

		DisplayText();
	}
}
