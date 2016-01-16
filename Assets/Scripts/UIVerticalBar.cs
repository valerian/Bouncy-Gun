using UnityEngine;
using System.Collections;

public class UIVerticalBar : MonoBehaviour {

    public Color specialColor;

    private Color initialColor;
    private float initialPosition;
    private float initialHeight;

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

    private UnityEngine.UI.Image _image;
    private UnityEngine.UI.Image image
    {
        get
        {
            if (_image == null)
                _image = GetComponent<UnityEngine.UI.Image>();
            return _image;
        }
    }

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
