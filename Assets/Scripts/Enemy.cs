using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public float thrust = 50f;
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
    public float rotateSpeed = 0.02f;
    public int scoreValue;
    public float damage;

    private float initialMass;
    private Vector3 initialScale;

    private float mutation = 1f;
    private float mutationMultiplicator { get { return Mathf.Pow(2, mutation) / 2f; } }

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
        initialMass = rb.mass;
        initialScale = transform.localScale;
    }

    void OnEnable()
    {
        health = startingHealth * mutationMultiplicator;
        lastCollisionSound = Time.time;
        lastCollisionSpark = Time.time;
    }

    void Mutate(float mutation)
    {
        this.mutation = mutation;
        rb.mass = initialMass * mutationMultiplicator;
        health = startingHealth * mutationMultiplicator;
        float mutationScaleFactor = 1f + (mutation * 0.16f);
        transform.localScale = new Vector3(initialScale.x * mutationScaleFactor, initialScale.y * mutationScaleFactor, initialScale.z);
        BroadcastMessage("MutateColor", mutation, SendMessageOptions.DontRequireReceiver);
    }
    
    // Update is called once per frame
    void FixedUpdate () {
        if (GameManager.instance.playing == false)
        {
            SimplePool.Despawn(gameObject);
            return;
        }

        // Unstuck out of game
        if (Mathf.Abs(transform.position.x) > 6.8f)
            transform.position = new Vector3((Random.value * 6f) - 3f, transform.position.y + 1f, transform.position.z);


        float wallAvoidAdjustX = Mathf.Clamp01(Mathf.Abs(transform.position.x) - 5.5f);
        float turretAvoidAdjustX = Mathf.Clamp01(1.8f - Mathf.Abs(transform.position.x)) * Mathf.Clamp01(2f - (Mathf.Abs(transform.position.y) / 4f));

        float directionFactor = Mathf.Pow(Mathf.Clamp01(Vector2.Dot(new Vector2(-transform.up.x, -transform.up.y), Vector2.down)), 2);
        float thrustFactor = GameManager.instance.enemySpeedFactor * Time.deltaTime * directionFactor * (1f - wallAvoidAdjustX) * mutationMultiplicator;
        float rotateSpeedFactor = 1f;

        if (transform.position.y > GameManager.instance.outOfScreenY)
        {
            thrustFactor *= GameManager.instance.outOfScreenBonusThrustFactor;
            rotateSpeedFactor *= GameManager.instance.outOfScreenBonusRotateFactor;
        }

        rb.AddForce(-transform.up * thrust * thrustFactor);

        if (transform.position.x < 0)
        {
            wallAvoidAdjustX *= -1;
        }
        else
        {
            turretAvoidAdjustX *= -1;
        }

        Vector3 direction = new Vector3(wallAvoidAdjustX + turretAvoidAdjustX, 1, 0).normalized;


        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(0, 0, 1), direction), rotateSpeed * rotateSpeedFactor);
        if (health <= 0)
            SimplePool.Despawn(gameObject);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        float mass = rb.mass;
        Rigidbody2D collisionRigidBody = collision.gameObject.GetComponent<Rigidbody2D>();
        if (collisionRigidBody != null)
            mass = Mathf.Min(rb.mass, collisionRigidBody.mass);
        float damage = Mathf.Abs(Vector2.Dot(collision.contacts[0].normal, collision.relativeVelocity)) * mass;
        if (collision.gameObject.tag == "Player")
            damage *= 0.66f;
        BroadcastMessage("HealthChanged", health / (startingHealth * mutationMultiplicator), SendMessageOptions.DontRequireReceiver);
        health -= damage;

        if (lastCollisionSpark + collisionSparkInterval < Time.time)
        {
            lastCollisionSpark = Time.time;
            SimplePool.Spawn(bounceSparks, new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, transform.position.z), transform.rotation);
        }

        if (lastCollisionSound + collisionSparkInterval < Time.time)
        {
            lastCollisionSound = Time.time;
            AudioSource bounceAudio = (SimplePool.Spawn(bounceSoundEffect, new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, transform.position.z), transform.rotation)).GetComponent<AudioSource>();
            bounceAudio.pitch = Random.Range(0.5f, 1.2f);
            bounceAudio.volume = Mathf.Min(1.0f, damage / 30f);
            bounceAudio.PlayDelayed(Random.Range(0.0f, 0.15f));
        }
    }

    void OnDisable()
    {
        if (GameManager.instance.playing == false)
        {
            return;
        }
        GameManager.instance.enemyAliveCounter--;
        GameManager.instance.score += (int) (scoreValue * mutationMultiplicator);

        SimplePool.Spawn(explosionParticles, transform.position + new Vector3(0f, 0f, 0f), transform.rotation);
        SimplePool.Spawn(debrisParticles, transform.position + new Vector3(0f, 0f, 0f), transform.rotation);
        AudioSource explosionAudio = (SimplePool.Spawn(explosionSoundEffect, transform.position + new Vector3(0f, 0f, 0f), transform.rotation)).GetComponent<AudioSource>();
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

    void EscapeDamage()
    {
        GameManager.instance.health -= damage * mutationMultiplicator;
        GUIManager.instance.DamageText(transform.position, (int) (damage * mutationMultiplicator), Color.red);
    }
}
