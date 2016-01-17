using UnityEngine;
using System.Collections;

public class DarkenOnDamaged : MonoBehaviour {

    private Color originalColor;
    private Color mutatedColor;
    private float mutation;

    #region private Renderer rendererComponent
    private Renderer _rendererComponent;
    private Renderer rendererComponent { get { return _rendererComponent ?? (_rendererComponent = GetComponent<Renderer>()); } }
    #endregion
    

    public void Awake()
    {
        originalColor = rendererComponent.material.color;
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
        rendererComponent.material.color = mutatedColor;
    }

    public void HealthChanged(float percent)
    {
        rendererComponent.material.color = mutatedColor * percent;
    }
}
