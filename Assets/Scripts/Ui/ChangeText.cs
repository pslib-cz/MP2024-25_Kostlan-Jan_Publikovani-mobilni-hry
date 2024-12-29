using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeText : MonoBehaviour
{
    [System.Serializable]
    public struct TimedText
    {
        public string text;
        public float delay;
    }

    public TimedText[] timedTexts;
    private int currentIndex;

    private UITextWritter textWriter;

    void Start()
    {
        textWriter = GetComponent<UITextWritter>();
        StartCoroutine(ChangeTextRoutine());
    }
    
    IEnumerator ChangeTextRoutine()
    {
        for (currentIndex = 0; currentIndex < timedTexts.Length; currentIndex++)
        {
            textWriter.ChangeText(timedTexts[currentIndex].text, timedTexts[currentIndex].delay);
            while (textWriter.txt.text != timedTexts[currentIndex].text)
            {
                yield return null;
            }
            yield return new WaitForSeconds(timedTexts[currentIndex].delay);
        }
        SceneManager.LoadScene("ZaMestem");
    }
}