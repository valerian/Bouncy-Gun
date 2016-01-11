using UnityEngine;
using System.Collections;

public class GunChargedEffect : MonoBehaviour {

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

    public void Fired()
    {
        ps.Stop();
    }

    public void Charged()
    {
        ps.Play();
    }
}
