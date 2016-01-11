using UnityEngine;
using System.Collections;

public class GunChargingEffect : MonoBehaviour {

    private ParticleSystem _ps = null;
    private ParticleSystem ps
    {
        get
        {
            if (_ps == null)
                _ps = GetComponent<ParticleSystem>();
            return _ps;
        }
    }

    public void Charging()
    {
        ps.Play();
    }

    public void Fired()
    {
        ps.Stop();
    }
}
