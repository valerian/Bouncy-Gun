using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ColorHandler))]
[RequireComponent(typeof(Ephemeral))]
public class TintOnEphemeral : MonoBehaviour
{
    [SerializeField]
    private Color Tnit;
    
    [Range(0f, 0.999f)]
    [SerializeField]
    private float startPoint = 0f;

    public bool applyHue = true;
    public bool applySaturation = true;
    public bool applyBrightness = true;
    public bool applyAlpha = true;

    private ColorHandler colorHandler;
    private Ephemeral ephemeral;
    private HSBColor tint;

    void Awake()
    {
        tint = HSBColor.FromColor(Tnit);
        colorHandler = GetComponent<ColorHandler>();
        ephemeral = GetComponent<Ephemeral>();
    }

    void Update()
    {
        if (ephemeral.elapsedTime < ephemeral.totalDuration * startPoint)
            return;
        float ratio = Time.deltaTime / (ephemeral.remainingTime);
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

