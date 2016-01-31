using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[RequireComponent(typeof (ParticleSystem))]
public class GunParticleOnEvent : MonoBehaviour
{
    private enum TRIGGER
    {
        charging,
        charged,
        fired,
    }

    [SerializeField]
    private TRIGGER triggerEvent;
    [SerializeField]
    private bool clearOnFired = false;

    private ParticleSystem particleSystem;

    void Start()
    {
        GunStatusControl gunStatusControl = FindObjectOfType<GunStatusControl>();
        if (gunStatusControl == null)
        {
            Debug.LogWarning("No instance of GunStatusControl found");
            return;
        }

        particleSystem = GetComponent<ParticleSystem>();
        gunStatusControl.onCharging.AddListener(((triggerEvent == TRIGGER.charging) ? (UnityAction)ParticlePlay : (UnityAction)ParticleStop));
        gunStatusControl.onCharged.AddListener(((triggerEvent == TRIGGER.charged) ? (UnityAction)ParticlePlay : (UnityAction)ParticleStop));
        gunStatusControl.onFired.AddListener(((triggerEvent == TRIGGER.fired) ? (UnityAction)ParticlePlay : (UnityAction)ParticleStopAndClear));
        gunStatusControl.onDisabled.AddListener(ParticleStop);
    }

    void ParticlePlay()
    {
        particleSystem.Play();
    }

    void ParticleStop()
    {
        particleSystem.Stop();
    }

    void ParticleStopAndClear()
    {
        particleSystem.Stop();
        if (clearOnFired)
            particleSystem.Clear();
    }
}
