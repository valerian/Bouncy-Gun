using UnityEngine;
using System.Collections;

public class GunChargedEffect : MonoBehaviour
{
    #region private ParticleSystem particleSystemComponent
    private ParticleSystem _particleSystemComponent;
    private ParticleSystem particleSystemComponent { get { return _particleSystemComponent ?? (_particleSystemComponent = GetComponent<ParticleSystem>()); } }
    #endregion


    public void Fired()
    {
        particleSystemComponent.Stop();
    }

    public void Charged()
    {
        particleSystemComponent.Play();
    }

    public void GameStop()
    {
        particleSystemComponent.Stop();
    }
}
