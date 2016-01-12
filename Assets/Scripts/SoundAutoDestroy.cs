using UnityEngine;
using System.Collections;

public class SoundAutoDestroy : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        Destroy(gameObject, audioSource.clip.length * (1 / audioSource.pitch));
    }
}
