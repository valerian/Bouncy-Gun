using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioRandomizer : MonoBehaviour
{
    [Header("percent ranges")]
    public float pitchMin = 1f;
    public float pitchMax = 1f;
    public float volumeMin = 1f;
    public float volumeMax = 1f;
    [Header("fixed ranges")]
    public float delayMin = 0f;
    public float delayMax = 0f;

    private AudioSource audioSource;
    private float initialPitch;
    private float initialVolume;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        initialPitch = audioSource.pitch;
        initialVolume = audioSource.volume;
    }

    void OnEnable()
    {
        audioSource.Stop();
        audioSource.pitch = initialPitch * Random.Range(pitchMin, pitchMax);
        audioSource.volume = initialVolume * Random.Range(volumeMin, volumeMax);
        audioSource.PlayDelayed(Random.Range(delayMin, delayMax));
    }
}
