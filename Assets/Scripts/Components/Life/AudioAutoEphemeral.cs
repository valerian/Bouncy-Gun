using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioAutoEphemeral : Ephemeral
{
    private AudioSource audioSource;
    public float extraTime = 0f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnableDelayed()
    {
        totalDuration = extraTime + (audioSource.clip.length / audioSource.pitch);
        StartCoroutine(DespawnAfterRemainingTime());
    }
}

