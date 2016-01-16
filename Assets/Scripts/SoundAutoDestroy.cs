using UnityEngine;
using System.Collections;

public class SoundAutoDestroy : MonoBehaviour
{
    void OnEnable() 
    {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        Invoke("Despawn", audioSource.clip.length * (1 / audioSource.pitch));
    }

    void Despawn()
    {
        SimplePool.Despawn(gameObject);
    }
}

