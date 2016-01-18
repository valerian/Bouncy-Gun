using UnityEngine;
using System.Collections.Generic;
using System;

public enum BONUS_TYPE
{
    bulletEnergy,
    chargeSpeed,
    duration,
    energy,
    energyRegen,
    health,
    mass,
    size,
    velocity
}

public static class BonusFactory
{
    private static Dictionary<BONUS_TYPE, Type> bonusTypes = new Dictionary<BONUS_TYPE, Type>
    {
        {BONUS_TYPE.bulletEnergy, typeof(BonusBulletEnergy)},
        {BONUS_TYPE.chargeSpeed, typeof(BonusChargeSpeed)},
        {BONUS_TYPE.duration, typeof(BonusDuration)},
        {BONUS_TYPE.energy, typeof(BonusEnergy)},
        {BONUS_TYPE.energyRegen, typeof(BonusEnergyRegen)},
        {BONUS_TYPE.health, typeof(BonusHealth)},
        {BONUS_TYPE.mass, typeof(BonusMass)},
        {BONUS_TYPE.size, typeof(BonusSize)},
        {BONUS_TYPE.velocity, typeof(BonusVelocity)}
    };

    public static Bonus CreateInstance(BONUS_TYPE type)
    {
        return (Bonus) Activator.CreateInstance(bonusTypes[type]);
    }
}
