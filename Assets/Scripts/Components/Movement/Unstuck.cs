using UnityEngine;
using System.Collections;

public class Unstuck : MonoBehaviour
{
    public float boundaryX = 6.8f;
    public float respawnMaxX = 6f;
    public float respawnShiftY = 1f;

    void FixedUpdate()
    {
        if (Mathf.Abs(transform.position.x) > boundaryX)
            transform.position = new Vector3(
                Random.Range(-respawnMaxX, respawnMaxX),
                transform.position.y + respawnShiftY,
                transform.position.z);
    }
}
