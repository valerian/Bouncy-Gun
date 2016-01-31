using UnityEngine;
using System.Collections;

public class GunAiming : MonoBehaviour
{
    void Update()
    {
        if (Game.State != Game.STATE.play)
            return;
        Vector2 pos = Game.InputManager.worldMousePosition;
        float angle = Mathf.Atan2(transform.position.x - pos.y, transform.position.y - pos.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
