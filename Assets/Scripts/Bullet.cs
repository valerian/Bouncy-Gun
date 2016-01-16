using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public GameObject bounceSparks;
    public GameObject bounceSound;

    private float creationTime;
    private float initialLightIntensity;
    private Color initialMaterialColor;

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
        initialLightIntensity = GetComponent<Light>().intensity;
        initialMaterialColor = GetComponent<Renderer>().material.GetColor("_TintColor");
    }

    void OnEnable ()
    {
        creationTime = Time.time;
        GetComponent<Light>().intensity = initialLightIntensity;
        GetComponent<Renderer>().material.SetColor("_TintColor", initialMaterialColor);
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
            SimplePool.Despawn(gameObject);
            return;
        }
        if ((Time.time - creationTime) > GameManager.instance.bulletDuration)
            SimplePool.Despawn(gameObject);
    }

    public void Launch(Vector3 direction, float initialVelocity)
    {
        rb.AddForce(new Vector2(direction.x, direction.y) * initialVelocity);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        SimplePool.Spawn(bounceSparks, transform.position, transform.rotation);
        SimplePool.Spawn(bounceSound, transform.position, transform.rotation);
    }
}
