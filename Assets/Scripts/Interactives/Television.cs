using Assets.Scripts.Minigames;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Television : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer screenImage;
    [SerializeField] private LectvarController lectvarController;
    [SerializeField] private float colorDisplayInterval = 2f;
    [SerializeField] private BoxCollider2D boxCollider2D;

    private int currentColorIndex = 0;
    private bool showColors = false;
    private float timer = 0f;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void ShowNextPotionColor()
    {
        Color[] colorOrder = lectvarController.GetColorOrder();

        if (currentColorIndex < colorOrder.Length)
        {
            screenImage.color = colorOrder[currentColorIndex];
            currentColorIndex++;
        }
        else
        {
            currentColorIndex = 0;

        }
    }

    private void Update()
    {
        if (showColors)
        {
            timer += Time.deltaTime;

            if (timer >= colorDisplayInterval)
            {
                ShowNextPotionColor();
                timer = 0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            showColors = true;
            timer = 0f;
            boxCollider2D.enabled = false;
            screenImage.gameObject.SetActive(true);
        }
    }
}
