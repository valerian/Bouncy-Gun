using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpgradeFactory
{
    private List<BONUS_TYPE> bonusCollection = new List<BONUS_TYPE>
    { 
        BONUS_TYPE.bulletEnergy,
        BONUS_TYPE.chargeSpeed, 
        BONUS_TYPE.duration,
        BONUS_TYPE.energy,
        BONUS_TYPE.energyRegen, 
        BONUS_TYPE.health,
        BONUS_TYPE.mass,
        BONUS_TYPE.size,
        BONUS_TYPE.velocity
    };

    public UpgradeFactory()
    {
        
    }

    public Bonus[] GetRandomUpgrade(int nbBonus, int nbMalus, int bonusValue, int malusValue)
    {
        Bonus[] bonusses = new Bonus[(nbBonus + nbMalus)];
        ShuffleBonusCollection();
        for (int i = 0; i < nbBonus + nbMalus; i++)
        {
            bonusses[i] = BonusFactory.CreateInstance(bonusCollection[i]);
            bonusses[i].value = (i < nbBonus) ? bonusValue : -malusValue;
        }
        return bonusses;
    }

    private void ShuffleBonusCollection()
    {
        int n = bonusCollection.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            BONUS_TYPE value = bonusCollection[k];
            bonusCollection[k] = bonusCollection[n];
            bonusCollection[n] = value;
        }
    }
}
