using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public class DamageOnCollision : MonoBehaviour
{
    public LayerMask layers;
    public float damageMultiplicator = 1f;
    public float collisionSoundMinInterval = 0.2f;

    [SerializeField]
    private GameObject collisionSoundPrefab;

    private Rigidbody2D rigidBody;
    private Health health;
    private float lastCollisionSoundTime = float.NegativeInfinity;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & layers.value) == 0)
            return;
        float mass = rigidBody.mass;
        Rigidbody2D collisionRigidBody = collision.gameObject.GetComponent<Rigidbody2D>();

        if (collisionRigidBody != null)
            mass = Mathf.Min(rigidBody.mass, collisionRigidBody.mass);
        float damage = Mathf.Abs(Vector2.Dot(collision.contacts[0].normal, collision.relativeVelocity)) * mass * damageMultiplicator;

        health.healthCurrent -= damage;

        if (collisionSoundPrefab && lastCollisionSoundTime + collisionSoundMinInterval < Time.time)
        {
            lastCollisionSoundTime = Time.time;
            AudioSource audioSource = Pool.Spawn(collisionSoundPrefab, transform.position).GetComponent<AudioSource>();
            audioSource.pitch = Random.Range(0.5f, 1.5f);
            audioSource.volume = Mathf.Min(0.8f, 0.5f * (damage / rigidBody.mass));
            audioSource.PlayDelayed(Random.Range(0.0f, 0.08f));
        }
    }
}
