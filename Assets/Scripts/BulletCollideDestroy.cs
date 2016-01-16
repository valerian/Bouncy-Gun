using UnityEngine;
using System.Collections;

public class BulletCollideDestroy : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SimplePool.Despawn(collision.gameObject);
        }
    }
}
