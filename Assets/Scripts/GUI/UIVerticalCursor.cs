using UnityEngine;
using System.Collections;

public class UIVerticalCursor : MonoBehaviour {
    private float initialPosition;

    #region private RectTransform rectTransform
    private RectTransform _rectTransform;
    private RectTransform rectTransform { get { return _rectTransform ?? (_rectTransform = GetComponent<RectTransform>()); } }
    #endregion

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
