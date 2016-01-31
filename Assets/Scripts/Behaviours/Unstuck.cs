using UnityEngine;
using System.Collections;

public class Unstuck : MonoBehaviour
{
    [SerializeField] private float boundaryX = 6.8f;
    [SerializeField] private float respawnMaxX = 6f;
    [SerializeField] private float respawnShiftY = 1f;

    void FixedUpdate()
    {
        if (Mathf.Abs(transform.position.x) > boundaryX)
            transform.position = new Vector3(
                Random.Range(-respawnMaxX, respawnMaxX),
                transform.position.y + respawnShiftY,
                transform.position.z);
    }
}
