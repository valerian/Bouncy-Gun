using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class GunAudioOnFire : MonoBehaviour
{
    private AudioSource audioSource;
    private GunStatusControl gunStatusControl;

    void Start()
    {
        gunStatusControl = GetComponentInParent<GunStatusControl>();
        if (gunStatusControl == null)
        {
            Debug.LogWarning("No instance of GunStatusControl found");
            return;
        }
        audioSource = GetComponent<AudioSource>();
        gunStatusControl.onFired.AddListener(AudioPlay);
    }

    void AudioPlay()
    {
        float powerRatio = gunStatusControl.chargingRatio;
        powerRatio *= powerRatio;
        audioSource.pitch = 1.3f - (powerRatio * 0.6f);
        audioSource.volume = 0.066f + (powerRatio * 0.2f);
        audioSource.Stop();
        audioSource.Play();
    }
}
