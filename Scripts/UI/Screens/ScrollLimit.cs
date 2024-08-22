using UnityEngine;
using UnityEngine.UI;

public class ScrollLimit : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform content;
    public float minY;
    public float maxY;
    public float minX;
    public float maxX;

    protected virtual void Start()
    {
        scrollRect.onValueChanged.AddListener(OnUpDownControl);
        scrollRect.onValueChanged.AddListener(OnLeftRigthControl);
    }

    protected virtual void OnUpDownControl(Vector2 position)
    {
        Vector2 pos = content.anchoredPosition;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        content.anchoredPosition = pos;
    }

    protected virtual void OnLeftRigthControl(Vector2 position)
    {
        Vector2 pos = content.anchoredPosition;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        content.anchoredPosition = pos;
    }
}
