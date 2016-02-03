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
        foreach (GameObject prefab in spawnPrefabsOnDeath)
            Pool.Spawn(prefab, transform.position);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D rigidBody = hit.GetComponent<Rigidbody2D>();
            if (rigidBody != null && ((1 << rigidBody.gameObject.layer) & affectedLayers.value) != 0)
            {
                Vector3 direction = (rigidBody.transform.position - transform.position);
                rigidBody.AddForce(direction.normalized * explosionForce * Mathf.Clamp01(1f - (direction.magnitude / explosionRadius)));
            }
        }
    }
}
