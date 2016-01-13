using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public GameObject bounceSparks;
    public GameObject bounceSound;

    private float creationTime;

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
        creationTime = Time.time;
    }
    
    void Update ()
    {
        GetComponent<Light>().intensity = Mathf.Sqrt(1f - ((Time.time - creationTime) / GameManager.instance.bulletDuration));
        GetComponent<Renderer>().material.SetColor("_TintColor", new Color(1, 1, 1, Mathf.Sqrt(1f - ((Time.time - creationTime) / GameManager.instance.bulletDuration))));
    }

    void FixedUpdate () 
    {
        if (GameManager.instance.playing == false)
        {
            Destroy(gameObject);
            return;
        }
        if ((Time.time - creationTime) > GameManager.instance.bulletDuration)
            Destroy(gameObject);
    }

    public void Launch(Vector3 direction, float initialVelocity)
    {
        rb.AddForce(new Vector2(direction.x, direction.y) * initialVelocity);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(bounceSparks, transform.position, transform.rotation);
        Instantiate(bounceSound, transform.position, transform.rotation);
    }
}
