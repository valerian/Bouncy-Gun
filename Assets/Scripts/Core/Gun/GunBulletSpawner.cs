using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[RequireComponent(typeof(GunStatusControl))]
public class GunBulletSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;

    void Start()
    {
        GunStatusControl gunStatusControl = GetComponent<GunStatusControl>();
        UnityAction d = () => SpawnBullet(gunStatusControl.chargingRatio);
        gunStatusControl.onFired.AddListener(d);
    }

    void SpawnBullet(float chargingRatio)
    {
        Vector3 bulletPosition = transform.position + (transform.up * (0.5f + (Game.GameData.bulletSize / 2.8f)));
        bulletPosition.z = 0.35f;
        GameObject bullet = Pool.Spawn(bulletPrefab, bulletPosition, Quaternion.identity);
        bullet.transform.localScale = bullet.transform.localScale.normalized * Game.GameData.bulletSize;
        Rigidbody2D bulletRigidBody = bullet.GetComponent<Rigidbody2D>();
        bulletRigidBody.mass = Game.GameData.bulletMass;
        bulletRigidBody.AddForce(transform.up * chargingRatio * Game.GameData.bulletMass * Game.GameData.bulletVelocity);
        Ephemeral.AddOrUpdateComponent(bullet, Game.GameData.bulletDuration);
    }
}
