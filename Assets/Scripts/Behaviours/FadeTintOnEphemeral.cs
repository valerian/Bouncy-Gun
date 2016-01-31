using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class FadeTintOnEphemeral : MonoBehaviour
{
    [Range(0f,0.999f)]
    [SerializeField]
    private float fadeStartPoint = 0f;
    private Renderer renderer;
    private Ephemeral ephemeral;
    private Color initialMaterialColor;

    void Start()
    {
        ephemeral = GetComponentInParent<Ephemeral>();
        if (!ephemeral)
        {
            enabled = false;
            Debug.LogWarning("Component Ephemeral not found in parents!");
            return;
        }
        renderer = GetComponent<Renderer>();
        initialMaterialColor = renderer.material.GetColor("_TintColor");
    }

    void Update()
    {
        Color newColor = initialMaterialColor;
        float alphaFactor = Mathf.Clamp01((ephemeral.remainingTime / ephemeral.totalDuration) / (1f - fadeStartPoint));
        newColor.a *= alphaFactor;
        renderer.material.SetColor("_TintColor", newColor);
    }
}
