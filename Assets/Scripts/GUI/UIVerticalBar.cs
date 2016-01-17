using UnityEngine;
using System.Collections;

public class UIVerticalBar : MonoBehaviour {

    public Color specialColor;

    private Color initialColor;
    private float initialPosition;
    private float initialHeight;

    #region private RectTransform rectTransform
    private RectTransform _rectTransform;
    private RectTransform rectTransform { get { return _rectTransform ?? (_rectTransform = GetComponent<RectTransform>()); } }
    #endregion

    #region private UnityEngine.UI.Image image
    private UnityEngine.UI.Image _image;
    private UnityEngine.UI.Image image { get { return _image ?? (_image = GetComponent<UnityEngine.UI.Image>()); } }
    #endregion
    

    void Awake()
    {
        initialPosition = rectTransform.anchoredPosition.y;
        initialHeight = rectTransform.sizeDelta.y;
        initialColor = image.color;
    }

    void UpdateFill(float percent)
    {
        percent = Mathf.Clamp01(percent);
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, initialHeight * percent);
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, initialPosition - ((initialHeight * (1f - percent)) / 2f));
    }

    void SpecialColor(bool active)
    {
        image.color = active ? specialColor : initialColor;
    }
}
