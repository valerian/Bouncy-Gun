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
    public float energyPerShot;
    [Space]
    public float fireRate;
    public float bulletVelocity;
    public float bulletMass;
    public float bulletSize;
    public float bulletDuration;
    [Space]
    public float levelSpawnWorth;
    [Space]
    public float mutationRate;
}
