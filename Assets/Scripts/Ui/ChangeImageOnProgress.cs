using UnityEngine;
using UnityEngine.UI;

public class ChangeImageOnProgress : MonoBehaviour
{
    [System.Serializable]
    public struct ProgressImagePair
    {
        public string progressString;
        public Sprite image;
    }

    public ProgressImagePair[] progressImagePairs;
    public Image imageComponent;

    private void Start()
    {
        string progressString = PlayerPrefs.GetString(PlayerPrefsKeys.LastScene, "");
        SetProgressImage(progressString);
    }

    private void SetProgressImage(string progressString)
    {
        foreach (ProgressImagePair pair in progressImagePairs)
        {
            if (pair.progressString == progressString)
            {
                imageComponent.sprite = pair.image;
                return; 
            }
        }
        Debug.LogError("Nenalezen žádný odpovídající obrázek pro postup: " + progressString);
    }
}