using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    [Header("Core")]
    public float startingHealth = 1f;
    public int scoreValue;
    public float damage;
    public float thrust = 50f;
    public float rotateSpeed = 0.02f;
    public float collisionAvoidanceDistance = 1.8f;

    [Header("Death Explosion")]
    public float explostionRadius = 2.0F;
    public float explostionPower = 100.0F;
    public GameObject explosionParticles;
    public GameObject debrisParticles;
    public GameObject explosionSoundEffect;

    [Header("Collision")]
    public float collisionSoundInterval = 0.5f;
    public float collisionSparkInterval = 0.2f; 
    public GameObject bounceSparks;
    public GameObject bounceSoundEffect;

    // Initial physical values
    private float initialMass;
    private Vector3 initialScale;

    // Mutation
    private float mutation = 1f;
    private float mutationMultiplicator { get { return Mathf.Pow(2, mutation) / 2f; } }

    private float lastCollisionSound;
    private float lastCollisionSpark;
    private float health;

    #region private Rigidbody2D rigidBody
    private Rigidbody2D _rigidBody;
    private Rigidbody2D rigidBody { get { return _rigidBody ?? (_rigidBody = GetComponent<Rigidbody2D>()); } }
    #endregion

    #region private CircleCollider2D circleCollider
    private CircleCollider2D _circleCollider;
    private CircleCollider2D circleCollider { get { return _circleCollider ?? (_circleCollider = GetComponent<CircleCollider2D>()); } }
    #endregion
    

    // Use this for initialization
    void Awake () {
        initialMass = rigidBody.mass;
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
        rigidBody.mass = initialMass * mutationMultiplicator;
        health = startingHealth * mutationMultiplicator;
        float mutationScaleFactor = 1f + ((mutation - 1f) * 0.16f);
        transform.localScale = new Vector3(initialScale.x * mutationScaleFactor, initialScale.y * mutationScaleFactor, initialScale.z);
        BroadcastMessage("MutateColor", mutation, SendMessageOptions.DontRequireReceiver);
    }
    
    void FixedUpdate () 
    {
        if (GameManager.instance.playing == false)
        {
            SimplePool.Despawn(gameObject);
            return;
        }

        if (health <= 0)
        {
            SimplePool.Despawn(gameObject);
            return;
        }

        float wallAvoidAdjustX = Mathf.Clamp01(Mathf.Abs(transform.position.x) - 5.5f);
        if (transform.position.x < 0)
            wallAvoidAdjustX *= -1;

        float turretAvoidAdjustX = Mathf.Clamp01(1.8f - Mathf.Abs(transform.position.x)) * Mathf.Clamp01(2f - (Mathf.Abs(transform.position.y) / 4f));
        if (transform.position.x > 0)
            turretAvoidAdjustX *= -1;

        float directionFactor = Mathf.Pow(Mathf.Clamp01(Vector2.Dot(new Vector2(-transform.up.x, -transform.up.y), Vector2.down)), 2);
        float thrustFactor = GameManager.instance.enemySpeedFactor * Time.deltaTime * directionFactor * (1f - wallAvoidAdjustX) * mutationMultiplicator;
        float thrustFactorUndirected = GameManager.instance.enemySpeedFactor * Time.deltaTime * mutationMultiplicator;
        float rotateSpeedFactor = 1f;

        if (transform.position.y > GameManager.instance.outOfScreenFarY)
        {
            thrustFactor *= GameManager.instance.outOfScreenFarBonusThrustFactor;
            rotateSpeedFactor *= GameManager.instance.outOfScreenFarBonusRotateFactor;
        }
        else if (transform.position.y > GameManager.instance.outOfScreenY)
        {
            thrustFactor *= GameManager.instance.outOfScreenBonusThrustFactor;
            rotateSpeedFactor *= GameManager.instance.outOfScreenBonusRotateFactor;
        }

        if (NeedBraking())
            rigidBody.AddForce(transform.up * thrust * thrustFactorUndirected);
        else
            rigidBody.AddForce(-transform.up * thrust * thrustFactor);

        Vector3 direction = new Vector3(wallAvoidAdjustX + turretAvoidAdjustX, 1, 0).normalized;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(0, 0, 1), direction), rotateSpeed * rotateSpeedFactor);
    }

    void Unstuck()
    {
        if (Mathf.Abs(transform.position.x) > 6.8f)
            transform.position = new Vector3((Random.value * 6f) - 3f, transform.position.y + 1f, transform.position.z);
    }

    bool NeedBraking()
    {
        GameObject obstacle = null;

        RaycastHit2D hitCenter = Physics2D.Raycast(transform.position + (-transform.up * (circleCollider.bounds.size.x / 1.9f)), -transform.up, collisionAvoidanceDistance);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position + (((transform.right * 2) - transform.up).normalized * (circleCollider.bounds.size.x / 1.9f)), -transform.up, collisionAvoidanceDistance);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position + ((-(transform.right * 2) - transform.up).normalized * (circleCollider.bounds.size.x / 1.9f)), -transform.up, collisionAvoidanceDistance);
        
        if (hitCenter.collider != null)
            obstacle = hitCenter.collider.gameObject;
        else if (hitLeft.collider != null)
            obstacle = hitLeft.collider.gameObject;
        else if (hitRight.collider != null)
            obstacle = hitRight.collider.gameObject;

        if (obstacle != null
            && Vector3.Distance(obstacle.transform.position, transform.position) < collisionAvoidanceDistance
            && obstacle.gameObject.GetComponent<Rigidbody2D>() != null
            && -transform.InverseTransformDirection(rigidBody.velocity).y - -transform.InverseTransformDirection(obstacle.gameObject.GetComponent<Rigidbody2D>().velocity * 0.9f).y > 0)
            return true;
        return false;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        float mass = rigidBody.mass;
        Rigidbody2D collisionRigidBody = collision.gameObject.GetComponent<Rigidbody2D>();

        if (collisionRigidBody != null)
            mass = Mathf.Min(rigidBody.mass, collisionRigidBody.mass);
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
            bounceAudio.pitch = Random.Range(0.5f, 1.5f);
            bounceAudio.volume = Mathf.Min(0.8f, damage / (rigidBody.mass * 4f));
            bounceAudio.PlayDelayed(Random.Range(0.0f, 0.08f));
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

        if (explosionParticles != null)
            SimplePool.Spawn(explosionParticles, transform.position + new Vector3(0f, 0f, 0f), transform.rotation);
        if (debrisParticles != null)
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

    void Escaped()
    {
        GameManager.instance.health -= damage * mutationMultiplicator;
        GUIManager.instance.DamageText(transform.position + new Vector3(0f, 1f, 0f), (int) (damage * mutationMultiplicator), Color.red);
        GameManager.instance.score -= (int)(scoreValue * mutationMultiplicator);
    }
}
