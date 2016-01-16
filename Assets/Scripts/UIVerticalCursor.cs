using UnityEngine;
using System.Collections;

public class UIVerticalCursor : MonoBehaviour {
    private float initialPosition;

    private RectTransform _rectTransform;
    private RectTransform rectTransform
    {
        get
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();
            return _rectTransform;
        }
    }

    void Awake()
    {
        initialPosition = rectTransform.anchoredPosition.y;
    }

    void UpdatePosition(float percent)
    {
        percent = Mathf.Clamp01(percent);
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, initialPosition * percent);
    }
}
