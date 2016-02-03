using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class EnemyMovementWheels : MonoBehaviour
{
    public float thrust = 50f;
    public float rotateSpeed = 0.02f;
    public float collisionAvoidanceDistance = 1.8f;
    [Space]
    public float outOfScreenY = 26f;
    public float outOfScreenBonusThrustFactor = 4f;
    public float outOfScreenBonusRotateFactor = 10f;
    [Space] 
    public float outOfScreenFarY = 33f;
    public float outOfScreenFarBonusThrustFactor = 12f;
    public float outOfScreenFarBonusRotateFactor = 20f;
    [Space] 
    public float wallAvoidLookahead = 3f;
    public float wallAvoidBoundaryX = 5.8f;
    [Space] 
    public float turretAvoidDistanceX = 1.8f;
    public float turretAvoidDistanceY = 8f;

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

    float GetWallAvoidAdjustX()
    {
        float wallAvoidAdjustX = transform.up.y < 0 ? 0f : Mathf.Clamp01(Mathf.Abs((transform.position + (-transform.up * wallAvoidLookahead)).x) - wallAvoidBoundaryX);
        return (transform.position.x > 0) ? wallAvoidAdjustX : -wallAvoidAdjustX;
    }

    float GetTurretAvoidAdjustX()
    {
        float turretAvoidAdjustX = Mathf.Clamp01(1.8f - Mathf.Abs(transform.position.x)) * Mathf.Clamp01((turretAvoidDistanceY - Mathf.Abs(transform.position.y)) / 4f);
        return (transform.position.x < 0) ? turretAvoidAdjustX : -turretAvoidAdjustX;
    }

    void Move()
    {
        float wallAvoidAdjustX = GetWallAvoidAdjustX();
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

        Vector3 direction = new Vector3(wallAvoidAdjustX + GetTurretAvoidAdjustX(), 0.5f, 0).normalized;
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
