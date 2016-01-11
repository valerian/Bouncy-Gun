using UnityEngine;
using System.Collections;

public class ParticleAutoDestroy : MonoBehaviour {

    // Use this for initialization
    void Start () {
        Destroy(gameObject, GetComponent<ParticleSystem>().startLifetime + GetComponent<ParticleSystem>().duration);
    }
}
