using UnityEngine;

/// <summary>
/// Definovaný různého UI závisle na zařízení.
/// </summary>
public class DeviceSpecificUI : MonoBehaviour
{
	public RectTransform movableUIElement;
	public Vector2 mobilePosition;
	public Vector2 desktopPosition;

	void Start()
	{
		SetUIPosition();
	}

	void SetUIPosition()
	{
#if UNITY_ANDROID || UNITY_IOS
		// Aplikace běží na mobilním zařízení
		movableUIElement.anchoredPosition = mobilePosition;
#else
        // Aplikace běží na počítači
        movableUIElement.anchoredPosition = desktopPosition;
#endif
	}
}