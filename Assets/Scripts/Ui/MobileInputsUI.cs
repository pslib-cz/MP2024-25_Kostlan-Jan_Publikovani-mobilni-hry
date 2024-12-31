using UnityEngine;
public class MobileInputsUI : MonoBehaviour
{
	void Awake()
	{
		if (IsMobilePlatform())
		{
			// Zobrazte tlačítka na mobilních zařízeních
			gameObject.SetActive(true);
		}
		else
		{
			// Skryjte tlačítka na ostatních platformách
			gameObject.SetActive(false);
		}
	}

	private bool IsMobilePlatform()
	{
#if UNITY_ANDROID || UNITY_IOS
		return true;
#else
            return false;
#endif
	}
}