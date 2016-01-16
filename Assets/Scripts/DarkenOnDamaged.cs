using UnityEngine;
using System.Collections;

public class DarkenOnDamaged : MonoBehaviour {

    private Color originalColor;
    private Color mutatedColor;
    private float mutation;
    private Renderer _mr = null;
    private Renderer mr
    {
        get
        {
            if (_mr == null)
                _mr = GetComponent<Renderer>();
            return _mr;
        }
    }

    public void Awake()
    {
        originalColor = mr.material.color;
    }

    public void OnEnable()
    {
        MutateColor(1f);
    }

    public void MutateColor(float mutation)
    {
        this.mutation = mutation;
        HSBColor color = HSBColor.FromColor(originalColor);
        color.h += (mutation - 1f) * 0.15f;
        while (color.h > 1f)
            color.h -= 1f;
        while (color.h < 0f)
            color.h += 1f;
        mutatedColor = color.ToColor();
        mr.material.color = mutatedColor;
    }

    public void HealthChanged(float percent)
    {
        mr.material.color = mutatedColor * percent;
    }
}
