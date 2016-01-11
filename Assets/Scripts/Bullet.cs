using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public GameObject bounceSparks;

    private float timeToLive;
    private float initialTimeToLive = 100;

    private Rigidbody2D _rb = null;
    private Rigidbody2D rb
    {
        get
        {
            if (_rb == null)
                _rb = GetComponent<Rigidbody2D>();
            return _rb;
        }
    }

    // Use this for initialization
    void Awake () 
    {
        timeToLive = initialTimeToLive;
    }
    
    // Update is called once per frame
    void FixedUpdate () 
    {
        if (GameManager.instance.playing == false)
        {
            Destroy(gameObject);
            return;
        }
        timeToLive--;
        GetComponent<Light>().intensity = Mathf.Sqrt(timeToLive / initialTimeToLive);
        GetComponent<Renderer>().material.SetColor("_TintColor", new Color(1, 1, 1, Mathf.Sqrt(timeToLive / initialTimeToLive)));
        if (timeToLive == 0)
            Destroy(gameObject);
    }

    public void Launch(Vector3 direction, float initialVelocity)
    {
        rb.AddForce(new Vector2(direction.x, direction.y) * initialVelocity);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(bounceSparks, transform.position, transform.rotation);
    }
}
