using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Health))]
public class ExplodeOnDeath : MonoBehaviour
{
    [SerializeField]
    private GameObject[] spawnPrefabsOnDeath;
    public float explosionRadius = 3.2f;
    public float explosionForce = 1000f;
    public LayerMask affectedLayers;

    void Start()
    {
        GetComponent<Health>().onDeath.AddListener(Explode);
    }

    void Explode()
    {
        for (int i = 0; i < spawnPrefabsOnDeath.Length; i++)
            Pool.Spawn(spawnPrefabsOnDeath[i], transform.position);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody2D rigidBody = colliders[i].GetComponent<Rigidbody2D>();
            if (rigidBody != null && ((1 << rigidBody.gameObject.layer) & affectedLayers.value) != 0)
            {
                Vector3 direction = (rigidBody.transform.position - transform.position);
                rigidBody.AddForce(direction.normalized * explosionForce * Mathf.Clamp01(1f - (direction.magnitude / explosionRadius)));
            }
        }
    }
}
