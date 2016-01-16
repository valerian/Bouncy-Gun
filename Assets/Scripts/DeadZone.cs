using UnityEngine;
using System.Collections;

public class DeadZone : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            GameManager.instance.EnemyEscaped(collision.gameObject);
        }
        SimplePool.Despawn(collision.gameObject);
    }
}
