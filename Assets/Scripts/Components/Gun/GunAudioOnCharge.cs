using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class GunAudioOnCharge : MonoBehaviour
{
    private AudioSource audioSource;
    private GunStatusControl gunStatusControl;
    private float chargingTime = 0;
    private bool looping = false;

    void Start()
    {
        gunStatusControl = GetComponentInParent<GunStatusControl>();
        if (gunStatusControl == null)
        {
            Debug.LogWarning("No instance of GunStatusControl found");
            return;
        }
        audioSource = GetComponent<AudioSource>();
        gunStatusControl.onCharging.AddListener(AudioPlay);
        gunStatusControl.onCharged.AddListener(AudioLoop);
        gunStatusControl.onFired.AddListener(AudioStop);
        gunStatusControl.onDisabled.AddListener(AudioStop);
    }

    void Update()
    {
        if (!looping)
            return;
        if (audioSource.time / Mathf.Abs(audioSource.pitch) >= chargingTime + 0.075f)
            audioSource.pitch = -Mathf.Abs(audioSource.pitch);
        if (audioSource.time / Mathf.Abs(audioSource.pitch) <= Mathf.Max(chargingTime - 0.075f, 0f))
            audioSource.pitch = Mathf.Abs(audioSource.pitch);
    }

    void AudioPlay()
    {
        audioSource.Play();
    }

    void AudioStop()
    {
        audioSource.Stop();
        looping = false;
        audioSource.pitch = Mathf.Abs(audioSource.pitch);
    }

    void AudioLoop()
    {
        chargingTime = Game.GameData.gunChargeTime;
        looping = true;
    }
}
