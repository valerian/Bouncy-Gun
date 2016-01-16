using UnityEngine;
using System.Collections;

public class ParticleAutoDestroy : MonoBehaviour {

    void OnEnable() 
    {
        Invoke("Despawn", GetComponent<ParticleSystem>().startLifetime + GetComponent<ParticleSystem>().duration);
    }

    void Despawn()
    {
        SimplePool.Despawn(gameObject);
    }
}
