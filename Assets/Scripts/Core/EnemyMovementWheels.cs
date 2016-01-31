using UnityEngine;
using System.Collections;

public class EnemyMovementWheels : MonoBehaviour
{
    [SerializeField] private float outOfScreenY = 26f;
    [SerializeField] private float outOfScreenBonusThrustFactor = 4f;
    [SerializeField] private float outOfScreenBonusRotateFactor = 10f;
    [Space] 
    [SerializeField] private float outOfScreenFarY = 33f;
    [SerializeField] private float outOfScreenFarBonusThrustFactor = 12f;
    [SerializeField] private float outOfScreenFarBonusRotateFactor = 20f;
}
