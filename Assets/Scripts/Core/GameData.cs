using UnityEngine;
using System.Collections;

[System.Serializable]
public struct GameData
{
    public int currentLevel;
    public long currentScore;
    [Space]
    public float healthMax;
    public float energyMax;
    public float energyRegen;
    [Space]
    public float gunChargeEnergy;
    public float gunChargeTime; // max 3.2f for particle effects
    public float bulletVelocity;
    public float bulletMass;
    public float bulletSize;
    public float bulletDuration;
}
