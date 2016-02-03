using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleAutoEphemeral : Ephemeral
{
    private ParticleSystem particleSystemComponent;
    public float extraTime = 0f;

    void Awake()
    {
        particleSystemComponent = GetComponent<ParticleSystem>();
    }

    void OnEnableDelayed()
    {
        totalDuration = extraTime + particleSystemComponent.duration + particleSystemComponent.startLifetime;
        StartCoroutine(DespawnAfterRemainingTime());
    }
}
