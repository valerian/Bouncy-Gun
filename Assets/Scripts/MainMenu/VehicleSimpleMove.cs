using UnityEngine;
using System.Collections;

public class VehicleSimpleMove : MonoBehaviour
{
    public float speed;

    void Update()
    {
        if (transform.position.z > 3 || transform.position.z < -45)
        {
            SimplePool.Despawn(gameObject);
            return;
        }
        transform.position += -1f * speed * Time.deltaTime * transform.up;
    }
}
