using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class ColorHandler : MonoBehaviour
{
    // TODO set private and see if that works
    [RequireComponent(typeof(Renderer))]
    private partial class ColorHandlerWorker : MonoBehaviour
    {  
        public static ColorHandlerWorker AddOrUpdateComponent(GameObject where)
        {
            ColorHandlerWorker component = where.GetComponent<ColorHandlerWorker>() ?? where.AddComponent<ColorHandlerWorker>();
            return component;
        }
    }

    [SerializeField]
    private bool handleChildren = true;
    private List<ColorHandlerWorker> workers = new List<ColorHandlerWorker>();
    
    void Awake()
    {
        if (handleChildren)
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
                workers.Add(ColorHandlerWorker.AddOrUpdateComponent(renderers[i].gameObject));
        }
        else
            if (GetComponent<Renderer>() != null)
                workers.Add(ColorHandlerWorker.AddOrUpdateComponent(gameObject));
    }

    // COLOR PARTS SETTERS

    public void SetHue(float hue)
    {
        foreach (ColorHandlerWorker worker in workers) worker.SetHue(hue);
    }

    public void SetSaturation(float saturation)
    {
        foreach (ColorHandlerWorker worker in workers) worker.SetSaturation(saturation);
    }

    public void SetBrightness(float brightness)
    {
        foreach (ColorHandlerWorker worker in workers) worker.SetBrightness(brightness);
    }

    public void SetAlpha(float alpha)
    {
        foreach (ColorHandlerWorker worker in workers) worker.SetAlpha(alpha);
    }

    // COLOR PARTS ADDERS

    public void AddHue(float hueAdd)
    {
        foreach (ColorHandlerWorker worker in workers) worker.AddHue(hueAdd);
    }

    public void AddSaturation(float saturationAdd)
    {
        foreach (ColorHandlerWorker worker in workers) worker.AddSaturation(saturationAdd);
    }

    public void AddBrightness(float brightnessAdd)
    {
        foreach (ColorHandlerWorker worker in workers) worker.AddBrightness(brightnessAdd);
    }

    public void AddAlpha(float alphaAdd)
    {
        foreach (ColorHandlerWorker worker in workers) worker.AddAlpha(alphaAdd);
    }

    // COLOR PARTS LERPERS

    public void LerpHue(float hue, float ratio)
    {
        foreach (ColorHandlerWorker worker in workers) worker.LerpHue(hue, ratio);
    }

    public void LerpSaturation(float saturation, float ratio)
    {
        foreach (ColorHandlerWorker worker in workers) worker.LerpSaturation(saturation, ratio);
    }

    public void LerpBrightness(float brightness, float ratio)
    {
        foreach (ColorHandlerWorker worker in workers) worker.LerpBrightness(brightness, ratio);
    }

    public void LerpAlpha(float alpha, float ratio)
    {
        foreach (ColorHandlerWorker worker in workers) worker.LerpAlpha(alpha, ratio);
    }
}
