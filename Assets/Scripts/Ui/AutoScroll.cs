using UnityEngine;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour
{
    public ScrollRect scrollRect;
    public float autoScrollSpeed = 0.1f;
    private bool autoScrollEnabled = true;
    private bool isAutoScrolling = false;

    private void Start()
    {
        if (scrollRect.verticalScrollbar != null)
        {
            scrollRect.verticalScrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);
        }
    }

    private void OnDestroy()
    {
        if (scrollRect.verticalScrollbar != null)
        {
            scrollRect.verticalScrollbar.onValueChanged.RemoveListener(OnScrollbarValueChanged);
        }
    }

    void Update()
    {
        if (autoScrollEnabled)
        {
            isAutoScrolling = true;

            scrollRect.verticalNormalizedPosition -= autoScrollSpeed * Time.deltaTime;

            scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition);

            if (scrollRect.verticalNormalizedPosition <= 0f || scrollRect.verticalNormalizedPosition >= 1f)
            {
                autoScrollEnabled = false;
            }

            isAutoScrolling = false;
        }
    }

    private void OnScrollbarValueChanged(float value)
    {
        if (!isAutoScrolling)
        {
            autoScrollEnabled = false;
        }
    }
}
