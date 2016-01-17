using UnityEngine;
using System.Collections;

public class DeadZone : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.SendMessage("Escaped");
        }
        SimplePool.Despawn(collision.gameObject);
    }
}
