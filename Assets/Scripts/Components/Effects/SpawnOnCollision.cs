using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class SpawnOnCollision : MonoBehaviour
{
    [System.Serializable]
    public struct CollisionSpawnSettings
    {
        public GameObject prefab;
        public float minInterval;
        [HideInInspector]
        public float lastSpawnTime;
    }

    public LayerMask layers;
    public CollisionSpawnSettings[] spawnSettings;

    void OnEnable()
    {
        for (int i = 0; i < spawnSettings.Length; i++)
        {
            spawnSettings[i].lastSpawnTime = Mathf.NegativeInfinity;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & layers.value) == 0)
            return;
        for (int i = 0; i < spawnSettings.Length; i++)
        {
            if (spawnSettings[i].lastSpawnTime + spawnSettings[i].minInterval > Time.time)
                continue;
            spawnSettings[i].lastSpawnTime = Time.time;
            Pool.Spawn(spawnSettings[i].prefab, new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, transform.position.z));
        }
    }

}
