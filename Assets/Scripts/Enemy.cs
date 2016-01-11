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
        Instantiate(bounceSparks, new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, transform.position.z), transform.rotation);
    }

    void OnDestroy()
    {
        Instantiate(explosionParticles, transform.position + new Vector3(0f, 0f, 0f), transform.rotation);
        Instantiate(debrisParticles, transform.position + new Vector3(0f, 0f, 0f), transform.rotation);

        Vector2 explosionPos = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, explostionRadius);
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

            if (rb != null && rb.gameObject.tag != "Player")
                rb.AddExplosionForce(explostionPower, explosionPos, explostionRadius);
        } 
    }
}

public static class Rigidbody2DExtension
{
    public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        var dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        body.AddForce(dir.normalized * explosionForce * wearoff);
    }

    public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius, float upliftModifier)
    {
        var dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        Vector3 baseForce = dir.normalized * explosionForce * wearoff;
        body.AddForce(baseForce);

        float upliftWearoff = 1 - upliftModifier / explosionRadius;
        Vector3 upliftForce = Vector2.up * explosionForce * upliftWearoff;
        body.AddForce(upliftForce);
    }
}
