using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleAutoEphemeral : Ephemeral
{
    private ParticleSystem particleSystemComponent;

    void Awake()
    {
        particleSystemComponent = GetComponent<ParticleSystem>();
    }

    void OnEnableDelayed()
    {
        totalDuration = particleSystemComponent.duration + particleSystemComponent.startLifetime;
        StartCoroutine(DespawnAfterRemainingTime());
    }
}
