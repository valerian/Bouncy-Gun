using UnityEngine;
using System.Collections;

public class SoundAutoDestroy : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, 1f);
    }
}
