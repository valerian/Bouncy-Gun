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

    private ParticleSystem particleSystemComponent;

    void Start()
    {
        GunStatusControl gunStatusControl = GetComponentInParent<GunStatusControl>();
        if (gunStatusControl == null)
        {
            Debug.LogWarning("No instance of GunStatusControl found");
            return;
        }

        particleSystemComponent = GetComponent<ParticleSystem>();
        gunStatusControl.onCharging.AddListener(((triggerEvent == TRIGGER.charging) ? (UnityAction)ParticlePlay : (UnityAction)ParticleStop));
        gunStatusControl.onCharged.AddListener(((triggerEvent == TRIGGER.charged) ? (UnityAction)ParticlePlay : (UnityAction)ParticleStop));
        gunStatusControl.onFired.AddListener(((triggerEvent == TRIGGER.fired) ? (UnityAction)ParticlePlay : (UnityAction)ParticleStopAndClear));
        gunStatusControl.onDisabled.AddListener(ParticleStop);
    }

    void ParticlePlay()
    {
        particleSystemComponent.Play();
    }

    void ParticleStop()
    {
        particleSystemComponent.Stop();
    }

    void ParticleStopAndClear()
    {
        particleSystemComponent.Stop();
        if (clearOnFired)
            particleSystemComponent.Clear();
    }
}
