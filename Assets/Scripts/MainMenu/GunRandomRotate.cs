using UnityEngine;
using System.Collections;

public class GunRandomRotate : MonoBehaviour
{
    public float rotateSpeed;
    public float aimingAngle;
    public float minimumAngle;
    public float pauseTimeMin;
    public float pauseTimeMax;

    private float rotateStartTime;
    private bool isRotating;
    private float targetAngle;

    void Awake()
    {
        transform.eulerAngles += new Vector3(0, 180, 0);
        InitAiming();
    }
    
    void InitAiming()
    {
        rotateStartTime = Time.time + Random.Range(pauseTimeMin, pauseTimeMax);

        do
            targetAngle = Random.Range(180 - aimingAngle, 180 + aimingAngle);
        while (Mathf.Abs(targetAngle - transform.eulerAngles.y) < minimumAngle);

        isRotating = false;
    }

    void Update()
    {
        if (!isRotating && Time.time >= rotateStartTime)
            isRotating = true;
        if (!isRotating)
            return;
        if (isRotating)
        {
            if (transform.eulerAngles.y > targetAngle - (rotateSpeed * (Time.deltaTime * 2)) &&
                transform.eulerAngles.y < targetAngle + (rotateSpeed * (Time.deltaTime * 2)))
            {
                InitAiming();
            }
            else
            {
                transform.eulerAngles += new Vector3(0, rotateSpeed * Time.deltaTime * (transform.eulerAngles.y < targetAngle ? 1f : -1f), 0);
            }
        }
    }
}
