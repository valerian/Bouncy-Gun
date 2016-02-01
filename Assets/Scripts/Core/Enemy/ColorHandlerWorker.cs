using UnityEngine;
using System.Collections;

public partial class ColorHander
{
    // TODO set private and see if that works
    public partial class ColorHandlerWorker
    {
        private Material material;
        private HSBColor initialColor;
        private HSBColor _currentColor;
        private HSBColor currentColor
        {
            get { return _currentColor; }
            set
            {
                _currentColor = value;
                material.color = value.ToColor();
            }
        }

        void Awake()
        {
            material = GetComponent<Renderer>().material;
            initialColor = _currentColor = HSBColor.FromColor(material.color);
        }

        void OnEnable()
        {
            Reset();
        }

        public void Reset()
        {
            currentColor = initialColor;
        }

        // COLOR PARTS SETTERS

        public void SetHue(float hue)
        {
            currentColor = new HSBColor(hue, currentColor.s, currentColor.b, currentColor.a);
        }

        public void SetSaturation(float saturation)
        {
            currentColor = new HSBColor(currentColor.h, saturation, currentColor.b, currentColor.a);
        }

        public void SetBrightness(float brightness)
        {
            currentColor = new HSBColor(currentColor.h, currentColor.s, brightness, currentColor.a);
        }

        public void SetAlpha(float alpha)
        {
            currentColor = new HSBColor(currentColor.h, currentColor.s, currentColor.b, alpha);
        }

        // COLOR PARTS ADDERS

        public void AddHue(float hueAdd)
        {
            currentColor = new HSBColor(Mathf.Repeat(currentColor.h + hueAdd, 1f), currentColor.s, currentColor.b, currentColor.a);
        }

        public void AddSaturation(float saturationAdd)
        {
            currentColor = new HSBColor(currentColor.h, Mathf.Clamp01(currentColor.h + saturationAdd), currentColor.b, currentColor.a);
        }

        public void AddBrightness(float brightnessAdd)
        {
            currentColor = new HSBColor(currentColor.h, currentColor.s, Mathf.Clamp01(currentColor.h + brightnessAdd), currentColor.a);
        }

        public void AddAlpha(float alphaAdd)
        {
            currentColor = new HSBColor(currentColor.h, currentColor.s, currentColor.b, Mathf.Clamp01(currentColor.h + alphaAdd));
        }

        // COLOR PARTS LERPERS

        public void LerpHue(float hue, float ratio)
        {
            currentColor = new HSBColor(Mathf.Lerp(currentColor.h, hue, ratio), currentColor.s, currentColor.b, currentColor.a);
        }

        public void LerpSaturation(float saturation, float ratio)
        {
            currentColor = new HSBColor(currentColor.h, Mathf.Lerp(currentColor.s, saturation, ratio), currentColor.b, currentColor.a);
        }

        public void LerpBrightness(float brightness, float ratio)
        {
            currentColor = new HSBColor(currentColor.h, currentColor.s, Mathf.Lerp(currentColor.b, brightness, ratio), currentColor.a);
        }

        public void LerpAlpha(float alpha, float ratio)
        {
            currentColor = new HSBColor(currentColor.h, currentColor.s, currentColor.b, Mathf.Lerp(currentColor.a, alpha, ratio));
        }
    }
}
