using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public float thrust = 0.0001f;
    public GameObject explosionParticles;
    public GameObject debrisParticles;
    public float startingHealth = 1f;
    public float explostionRadius = 2.0F;
    public float explostionPower = 100.0F;
    public GameObject bounceSparks;
    public GameObject bounceSoundEffect;
    public GameObject explosionSoundEffect;
    public float collisionSoundInterval = 0.5f;
    public float collisionSparkInterval = 0.2f;

    private float lastCollisionSound;
    private float lastCollisionSpark;
    private float health;
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
    void Awake () {
        health = startingHealth;
        lastCollisionSound = Time.time;
        lastCollisionSpark = Time.time;
    }
    
    // Update is called once per frame
    void FixedUpdate () {
        if (GameManager.instance.playing == false)
        {
            Destroy(gameObject);
            return;
        }
        rb.AddForce(-transform.up * thrust);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(0, 0, 1), new Vector3(0, 1, 0)), 0.035f);
        if (health <= 0)
            Destroy(gameObject);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        float mass = rb.mass;
        Rigidbody2D collisionRigidBody = collision.gameObject.GetComponent<Rigidbody2D>();
        if (collisionRigidBody != null)
            mass = Mathf.Min(rb.mass, collisionRigidBody.mass);
        float damage = Mathf.Abs(Vector2.Dot(collision.contacts[0].normal, collision.relativeVelocity)) * mass;
        if (collision.gameObject.tag == "Player")
            damage /= 2;
        BroadcastMessage("Damaged", damage / health, SendMessageOptions.DontRequireReceiver);
        health -= damage;

        if (lastCollisionSpark + collisionSparkInterval < Time.time)
        {
            lastCollisionSpark = Time.time;
            Instantiate(bounceSparks, new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, transform.position.z), transform.rotation);
        }

        if (lastCollisionSound + collisionSparkInterval < Time.time)
        {
            lastCollisionSound = Time.time;
            AudioSource bounceAudio = ((GameObject)Instantiate(bounceSoundEffect, new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, transform.position.z), transform.rotation)).GetComponent<AudioSource>();
            bounceAudio.pitch = Random.Range(0.5f, 1.2f);
            bounceAudio.volume = Mathf.Min(1.0f, damage / 30f);
            bounceAudio.PlayDelayed(Random.Range(0.0f, 0.15f));
        }
    }

    void OnDestroy()
    {
        if (GameManager.instance.playing == false)
        {
            return;
        }
        Instantiate(explosionParticles, transform.position + new Vector3(0f, 0f, 0f), transform.rotation);
        Instantiate(debrisParticles, transform.position + new Vector3(0f, 0f, 0f), transform.rotation);
        AudioSource explosionAudio = ((GameObject)Instantiate(explosionSoundEffect, transform.position + new Vector3(0f, 0f, 0f), transform.rotation)).GetComponent<AudioSource>();
        explosionAudio.pitch = Random.Range(1.0f, 1.5f);
        explosionAudio.PlayDelayed(Random.Range(0.0f, 0.15f));

        Vector2 explosionPos = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, explostionRadius);
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

            if (rb != null && rb.gameObject.tag != "Player")
                AddExplosionForce(rb, explostionPower, explosionPos, explostionRadius);
        } 
    }

    void AddExplosionForce(Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        var dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        body.AddForce(dir.normalized * explosionForce * wearoff);
    }
}
