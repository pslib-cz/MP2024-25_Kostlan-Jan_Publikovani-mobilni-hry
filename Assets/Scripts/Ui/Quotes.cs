using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using Random = UnityEngine.Random;
public class Quotes : MonoBehaviour
{
    public LocalizeStringEvent localizeStringEvent;
    public string[] quoteKeys;
    public Text quoteText;


    public void Start()
    {
        GenerateRandomQuote();
    }

    public void GenerateRandomQuote()
    {
        // Zvolí náhodný klíč citátu
        string quoteKey = quoteKeys[Random.Range(0, quoteKeys.Length)];

        // Připojí se k události OnUpdateString
        localizeStringEvent.OnUpdateString.AddListener(OnStringUpdate);

        // Nastaví klíč citátu a obnoví lokalizovaný text
        localizeStringEvent.StringReference.TableEntryReference = quoteKey;
        localizeStringEvent.StringReference.RefreshString();
    }

    private void OnStringUpdate(string localizedQuote)
    { // Odebrání posluchače, aby se zabránilo opětovnému volání
        localizeStringEvent.OnUpdateString.RemoveListener(OnStringUpdate);

        // Nastaví text
        quoteText.text = localizedQuote;
    }
}