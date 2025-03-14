﻿using UnityEngine;

/// <summary>
/// Mění dialog podle typu zařízení.
/// </summary>
public class ActiveByDeviceDialog : MonoBehaviour
{
	[SerializeField] ShowTextOnTouch mobileVersion;
	[SerializeField] ShowTextOnTouch pcVersion;

	void Awake()
	{
		if (Application.isMobilePlatform)
		{
			Destroy(pcVersion);
		}
		else
		{
			Destroy(mobileVersion);
		}
	}
}
