using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class ChangeIntensityColor : MonoBehaviour
	{
		[SerializeField] Image image;
		public void ChangeColor(float intensity)
		{
			UnityEngine.Color newColor = image.color;
			newColor.a = intensity;
			image.color = newColor;
		}
	}
}
