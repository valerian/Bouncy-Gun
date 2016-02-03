using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioAutoEphemeral : Ephemeral
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnableDelayed()
    {
        totalDuration = audioSource.clip.length / audioSource.pitch;
        StartCoroutine(DespawnAfterRemainingTime());
    }
}

