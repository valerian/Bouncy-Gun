using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ColorHander))]
[RequireComponent(typeof(Health))]
public class TintOnDamage : MonoBehaviour
{
    [SerializeField]
    private Color tintInit;

    public bool applyHue = true;
    public bool applySaturation = true;
    public bool applyBrightness = true;
    public bool applyAlpha = true;

    private ColorHander colorHandler;
    private HSBColor tint;

    void Awake()
    {
        tint = HSBColor.FromColor(tintInit);
        colorHandler = GetComponent<ColorHander>();
        GetComponent<Health>().onHealthChanged.AddListener(OnHealthChanged);
    }

    void OnHealthChanged(float previous, float current, float max)
    {
        float ratio = 1f - (current / previous);
        if (applyHue)
            colorHandler.LerpHue(tint.h, ratio);
        if (applySaturation)
            colorHandler.LerpSaturation(tint.s, ratio);
        if (applyBrightness)
            colorHandler.LerpBrightness(tint.b, ratio);
        if (applyAlpha)
            colorHandler.LerpAlpha(tint.a, ratio);
    }
}
