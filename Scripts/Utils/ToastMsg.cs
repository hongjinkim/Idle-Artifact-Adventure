using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class ToastMsg : MonoSingleton<ToastMsg>
{
    [SerializeField] private TMP_FontAsset customFont;
    [SerializeField] private float fontSize = 36f;
    private TextMeshProUGUI tmpText;
    private float fadeInOutTime = 0.3f;
    public GameObject textObject;

    protected override void Awake()
    {
        base.Awake();

        InitializeText();
    }

    private void InitializeText()
    {
        tmpText = GetComponent<TextMeshProUGUI>();
        if (tmpText == null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObject = new GameObject("ToastCanvas");
                canvas = canvasObject.AddComponent<Canvas>();
                //canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                //canvas.sortingOrder = 32767;
                canvasObject.AddComponent<CanvasScaler>();
                canvasObject.AddComponent<GraphicRaycaster>();
            }

            textObject = new GameObject("ToastText");
            textObject.transform.SetParent(canvas.transform, false);

            tmpText = textObject.AddComponent<TextMeshProUGUI>();
            SetFont(tmpText);
            tmpText.alignment = TextAlignmentOptions.Center;
            tmpText.color = Color.red;

            RectTransform rectTransform = tmpText.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0);
            rectTransform.anchorMax = new Vector2(0.5f, 0);
            rectTransform.anchoredPosition = new Vector2(0, 600);
            rectTransform.sizeDelta = new Vector2(600, 100);
            
        }
        else
        {
            SetFont(tmpText);
        }
        tmpText.fontSize = fontSize;
        tmpText.enabled = false;
        
    }

    private void SetFont(TextMeshProUGUI textComponent)
    {
        if (customFont != null)
        {
            textComponent.font = customFont;
        }
        else
        {
            TMP_FontAsset[] fonts = Resources.FindObjectsOfTypeAll<TMP_FontAsset>();
            if (fonts.Length > 0)
            {
                textComponent.font = fonts[0];
            }
            else
            {
                Debug.LogWarning("ToastMsg: No TMP_FontAsset found. Text may not display correctly.");
            }
        }
    }

    public void ShowMessage(string msg, float durationTime)
    {
        if (tmpText == null)
        {
            Debug.LogError("ToastMsg: TextMeshProUGUI component is null. Attempting to reinitialize.");
            InitializeText();
            if (tmpText == null)
            {
                Debug.LogError("ToastMsg: Failed to initialize TextMeshProUGUI component. Cannot show message.");
                return;
            }
        }

        textObject.transform.SetAsLastSibling();
        StopAllCoroutines();
        StartCoroutine(ShowMessageCoroutine(msg, durationTime));
    }

    private IEnumerator ShowMessageCoroutine(string msg, float durationTime)
    {
        Color originalColor = tmpText.color;
        tmpText.text = msg;
        tmpText.enabled = true;

        yield return FadeInOut(tmpText, fadeInOutTime, true);
        yield return new WaitForSeconds(durationTime);
        yield return FadeInOut(tmpText, fadeInOutTime, false);

        tmpText.enabled = false;
        tmpText.color = originalColor;
    }

    private IEnumerator FadeInOut(TextMeshProUGUI target, float durationTime, bool fadeIn)
    {
        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;

        for (float t = 0; t < durationTime; t += Time.deltaTime)
        {
            float normalizedTime = t / durationTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, normalizedTime);
            target.alpha = alpha;
            yield return null;
        }

        target.alpha = endAlpha;
        
    }

    public void SetFontSize(float newSize)
    {
        fontSize = newSize;
        if (tmpText != null)
        {
            tmpText.fontSize = fontSize;
        }
    }
}