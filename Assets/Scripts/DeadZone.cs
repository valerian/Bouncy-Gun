using UnityEngine;
using System.Collections;

public class DeadZone : MonoBehaviour {

    // Use this for initialization
    void Start () {
        
    }
    
    // Update is called once per frame
    void Update () {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            GameManager.instance.health = GameManager.instance.health - 10;
        }
        Destroy(collision.gameObject);
    }
}
