using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class EnemyMovementWheels : MonoBehaviour
{
    [SerializeField] private float thrust = 50f;
    [SerializeField] private float rotateSpeed = 0.02f;
    [SerializeField] private float collisionAvoidanceDistance = 1.8f;
    [Space] 
    [SerializeField] private float outOfScreenY = 26f;
    [SerializeField] private float outOfScreenBonusThrustFactor = 4f;
    [SerializeField] private float outOfScreenBonusRotateFactor = 10f;
    [Space] 
    [SerializeField] private float outOfScreenFarY = 33f;
    [SerializeField] private float outOfScreenFarBonusThrustFactor = 12f;
    [SerializeField] private float outOfScreenFarBonusRotateFactor = 20f;

    private Rigidbody2D rigidBody;
    private CircleCollider2D circleCollider;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float wallAvoidAdjustX = transform.up.y < 0 ? 0f : Mathf.Clamp01(Mathf.Abs((transform.position + (-transform.up * 3f)).x) - 5.8f);
        if (transform.position.x < 0)
            wallAvoidAdjustX *= -1;

        float turretAvoidAdjustX = Mathf.Clamp01(1.8f - Mathf.Abs(transform.position.x)) * Mathf.Clamp01(2f - (Mathf.Abs(transform.position.y) / 4f));
        if (transform.position.x > 0)
            turretAvoidAdjustX *= -1;

        float directionFactor = Mathf.Pow(Mathf.Clamp01(Vector2.Dot(new Vector2(-transform.up.x, -transform.up.y), Vector2.down)), 2);
        float thrustFactor = Time.fixedDeltaTime * directionFactor * (1f - Mathf.Abs(wallAvoidAdjustX)) * rigidBody.mass;
        float thrustFactorUndirected = Time.fixedDeltaTime * rigidBody.mass;
        float rotateSpeedFactor = 1f;

        if (transform.position.y > outOfScreenFarY)
        {
            thrustFactor *= outOfScreenFarBonusThrustFactor;
            rotateSpeedFactor *= outOfScreenFarBonusRotateFactor;
        }
        else if (transform.position.y > outOfScreenY)
        {
            thrustFactor *= outOfScreenBonusThrustFactor;
            rotateSpeedFactor *= outOfScreenBonusRotateFactor;
        }

        if (NeedBraking())
            rigidBody.AddForce(transform.up * thrust * thrustFactorUndirected);
        else
            rigidBody.AddForce(-transform.up * thrust * thrustFactor);

        Vector3 direction = new Vector3(wallAvoidAdjustX + turretAvoidAdjustX, 0.5f, 0).normalized;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(0, 0, 1), direction), rotateSpeed * rotateSpeedFactor);
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
}
