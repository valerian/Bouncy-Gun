using UnityEngine;
using System.Collections;

public class GunChargingEffect : MonoBehaviour {

    #region private ParticleSystem particleSystemComponent
    private ParticleSystem _particleSystemComponent;
    private ParticleSystem particleSystemComponent { get { return _particleSystemComponent ?? (_particleSystemComponent = GetComponent<ParticleSystem>()); } }
    #endregion

    public void Charging(float duration)
    {
        particleSystemComponent.Play();
    }

    public void Fired()
    {
        particleSystemComponent.Stop();
    }

    public void Charged()
    {
        particleSystemComponent.Stop();
    }

    public void GameStop()
    {
        particleSystemComponent.Stop();
    }
}
